using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Xml;
using Newtonsoft.Json;

namespace TasksWithTimeouts
{
    public class ProxyRotatorService
    {
        
        
        public void ChangeProxy()
        {
            WebRequest.DefaultWebProxy = new WebProxy();
            string serviceUri =
                "http://falcon.proxyrotator.com:51337/?apiKey=&get=true";
            string jsonString = string.Empty;
            string proxyAddress = string.Empty;

            HttpWebRequest request = (HttpWebRequest) WebRequest.Create(serviceUri);

            using (HttpWebResponse response = (HttpWebResponse) request.GetResponse())
            using (Stream stream = response.GetResponseStream())
            using (StreamReader reader = new StreamReader(stream ?? throw new InvalidOperationException()))
            {
                jsonString = reader.ReadToEnd();
                var px = JsonConvert.DeserializeObject<prxy>(jsonString);
                proxyAddress = "http://" + ((prxy) px).proxy + "/";
            }
            WebProxy proxyObject = new WebProxy(proxyAddress);
            WebRequest.DefaultWebProxy = proxyObject;

        }
        public async Task WebAccessViaProxyRotatorApi(string url)
        {
            var cts = new CancellationTokenSource();
            cts.CancelAfter(4000);

            // create a request
            HttpWebRequest request = (HttpWebRequest) WebRequest.Create(url);

            using (WebResponse response = await request.GetResponseAsync(cts.Token))
            using (Stream stream = response.GetResponseStream())
            using (StreamReader reader = new StreamReader(stream ?? throw new InvalidOperationException()))
            {
                var jsonString = reader.ReadToEnd();
                Console.WriteLine(getIpAddress(jsonString));
            }
        }

        private string getIpAddress(string source)
        {
            var sPattern =  new Regex(@"<p class='ip-address'>\n?(.*)\n?</p>");
            var result =  sPattern.Match(source);
            return result.Groups.Count > 0 ? result.Groups[1].Value : "ip address not found";
        }
    }

    public class prxy
    {
        public string proxy { get; set; }
        public string ip { get; set; }
        public string port { get; set; }
        public string type { get; set; }
        public int lastChecked { get; set; }
        public bool get { get; set; }
        public bool post { get; set; }
        public bool cookies { get; set; }
        public bool referer { get; set; }
        public bool userAgent { get; set; }
        public string city { get; set; }
        public string state { get; set; }
        public string country { get; set; }
        public int currentThreads { get; set; }
        public int threadsAllowed { get; set; }
    }
}