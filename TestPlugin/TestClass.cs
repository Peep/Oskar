﻿using System;
using ChatSharp.Events;
using Oskar;
using Oskar.Wrappers;
using IrcClient = Oskar.Wrappers.IrcClient;

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
            //Client.ChannelMessageRecieved += OnChannelMessageReceived;
        }

        //public void OnUserJoinedChannel(object sender, IrcUser user)
        //{
        //    Bot.Instance.Client.SendMessage("Sup, " + user.Nick, "#bmrftest");
        //}

        public void OnUserJoinedChannel(object sender, IrcUser e)
        {
            //Bot.Instance.Client.SendMessage("Sup, " + e.User, e.);
            Console.WriteLine(e);
        }

        public void OnChannelMessageReceived(object sender, PrivateMessageEventArgs e)
        {
            Bot.Instance.Client.SendMessage(e.PrivateMessage.Message);
        }

        public override void OnDestroy()
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("Bye. I'm a plugin! I live in " + AppDomain.CurrentDomain.FriendlyName);
            Console.ResetColor();
        }
    }
}
