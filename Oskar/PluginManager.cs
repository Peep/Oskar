using System;
using ChatSharp.Events;

namespace Oskar
{
    public class PluginManager : MarshalByRefObject
    {
        public event UserJoinedChannelDelegate UserJoinedChannel;
        public delegate void UserJoinedChannelDelegate(object sender, Serializer<ChannelUserEventArgs> s);

        public PluginManager()
        {
            Bot.Instance.Client.UserJoinedChannel += OnUserJoinedChannel;
        }

        private void OnUserJoinedChannel(object sender, ChannelUserEventArgs e)
        {
            UserJoinedChannel(this, new Wrappers.ChannelUserEventArgs(
                new Wrappers.IrcChannel(e.Channel), new Wrappers.IrcUser(e.User)));
        }
    }
}
