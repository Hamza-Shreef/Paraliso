using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
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

namespace Paraliso
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private ImageProcessor _imgProcessor;
        private int _counter = 0;

        CancellationTokenSource cts = new CancellationTokenSource();

        public MainWindow()
        {
            InitializeComponent();
          

            string outputDirectory = @"D:\Collection\Special\Faculty\Senior\Parallel\Practical\Output";
            string inputDirectory = @"D:\Collection\Special\Faculty\Senior\Parallel\Practical\Brain Tumor Dataset\Testing\notumor";
            _imgProcessor = new ImageProcessor(inputDirectory,outputDirectory);
        }

        private async void executeSync_Click(object sender, RoutedEventArgs e)
        {
            resetView();
           

            Progress<ProgressReportModel> progress = new Progress<ProgressReportModel>();
            progress.ProgressChanged += ReportProgress;

            var watch = System.Diagnostics.Stopwatch.StartNew();

            await _imgProcessor.SobelFilterSerial(progress);

            watch.Stop();
            var elapsedMs = watch.ElapsedMilliseconds;

            resultsWindow.Text += $"Total [Serial] execution time: {elapsedMs}";
        }

        void resetView()
        {
            //clear the progress data
            _counter = 0;
            resultsWindow.Text = "";
            dashboardProgress.Value = 0;
        }

        private async void executeParallelAsync_Click(object sender, RoutedEventArgs e)
        {
            resetView();


            Progress<ProgressReportModel> progress = new Progress<ProgressReportModel>();
            progress.ProgressChanged += ReportProgress;

            var watch = System.Diagnostics.Stopwatch.StartNew();

            await _imgProcessor.SobelFilterParallel(progress);

            watch.Stop();
            var elapsedMs = watch.ElapsedMilliseconds;

            resultsWindow.Text += $"Total [Parallel] execution time: {elapsedMs}";
        }

        private void cancelOperation_Click(object sender, RoutedEventArgs e)
        {
            cts.Cancel();
        }


        private void ReportProgress(object sender, ProgressReportModel e)
        {
            dashboardProgress.Value = e.PercentageComplete;
            var item = e.ProcessedImages[_counter];

            resultsWindow.Text += $" {item.FileName} Processed On Thread: [{item.ProcessingThreadId}] output size {item.SizeInPixels} pixels.{Environment.NewLine}";
            _counter++;
        }

        private async void executeAsync_Click(object sender, RoutedEventArgs e)
        {
            resetView();


            Progress<ProgressReportModel> progress = new Progress<ProgressReportModel>();
            progress.ProgressChanged += ReportProgress;

            var watch = System.Diagnostics.Stopwatch.StartNew();

            await _imgProcessor.SobelFilterAsync(progress);

            watch.Stop();
            var elapsedMs = watch.ElapsedMilliseconds;

            resultsWindow.Text += $"Total [Async] execution time: {elapsedMs}";
        }
       
    }
}
