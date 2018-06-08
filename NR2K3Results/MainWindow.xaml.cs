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
using System.Text.RegularExpressions;
using HtmlAgilityPack;

namespace NR2K3Results
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private List<Driver> drivers;
        private string track = "";
        private decimal trackLength;
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
            
            if (openFile.ShowDialog() == true)
            {
                string[] filePath = openFile.FileName.Split('\\');
                RosterFileTextBox.Text = filePath[filePath.Length - 1];
                drivers = CarFileParser.CarFileParser.GetRosterDrivers(System.IO.Path.GetDirectoryName(openFile.FileName), openFile.FileName);              
                OpenResultButton.IsEnabled = true;
            }
            
        }

        private void Open_Result(object sender, RoutedEventArgs e)
        {
            OpenFileDialog resultFile = new OpenFileDialog
            {
                Filter = "HTML Files (*.html)|*.html"
            };

            if (resultFile.ShowDialog() == true)
            {
                string session = "Happy Hour";
                string[] filePath = resultFile.FileName.Split('\\');
                ResultFileTextBox.Text = filePath[filePath.Length - 1];
                GetTrackData(resultFile.FileName);
                NR2K3ResultParser.ResultParser.Parse(ref drivers, resultFile.FileName, ref session, trackLength);
                drivers.Sort();
                PDFGeneration.PracticePDFGenerators.OutputPDF(drivers, "Monster Energy NASCAR Cup Series", "Happy Hour", "Pennzoil 400", track);
            } 
        }

        private void GetTrackData(string filepath)
        {
            string path = @filepath;
            string line;
            StreamReader file = new StreamReader(path);
            while((line = file.ReadLine()) != null)
            {
                if (line.Contains("Track: "))
                {
                    track = line.Split(':')[1].Trim();               
                    break;
                }
            }
            //gets imports/exports path
            path = Directory.GetParent(path).FullName;

            //gets NR2003 root directory
            path = Directory.GetParent(path).FullName;

            //goes to tracks
            path += "\\tracks";

            //gets all directories in that folder, i.e. the tracks
            string[] tracks = Directory.GetDirectories(path);
            bool trackFound = false;

            foreach (string track in tracks)
            {
                try
                {
                    file = new StreamReader(track + "\\track.ini");

                    while ((line = file.ReadLine()) != null)
                    {
                        string[] splitLine = line.Split('=');
                        if (splitLine[0].Trim().Equals("track_name"))
                        {
                            //if this is not the track we want, move on to the next folder
                            if (!splitLine[1].Trim().Equals(this.track))
                            {
                                break;
                            }
                            else
                                trackFound = true;
                        }
                        else if (splitLine[0].Trim().Equals("track_length"))
                        {
                            string length = splitLine[1];
                            length = Regex.Replace(length, "[^0-9.]", "");
                            trackLength = Convert.ToDecimal(length);
                        }
                        else if (splitLine[0].Trim().Equals("track_length_n_type"))
                        {

                        }
                        else if (splitLine[0].Trim().Equals("track_city"))
                        {

                        }
                        else if (splitLine[0].Trim().Equals("track_state"))
                        {
                            
                        }
                    }
                    //if we found the track, we should stop there
                    if (trackFound)
                        break;
                } catch (IOException e)
                {
                    //just means we hit a folder without a track.ini file, such as the "shared" folder
                    continue;
                }
                
            }

            TextInfo textInfo = new CultureInfo("en-US", false).TextInfo;

            track = textInfo.ToTitleCase(track.ToLower());
        }

    }
}
