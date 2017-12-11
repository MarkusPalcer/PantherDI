using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using PantherDI.Registry.Registration.Registration;

namespace PantherDI.Registry.Catalog
{
    public class DirectoryCatalog : ICatalog
    {
        private readonly MergedCatalog _content;

        public DirectoryCatalog(string directory, bool recursive = false) : this(new DirectoryInfo(directory), recursive) { }

        public DirectoryCatalog(DirectoryInfo directory, bool recursive = false)
        {
            var assemblies = new List<Assembly>();

            foreach (var file in directory.EnumerateFiles("*.dll", recursive ? SearchOption.AllDirectories : SearchOption.TopDirectoryOnly))
            {
                try
                {
                    assemblies.Add(Assembly.LoadFrom(file.FullName));
                }
                catch (Exception)
                {
                    // TODO: Add logging
                }
            }

            _content = new MergedCatalog(assemblies.Select(x => new AssemblyCatalog(x)).Cast<ICatalog>().ToArray());
        }


        #region Implementation of ICatalog

        public IEnumerable<IRegistration> Registrations
        {
            get { return _content.Registrations; }
        }

        #endregion
    }
}