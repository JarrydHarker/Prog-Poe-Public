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
    /// Interaction logic for uscEventLayout.xaml
    /// </summary>
    public partial class uscEventLayout : UserControl
    {
        Event currentEvent = new Event();

        public uscEventLayout(Event currentEvent)
        {
            InitializeComponent();

            this.currentEvent = currentEvent;
            lblTitle.Content = currentEvent.Name;
            lblDescription.Content = currentEvent.Description;
            lblCategory.Content = currentEvent.Category;
            lblDate.Content = currentEvent.Date;
            lblTime.Content = currentEvent.Time;
            lblFee.Content = currentEvent.Fee > 0 ? $"R{currentEvent.Fee}.00" : "Free";
            lblVenue.Content = currentEvent.Venue;

            imgCategory.Source = new BitmapImage(new Uri($"Images/{currentEvent.Category}.png", UriKind.Relative));
        }
    }
}
