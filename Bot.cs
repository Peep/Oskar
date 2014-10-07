using System.Collections.Generic;
using ChatSharp;

namespace Oskar
{
    public class Bot
    {
        public BotConfig Config { get; private set; }
        public List<IrcClient> Clients { get; private set; }
        public MessageProcessor MsgProc;

        public Bot()
        {
            MsgProc = new MessageProcessor();
            Clients = new List<IrcClient>();

            LoadConfig();
        }

        private void LoadConfig()
        {
            Config = new BotConfig("config.ini");
        }

        public void Setup()
        {
            foreach (var server in Config.AutoJoinChannels.Keys)
            {
                var channels = Config.AutoJoinChannels[server].Split(',');
                var client = new IrcClient(server, new IrcUser(Config.BotNick, Config.BotNick));

                client.ConnectionComplete += (s, e) =>
                {
                    foreach (var channel in channels)
                        client.JoinChannel(channel);
                };
                client.ConnectAsync();
                Clients.Add(client); // TODO: Set each client up with a MessageProcessor
            }
        }
    }
}
