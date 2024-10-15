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
    /// Interaction logic for uscChat.xaml
    /// </summary>
    public partial class uscChat : UserControl
    {
        OllamaAPI apiChat = new OllamaAPI();
        static Queue<Tuple<bool, TextBlock>> lstMessages = new Queue<Tuple<bool, TextBlock>>();
        static TextBlock? currentResponse;
        int subscriberCount = 0;
        static double messageHeight = 30;
        static bool isDone = true;
        public event EventHandler<SaveMessagesEventArgs> SaveMessages;
        static bool isBold = false;

        public uscChat()
        {
            InitializeComponent();

            var qMessages = new Queue<Tuple<bool, TextBlock>>(lstMessages);
            
            while (qMessages.Count > 0)
            {
                var tuple = qMessages.Dequeue();
                var message = tuple.Item2;

                Border border = new Border
                {
                    BorderThickness = new Thickness(0),
                    Background = tuple.Item1 ? (SolidColorBrush)(new BrushConverter().ConvertFromString("#EDC0DB")) : (SolidColorBrush)(new BrushConverter().ConvertFromString("#E9EBED")),
                    Child = message,
                    CornerRadius = new CornerRadius(10),
                    Padding = new Thickness(10),
                    HorizontalAlignment = HorizontalAlignment.Right,
                    VerticalAlignment = VerticalAlignment.Top,
                    Margin = new Thickness(20)
                };

                grdChat.Children.Add(border);
            }
        }

        private void btnChat_Click(object sender, RoutedEventArgs e)
        {// Calls the SendMessage method 
            if (isDone)
            {
                SendMessage();
                isDone = false;
            }
        }

        public void SendMessage()
        {
            string request = txtChat.Text;

            if (request != null) //Null check
            {
                CreateMessage(request); // Create a message to display the user's input in the chat UI

                SubscribeToEvent(); // Subscribe to the event that processes the response if not already subscribed


                // CS4014 warning is suppressed because we're not awaiting the task, but the
                // method continues execution before the request completes.
#pragma warning disable CS4014
                apiChat.MakePostRequest(request); // Make a POST request to the AI API to process the user's message
#pragma warning restore CS4014

                // Create a temporary "..." message to show that the system is processing the response
                TextBlock message = new TextBlock
                {
                    Text = " ... ",
                    Focusable = false,
                    TextWrapping = TextWrapping.Wrap,
                    Background = (SolidColorBrush)(new BrushConverter().ConvertFromString("#EDC0DB")),
                    Foreground = new SolidColorBrush(Colors.Black),
                    MaxWidth = 500,
                };

                // Create a border to visually separate the message in the chat UI
                Border border = new Border
                {
                    BorderThickness = new Thickness(0),
                    Background = (SolidColorBrush)(new BrushConverter().ConvertFromString("#EDC0DB")),
                    Child = message,
                    CornerRadius = new CornerRadius(10),
                    Padding = new Thickness(10),
                    HorizontalAlignment = HorizontalAlignment.Right,
                    VerticalAlignment = VerticalAlignment.Top,
                    Margin = new Thickness(20)
                };

                currentResponse = message;

                // Add the message to the queue of chat messages
                lstMessages.Enqueue(Tuple.Create(false, message));
                grdChat.Children.Add(border);

                // Clear the textbox after the message is sent
                txtChat.Clear();
            }

        }

        private void CreateMessage(string request)// Creates and displays the user's message in the chat UI
        {
            TextBlock message = new TextBlock
            {
                Text = request,
                Focusable = false,
                TextWrapping = TextWrapping.Wrap,
                Background = (SolidColorBrush)(new BrushConverter().ConvertFromString("#E9EBED")),
                Foreground = new SolidColorBrush(Colors.Black),
                MaxWidth = 500,
            };

            Border border = new Border
            {
                BorderThickness = new Thickness(0),
                Background = (SolidColorBrush)(new BrushConverter().ConvertFromString("#E9EBED")),
                Child = message,
                CornerRadius = new CornerRadius(10),
                Padding = new Thickness(10),
                HorizontalAlignment = HorizontalAlignment.Left,
                VerticalAlignment = VerticalAlignment.Top,
                Margin = new Thickness(20)
            };

            lstMessages.Enqueue(Tuple.Create(true, message)); // Add the message to the queue of chat messages
            grdChat.Children.Add(border);
        }

        // Handles the API's response, appending the response text to the placeholder message
        private static void HandleResponse(string word, bool done)
        {
            if (done)
            {
                currentResponse = null; // Clear the current response
                isDone = true;
            } else
            {
                isDone = false;
            }

            // If there is an active response, append the words to the current message
            if (currentResponse != null)
            {
                // If the placeholder "..." is still there, clear it
                if (currentResponse.Text == " ... ")
                {
                    currentResponse.Text = "";
                }

                /*if (isBold)
                {
                    if (word.Contains("**"))
                    {
                        isBold = false;
                        word.Remove(word.IndexOf("**"), 2);
                    }
                }

                if (word.Contains("**") || isBold)
                {
                    isBold = true;
                    Run run = new Run(word.Remove(word.IndexOf("**"), 2));
                    run.FontWeight = FontWeights.Bold;
                    currentResponse.Inlines.Add(run);
                } else
                {*/
                    currentResponse.Text += word;
                //}
                
            }
        }

        private void SubscribeToEvent()
        { // Check if no subscribers exist, then subscribe to the response handling event
            if (subscriberCount == 0)
            {
                apiChat.OnStringProcessed += HandleResponse;
                subscriberCount++; // Increment the subscriber count
            }
        }

        private void txtChat_KeyDown(object sender, KeyEventArgs e)
        {// If the Enter key is pressed, simulate the chat button click
            if (e.Key == Key.Enter)
            {
                btnChat_Click(new object(), new RoutedEventArgs()); // Call the button click event
                e.Handled = true; // Mark the event as handled to prevent further processing
            }
        }

        private void btnHelp_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Welcome to the municipal app! Our chatbot is here to assist you in reporting local issues and staying informed about community events. If you encounter problems like illegal dumping or broken streetlights, simply describe the issue and its location, and the chatbot will guide you through the reporting process. It can also provide insights into how these issues impact public safety, utilities, and sanitation.\r\n\r\nUsing the chatbot is easy. Just type your question or report, and follow the prompts to explore various options related to municipal services. You can also ask about upcoming events, allowing you to engage more with your community. The chatbot offers quick responses, making it convenient for you to access essential information and report issues. By utilizing this feature, you're playing an active role in helping create a cleaner, safer, and more engaged community. Thank you for using the municipal app; we hope you find the chatbot helpful in your interactions with local services!", "ChatBot Help", MessageBoxButton.OK, MessageBoxImage.Question);
        }

        internal void GetMessages(Queue<Tuple<bool, TextBlock>> qMessages)
        {
            lstMessages = qMessages;
        }
    }

    public class SaveMessagesEventArgs
    {
        public Queue<Tuple<bool, TextBlock>> lstMessages = new Queue<Tuple<bool, TextBlock>>();

        public SaveMessagesEventArgs(Queue<Tuple<bool, TextBlock>> lstMessages)
        {
            this.lstMessages = lstMessages;
        }
    }
}
