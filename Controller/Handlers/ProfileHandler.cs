using ControllerServer.Database;
using System.Collections.Generic;
using System.Linq;

namespace ControllerServer
{
    public static class ProfileHandler
    {
        public static void SendProfiles(Client client)
        {
            Profiles profiles = new Profiles()
            {
                List = new List<SaveProfileRequest>()
            };

            using (var db = new DatabaseContainer())
            {
                var query = from p in db.ProfilesSet
                            where p.UserId == client.Id
                            select p;

                if (query.Count() > 0)
                {
                    foreach (var profile in query)
                    {
                        SaveProfileRequest profileData = new SaveProfileRequest()
                        {
                            Name = profile.Name
                        };

                        TaskData taskData = new TaskData()
                        {
                            Duration = profile.Duration,
                            RequestDuration = profile.RequestDuration,
                            Timeout = profile.Timeout,
                            VirtualUsers = profile.VirtualUsers,
                            Strategy = (TestingStrategy)profile.Strategy
                        };

                        taskData._URLs = new List<string>();

                        var queryUrls = from u in db.UrlsSet
                                        where u.Group == profile.UrlGroup
                                        select u;

                        foreach (var url in queryUrls)
                        {
                            taskData._URLs.Add(url.Url);
                        }

                        profileData.Data = taskData;
                        profiles.List.Add(profileData);
                    }
                }
            }

            client.Send(profiles);
        }

        public static void HandleDeleteProfile(Client client, object data)
        {
            if (client.Generator != null || !client.Authenticated)
            {
                return;
            }

            DeleteProfileRequest request = (DeleteProfileRequest)data;

            if (request.Name == null)
            {
                return;
            }

            using (var db = new DatabaseContainer())
            {
                var result = from p in db.ProfilesSet
                             where p.Name == request.Name && p.UserId == client.Id
                             select p;

                if (result.Count() > 0)
                {
                    foreach (var profile in result)
                    {
                        int urlGroup = profile.UrlGroup;

                        var urls = from u in db.UrlsSet
                                   where u.Group == urlGroup
                                   select u;

                        if (urls.Count() > 0)
                        {
                            foreach (var url in urls)
                            {
                                db.UrlsSet.Remove(url);
                            }
                        }

                        db.ProfilesSet.Remove(profile);
                    }
                }

                db.SaveChanges();
            }

            SendProfiles(client);
        }

        public static void HandleSaveProfile(Client client, object data)
        {
            if (client.Generator != null || !client.Authenticated)
            {
                return;
            }

            SaveProfileRequest request = (SaveProfileRequest)data;

            if (request.Name == null || request.Data == null)
            {
                return;
            }

            using (var db = new DatabaseContainer())
            {
                var query = from p in db.ProfilesSet
                            where p.Name == request.Name
                            select p;

                if (query.Count() > 0)
                {
                    client.Send(new SaveProfileResponse()
                    {
                        Result = SaveProfileResult.NAME_EXISTS
                    });
                }
                else
                {
                    int max = db.UrlsSet.Count() > 0 ? db.UrlsSet.Max(u => u.Group) : 0;

                    foreach (var url in request.Data._URLs)
                    {
                        db.UrlsSet.Add(new Urls()
                        {
                            Group = max + 1,
                            Url = url
                        });
                    }

                    db.ProfilesSet.Add(new Database.Profiles()
                    {
                        UserId = client.Id,
                        Duration = request.Data.Duration,
                        Name = request.Name,
                        RequestDuration = request.Data.RequestDuration,
                        Strategy = (byte)request.Data.Strategy,
                        Timeout = request.Data.Timeout,
                        UrlGroup = max + 1,
                        VirtualUsers = request.Data.VirtualUsers
                    });

                    db.SaveChanges();

                    client.Send(new SaveProfileResponse()
                    {
                        Result = SaveProfileResult.OK
                    });

                    SendProfiles(client);
                }
            }
        }
    }
}
