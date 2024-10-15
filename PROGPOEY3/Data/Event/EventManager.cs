using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace PROGPOEY3.Data.Event
{
    static class EventManager
    {
        private static PriorityQueue<Event, DateOnly> eventQueue = new PriorityQueue<Event, DateOnly>();

        public static void LoadEvents()
        {
            string filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Events.txt");

            using (StreamReader reader = new StreamReader(filePath))
            {
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    var eventLine = line.Split(';');

                    Event newEvent = new Event
                    {
                        Name = eventLine[0],
                        Category = eventLine[1],
                        Description = eventLine[2],
                        Fee = float.Parse(eventLine[3]),
                        Date = DateOnly.Parse(eventLine[4]),
                        Time = TimeOnly.Parse(eventLine[5]),
                        Venue = eventLine[6],
                    };

                    eventQueue.Enqueue(newEvent, newEvent.Date);
                }
            }
        }

        public static PriorityQueue<Event, DateOnly> GetEvents()
        {
            return eventQueue;
        }
    }
}
