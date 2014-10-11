using System.Collections.Generic;
using EasyConfigLib.Storage;

namespace Oskar
{
    public class BotConfig : EasyConfig
    {
        [Field("Nick", "General")] 
        public string BotNick = "Oskar";

        [Field("ServerHostname")]
        public string ServerHostname = "irc.synirc.net";

        [Field("AutoJoinChannels")]
        public List<string> AutoJoinChannels = new List<string> 
        { "#bmrftest" };

        [Field("Enable", "NickServ")] 
        public bool NickServEnabled = true;

        [Field("Username", "NickServ")] 
        public string NickServUser = "USERNAME";

        [Field("Password", "NickServ")]
        public string NickServPass = "PASSWORD";

        public BotConfig(string file)
            : base(file)
        {
        }
    }
}
