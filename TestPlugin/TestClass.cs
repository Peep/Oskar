using System;
using ChatSharp.Events;
using Oskar;

namespace TestPlugin
{
    public class TestClass : Plugin
    {
        public override string Name { get { return "Test Plugin"; }}
        public override string Author { get { return "Peep"; }}

        public override void OnCreate()
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("Hi. I'm a plugin! I live in " + AppDomain.CurrentDomain.FriendlyName);
            Console.ResetColor();

            Client.UserJoinedChannel += OnUserJoinedChannel;
        }

        public void OnUserJoinedChannel(object sender, ChannelUserEventArgs e)
        {
            Bot.Instance.Client.SendMessage("Sup, " + e.User, e.Channel.Name);
        }

        public override void OnDestroy()
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("Bye. I'm a plugin! I live in " + AppDomain.CurrentDomain.FriendlyName);
            Console.ResetColor();
        }
    }
}
