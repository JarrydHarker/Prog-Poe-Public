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
    /// Interaction logic for ServiceRequest.xaml
    /// </summary>
    public partial class ServiceRequest : UserControl
    {
        Queue<Report> reportQueue = new Queue<Report>();

        public ServiceRequest(Queue<Report>? reports = null)
        {
            InitializeComponent();

            if (reports != null)
            {
                reportQueue = reports;
            }
        }
    }
}
