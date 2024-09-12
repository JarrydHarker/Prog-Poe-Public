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
        List<Report>? lstReports = null;

        public uscPending(List<Report>? qReports = null)
        {
            InitializeComponent();

            if (qReports != null)
            {
                lstReports = qReports;

                pnlReports.Children.Remove(lblTemp);
                DisplayReports();
            }
        }

        private void PrepareReportsPanel()
        {
            StackPanel stackPanel = new StackPanel{
                Orientation = Orientation.Horizontal,
                HorizontalAlignment = HorizontalAlignment.Stretch,
                Margin = new Thickness(10, 5, 10, 5),
            };

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

            stackPanel.Children.Add(lblID);
            stackPanel.Children.Add(lblLocation);
            stackPanel.Children.Add(lblCategory);
            stackPanel.Children.Add(lblDescription);
            stackPanel.Children.Add(lblAttachments);

            pnlReports.Children.Add(stackPanel);
        }

        public void DisplayReports()
        {
            pnlReports.Children.Clear();
            PrepareReportsPanel();

            foreach (Report report in lstReports)
            {
                StackPanel pnlReport = new StackPanel
                {
                    Orientation = Orientation.Horizontal,
                    Margin = new Thickness(10, 5, 10, 5)
                };

                StackPanel stckReport = new StackPanel
                {
                    Width = 375,
                    Orientation = Orientation.Horizontal,
                };

                Label lblID = new Label
                {
                    Content = report.reportID,
                    Width = 50, // Define width to avoid overlap or compression
                };

                Label lblLocation = new Label
                {
                    Content = report.location,
                    Width = 120, // Define width as needed
                };

                Label lblCategory = new Label
                {
                    Content = report.category,
                    Width = 80,
                };

                Label lblDescription = new Label
                {
                    Content = report.description,
                    Width = 120,
                };

                stckReport.Children.Add(lblID);
                stckReport.Children.Add(lblLocation);
                stckReport.Children.Add(lblCategory);
                stckReport.Children.Add(lblDescription);

                ListView lvReport = new ListView
                {
                    ItemsSource = report.attachments != null ? report.attachments : new List<string> { "No Attachments" },
                    MaxWidth = 120,
                    Margin = new Thickness(0, 0, 0, 20),
                };

                Button btnReport = new Button
                {
                    Uid = report.reportID.ToString(),
                    Content = "Cancel",
                    Width = 50,
                    MaxHeight = 25,
                    Margin = new Thickness(10, 0, 0, 20),
                    Style = (Style)FindResource("BaseButton")
                };

                btnReport.Click += btnReport_Click;

                pnlReport.Children.Add(stckReport);
                pnlReport.Children.Add(lvReport);
                pnlReport.Children.Add(btnReport);

                pnlReports.Children.Add(pnlReport);
            }
        }

        public void btnReport_Click(object sender, RoutedEventArgs e)
        {

            Button button = sender as Button;

            Report report = lstReports.Where(x => x.reportID == button.Uid).FirstOrDefault();
            if (report != null)
            {
                RemoveReport(report);
            }
        }

        public void RemoveReport(Report report)
        {
            lstReports.Remove(report);
            DisplayReports();
        }
    }
}
