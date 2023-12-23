using AForge.Imaging.Filters;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Paraliso
{
    internal class ImageProcessor
    {
        private readonly string _inputDirectory;
        private readonly string _outputDirectory;

        public ImageProcessor(string inputDirectory,string outputDirectory)
        {
            _inputDirectory = inputDirectory;
            _outputDirectory = outputDirectory;
        }

        public async Task SobelFilterParallel(IProgress<ProgressReportModel> progress)
        {
            string[] imageFiles = Directory.GetFiles(_inputDirectory, "*.jpg");

            int numberOfFiles = imageFiles.Count();
            var output = new List<ImageProcessModel>();
            var report = new ProgressReportModel();

            await Task.Run(() =>
            {
                Parallel.ForEach(imageFiles, imagePath =>
                {
                    // Load the image
                    using (Bitmap originalImage = new Bitmap(imagePath))
                    {

                        using (Bitmap edgeDetectedImage = ApplySobelFilter(originalImage))
                        {
                            // Save the processed image with a new name
                            var fileName = Path.GetFileName(imagePath);
                            string outputFileName = Path.Combine(_outputDirectory, "sobeled " + fileName);
                            edgeDetectedImage.Save(outputFileName, ImageFormat.Jpeg);

                            //outputing progress
                            output.Add(new ImageProcessModel
                            {
                                FileName = fileName,
                                ProcessingThreadId= Thread.CurrentThread.ManagedThreadId ,
                                SizeInPixels = edgeDetectedImage.Width * edgeDetectedImage.Height
                            });
                        }

                    }

                    report.ProcessedImages = output;
                    report.PercentageComplete = output.Count * 100 / numberOfFiles;
                    progress.Report(report);
                    

                });
            });


        }

        public async Task SobelFilterSerial(IProgress<ProgressReportModel> progress)
        {
            string[] imageFiles = Directory.GetFiles(_inputDirectory, "*.jpg");

            int numberOfFiles = imageFiles.Count();
            var output = new List<ImageProcessModel>();
            var report = new ProgressReportModel();


            foreach (var imagePath in imageFiles)
            {
                await Task.Run(() =>
                {

                    // Load the image
                    using (Bitmap originalImage = new Bitmap(imagePath))
                    {

                        using (Bitmap edgeDetectedImage = ApplySobelFilter(originalImage))
                        {
                            // Save the processed image with a new name
                            var fileName = Path.GetFileName(imagePath);
                            string outputFileName = Path.Combine(_outputDirectory, "sobeled " + fileName);
                            edgeDetectedImage.Save(outputFileName, ImageFormat.Jpeg);

                            //outputing progress
                            output.Add(new ImageProcessModel
                            {
                                FileName = fileName ,
                                ProcessingThreadId = Thread.CurrentThread.ManagedThreadId,
                                SizeInPixels = edgeDetectedImage.Width * edgeDetectedImage.Height * 3
                            });
                        }

                    }
                });

                report.ProcessedImages = output;
                report.PercentageComplete = output.Count * 100 / numberOfFiles;
                progress.Report(report);
            }

        }

        public async Task SobelFilterAsync(IProgress<ProgressReportModel> progress)
        {
            Trace.WriteLine($"Before thread-id: {Thread.CurrentThread.ManagedThreadId}");
            
            string[] imageFiles = Directory.GetFiles(_inputDirectory, "*.jpg");

            int numberOfFiles = imageFiles.Count();
            var output = new List<ImageProcessModel>();
            var report = new ProgressReportModel();

            List<Task> tasks = new List<Task>();

            foreach (var imagePath in imageFiles)
            {
                var task = Task.Run(() =>
                {

                    // Load the image
                    using (Bitmap originalImage = new Bitmap(imagePath))
                    {

                        using (Bitmap edgeDetectedImage = ApplySobelFilter(originalImage))
                        {
                            // Save the processed image with a new name
                            var fileName = Path.GetFileName(imagePath);
                            string outputFileName = Path.Combine(_outputDirectory, "sobeled " + fileName);
                            edgeDetectedImage.Save(outputFileName, ImageFormat.Jpeg);

                            //outputing progress
                            output.Add(new ImageProcessModel
                            {
                                FileName = fileName,
                                ProcessingThreadId = Thread.CurrentThread.ManagedThreadId,
                                SizeInPixels = edgeDetectedImage.Width * edgeDetectedImage.Height * 3
                            });
                        }

                    }

                    report.ProcessedImages = output;
                    report.PercentageComplete = output.Count * 100 / numberOfFiles;
                    progress.Report(report);
                });



                tasks.Add(task);
            }
            Trace.WriteLine($"After thread-id: {Thread.CurrentThread.ManagedThreadId}");

            await Task.WhenAll(tasks);
        }


        private static Bitmap ApplySobelFilter(Bitmap originalImage)
        {
            Bitmap compatibleImage = ConvertToCompatibleFormat(originalImage);

            Convolution sobelX = new Convolution(new int[,] { { -1, 0, 1 }, { -2, 0, 2 }, { -1, 0, 1 } });
            Convolution sobelY = new Convolution(new int[,] { { -1, -2, -1 }, { 0, 0, 0 }, { 1, 2, 1 } });

            Bitmap filteredX = sobelX.Apply(compatibleImage);
            Bitmap filteredY = sobelY.Apply(compatibleImage);

            Bitmap filteredImage = new Bitmap(filteredX.Width, filteredX.Height);

            for (int x = 0; x < filteredX.Width; x++)
            {
                for (int y = 0; y < filteredX.Height; y++)
                {
                    Color colorX = filteredX.GetPixel(x, y);
                    Color colorY = filteredY.GetPixel(x, y);

                    int magnitude = (int)Math.Sqrt(colorX.R * colorX.R + colorY.R * colorY.R);

                    magnitude = Math.Min(255, magnitude); 

                    filteredImage.SetPixel(x, y, Color.FromArgb(magnitude, magnitude, magnitude));
                }
            }

            return filteredImage;
        }

        private static Bitmap ConvertToCompatibleFormat(Bitmap originalImage)
        {
            Bitmap compatibleImage = new Bitmap(originalImage.Width, originalImage.Height, PixelFormat.Format24bppRgb);

            using (Graphics g = Graphics.FromImage(compatibleImage))
            {
                g.DrawImage(originalImage, 0, 0, originalImage.Width, originalImage.Height);
            }

            return compatibleImage;
        }

    }
}
