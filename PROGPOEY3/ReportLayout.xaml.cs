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
    /// Interaction logic for ReportLayout.xaml
    /// </summary>
    public partial class ReportLayout : UserControl
    {
        public Report currentReport = new Report();
        public delegate void ReportCancelEventHandler(object sender, Report report);
        public event ReportCancelEventHandler ReportCanceled;

        public ReportLayout(Report currentReport)
        {
            InitializeComponent();

            this.currentReport = currentReport;
            lblID.Content = currentReport.reportID;
            lblLocation.Content = currentReport.location;
            lblCategory.Content = currentReport.category;
            lblDescription.Content = currentReport.description;
            lblStatus.Content = currentReport.GetStatus();
            lvAttachments.ItemsSource = currentReport.attachments;

            if (currentReport.status == 1)
            {
                //btnResolve.IsEnabled = false;
                //btnResolve.Visibility = Visibility.Hidden;
                pnlReports.Children.Remove(btnResolve);
            }
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            ReportCanceled?.Invoke(this, currentReport);
        }

        private void btnResolve_Click(object sender, RoutedEventArgs e)
        {
            currentReport.ResolveReport();
            lblStatus.Content = currentReport.GetStatus();

            pnlReports.Children.Remove(btnResolve);
        }
    }
}
