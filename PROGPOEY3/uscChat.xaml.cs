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
        static int charCount = 0;
        static int lineCount = 0;
        static int wordCount = 0;
        int subscriberCount = 0;
        static double messageHeight = 30;

        public uscChat()
        {
            InitializeComponent();
        }

        private void btnChat_Click(object sender, RoutedEventArgs e)
        {
            SendMessage();
        }

        public void SendMessage()
        {
            string request = txtChat.Text;

            if (request != null)
            {
                CreateMessage(request);

                SubscribeToEvent();

#pragma warning disable CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
                apiChat.MakePostRequest(request);
#pragma warning restore CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed

                TextBlock message = new TextBlock
                {
                    Text = "   ... ",
                    Focusable = false,
                    TextWrapping = TextWrapping.Wrap,
                    Background = (SolidColorBrush)(new BrushConverter().ConvertFromString("#FE5DA0")),
                    Foreground = new SolidColorBrush(Colors.White),
                    MaxWidth = 300,
                };

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

                lstMessages.Enqueue(message);
                grdChat.Children.Add(border);

                txtChat.Clear();
            }

        }

        private void CreateMessage(string request)
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

            lstMessages.Enqueue(message);
            grdChat.Children.Add(border);
        }

        private static void HandleResponse(string word, bool done)
        {
            const int topOffset = 80;
            int rightMargin = 100;

            if (done)
            {
                currentResponse = null;
                lineCount = 0;
                wordCount = 0;
            }

            if (currentResponse != null)
            {
                if (currentResponse.Text == "   ... ")
                {
                    currentResponse.Text = "";
                }

                currentResponse.Text += word;
                wordCount ++;

                if (wordCount > 10)
                {
                    //currentResponse.Text += "\n";
                    //currentResponse.Height += 22;
                    wordCount = 0;
                    lineCount++;
                }
            }
        }

        private void SubscribeToEvent()
        {
            if (subscriberCount == 0)
            {
                apiChat.OnStringProcessed += HandleResponse;
                subscriberCount++;
            }
        }
    }
}
