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
        static Queue<TextBlock> lstMessages = new Queue<TextBlock>();
        static TextBlock? currentResponse;
        int subscriberCount = 0;
        static double messageHeight = 30;

        public uscChat()
        {
            InitializeComponent();
        }

        private void btnChat_Click(object sender, RoutedEventArgs e)
        {// Calls the SendMessage method 
            SendMessage();
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
                    Background = (SolidColorBrush)(new BrushConverter().ConvertFromString("#FE5DA0")),
                    Foreground = new SolidColorBrush(Colors.White),
                    MaxWidth = 300,
                };

                // Create a border to visually separate the message in the chat UI
                Border border = new Border
                {
                    BorderThickness = new Thickness(0),
                    Background = (SolidColorBrush)(new BrushConverter().ConvertFromString("#FE5DA0")),
                    Child = message,
                    CornerRadius = new CornerRadius(10),
                    Padding = new Thickness(10),
                    HorizontalAlignment = HorizontalAlignment.Right,
                    VerticalAlignment = VerticalAlignment.Top,
                    Margin = new Thickness(0, 0, 0, 20)
                };

                currentResponse = message;

                // Add the message to the queue of chat messages
                lstMessages.Enqueue(message);
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
                Background = (SolidColorBrush)(new BrushConverter().ConvertFromString("#266DD3")),
                Foreground = new SolidColorBrush(Colors.White),
                MaxWidth = 300,
            };

            Border border = new Border
            {
                BorderThickness = new Thickness(0),
                Background = (SolidColorBrush)(new BrushConverter().ConvertFromString("#266DD3")),
                Child = message,
                CornerRadius = new CornerRadius(10),
                Padding = new Thickness(10),
                HorizontalAlignment = HorizontalAlignment.Left,
                VerticalAlignment = VerticalAlignment.Top,
                Margin = new Thickness(0, 0, 0, 20)
            };

            lstMessages.Enqueue(message); // Add the message to the queue of chat messages
            grdChat.Children.Add(border);
        }

        // Handles the API's response, appending the response text to the placeholder message
        private static void HandleResponse(string word, bool done)
        {
            if (done)
            {
                currentResponse = null; // Clear the current response
            }

            // If there is an active response, append the words to the current message
            if (currentResponse != null)
            {
                // If the placeholder "..." is still there, clear it
                if (currentResponse.Text == "   ... ")
                {
                    currentResponse.Text = "";
                }

                currentResponse.Text += word;
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
    }
}
