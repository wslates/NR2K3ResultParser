using CarFileParser;
using NR2K3Results.DriverAndResults;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
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
using System.Windows.Shapes;
using System.Globalization;

namespace NR2K3Results
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private List<Driver> drivers;
        private OpenFileDialog resultFile;
        private string track = "";
        private decimal trackLength = 0;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void Open_Roster(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFile = new OpenFileDialog
            {
                Filter = ".lst Files (*.lst)|*.lst"
            };
            
            if (openFile.ShowDialog()==true)
            {
                string[] filePath = openFile.FileName.Split('\\');
                RosterFileTextBox.Text = filePath[filePath.Length - 1];
                drivers = CarFileParser.CarFileParser.GetRosterDrivers(System.IO.Path.GetDirectoryName(openFile.FileName), openFile.FileName);
                
                OpenResultButton.IsEnabled = true;
            }
            
        }

        private void Open_Result(object sender, RoutedEventArgs e)
        {
            resultFile = new OpenFileDialog
            {
                Filter = "HTML Files (*.html)|*.html"
            };

            if (resultFile.ShowDialog() == true)
            {
                string[] filePath = resultFile.FileName.Split('\\');
                ResultFileTextBox.Text = filePath[filePath.Length - 1];
                NR2K3ResultParser.ResultParser.Parse(ref drivers, resultFile.FileName, trackLength);
                drivers.Sort();
                PDFGeneration.PDFGenerators.OutputPracticePDF(drivers, null, null, null, track);
            }
        }

        private void OpenTrack(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFile = new OpenFileDialog
            {
                Filter = ".ini Files (*.ini)|*.ini"
            };

            if (openFile.ShowDialog() == true)
            {
                string[] filePath = openFile.FileName.Split('\\');
                TrackFileTextBox.Text = filePath[filePath.Length - 1];
                
                try
                {
                    var lines = System.IO.File.ReadLines(openFile.FileName).Take(20);
                    foreach (string line in lines)
                    {
                        if (line.Split('=')[0].Replace(" ", string.Empty).Equals("track_name"))
                        {
                            track = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(line.Split('=')[1].ToLower());
                            
                        } else if (line.Split('=')[0].Replace(" ", string.Empty).Equals("track_length"))
                        {
                            trackLength = Convert.ToDecimal(line.Split('=')[1].Replace(" ", string.Empty).Replace("m", string.Empty));
                        }
                    }

                } catch (Exception ex)
                {

                }
               
            }
        }
    }
}
