using ImageMagick;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Web;

// PdfSvc - Converts PDF files to and from various file formats
// Copyright (C) 2014  Anand Khedkar - Ted Spence

// This program is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.

// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.

// You should have received a copy of the GNU General Public License
// along with this program.  If not, see <http://www.gnu.org/licenses/>.


namespace PdfSvc.Helpers
{
    public class FileConverter : IFileConverter
    {

        public byte[] PngToPdf(byte[] pngImageBytes)
        {
            using (MemoryStream memStream = new MemoryStream())
            {
                using (MagickImage image = new MagickImage(pngImageBytes))
                {
                    // Sets the output format to Pdf
                    image.Format = MagickFormat.Pdf;

                    // Convert the pdf to ByteArray
                    pngImageBytes = image.ToByteArray();
                }
            }

            return pngImageBytes;
        }

        /// <summary>
        /// Convert a PDF to a series of PNG files
        /// </summary>
        /// <param name="pdfFileBytes">Raw bytes of the input file</param>
        /// <param name="dpi">Dots per inch for the final file, defaulting to 300</param>
        /// <returns></returns>
        public List<byte[]> PdfToPng(byte[] pdfFileBytes, int dpi = 300)
        {
            List<byte[]> results = new List<byte[]>();

            // Command Line Pdf Converter Version
            var guid = Guid.NewGuid();
            string guidPrefix = guid.ToString();
            string tempDirectory = Path.GetTempPath();
            try
            {
                // Save this file to the temp folder
                string baseFilename = Path.Combine(tempDirectory, guidPrefix + ".pdf");
                File.WriteAllBytes(baseFilename, pdfFileBytes);
                string cmdline = System.Web.HttpContext.Current.Server.MapPath(ConfigurationManager.AppSettings["PdfConversionCommand"]);
                string cmdparams = ConfigurationManager.AppSettings["PdfConversionParameters"]
                    .Replace("@GUID@", guidPrefix)
                    .Replace("@DPI@", dpi.ToString());

                // Start command line process
                RunCommand(cmdline, cmdparams, tempDirectory);

                // Look for the results
                foreach (var f in Directory.GetFiles(tempDirectory, guidPrefix + "*")) {
                    var b = File.ReadAllBytes(f);
                    results.Add(b);
                    File.Delete(f);
                }

                // Here are your results mr caller sir
                return results;

            } catch (Exception ex) {
                Console.WriteLine(ex.Message);

            // No matter what we do, make sure we clean up all files matching this Guid in the temp directory
            } finally {
                foreach (var f in Directory.GetFiles(tempDirectory, guidPrefix + "*")) {
                    File.Delete(f);
                }
            }

            // Failed
            return null;
        }

        /// <summary>
        /// Run a command line task and return results
        /// </summary>
        /// <param name="cmdline"></param>
        /// <param name="cmdparams"></param>
        /// <param name="workingDirectory"></param>
        private void RunCommand(string cmdline, string cmdparams, string workingDirectory)
        {
            // Use ProcessStartInfo class
            var startInfo = new ProcessStartInfo();
            startInfo.CreateNoWindow = false;
            startInfo.UseShellExecute = false;
            startInfo.FileName = cmdline;
            startInfo.WindowStyle = ProcessWindowStyle.Normal;
            startInfo.WorkingDirectory = workingDirectory;
            startInfo.Arguments = cmdparams;

            try
            {
                // Start the process with the info we specified.
                // Call WaitForExit and then the using statement will close.
                using (Process exeProcess = Process.Start(startInfo))
                {
                    exeProcess.WaitForExit();
                }
            }
            catch (Exception ex) 
            {
                System.Diagnostics.Debug.WriteLine(ex.ToString());
            }
        }
    }
}