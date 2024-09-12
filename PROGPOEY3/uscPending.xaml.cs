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
                this.lstReports = qReports;

                pnlReports.Children.Remove(lblTemp);
                DisplayReports();
            }
        }

        public void DisplayReports()
        {
            pnlReports.Children.Clear();

            foreach (Report report in lstReports)
            {
                Grid gridReport = new Grid();
                gridReport.ColumnDefinitions.Add(new ColumnDefinition());
                gridReport.ColumnDefinitions.Add(new ColumnDefinition());
                gridReport.ColumnDefinitions.Add(new ColumnDefinition());

                Label lblReport = new Label
                {
                    Content = report.ToString(),
                    Width = 250,
                    Height = 50,
                };

                ListView lvReport = new ListView
                {
                    ItemsSource = report.attachments,
                    MaxHeight = 50,
                };

                Button btnReport = new Button
                {
                    Uid = report.reportID.ToString(),
                    Content = "Cancel",
                    Width = 100,
                    Height = 50,
                };

                btnReport.Click += btnReport_Click;

                Grid.SetColumn(lblReport, 0);
                Grid.SetColumn(lvReport, 1);
                Grid.SetColumn(btnReport, 2);

                gridReport.Children.Add(lblReport);
                gridReport.Children.Add(lvReport);
                gridReport.Children.Add(btnReport);

                pnlReports.Children.Add(gridReport);
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
