using PROGPOEY3.Data;
using PROGPOEY3.Data.Event;
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
    /// Interaction logic for SuggestionLayout.xaml
    /// </summary>
    public partial class SuggestionLayout : UserControl
    {
        Event currentEvent = new Event();
        public delegate void SuggestionClickEventHandler(object sender, Event e);
        public event SuggestionClickEventHandler SuggestionClicked;

        public SuggestionLayout(Event currentEvent)
        {
            InitializeComponent();

            this.currentEvent = currentEvent;
            lblTitle.Content = currentEvent.Name;

            imgCategory.Source = new BitmapImage(new Uri($"Images/{currentEvent.Category}.png", UriKind.Relative));
        }

        private void UserControl_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            SuggestionClicked?.Invoke(this, currentEvent);
        }
    }
}
