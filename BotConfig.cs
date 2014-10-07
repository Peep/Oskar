using System.Collections.Generic;
using EasyConfigLib.Storage;

namespace Oskar
{
    public class BotConfig : EasyConfig
    {
        [Field("Nick", "BotSettings")] 
        public string BotNick = "Oskar";

        [Field("AutoJoinChannels")]
        public Dictionary<string, string> AutoJoinChannels = new Dictionary<string, string> 
        { { "irc.synirc.net", "#bmrftest"} };

        public BotConfig(string file)
            : base(file)
        {
        }
    }
}
