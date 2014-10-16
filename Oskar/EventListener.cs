using System;
using ChatSharp;
using ChatSharp.Events;

namespace Oskar
{
    public class EventListener
    {
        private readonly IrcClient _client;

        public EventListener()
        {
            _client = Bot.Instance.Client;
        }

        public void Listen()
        {
            _client.RawMessageRecieved += OnRawMessageReceived;
            _client.RawMessageSent += OnRawMessageSent;
            _client.ConnectionComplete += OnConnectionComplete;
        }

        private static void OnRawMessageReceived(object sender, RawMessageEventArgs e)
        {
            Console.WriteLine(e.Message);
        }

        private static void OnRawMessageSent(object sender, RawMessageEventArgs e)
        {
            Console.WriteLine(e.Message);
        }

        private void OnConnectionComplete(object sender, EventArgs e)
        {
            if (Bot.Instance.Config.NickServEnabled)
                NickServLogin();

            foreach (var channel in Bot.Instance.Config.AutoJoinChannels)
                _client.JoinChannel(channel);
        }

        private void NickServLogin()
        {
            if (String.Equals(_client.User.Nick, Bot.Instance.Config.NickServUser, StringComparison.CurrentCultureIgnoreCase))
                _client.SendMessage("IDENTIFY " + Bot.Instance.Config.NickServPass, "NICKSERV");
        }
    }
}
