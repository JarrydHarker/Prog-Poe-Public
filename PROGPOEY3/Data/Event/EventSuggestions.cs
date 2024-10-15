using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace PROGPOEY3.Data.Event
{
    public class EventSuggestions
    {
        private float avgPrice = 0;
        private DateRange? userDateRange = null;
        private SortedDictionary<int, Tuple<float, string>> userCategories = new SortedDictionary<int, Tuple<float, string>>();
        private SortedDictionary<int, Tuple<float, string>> userVenues = new SortedDictionary<int, Tuple<float, string>>();
        private SortedDictionary<int, Tuple<float, DateOnly>> userDates = new SortedDictionary<int, Tuple<float, DateOnly>>();
        public SortedDictionary<int, Tuple<float, Event>> suggestedEvents = new SortedDictionary<int, Tuple<float, Event>>();

        private List<Event> userEvents = new List<Event>();

        public void AddEvent(Event userEvent)
        {
            if (userEvents.Count < 10)
            {
                userEvents.Add(userEvent);
            }
            else
            {
                userEvents.RemoveAt(0);
                userEvents.Add(userEvent);
            }


            UpdateUserCategories();
            UpdateUserVenues();
            UpdateUserDates();
            CalcDateRange();
            CalcAvgPrice();

            PopulateSuggestions();
        }

        private void UpdateUserDates()
        {
            userDates.Clear();
            foreach (var e in userEvents)
            {
                userDates.Add(userDates.Count + 1, Tuple.Create((float)(userEvents.IndexOf(e) + 1) / 10, e.Date));
            }

            CalcDateRange();
        }

        private void UpdateUserVenues()
        {
            userVenues.Clear();
            foreach (var e in userEvents)
            {
                userVenues.Add(userVenues.Count + 1, Tuple.Create((float)(userEvents.IndexOf(e) + 1) / 10, e.Venue));
            }
        }

        private void UpdateUserCategories()
        {
            userCategories.Clear();
            foreach (var e in userEvents)
            {
                userCategories.Add(userCategories.Count + 1, Tuple.Create((float)(userEvents.IndexOf(e) + 1) / 10, e.Category));
            }
        }

        private void CalcAvgPrice()
        {
            float totalPrice = 0;

            foreach (var userEvent in userEvents)
            {
                totalPrice += userEvent.Fee;
            }

            avgPrice = totalPrice / userEvents.Count;
        }

        private void CalcDateRange()
        {
            List<DateRange> ranges = new List<DateRange>();

            foreach (var date in userDates)
            {
                ranges.Add(new DateRange(date.Value));
            }

            userDateRange = GetWeightedDateRange(ranges);
        }

        private DateRange GetWeightedDateRange(List<DateRange> ranges)
        {
            double totalWeight = ranges.Sum(r => r.weight);

            // Calculate weighted start and end
            double weightedStartSum = ranges.Sum(r => r.startDate.DayNumber * r.weight);
            double weightedEndSum = ranges.Sum(r => r.endDate.DayNumber * r.weight);

            DateOnly weightedStart = DateOnly.FromDayNumber((int)(weightedStartSum / totalWeight));
            DateOnly weightedEnd = DateOnly.FromDayNumber((int)(weightedEndSum / totalWeight));

            return new DateRange(weightedStart, weightedEnd);
        }

        private void PopulateSuggestions()
        {
            EventManager.LoadEvents();

            var qEvents = EventManager.GetEvents();

            while (qEvents.Count > 0)
            {
                var currentEvent = qEvents.Dequeue();

                AddSuggestion(currentEvent);
            }
        }

        private void AddSuggestion(Event currentEvent)
        {
            bool isDupe = false;

            foreach (var e in suggestedEvents)
            {
                isDupe = e.Value.Item2.Name == currentEvent.Name;

                if (isDupe)
                {
                    break;
                }
            }

            if (!isDupe)
            {
                float minScore = float.MaxValue;

                foreach (var se in suggestedEvents)
                {
                    if (se.Value.Item1 < minScore)
                    {
                        minScore = se.Value.Item1;
                    }
                }

                float currentScore = CalcEventScore(currentEvent);

                if (suggestedEvents.Count > 9)
                {
                    if (minScore < currentScore)
                    {
                        var tupleToRemove = suggestedEvents.FirstOrDefault(t => t.Value.Item1 == minScore);

                        if (tupleToRemove.Value != null)
                        {
                            suggestedEvents[tupleToRemove.Key] = Tuple.Create(currentScore, currentEvent);
                        }
                    }
                } else suggestedEvents.Add(suggestedEvents.Count + 1, Tuple.Create(currentScore, currentEvent));
            }
        }

        private float CalcEventScore(Event currentEvent)
        {
            float categoryScore, venueScore, eventScore = 0;

            categoryScore = userCategories.Where(x => x.Value.Item2 == currentEvent.Category).Sum(x => x.Key);

            venueScore = userVenues.Where(x => x.Value.Item2 == currentEvent.Venue).Sum(x => x.Key);

            eventScore = categoryScore + venueScore;

            if (currentEvent.Date > userDateRange?.startDate && currentEvent.Date < userDateRange?.endDate)
            {
                eventScore += 0.25f;
            }

            if (currentEvent.Fee < avgPrice)
            {
                eventScore += 0.25f;
            }

            return eventScore;
        }
    }

    class DateRange
    {
        public DateOnly startDate;
        public DateOnly endDate;
        public float weight;

        public DateRange(Tuple<float, DateOnly> userDate)
        {
            startDate = userDate.Item2.AddDays(-7);
            endDate = userDate.Item2.AddDays(7);

            weight = userDate.Item1;
        }

        public DateRange(DateOnly startDate, DateOnly endDate)
        {
            weight = 1;
            this.startDate = startDate;
            this.endDate = endDate;
        }
    }
}
