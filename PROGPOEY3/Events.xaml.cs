using PROGPOEY3.Data.Event;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;

namespace PROGPOEY3
{
    /// <summary>
    /// Interaction logic for Events.xaml
    /// </summary>
    public partial class Events : UserControl
    {

        HashSet<Event> hashEvents = new HashSet<Event>();
        HashSet<Event> similarEvents = new HashSet<Event>();
        EventSuggestions suggestions = new EventSuggestions();

        public Events(EventSuggestions userSuggestions, HashSet<Event> hashEvents)
        {
            InitializeComponent();
            suggestions = userSuggestions;
            cmbCategory.ItemsSource = EventCategories.Categories;

            this.hashEvents = hashEvents;

            pnlEvents.Children.Clear();
            lstSearch.SelectionChanged += LstSearch_SelectionChanged;

            pnlEvents.Children.Add(new Label { Content = "Loading Events...", Style = (Style)FindResource("Title") });

            LoadEvents();
        }

        public async Task LoadEvents()
        {
            pnlEvents.Children.Clear();

            await pnlEvents.Dispatcher.InvokeAsync(() =>
            {
                foreach (Event currentEvent in hashEvents)
                {
                    uscEventLayout layout = new uscEventLayout(currentEvent);
                    pnlEvents.Children.Add(layout);
                }
            });
        }

        private void LstSearch_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (lstSearch.SelectedItem != null) // Check if an item is selected
            {
                // Assign the selected item to the TextBox
                txtSearch.Text = lstSearch.SelectedItem.ToString();
                btnSearch_Click(new object(), new RoutedEventArgs());
            }
        }

        private void txtSearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            string searchStr = txtSearch.Text;

            if (lstSearch != null)
            {
                if (!string.IsNullOrEmpty(searchStr))
                {
                    lstSearch.Items.Clear();

                    hashEvents.Where(x => x.Name.ToLower().StartsWith(searchStr.ToLower()) || x.Name.ToLower().Contains(searchStr.ToLower())).Select(x => x.Name).OrderBy(x => x.ToLower().IndexOf(searchStr.ToLower())).Take(5).ToList().ForEach(x => lstSearch.Items.Add(x));
                    lstSearch.Visibility = Visibility.Visible;
                } else
                {
                    lstSearch.Items.Clear();
                    lstSearch.Visibility = Visibility.Collapsed;
                }
            }
        }

        private void btnSearch_Click(object sender, RoutedEventArgs e)
        {
            lstSearch.Visibility = Visibility.Collapsed;

            pnlEvents.Children.Clear();
            pnlSuggest.Children.Clear();

            string searchStr = txtSearch.Text;
            string categoryFilter = cmbCategory.Text;
            DateOnly? startDate = null;
            DateOnly? endDate = null;
            List<Event> lstEvents = new List<Event>();
            bool isDateFilter = false;

            if (dpStart.SelectedDate != null)
            {
                startDate = DateOnly.FromDateTime((DateTime)dpStart.SelectedDate);
            }

            if (dpEnd.SelectedDate != null)
            {
                endDate = DateOnly.FromDateTime((DateTime)dpEnd.SelectedDate);
            }

            txtSearch.Text = "";
            cmbCategory.SelectedItem = null;
            dpStart.SelectedDate = null;
            dpEnd.SelectedDate = null;

            if (startDate == null && endDate != null)
            {
                MessageBox.Show("You must enter a start date to filter by date");
            } else if (startDate != null && endDate == null)
            {
                MessageBox.Show("You must enter an end date to filter by date");
            } else if (startDate != null && endDate != null)
            {
                isDateFilter = true;
            }

            if (!string.IsNullOrWhiteSpace(searchStr))
            {
                lstEvents = hashEvents.ToList().Where(x => x.Name.ToLower().StartsWith(searchStr.ToLower()) || x.Name.ToLower().Contains(searchStr.ToLower())).ToList();
            } else
            {
                lstEvents = hashEvents.ToList();
            }

            if (isDateFilter)
            {
                if (!string.IsNullOrEmpty(categoryFilter))
                {// Apply both date and category filters
                    lstEvents = lstEvents.Where(x => x.Category == categoryFilter && x.Date > startDate && x.Date < endDate).ToList();
                } else
                {// Apply only date filter
                    lstEvents = lstEvents.Where(x => x.Date > startDate && x.Date < endDate).ToList();
                }
            } else
            {
                if (!string.IsNullOrEmpty(categoryFilter))
                {// Apply only category filter
                    lstEvents = lstEvents.Where(x => x.Category == categoryFilter).ToList();
                }
            }

            for (int i = 0; i < lstEvents.Count; i++)
            {
                pnlEvents.Children.Add(new uscEventLayout(lstEvents[i]));
                suggestions.AddEvent(lstEvents[i]);
            }

            HashSet<Event> suggestionEvents = new HashSet<Event>();

            foreach (var suggestion in suggestions.suggestedEvents)
            {
                suggestionEvents.Add(suggestion.Value.Item2);
            }

            foreach (var suggestion in suggestionEvents) 
            {
                SuggestionLayout layout = new SuggestionLayout(suggestion);
                layout.SuggestionClicked += SuggestionLayout_SuggestionClicked;

                pnlSuggest.Children.Add(layout);
            }
        }

        private void SuggestionLayout_SuggestionClicked(object sender, Event e)
        {
            txtSearch.Text = e.Name;
            btnSearch_Click(new object(), new RoutedEventArgs());
        }
    }
}
