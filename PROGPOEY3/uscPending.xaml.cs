using PROGPOEY3.Data;
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
    /// Interaction logic for uscPending.xaml
    /// </summary>
    public partial class uscPending : UserControl
    {
        Data.LinkedList<Report>? lstReports = null;

        public uscPending(Data.LinkedList<Report>? qReports = null)
        {
            InitializeComponent();

            if (qReports != null)
            {
                lstReports = qReports;

                pnlReports.Children.Remove(lblTemp);
                DisplayReports();
            }
        }

        private void PrepareReportsPanel() // Prepares a header for the reports panel
        {
            // Create a horizontal stack panel for the column headers
            StackPanel stackPanel = new StackPanel{
                Orientation = Orientation.Horizontal,
                HorizontalAlignment = HorizontalAlignment.Stretch,
                Margin = new Thickness(10, 5, 10, 5),
            };

            // Create labels for each column with defined widths to ensure alignment
            Label lblID = new Label
            {
                Content = "ID:",
                Width = 50,
            };

            Label lblLocation = new Label
            {
                Content = "Location:",
                Width = 120, 
            };

            Label lblCategory = new Label
            {
                Content = "Category:",
                Width = 80,
            };

            Label lblDescription = new Label
            {
                Content = "Description:",
                Width = 120,
            };

            Label lblAttachments = new Label
            {
                Content = "Attachments:",
                Width = 100,
            };

            // Add the labels to the stack panel (headers)
            stackPanel.Children.Add(lblID);
            stackPanel.Children.Add(lblLocation);
            stackPanel.Children.Add(lblCategory);
            stackPanel.Children.Add(lblDescription);
            stackPanel.Children.Add(lblAttachments);

            // Add the header panel to the reports panel
            pnlReports.Children.Add(stackPanel);
        }

        public void DisplayReports()// Displays the list of reports in the UI
        {
            pnlReports.Children.Clear(); // Clear any existing content in the reports panel
            PrepareReportsPanel(); // Add the header labels for the columns

            var currentNode = lstReports!.Head; // Get the head node of the linked list

            int count = 0;

            while (currentNode != null) // Iterate through the list of reports and display each one
            {
                count++;
                Report report = currentNode.Value;

                StackPanel pnlReport = new StackPanel
                {
                    Orientation = Orientation.Horizontal,
                    Margin = new Thickness(10, 0, 10, 0)
                };

                StackPanel stckReport = new StackPanel
                {
                    Width = 375,
                    Orientation = Orientation.Horizontal,
                    VerticalAlignment = VerticalAlignment.Center,
                };

                Label lblID = new Label
                {
                    Content = report.reportID,
                    Width = 50, // Define width to avoid overlap or compression
                    VerticalAlignment = VerticalAlignment.Center,
                };

                Label lblLocation = new Label
                {
                    Content = report.location,
                    Width = 120, // Define width as needed
                    VerticalAlignment = VerticalAlignment.Center,
                };

                Label lblCategory = new Label
                {
                    Content = report.category,
                    Width = 80,
                    VerticalAlignment = VerticalAlignment.Center,
                };

                Label lblDescription = new Label
                {
                    Content = report.description,
                    Width = 120,
                    VerticalAlignment = VerticalAlignment.Center,
                };

                // Add the labels to the sub-stack panel (report details)
                stckReport.Children.Add(lblID);
                stckReport.Children.Add(lblLocation);
                stckReport.Children.Add(lblCategory);
                stckReport.Children.Add(lblDescription);

                // Create a ListView to display report attachments or a placeholder if none exist
                ListView lvReport = new ListView
                {
                    ItemsSource = report.attachments != null ? report.attachments : new List<string> { "No Attachments" },
                    MaxWidth = 120,
                    MinWidth = 120,
                    VerticalAlignment = VerticalAlignment.Center,
                };

                // Create a button to allow the user to cancel the report
                Button btnReport = new Button
                {
                    Uid = report.reportID.ToString(),
                    Content = "Cancel",
                    Width = 50,
                    MaxHeight = 25,
                    Style = (Style)FindResource("BaseButton"),
                    VerticalAlignment = VerticalAlignment.Center,
                    HorizontalAlignment = HorizontalAlignment.Right,
                };

                // Attach the event handler for the button click event
                btnReport.Click += btnReport_Click;

                // Add the report details and button to the main panel for the report
                pnlReport.Children.Add(stckReport);
                pnlReport.Children.Add(lvReport);
                pnlReport.Children.Add(btnReport);

                Border border = new Border();

                border.Style = count % 2 == 1 ? (Style)FindResource("ReportBorderA") : (Style)FindResource("ReportBorderB");

                border.Child = pnlReport;

                // Add the entire report panel to the reports panel
                pnlReports.Children.Add(border);

                // Move to the next report in the linked list
                currentNode = currentNode.Next;
            }
        }

        public void btnReport_Click(object sender, RoutedEventArgs e) // Event handler for the "Cancel" button click, used to remove a report
        {
            var currentNode = lstReports!.Head; // Get the head node of the linked list
            Button button = sender as Button; // Get the button that was clicked
            Report report = new Report();

            while (currentNode != null)  // Find the report in the list with the matching ID
            {
                if (currentNode.Value.reportID == button!.Uid)
                {// If the report ID matches the button's UID, store the report
                    report = currentNode.Value;
                }

                currentNode = currentNode.Next;
            }

            if (report != null)// If a matching report is found, remove it
            {
                RemoveReport(report);
            }
        }

        public void RemoveReport(Report report)// Removes the given report from the list and refreshes the display
        {
            lstReports.Remove(report); // Remove the report from the linked list
            DisplayReports(); // Refresh the report display to reflect the change
        }
    }
}
