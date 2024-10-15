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
    /// Interaction logic for uscPending.xaml
    /// </summary>
    public partial class uscPending : UserControl
    {
        Data.LinkedList<Report>? lstReports = null;

        public uscPending(Data.LinkedList<Report>? qReports = null)
        {
            InitializeComponent();

            if (qReports != null)
            {
                lstReports = qReports;

                pnlReports.Children.Remove(lblTemp);
                DisplayReports();
            }
        }

        public void DisplayReports()// Displays the list of reports in the UI
        {
            pnlReports.Children.Clear(); // Clear any existing content in the reports panel

            var currentNode = lstReports!.Head; // Get the head node of the linked list
            int count = 0;

            while (currentNode != null) // Iterate through the list of reports and display each one
            {
                count++;
                Report report = currentNode.Value;

                ReportLayout reportLayout = new ReportLayout(report);
                reportLayout.ReportCanceled += ReportLayout_ReportCanceled;

                Border border = new Border();

                if (count % 2 == 1)
                {
                    reportLayout.grdLayout.Background = (SolidColorBrush)(new BrushConverter().ConvertFrom("#EDC0DB"));
                } else
                {
                    reportLayout.grdLayout.Background = (SolidColorBrush)(new BrushConverter().ConvertFrom("#E9EBED"));
                }

                border.Child = reportLayout;

                // Add the entire report panel to the reports panel
                pnlReports.Children.Add(border);

                // Move to the next report in the linked list
                currentNode = currentNode.Next;
            }
        }

        public void ReportLayout_ReportCanceled(object sender, Report eventReport)
        {
            var currentNode = lstReports!.Head; // Get the head node of the linked list
            Report report = new Report();

            while (currentNode != null)  // Find the report in the list with the matching ID
            {
                if (currentNode.Value.reportID == eventReport.reportID)
                {// If the report ID matches the button's UID, store the report
                    report = currentNode.Value;
                }

                currentNode = currentNode.Next;
            }

            if (report != null)// If a matching report is found, remove it
            {
                RemoveReport(report);
            }
        }

        public void RemoveReport(Report report)// Removes the given report from the list and refreshes the display
        {
            lstReports.Remove(report); // Remove the report from the linked list
            DisplayReports(); // Refresh the report display to reflect the change
        }
    }
}
