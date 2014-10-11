using System;
using ChatSharp;

namespace Oskar
{
    public class Bot
    {
        public static readonly Bot Instance;

        public BotConfig Config;
        public IrcClient Client { get; private set; }
        private EventListener _listener;

        static Bot()
        {
            Instance = new Bot();
        }

        private Bot()
        {
            Config = new BotConfig("config.ini");
        }

        public void Setup()
        {
            var server = Instance.Config.ServerHostname;
            var client = new IrcClient(server, new IrcUser(Instance.Config.BotNick, Instance.Config.BotNick));
            client.ConnectAsync();
            Client = client;

            _listener = new EventListener();
            _listener.Listen();
        }
    }
}
