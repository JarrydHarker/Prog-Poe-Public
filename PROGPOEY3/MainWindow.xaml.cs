using PROGPOEY3.Data;
using System.Text;
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
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        Data.LinkedList<Report> lstReports = new Data.LinkedList<Report>();
        uscReport reportControl = new uscReport();

        public MainWindow()
        {
            InitializeComponent();

            reportControl.ReportAdded += AddReport; //Adding subscriber to custom event ReportAdded
            cncScreen.Content = new uscHome();
        }

        private void btnService_Click(object sender, RoutedEventArgs e)
        {

            cncScreen.Content = new ServiceRequest();
        }

        private void btnEvents_Click(object sender, RoutedEventArgs e)
        {
            cncScreen.Content = new Events();
        }

        private void btnReport_Click(object sender, RoutedEventArgs e)
        {
            cncScreen.Content = reportControl;
        }

        public void AddReport(object sender, ReportAddedEventArgs e)
        {
            //Method ran when a report is added in uscPending
            lstReports.Add(e.Report);
        }

        private void btnPending_Click(object sender, RoutedEventArgs e)
        {
            //Checking if the reports list is empty
            cncScreen.Content = lstReports.Count == 0 ? new uscPending() : new uscPending(lstReports);
        }

        private void btnHome_Click(object sender, RoutedEventArgs e)
        {
            cncScreen.Content = new uscHome();
        }

        private void btnChat_Click(object sender, RoutedEventArgs e)
        {
            cncScreen.Content = new uscChat();
        }
    }
}