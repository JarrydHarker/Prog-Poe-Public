using PROGPOEY3.Data;
using PROGPOEY3.Data.Event;
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
        uscChat chatControl = new uscChat();
        EventSuggestions userSuggestions = new EventSuggestions();
        Button? currentButton = null;
        PriorityQueue<Event, DateOnly> qEvents = new PriorityQueue<Event, DateOnly>();
        HashSet<Event> hashEvents = new HashSet<Event>();
        public Queue<Tuple<bool, TextBlock>> qMessages = new Queue<Tuple<bool, TextBlock>>();

        public MainWindow()
        {
            InitializeComponent();

            reportControl.ReportAdded += AddReport; //Adding subscriber to custom event ReportAdded
            chatControl.SaveMessages += SaveMessages;
            cncScreen.Content = new uscHome();

            Data.Event.EventManager.LoadEvents();
            qEvents = Data.Event.EventManager.GetEvents();

            LoadEvents();

            WindowState = WindowState.Maximized;
            currentButton = btnHome;
            currentButton.Style = (Style)FindResource("SelectedMenuButton");
        }

        private void btnService_Click(object sender, RoutedEventArgs e)
        {
            if (currentButton != null)
            {
                currentButton.Style = (Style)FindResource("MenuButton");
            }

            currentButton = sender as Button;
            cncScreen.Content = new ServiceRequest();
            currentButton.Style = (Style)FindResource("SelectedMenuButton");
        }

        private void btnEvents_Click(object sender, RoutedEventArgs e)
        {
            if (currentButton != null)
            {
                currentButton.Style = (Style)FindResource("MenuButton");
            }

            currentButton = sender as Button;
            cncScreen.Content = new Events(userSuggestions, hashEvents);
            currentButton.Style = (Style)FindResource("SelectedMenuButton");
        }

        private void btnReport_Click(object sender, RoutedEventArgs e)
        {
            if (currentButton != null)
            {
                currentButton.Style = (Style)FindResource("MenuButton");
            }

            currentButton = sender as Button;
            cncScreen.Content = reportControl;
            currentButton.Style = (Style)FindResource("SelectedMenuButton");
        }

        public void AddReport(object sender, ReportAddedEventArgs e)
        {
            //Method ran when a report is added in uscPending
            lstReports.Add(e.Report);
        }

        public void SaveMessages(object sender, SaveMessagesEventArgs e)
        {
            while(e.lstMessages.Count > 0)
            {
                qMessages.Enqueue(e.lstMessages.Dequeue());
            }
        }

        private void btnPending_Click(object sender, RoutedEventArgs e)
        {
            if (currentButton != null)
            {
                currentButton.Style = (Style)FindResource("MenuButton");
            }

            currentButton = sender as Button;
            //Checking if the reports list is empty
            cncScreen.Content = lstReports.Count == 0 ? new uscPending() : new uscPending(lstReports);
            currentButton.Style = (Style)FindResource("SelectedMenuButton");
        }

        private void btnHome_Click(object sender, RoutedEventArgs e)
        {
            if (currentButton != null)
            {
                currentButton.Style = (Style)FindResource("MenuButton");
            }

            currentButton = sender as Button;
            cncScreen.Content = new uscHome();
            currentButton.Style = (Style)FindResource("SelectedMenuButton");
        }

        private void btnChat_Click(object sender, RoutedEventArgs e)
        {
            if (currentButton != null)
            {
                currentButton.Style = (Style)FindResource("MenuButton");
            }

            currentButton = sender as Button;
            chatControl.GetMessages(qMessages);
            cncScreen.Content = chatControl;
            currentButton.Style = (Style)FindResource("SelectedMenuButton");
        }

        public void LoadEvents()
        {
            var tempQ = qEvents;
            while (tempQ.Count > 0)
            {
                var currentEvent = tempQ.Dequeue();
                
                hashEvents.Add(currentEvent);
            }
        }
    }
}