using System;
using System.Diagnostics;
using System.Threading;

namespace Generator
{
    public static class TaskPerformer
    {
        public static void Perform(CancellationTokenSource token, string[] URLs, int timeout, int requestDuration)
        {
            if (timeout < 0)
            {
                throw new ArgumentException("timeout below zero");
            }
            if (requestDuration < 0)
            {
                throw new ArgumentException("requestDuration below zero");
            }
            if (token == null)
            {
                throw new NullReferenceException("token is null");
            }
            if (URLs == null)
            {
                throw new NullReferenceException("URLs is null");
            }

            Random random = new Random();
            int number = random.Next(URLs.Length);

            while (!token.IsCancellationRequested)
            {
                var watch = Stopwatch.StartNew();

                try
                {
                    RequestMaker.MakeWebRequest(timeout, URLs[number]);
                }
                catch (Exception) { }

                watch.Stop();
                number++;
                number %= URLs.Length;

                int elapsed = (int)watch.ElapsedMilliseconds;

                if  (elapsed < requestDuration)
                {
                    Thread.Sleep(requestDuration - elapsed);
                }
            }
        }
    }
}
