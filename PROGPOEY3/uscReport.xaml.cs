using Microsoft.Win32;
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
    /// Interaction logic for uscReport.xaml
    /// </summary>
    public partial class uscReport : UserControl
    {
        public event EventHandler<ReportAddedEventArgs> ReportAdded;
        List<Report> reportQueue = new List<Report>();
        public List<string> filesAdded = new List<string>();

        public uscReport(List<Report>? reports = null)
        {
            InitializeComponent();

            cmbCategory.ItemsSource = IssueCategories.Categories;

            if (reports != null)
            {
                reportQueue = reports;
            }
        }

        private void UploadPanel_Drop(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                // Note that you can have more than one file.
                string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);

                filesAdded.AddRange(files);
            }

            RefreshPanelUpload();
        }

        private void btnSubmit_Click(object sender, RoutedEventArgs e)
        {
            string location = txtLocation.Text;
            string category = cmbCategory.Text;
            string description = GetStringFromRichTextBox(rtxDescription);

            if (!string.IsNullOrEmpty(location) && !string.IsNullOrEmpty(category) && !string.IsNullOrEmpty(description))
            {
                Report newReport = new Report(location, category, description, new List<string>(filesAdded));
                reportQueue.Add(newReport);
                ReportAdded?.Invoke(this, new ReportAddedEventArgs(newReport));

                
                
                MessageBox.Show("Report created successfully!", "Success", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                txtLocation.Clear();
                cmbCategory.Text = "";
                rtxDescription.Document.Blocks.Clear();
                RefreshPanelUpload();

                filesAdded.Clear();
            } else MessageBox.Show("Not all fields have been completed", "Missing Fields",MessageBoxButton.OK, MessageBoxImage.Error);
        }

        private void pnlUpload_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            OpenFileDialog fileDialog = new OpenFileDialog();

            fileDialog.ShowDialog();

            filesAdded.Add(fileDialog.FileName);

            RefreshPanelUpload();
        }

        string GetStringFromRichTextBox(RichTextBox rtb)
        {
            TextRange textRange = new TextRange(
                // TextPointer to the start of content in the RichTextBox.
                rtb.Document.ContentStart,
                // TextPointer to the end of content in the RichTextBox.
                rtb.Document.ContentEnd
            );

            // The Text property on a TextRange object returns a string
            // representing the plain text content of the TextRange.
            return textRange.Text;
        }

        public void RefreshPanelUpload()
        {
            pnlUpload.Children.Clear();

            foreach (string file in filesAdded)
            {
                pnlUpload.Children.Add(new Label
                {
                    Content = file
                });
            }
        }

        public void ResetPanelUpload()
        {
            filesAdded.Clear();

            pnlUpload.Children.Clear();
            pnlUpload.Children.Add(new Image
            {
                Source = new BitmapImage(new Uri("Images/DDUpload.png", UriKind.Relative))
            });
        }
    }

    public class ReportAddedEventArgs : EventArgs 
    {
        public Report Report;

        public ReportAddedEventArgs(Report report)
        {
            Report = report;
        }
    }
}
