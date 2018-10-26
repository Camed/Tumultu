using System.IO;
using VirusTotalNET;
using System.Threading.Tasks;
using VirusTotalNET.Results;
using System.Text;
using VirusTotalNET.ResponseCodes;

namespace Tumultu_x
{
    public class VirusTotalParser
    {
        private VirusTotal vt;


        /// <summary>
        ///  Basic constructor for VirusTotalParser class
        /// </summary>
        /// <param name="apiKey">
        ///     Your API key that you can aquire from virustotal webpage
        /// </param>
        public VirusTotalParser(string apiKey)
        {
            vt = new VirusTotal(apiKey);
            vt.UseTLS = true;
        }

        public async Task<string> GetReport(FileInfo fileInfo)
        {
            FileReport report = await vt.GetFileReportAsync(fileInfo);
            StringBuilder sb = new StringBuilder();

            sb.Append($"Seen before: {(report.ResponseCode == FileReportResponseCode.Present ? true : false)}\n");
            sb.Append($"Date: {report.ScanDate.ToLongTimeString()} {report.ScanDate.ToLongDateString()}\n");
            sb.Append($"Positives: {report.Positives}\n");
            sb.Append($"Results:\n");
            foreach(var s in report.Scans)
            {
                sb.Append(s.Key + "\n");
            }
            return sb.ToString();
            
        }
    }
}
