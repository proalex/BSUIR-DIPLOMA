using ControllerServer.Database;
using System;
using System.Net.Sockets;
using System.Linq;
using System.Net;

namespace ControllerServer
{
    public enum ClientType
    {
        CLIENT,
        GENERATOR
    }

	public sealed class Client
	{
        public readonly Socket Socket;
        private ControllerServer _server = ControllerServer.Instance;
        private byte[] _salt;
        private bool _authenticated;
        private string _email;
        private int _id;

        public Generator Generator;

        public bool Authenticated
        {
            get
            {
                return _authenticated;
            }
        }

        public byte[] Salt
        {
            get
            {
                return _salt;
            }
        }

        public String Email
        {
            get
            {
                return _email;
            }
        }

        public int Id
        {
            get
            {
                return _id;
            }
        }

        public String IP
        {
            get
            {
                IPEndPoint endPoint = Socket.RemoteEndPoint as IPEndPoint;
                return endPoint.Address.ToString();
            }
        }

        public int Port
        {
            get
            {
                IPEndPoint endPoint = Socket.RemoteEndPoint as IPEndPoint;
                return endPoint.Port;
            }
        }


        public Client(Socket client)
        {
            Socket = client;// ?? throw new NullReferenceException("client is null");
        }

        public void Send<T>(T message)
        {
            if (message == null)
            {
                throw new NullReferenceException("message is null");
            }

            Packet packet = PacketBuilder.Build(message);

            _server.Send(Socket, packet);
        }

        public void Send(ServerOpcode opcode)
        {
            Packet packet = new Packet(opcode, new byte[0]);
            _server.Send(Socket, packet);
        }

        public void UpdateSalt()
        {
            if (_salt != null)
            {
                return;
            }

            _salt = SaltGenerator.GenerateSalt();
        }

        public void Logout()
        {
            if (_authenticated)
            {
                using (var db = new DatabaseContainer())
                {
                    var query = from u in db.UsersSet
                                where u.Id == _id
                                select u;

                    if (query.Count() > 0)
                    {
                        query.First().Online = false;
                    }

                    db.SaveChanges();
                }

                _authenticated = false;
                _email = null;
                _id = 0;
            }
        }

        public bool Authenticate(string email, byte[] token, out byte attemptsLeft)
        {
            if (_salt == null || _authenticated)
            {
                attemptsLeft = 255;
                return false;
            }

            using (var db = new DatabaseContainer())
            {
                var query = from u in db.UsersSet
                            where u.Email == email
                            select u;

                if (query.Count() == 0)
                {
                    attemptsLeft = 255;
                    return false;
                }

                var user = query.First();

                attemptsLeft = user.AttemptsLeft;

                if (attemptsLeft == 0)
                {
                    return false;
                }

                if (user.Online)
                {
                    attemptsLeft = 255;
                    return false;
                }

                _authenticated = AuthChecker.CheckToken(token, _salt, user.Password);

                if (!_authenticated)
                {
                    attemptsLeft--;
                    user.AttemptsLeft--;
                }
                else
                {
                    user.AttemptsLeft = Config.MaxAuthAttempts;
                    user.Online = true;
                    _email = user.Email;
                    _id = user.Id;
                }

                var entry = db.Entry(user);
                entry.Property(e => e.AttemptsLeft).IsModified = true;
                entry.Property(e => e.Online).IsModified = true;
                db.SaveChanges();
                _salt = null;
                return _authenticated;
            }
        }

        public void Close()
        {
            if (Socket.Connected)
            {
                Socket.Close();
            }
        }
	}
}
