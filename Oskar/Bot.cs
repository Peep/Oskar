using System;
using System.IO;
using System.Collections.Generic;
using ChatSharp;

namespace Oskar
{
    public class Bot : MarshalByRefObject
    {
        public static readonly Bot Instance;
        public BotConfig Config { get; private set; }
        public IrcClient Client { get; private set; }

        static Dictionary<string, DomainHelper> LoadedDomains;
        static Dictionary<string, Plugin> LoadedPlugins;
        EventListener _listener;

        static Bot()
        {
            Instance = new Bot();
        }

        private Bot()
        {
            Config = new BotConfig("config.ini");
            LoadedDomains = new Dictionary<string, DomainHelper>();
            LoadedPlugins = new Dictionary<string, Plugin>();
        }

        public void Connect()
        {
            var client = new IrcClient(Instance.Config.ServerHostname,
                new IrcUser(Instance.Config.BotNick, Instance.Config.BotNick));
            client.ConnectAsync();
            Client = client;

            _listener = new EventListener();
            _listener.Listen();
            WatchForPlugins();
        }

        public void WatchForPlugins()
        {
            if (!Directory.Exists("plugins"))
                Directory.CreateDirectory("plugins");
            var pluginDirectory = Path.Combine(Environment.CurrentDirectory, "plugins");

            var set = new AppDomainSetup();
            set.ApplicationBase = Environment.CurrentDirectory;

            var watcher = new FileSystemWatcher(pluginDirectory, "*.dll");
            watcher.EnableRaisingEvents = false;

            foreach (var ass in Directory.GetFiles(pluginDirectory, "*.dll"))
                LoadAss(ass);

            while (!Console.KeyAvailable)
            {
                var fsr = watcher.WaitForChanged(WatcherChangeTypes.Created | WatcherChangeTypes.Deleted | WatcherChangeTypes.Changed, 100);
                if (fsr.TimedOut)
                    continue;

                switch (fsr.ChangeType)
                {
                    case WatcherChangeTypes.Created:
                        LoadAss(Path.GetFullPath(fsr.Name));
                        break;
                    case WatcherChangeTypes.Deleted:
                        UnloadAss(Path.GetFullPath(fsr.Name));
                        break;
                    case WatcherChangeTypes.Changed:
                        UnloadAss(Path.GetFullPath(fsr.Name));
                        LoadAss(Path.GetFullPath(fsr.Name));
                        break;
                }
            }
        }

        static void LoadAss(string assName)
        {
            if (LoadedDomains.ContainsKey(assName))
                return;

            var helper = new DomainHelper(assName);
            LoadedDomains[assName] = helper;

            var plugin = helper.GetPlugin();
            LoadedPlugins[assName] = plugin;
            Console.WriteLine("Loading '{0}' by {1}", plugin.Name, plugin.Author);

            // For some reason this throws an NRE despite still calling the method properly.
            plugin.Client = new PluginManager();
            try { plugin.OnCreate(); } catch { Console.WriteLine("Exception thrown when loading plugin.");}
        }

        static void UnloadAss(string assname)
        {
            if (!LoadedDomains.ContainsKey(assname))
                return;

            var helper = LoadedDomains[assname];
            var plugin = LoadedPlugins[assname];
            Console.WriteLine("Unloading '{0}' by {1}", plugin.Name, plugin.Author);

            LoadedPlugins.Remove(assname);
            LoadedDomains.Remove(assname);
            helper.Unload();
        }
    }
}
