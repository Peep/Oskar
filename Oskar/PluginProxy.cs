// Thanks to an anonymous friend and #csharp for helping me
// figure this one out. Unloading assemblies is a bit tricky!

using System;
using System.IO;
using System.Linq;
using System.Reflection;

namespace Oskar
{
    class ProxyDomain : MarshalByRefObject
    {
        public string Path;
        public Assembly Ass;
        public Plugin Plugin;


        static Assembly domain_AssemblyResolve(object sender, ResolveEventArgs args)
        {
            Console.WriteLine(args.Name);
            return null;
        }


        public void LoadAssembly()
        {
            AppDomain.CurrentDomain.AssemblyResolve += domain_AssemblyResolve;
            Ass = Assembly.Load(File.ReadAllBytes(Path));
            var k = Ass.GetExportedTypes().Where(type => type.IsSubclassOf(typeof (Plugin)))
                .Select(x => (Plugin)Activator.CreateInstance(x));
            Plugin = k.First();
        }
    }

    class DomainHelper
    {
        readonly AppDomain _domain;
        readonly ProxyDomain _proxy;

        public DomainHelper(string path)
        {
            AppDomain.CurrentDomain.AssemblyResolve += CurrentDomain_AssemblyResolve;
            //_domain = AppDomain.CreateDomain(path, null);
            _domain = AppDomain.CurrentDomain;

            var proxyType = typeof(ProxyDomain);
            _proxy = (ProxyDomain)_domain.CreateInstanceAndUnwrap(proxyType.Assembly.FullName, proxyType.FullName);
            _proxy.Path = path;
            _proxy.LoadAssembly();
        }

        static Assembly CurrentDomain_AssemblyResolve(object sender, ResolveEventArgs args)
        {
            return args.RequestingAssembly;
        }

        public Plugin GetPlugin()
        {
            return _proxy.Plugin;
        }

        public void Unload()
        {
            _proxy.Plugin.OnDestroy();
            AppDomain.Unload(_domain);
        }
    }
}
