using System;
using ChatSharp;

namespace Oskar
{
    public abstract class Plugin : MarshalByRefObject
    {
        /// <summary>
        /// Exposes the event handlers to the plugin.
        /// </summary>
        public IrcClient Client { get; set; }

        /// <summary>
        /// The plugin name.
        /// </summary>
        public abstract string Name { get; }

        /// <summary>
        /// The plugin author's name.
        /// </summary>
        public abstract string Author { get; }

        /// <summary>
        /// Called when the plugin is loaded. A good time to hook into IRC events from Bot.Instance.Client.
        /// </summary>
        public abstract void OnCreate();

        /// <summary>
        /// Called just before the plugin is unloaded.
        /// </summary>
        public abstract void OnDestroy();
    }
}
