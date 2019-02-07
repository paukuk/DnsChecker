using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.WindowsAPICodePack.Dialogs;

namespace DnsChecker
{
    public partial class Form1 : Form
    {
        Excel excel;
        Dictionary<int, string> dns_dic = new Dictionary<int, string>();
        Dictionary<int, string> ns_dic = new Dictionary<int, string>();
        TimeSpan requestInterval = new TimeSpan(0, 0, 60);
        WebScraper scraper = new WebScraper();

        string urlToScrape = "https://dns-api.org/ns/";

        public Form1()
        {
            InitializeComponent();
        }

        private void UploadButton_Click(object sender, EventArgs e)
        {
            CommonOpenFileDialog dialog = new CommonOpenFileDialog();
            // Change hardcoded initial directory!!!!!!
            dialog.InitialDirectory = DnsChecker.Properties.Settings.Default.CommonFileDialogLastLocation;
            Console.WriteLine("dialog.InitialDirectory: {0}", dialog.InitialDirectory);
            if (dialog.ShowDialog() == CommonFileDialogResult.Ok)
            {
                Console.WriteLine("dialog ok");
                string folderPath = System.IO.Path.GetDirectoryName(dialog.FileName);
                DnsChecker.Properties.Settings.Default.CommonFileDialogLastLocation = folderPath;


                //excel = new Excel(dialog.FileName, 1);
                RunApp(dialog);
            }
            else
            {
                Console.WriteLine("dialog not ok!!!");
            }
        }

        private void RunApp(CommonOpenFileDialog dialog)
        {
            Task temp = DoWorkAsyncInfiniteLoop(dialog);
        }

        private async Task DoWorkAsyncInfiniteLoop(CommonOpenFileDialog dialog)
        {
            while (true)
            {
                DateTime lastRequestTime = DnsChecker.Properties.Settings.Default.LastProcessedRequest;
                DateTime currTime = DateTime.Now;
                TimeSpan timeDiff = currTime - lastRequestTime;

                Console.WriteLine("lastRequestTime: {0}", lastRequestTime);
                Console.WriteLine("currTime: {0}", currTime);
                Console.WriteLine("timeDiff: {0}", timeDiff);
                                
                //TimeSpan requestInterval = new TimeSpan(0, 0, 30);

                int compareResult = TimeSpan.Compare(timeDiff, requestInterval);
                if (compareResult == 1)
                {
                    Console.WriteLine("Executing");
                    DnsChecker.Properties.Settings.Default.LastProcessedRequest = DateTime.Now;
                    Properties.Settings.Default.Save();
                }
                else
                {
                    Console.WriteLine("Not Executing!!");
                }
                excel = new Excel(dialog.FileName, 1);
                excel.ReadCells(dns_dic);
                Console.WriteLine("count of dns_dic{0}", dns_dic.Count);
                scraper.Scrape(urlToScrape, dns_dic, ns_dic);
                excel.WriteToCells(2, ns_dic);
                excel.Save();
                excel.FreeExcelResources();


                //await Task.Delay(30000);
                //await Task.Delay(new TimeSpan(0, 0, 30));
                await Task.Delay(requestInterval);
            }
        }
    }
}
