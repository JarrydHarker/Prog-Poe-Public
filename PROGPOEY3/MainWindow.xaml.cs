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
        List<Report> qReports = new List<Report>();
        uscReport reportControl = new uscReport();

        public MainWindow()
        {
            InitializeComponent();

            reportControl.ReportAdded += AddReport;
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
            qReports.Add(e.Report);
        }

        private void btnPending_Click(object sender, RoutedEventArgs e)
        {
            cncScreen.Content = qReports.Count == 0 ? new uscPending() : new uscPending(qReports);
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