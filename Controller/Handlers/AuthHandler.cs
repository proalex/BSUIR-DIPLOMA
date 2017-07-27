using System;

namespace ControllerServer
{
    public static class AuthHandler
    {
        public static void HandleAuthRequest(Client client, object data)
        {
            if (client.Generator != null)
            {
                return;
            }

            AuthData authData = (AuthData)data;
            byte attemptsLeft;

            client.Send(new AuthResponse()
            {
                Authenticated = client.Authenticate(authData.Email, authData.Token, out attemptsLeft),
                AttemptsLeft = attemptsLeft
            });

            if (client.Authenticated)
            {
                Console.WriteLine("User {0} authenticated.", client.Email);
                UpdateHandler.UpdateInfo(client);
                ProfileHandler.SendProfiles(client);
                ReportsHandler.SendReports(client);
            }
            else if (attemptsLeft == 0)
            {
                Console.WriteLine("User {0} blocked.", authData.Email);
            }
        }

        public static void HandleSaltRequest(Client client, object data)
        {
            if (client.Generator != null || client.Authenticated)
            {
                return;
            }

            client.UpdateSalt();
            client.Send(new AuthSalt() { Salt = client.Salt });
        }

        public static void HandleLogoutRequest(Client client, object data)
        {
            if (client.Authenticated)
            {
                Console.WriteLine("User {0} logged out.", client.Email);
                client.Logout();
            }
        }
    }
}
