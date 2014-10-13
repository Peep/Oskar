using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using EasyConfigLib;
using ChatSharp;

namespace Oskar
{
    class Program
    {
        static void Main()
        {
            var botThread = new Thread(Bot.Instance.Setup);
            Bot.Instance.LoadAllPlugins();
            Bot.Instance.UnloadAllPlugins();

            botThread.Start();

            while (true) ;
        }
    }
}
