using PROGPOEY3.Data;
using PROGPOEY3.Data.BST;
using PROGPOEY3.Data.Graph;
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
        ReportBST? reportTree = null;
        CategoryGraph? reportGraph = null;

        public uscPending(ReportBST? reportTree = null, CategoryGraph? reportGraph = null)
        {
            InitializeComponent();

            if (reportTree != null)
            {
                this.reportTree = reportTree;
                this.reportGraph = reportGraph;

                pnlReports.Children.Remove(lblTemp);
                DisplayReports();
            }

            string[] statii = { "Pending", "Resolved" };
            cmbCategories.ItemsSource = IssueCategories.Categories;
            cmbCategories.SelectedIndex = 0;
            cmbResolved.ItemsSource = statii;
            cmbResolved.SelectedIndex = 0;
        }

        public void DisplayReports(string? id = null, string? category = null, int? status = null) // Displays the list of reports in the UI
        {
            pnlReports.Children.Clear(); // Clear any existing content in the reports panel

            List<Report> sortedReports = new List<Report>();

            // Get all reports in order from the BST
            if (reportTree != null)
            {
                sortedReports = reportTree!.GetReportsInOrder();
            }
            
            var filteredReports = sortedReports;

            if (id != null)
            {
                filteredReports = filteredReports.Where(x => x.reportID == id || x.reportID.StartsWith(id) || x.reportID.Contains(id)).ToList();
            }

            if (category != null)
            {
                filteredReports = filteredReports.Where(x => x.category.ToLower() == category.ToLower()).ToList();
            }

            if (status != null)
            {
                filteredReports = filteredReports.Where(x => x.status == status).ToList();
            }

            for (int i = 0; i < filteredReports.Count; i++)
            {
                Report report = filteredReports[i];

                // Create the UI layout for each report
                ReportLayout reportLayout = new ReportLayout(report);
                reportLayout.ReportCanceled += ReportLayout_ReportCanceled;

                Border border = new Border();

                // Alternate background colors for visual clarity
                if (i % 2 == 0)
                {
                    reportLayout.grdLayout.Background = (SolidColorBrush)(new BrushConverter().ConvertFrom("#EDC0DB"));
                } else
                {
                    reportLayout.grdLayout.Background = (SolidColorBrush)(new BrushConverter().ConvertFrom("#E9EBED"));
                }

                border.Child = reportLayout;

                // Add the entire report panel to the UI panel
                pnlReports.Children.Add(border);
            }
        }

        public void ReportLayout_ReportCanceled(object sender, Report eventReport)
        {
            // Find the report in the BST with the matching reportID
            Report report = reportTree!.Search(eventReport.reportID);

            if (report != null) // If a matching report is found, remove it
            {
                RemoveReport(report);
            }
        }

        public void RemoveReport(Report report)// Removes the given report from the list and refreshes the display
        {
            reportGraph!.RemoveReport(report.reportID);
            reportTree!.Remove(report.reportID); // Remove the report from the linked list
            DisplayReports(); // Refresh the report display to reflect the change
        }

        private void btnSearch_Click(object sender, RoutedEventArgs e)
        {
            string searchId = txtSearch.Text;

            if (!string.IsNullOrEmpty(searchId))
            {
                pnlReports.Children.Clear();
                DisplayReports(searchId);
            }
        }

        private void btnFilter_Click(object sender, RoutedEventArgs e)
        {
            string category = cmbCategories.Text;
            string resolved = cmbResolved.Text;

            pnlReports.Children.Clear();

            int? status = resolved == "Pending" ? 0 : 1;

            DisplayReports(null, category, status);
        }

        private void txtSearch_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void btnClear_Click(object sender, RoutedEventArgs e)
        {
            DisplayReports(null, null, null);
        }
    }
}
