using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oskar.Wrappers
{
    public class IrcChannel : MarshalByRefObject
    {
        internal ChatSharp.IrcChannel _channel;
        internal IrcClient _client;
        internal UserCollection _users;
        internal Dictionary<char, UserCollection> _usersByMode;

        public string Topic { get { return _channel.Topic; } }
        public string Name { get { return _channel.Name; } }
        public string Mode { get { return _channel.Mode; } }
         

        public IrcChannel(ChatSharp.IrcChannel channel, ChatSharp.UserCollection users)
        {
            _channel = channel;
            _users = new UserCollection(users);
            _usersByMode = new Dictionary<char, UserCollection>();
        }

        public void Invite(string nick)
        {
            
        }
    }
}
