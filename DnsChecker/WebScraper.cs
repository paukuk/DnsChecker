using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HtmlAgilityPack;
using System.Net;
using System.IO;
using Newtonsoft.Json;

namespace DnsChecker
{
    class WebScraper
    {
        HtmlWeb web = new HtmlWeb();
        public void Scrape(string url, Dictionary<int, string> dic, Dictionary<int, string> outDic)
        {
            int debug_request_cnt = 0;
            foreach (var item in dic)
            {
                var cookieContainer = new CookieContainer();
                string url_to_pass;
                if (!item.Value.EndsWith("/"))
                {
                    url_to_pass = url + item.Value + "/";
                }
                else
                {
                    url_to_pass = url + item.Value;
                }
                HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(url_to_pass);
                Console.WriteLine("debug_request_cnt {0}", debug_request_cnt++);
                request.CookieContainer = cookieContainer;
                request.AutomaticDecompression = DecompressionMethods.Deflate | DecompressionMethods.GZip;
                //set the user agent and accept header values, to simulate a real web browser
                request.UserAgent = "Mozilla/5.0 (Windows NT 10.0; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/45.0.2454.101 Safari/537.36";
                request.Accept = "application/json";
                try
                {
                    using (WebResponse response = request.GetResponse())
                    {
                        using (StreamReader sr = new StreamReader(response.GetResponseStream()))
                        {
                            var nsList = JsonConvert.DeserializeObject<List<NsInfo>>(sr.ReadToEnd());
                            if (nsList[0].value != null)
                            {
                                string nsfield = nsList[0].value.ToString();
                                if (nsfield != null)
                                {
                                    outDic.Add(item.Key, nsfield);
                                }
                                else
                                {
                                    outDic.Add(item.Key, String.Empty);
                                }
                            }
                        }
                    }
                }
                catch (System.Net.WebException)
                {
                    outDic.Add(item.Key, String.Empty);
                    continue;
                }
            }
            dic.Clear();
        }
    }

    public class NsInfo
    {
        [JsonProperty("name")]
        public string name { get; set; }
        [JsonProperty("ttl")]
        public string ttl { get; set; }
        [JsonProperty("type")]
        public string type { get; set; }
        [JsonProperty("value")]
        public string value { get; set; }
    }
}
