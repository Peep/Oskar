using System;

namespace Oskar.Wrappers
{
    public class IrcUser : MarshalByRefObject
    {
        internal ChatSharp.IrcUser _user;

        public IrcUser(ChatSharp.IrcUser user)
        {
            _user = user;
        }

        public string Nick { get { return _user.Nick; } }
        public string User { get { return _user.User; } }
        public string Password { get { return _user.Password; } }
        public string Mode { get { return _user.Mode; } }
        public string RealName { get { return _user.RealName; } }
        public string Hostname { get { return _user.Hostname; } }

        public string Hostmask { get { return _user.Hostmask; } }

        public bool Match(string mask)
        {
            return _user.Match(mask);
        }

        public static bool Match(string mask, string value)
        {
            return ChatSharp.IrcUser.Match(mask, value);
        }

        public bool Equals(IrcUser other)
        {
            return other.Hostmask == _user.Hostmask;
        }

        public override bool Equals(object obj)
        {
            return _user.Equals(obj);
        }

        public override int GetHashCode()
        {
            return _user.Hostmask.GetHashCode();
        }

        public override string ToString()
        {
            return _user.Hostmask;
        }
    }
}
