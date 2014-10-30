using System;
using System.Runtime.Remoting.Channels;
using ChatSharp.Events;
using Oskar.Wrappers;

namespace Oskar
{
    public class PluginManager : MarshalByRefObject
    {
        public event UserJoinedChannelDelegate UserJoinedChannel;
        public delegate void UserJoinedChannelDelegate(object sender, IrcUser s);

        public PluginManager()
        {
            Bot.Instance.Client.UserJoinedChannel += OnUserJoinedChannel;
        }

        private void OnUserJoinedChannel(object sender, ChannelUserEventArgs e)
        {
            UserJoinedChannel(this, new IrcUser(Bot.Instance.Client.User));
            //UserJoinedChannel(this, new Wrappers.ChannelUserEventArgs(
            //    new Wrappers.IrcChannel(e.Channel), new Wrappers.IrcUser(e.User)));
        }
    }
}
