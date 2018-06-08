
using NR2K3Results.DriverAndResults;
using Microsoft.Win32;
using System;
using NR2K3Results.Parsers;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Microsoft.WindowsAPICodePack;
using System.Windows.Forms;
using Microsoft.WindowsAPICodePack.Dialogs;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Globalization;
using System.Text.RegularExpressions;
using HtmlAgilityPack;
using System.Xml;

namespace NR2K3Results
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private List<Driver> drivers;
        private Track track = new Track();
        private string nr2003Dir;
        private List<string> series = new List<string>();
        public MainWindow()
        {
            InitializeComponent();
            CheckIfSaveFileExists();
        }

        private void CheckIfSaveFileExists()
        {
            string path = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
            path += "\\Series";
            if (Directory.Exists(path))
            {
                List<string> files = new List<string>(Directory.GetFiles(path, "*.xml", SearchOption.AllDirectories));
                if (files.Count==0)
                {
                    Console.WriteLine("Does not exist");
                    System.Windows.MessageBox.Show("No saved series found. Please select your NR2003 folder.", "No Series!", MessageBoxButton.OK, MessageBoxImage.Error);
                    CreateNewSeries();
                } else
                {
                    XmlReader reader;
                    foreach(string file in files)
                    {
                        
                        reader = XmlReader.Create(file);
                        series.Add(file);
                    }
                }
                    

            } else
            {
                System.Windows.MessageBox.Show("No saved series found. Please select your NR2003 folder.", "No Series!", MessageBoxButton.OK, MessageBoxImage.Error);
                CreateNewSeries();
                Directory.CreateDirectory(path);
            }
        }

        private void CreateNewSeries()
        {
            CommonOpenFileDialog dialog = new CommonOpenFileDialog
            {
                InitialDirectory = "C:\\",
                IsFolderPicker = true
            };
            if (dialog.ShowDialog() == CommonFileDialogResult.Ok && !String.IsNullOrWhiteSpace(dialog.FileName))
            {
                nr2003Dir = dialog.FileName;
                
            }

            Microsoft.Win32.SaveFileDialog saveDialog = new Microsoft.Win32.SaveFileDialog
            {
                InitialDirectory = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location) + "\\Series",
                Filter = ".xml Files (*.xml)|.xml"
            };

            if (saveDialog.ShowDialog() == true)
            {
                XmlWriter writer = XmlWriter.Create(saveDialog.FileName);
                writer.WriteStartDocument();
                writer.WriteStartElement("directory");
                writer.WriteString(nr2003Dir);
                writer.WriteEndElement();
                writer.WriteEndDocument();
                writer.Close();
            } 
                
        }

        private void Open_Roster(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.OpenFileDialog openFile = new Microsoft.Win32.OpenFileDialog
            {
                Filter = ".lst Files (*.lst)|*.lst"
            };
            
            if (openFile.ShowDialog() == true)
            {
                string[] filePath = openFile.FileName.Split('\\');
                RosterFileTextBox.Text = filePath[filePath.Length - 1];
                drivers = CarFileParser.GetRosterDrivers(System.IO.Path.GetDirectoryName(openFile.FileName), openFile.FileName);              
                OpenResultButton.IsEnabled = true;
            }
            
        }

        private void Open_Result(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.OpenFileDialog resultFile = new Microsoft.Win32.OpenFileDialog
            {
                Filter = "HTML Files (*.html)|*.html"
            };

            if (resultFile.ShowDialog() == true)
            {
                string session = "Qualifying";
                string[] filePath = resultFile.FileName.Split('\\');
                ResultFileTextBox.Text = filePath[filePath.Length - 1];
                track = TrackParser.Parse(resultFile.FileName);
                ResultParser.Parse(ref drivers, resultFile.FileName, ref session, track.length);
                drivers.Sort();
                track.laps = drivers[1].result.laps;
                PDFGeneration.PracticePDFGenerators.OutputPDF(drivers, "Monster Energy NASCAR Cup Series", session, "Pennzoil 400", track.name);
                //PDFGeneration.RacePDFGenerator.OutputPDF(drivers, "Monster Energy NASCAR Cup Series", "Pennzoil 400", track);
            } 
        }

        public static void Exit()
        {
            if (System.Windows.Forms.Application.MessageLoop)
            {
                // WinForms app
                System.Windows.Forms.Application.Exit();
            }
            else
            {
                // Console app
                System.Environment.Exit(1);
            }
        }

        private void button_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
