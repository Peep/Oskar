using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oskar.Wrappers
{
    public class UserCollection : MarshalByRefObject
    {
        internal ChatSharp.UserCollection _users;

        public UserCollection(ChatSharp.UserCollection users)
        {
            _users = users;
        }

        public bool ContainsMask(string mask)
        {
            return _users.ContainsMask(mask);
        }

        public bool Contains(string nick)
        {
            return false;
        }

        public bool Contains(IrcUser user)
        {
            return false;
        }

        //public IrcUser this[int index]
        //{
        //    return _users[index];
        //}
    }
}
