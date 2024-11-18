using Microsoft.Win32;
using PROGPOEY3.Data;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
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

namespace PROGPOEY3
{
    /// <summary>
    /// Interaction logic for uscReport.xaml
    /// </summary>
    public partial class uscReport : UserControl
    {
        public event EventHandler<ReportAddedEventArgs> ReportAdded;
        Data.LinkedList<Report> lstReports = new Data.LinkedList<Report>();
        public List<string> filesAdded = new List<string>();
        bool isLocation, isDescription, isCategory = false;
        string[] locations = 
        {
            "Summerstrand",
            "Walmer",
            "Humewood",
            "Newton Park",
            "Central",
            "Mill Park",
            "Mount Croix",
            "Parson's Hill",
            "Greenacres",
            "Richmond Hill",
            "Algoa Park",
            "Charlo",
            "Lorraine",
            "Fairview",
            "Bluewater Bay",
            "South End",
            "Gelvandale",
            "Schauderville",
            "Cotswold",
            "Kabega Park",
            "Westering",
            "Theescombe",
            "Seaview",
            "Motherwell",
            "Bethelsdorp",
            "Vanes Estate",
            "Fairbridge Heights",
            "Strelitzia Park",
            "Penford",
            "Rosedale",
            "KwaNobuhle",
            "Despatch",
            "Bothasrus",
            "Heuwelkruin",
            "Azalea Park",
            "Campher Park",
            "Retief"
        };


        public uscReport(Data.LinkedList<Report>? reports = null)
        {
            InitializeComponent();

            ResetPanelUpload(); //Reset panel on user control initialisation
            isLocation = isDescription = isCategory = false; //Resetting progress bar

            cmbCategory.ItemsSource = IssueCategories.Categories; //Populating categories combo box

            if (reports != null)//Null check for reportTree
            {
                lstReports = reports;
            }
        }

        private void UploadPanel_Drop(object sender, DragEventArgs e) //Method allowing drag & drop for files
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                string[] files = (string[])e.Data.GetData(DataFormats.FileDrop); //Get file path from dropped file

                foreach (string path in files)
                {
                    filesAdded.Add(UploadFile(path));
                }
            }

            RefreshPanelUpload(); //Refresh panel to include dropped file
        }

        private void btnSubmit_Click(object sender, RoutedEventArgs e)
        {
            string location = txtLocation.Text;
            string category = cmbCategory.Text;
            string description = GetStringFromRichTextBox(rtxDescription);

            if (!string.IsNullOrEmpty(location) && !string.IsNullOrEmpty(category) && !string.IsNullOrEmpty(description)) //Null checks 
            {
                Report newReport = new Report(location, category, description, new List<string>(filesAdded));
                lstReports.Add(newReport);
                ReportAdded?.Invoke(this, new ReportAddedEventArgs(newReport)); // Invoke ReportAdded event

                //Clear all components on submit
                MessageBox.Show($"Report created successfully!\nYour report number is: {newReport.reportID}", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                txtLocation.Clear();
                cmbCategory.Text = "";
                rtxDescription.Document.Blocks.Clear();
                isLocation = isDescription = isCategory = false; //Resetting progress bar
                CalculateProgress();
                ResetPanelUpload();
            } else MessageBox.Show("Not all fields have been completed", "Missing Fields", MessageBoxButton.OK, MessageBoxImage.Error);
        }

        private void pnlUpload_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)// Handling if the user clicks on the upload panel
        {
            OpenFileDialog fileDialog = new OpenFileDialog();

            fileDialog.ShowDialog();

            if (!string.IsNullOrEmpty(fileDialog.FileName))
            {
                filesAdded.Add(UploadFile(fileDialog.FileName));

                RefreshPanelUpload();
            }
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

        public void RefreshPanelUpload()// Refreshing upload panel ran when a file is uploaded
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

        public void ResetPanelUpload()//Clearing upload panel and replacing image
        {
            filesAdded.Clear();

            pnlUpload.Children.Clear();
            pnlUpload.Children.Add(new Image
            {
                Source = new BitmapImage(new Uri("Images/Upload.png", UriKind.Relative))
            });
        }

        private void txtLocation_TextChanged(object sender, TextChangedEventArgs e) //Checking for text in the textbox
        {
            if (!string.IsNullOrWhiteSpace(txtLocation.Text))
            {
                var location = txtLocation.Text;

                lstLocations.ItemsSource = locations.Where(x => x.ToLower().StartsWith(location.ToLower()) || x.ToLower().Contains(location.ToLower())).OrderBy(x => x.ToLower().IndexOf(location.ToLower())).Take(5).ToList();
                
                isLocation = true;
                CalculateProgress();
            } else
            {
                isLocation = false;
                CalculateProgress();
            }

            if (lstLocations.ItemsSource != null) 
            {
                lstLocations.Visibility = Visibility.Visible;
            }else
            {
                lstLocations.Visibility= Visibility.Collapsed;
            }
        }

        private void cmbCategory_SelectionChanged(object sender, SelectionChangedEventArgs e)//Checking wether a category is selected
        {
            isCategory = true;
            CalculateProgress();
        }

        private void rtxDescription_TextChanged(object sender, TextChangedEventArgs e)//Checking for text in the rich textbox
        {
            if (!string.IsNullOrEmpty(GetStringFromRichTextBox(rtxDescription)) && !string.IsNullOrWhiteSpace(GetStringFromRichTextBox(rtxDescription)))
            {
                isDescription = true;
                CalculateProgress();
            } else
            {
                isDescription = false;
                CalculateProgress();
            }
        }

        private void CalculateProgress()//Calculate user progress in reporting 
        {
            if (pbProgress != null)
            {
                int progress = GetTrueCountValue(isDescription, isLocation, isCategory);
                pbProgress.Value = ((double)progress / 3) * 100;//Update progress bar
            }

        }

        private void lstLocations_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (lstLocations.SelectedItem != null)
            {
                txtLocation.Text = lstLocations.SelectedItem.ToString();
                lstLocations.Visibility = Visibility.Collapsed;
            }  
        }

        public int GetTrueCountValue(bool a, bool b, bool c)
        {
            // Count the number of true values
            int trueCount = 0;

            if (a) trueCount++;
            if (b) trueCount++;
            if (c) trueCount++;

            // Return an integer Value based on the number of true values
            // You can modify the return values based on your requirements
            switch (trueCount)
            {
                case 0: return 0; // No true values
                case 1: return 1; // One true Value
                case 2: return 2; // Two true values
                case 3: return 3; // All are true
                default: return -1; // Should not happen
            }
        }

        private string UploadFile(string filePath)
        {
            var newDestination = Environment.CurrentDirectory;
            string fileName = Path.GetFileName(filePath);
            string newPath = Path.Combine(newDestination, "Uploads");

            if (!Directory.Exists(newPath))
            {
                Directory.CreateDirectory(newPath);
            }

            string newFullPath = Path.Combine(newPath, fileName);

            while (File.Exists(newFullPath))
            {
                var array = fileName.Split('.');
                string changedName = array[0] + " - copy." + array[1];
                newFullPath = Path.Combine(newPath, changedName);
                fileName = changedName;
            }

            File.Copy(filePath, newFullPath);

            return fileName;
        }
    }

    public class ReportAddedEventArgs : EventArgs //Custom event arguments to pass data back to the mainwindow
    {
        public Report Report;

        public ReportAddedEventArgs(Report report)
        {
            Report = report;
        }
    }
}
