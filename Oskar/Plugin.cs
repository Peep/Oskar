using System;

namespace Oskar
{
    public abstract class Plugin : MarshalByRefObject
    {
        /// <summary>
        /// A reference to the application domain hosting the plugin.
        /// </summary>
        public AppDomain Domain { get; set; }

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
