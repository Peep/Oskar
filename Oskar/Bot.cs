using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Collections.Generic;
using ChatSharp;

namespace Oskar
{
    public class Bot
    {
        public static readonly Bot Instance;
        public BotConfig Config { get; private set; }
        public IrcClient Client { get; private set; }
        public List<Plugin> Plugins; 

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
            //var client = new IrcClient(Instance.Config.ServerHostname, 
            //    new IrcUser(Instance.Config.BotNick, Instance.Config.BotNick));
            //client.ConnectAsync();
            //Client = client;

            //_listener = new EventListener();
            //_listener.Listen();
        }

        internal void LoadAllPlugins()
        {
            Plugins = new List<Plugin>();

            if (!Directory.Exists("plugins"))
                Directory.CreateDirectory("plugins");

            // We need a separate AppDomain to load the plugins folder.
            var pluginLoadDomain = AppDomain.CreateDomain("PluginLoadDomain");

            try
            {
                // Load all assemblies in the 'plugins' folder into the PluginLoadDomain.
                var allPlugins = Directory.GetFiles(    
                    Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "plugins"), "*.dll")
                        .Select(p => pluginLoadDomain.Load(AssemblyName.GetAssemblyName(p))).ToList();

                foreach (var assembly in allPlugins)
                {
                    // Check each assembly to ensure it's actually a plugin.
                    var types = assembly.GetTypes().Where(x => x.IsSubclassOf(typeof(Plugin)));

                    // Attempt to load them into their own, separate AppDomain.
                    foreach (var type in types)
                    {
                        var domain = AppDomain.CreateDomain(type.Assembly.GetName().Name);
                        Plugin plugin;

                        try
                        {
                            plugin = (Plugin) domain.CreateInstanceAndUnwrap(type.Assembly.FullName, type.FullName);
                        }
                        catch (TypeLoadException e)
                        {
                            Console.WriteLine("Error loading plugin: {0}.", e.Message);
                            continue;
                        }

                        Console.WriteLine(@"Loaded ""{0}"" by {1}.", plugin.Name, plugin.Author);
                        plugin.Domain = domain;

                        // This throws an NRE despite still calling OnCreate(). Temporary workaround.
                        try { plugin.OnCreate(); }
                        catch { }

                        plugin.Enabled = true;
                        Plugins.Add(plugin);
                    }
                }
            }
            finally
            {
                // Unload the PluginLoadDomain, releasing unnecessary file locks.
                AppDomain.Unload(pluginLoadDomain);
                try
                {
                    Console.WriteLine(pluginLoadDomain.FriendlyName + " is still loaded!");
                }
                catch (AppDomainUnloadedException)
                {
                    Console.WriteLine("The 'PluginLoadDomain' AppDomain no longer exists.");
                }
            }

        }

        internal void UnloadAllPlugins()
        {
            foreach (var plugin in Plugins.ToList())
            {
                plugin.OnDestroy();
                Console.WriteLine(@"Unloaded ""{0}"" by {1}.", plugin.Name, plugin.Author);
                plugin.Enabled = false;
                AppDomain.Unload(plugin.Domain);

                try
                {
                    Console.WriteLine(plugin.Domain.FriendlyName + " is still loaded!");
                }
                catch (AppDomainUnloadedException)
                {
                    Console.WriteLine("The plugin-specific AppDomain no longer exists.");
                }
            }
        }
    }
}
