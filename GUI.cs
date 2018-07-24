using System;
using System.IO;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Text;
using System.Windows.Forms.DataVisualization.Charting;
using System.Security.Cryptography;

namespace Tumultu_
{
    public static class GUI
    {
        /// <summary>
        ///     Function to "open" file
        /// </summary>
        /// <param name="f">
        ///     Out parameter, FileInfo object that contains basic info about file
        /// </param>
        /// <returns>If file was opened correctly, returns TRUE. Else, it throws exception.</returns>
        public static bool OpenFile(out FileInfo f)
        {
            OpenFileDialog ofd = new OpenFileDialog
            {
                InitialDirectory = "C:\\",
                Filter = "All Files (*.*)|*.*",
                FilterIndex = 1,
                RestoreDirectory = true
            };

            if (ofd.ShowDialog() == DialogResult.OK)
            {
                f = new FileInfo(ofd.FileName);
                return true;
            }
            else
            {
                throw new Exception("No File Chosen!");
            }     
        }

        /// <summary>
        ///     Running down all tasks
        /// </summary>
        /// <param name="samples">
        ///     The number of parts we want to divide file
        /// </param>
        /// <param name="filePath">
        ///     Path to the file we want to work on
        /// </param>
        /// <param name="sb">
        ///     Out parameter, StringBuilder containing Entropy values of each sample
        /// </param>
        /// <param name="chart">
        ///     Reference type parameter, Chart we are working on
        /// </param>
        public static void GoButton(int samples, string filePath, out StringBuilder sb, ref Chart chart)
        {
            FileInfo fInfo = new FileInfo(filePath);
            Entropy ent = new Entropy(fInfo, samples);
            List<double> entropies = ent.EntropyList;
            if(chart.Series.Count > 0) chart.Series.RemoveAt(0);
            chart.Series.Add("values");
            chart.Series["values"].ChartType = SeriesChartType.Column;
            sb = new StringBuilder();
            for(int i = 0; i < entropies.Count; i++)
            {
                sb.Append(entropies[i].ToString() + "\n");
                chart.Series["values"].Points.AddY(entropies[i]);
            }
            chart.ChartAreas[0].AxisX.ScaleView.Zoomable = true;
            chart.ChartAreas[0].AxisY.ScaleView.Zoomable = true;
            chart.MouseWheel += (o, e) =>
            {
                var chartx = (Chart)o;
                var xAxis = chartx.ChartAreas[0].AxisX;
                var yAxis = chartx.ChartAreas[0].AxisY;

                try
                {
                    if (e.Delta < 0)
                    {
                        xAxis.ScaleView.ZoomReset();
                        yAxis.ScaleView.ZoomReset();
                    }
                    else if (e.Delta > 0)
                    {
                        var xMin = xAxis.ScaleView.ViewMinimum;
                        var xMax = xAxis.ScaleView.ViewMaximum;
                        var yMin = yAxis.ScaleView.ViewMinimum;
                        var yMax = yAxis.ScaleView.ViewMaximum;

                        var posXStart = xAxis.PixelPositionToValue(e.Location.X) - (xMax - xMin) / 16;
                        var posXFinish = xAxis.PixelPositionToValue(e.Location.X) + (xMax - xMin) / 16;
                        var posYStart = yAxis.PixelPositionToValue(e.Location.Y) - (yMax - yMin) / 16;
                        var posYFinish = yAxis.PixelPositionToValue(e.Location.Y) + (yMax - yMin) / 16;

                        xAxis.ScaleView.Zoom(posXStart, posXFinish);
                        yAxis.ScaleView.Zoom(posYStart, posYFinish);
                    }
                }
                catch { }
            };
        }

        /// <summary>
        ///     Counting checksums
        /// </summary>
        /// <param name="path">File path</param>
        /// <param name="s1">MD5</param>
        /// <param name="s2">SHA1</param>
        /// <param name="s3">SHA256</param>
        public static void HashUpdate(string path, ref string s1, ref string s2, ref string s3)
        {
            using (var md5 = MD5.Create())
            {
                using (var stream = File.OpenRead(path))
                {
                    s1 = BitConverter.ToString(md5.ComputeHash(stream)).Replace("-", "").ToLower();
                }
            }
            using (var sha1 = new SHA1CryptoServiceProvider())
            {
                using (var stream = File.OpenRead(path))
                {
                    s2 = BitConverter.ToString(sha1.ComputeHash(stream)).Replace("-", "").ToLower();
                }
            }
            using (var sha256 = new SHA256CryptoServiceProvider())
            {
                using (var stream = File.OpenRead(path))
                {
                    s3 = BitConverter.ToString(sha256.ComputeHash(stream)).Replace("-", "").ToLower();
                }
            }

        }
    }
}
