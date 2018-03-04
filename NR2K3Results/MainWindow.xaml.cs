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

namespace NR2K3Results
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private List<Driver> drivers;
        private OpenFileDialog resultFile;

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
                NR2K3ResultParser.ResultParser.Parse(ref drivers, resultFile.FileName);
                drivers.Sort();
                PDFGeneration.PDFGenerators.OutputPracticePDF(drivers, null, null, null);
            }
        }
    }
}
