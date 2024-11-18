using PROGPOEY3.Data.BST;
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
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace PROGPOEY3
{
    /// <summary>
    /// Interaction logic for uscHome.xaml
    /// </summary>
    public partial class uscHome : UserControl
    {
        ReportBST reportTree = new ReportBST();
        EventSuggestions suggestions = new EventSuggestions();

        public uscHome(ReportBST reportTree, EventSuggestions suggestions)
        {
            InitializeComponent();

            this.reportTree = reportTree;
            this.suggestions = suggestions;

            DisplayEvents();
            DisplayReports();
        }

        private void brReport_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {

        }

        private void brEvents_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {

        }

        private void DisplayEvents()
        {
            pnlEvents.Children.Clear();

            foreach (var e in suggestions.suggestedEvents)
            {
                SuggestionLayout eventSummary = new SuggestionLayout(e.Value.Item2);
                pnlEvents.Children.Add(eventSummary);
            }
        }

        private void DisplayReports()
        {
            pnlReports.Children.Clear();

            foreach (var report in reportTree.GetReportsInOrder())
            {
                ReportLayout layout = new ReportLayout(report);
                pnlEvents.Children.Add(layout);
            }
        }
    }
}
