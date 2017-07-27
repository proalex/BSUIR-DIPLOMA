using System;
using System.Diagnostics;
using System.Net;
using System.Net.Cache;

namespace Generator
{
    public static class RequestMaker
    {
        public static long MakeWebRequest(int timeout, string URL)
        {
            if (URL == null)
            {
                throw new NullReferenceException("URL is null");
            }
            if (timeout < 1)
            {
                throw new ArgumentException("timeout < 1");
            }

            Uri uri = new Uri(URL);

            HttpWebRequest request = null;

            request = WebRequest.CreateHttp(uri);
            request.Timeout = timeout;
            request.CachePolicy = new HttpRequestCachePolicy(HttpRequestCacheLevel.BypassCache);
            var watch = Stopwatch.StartNew();
            WebResponse responce = request.GetResponse();

            watch.Stop();
            responce.Close();
            return watch.ElapsedMilliseconds;
        }
    }
}
