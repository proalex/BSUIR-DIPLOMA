using ControllerServer.Database;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ControllerServer
{
    public static class ReportsHandler
    {
        public static void SendReports(Client client)
        {
            if (client == null)
            {
                throw new NullReferenceException("client is null");
            }

            Reports reports = new Reports()
            {
                List = new List<Report>()
            };

            using (var db = new DatabaseContainer())
            {
                var queryReports = from r in db.ReportsSet
                                   where r.UserId == client.Id
                                   select r;

                if (queryReports.Count() > 0)
                {
                    foreach (var report in queryReports)
                    {
                        Report info = new Report()
                        {
                            URLs = new List<string>(),
                            Points = new List<Point>(),
                            Duration = report.Duration,
                            RequestDuration = report.RequestDuration,
                            Strategy = (TestingStrategy)report.Strategy,
                            Time = report.Time,
                            Timeout = report.Timeout,
                            VirtualUsers = report.VirtualUsers,
                            Id = report.Id
                        };

                        var queryUrls = from u in db.UrlsSet
                                        where u.Group == report.UrlGroup
                                        select u.Url;

                        if (queryUrls.Count() > 0)
                        {
                            foreach (var url in queryUrls)
                            {
                                info.URLs.Add(url);
                            }
                        }

                        var queryPoints = from p in db.PointsSet
                                          where p.ReportId == report.Id
                                          select p;

                        if (queryPoints.Count() > 0)
                        {
                            foreach (var point in queryPoints)
                            {
                                info.Points.Add(new Point()
                                {
                                    x = point.X,
                                    y = point.Y
                                });
                            }
                        }

                        reports.List.Add(info);
                    }
                }
            }

            client.Send(reports);
        }
    }
}
