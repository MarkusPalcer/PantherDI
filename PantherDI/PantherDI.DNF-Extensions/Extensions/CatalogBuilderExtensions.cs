using System.IO;
using PantherDI.ContainerCreation;
using PantherDI.Registry.Catalog;

namespace PantherDI.Extensions
{
    public static class CatalogBuilderExtensions
    {
        public static ContainerBuilder WithAssembliesFrom(this ContainerBuilder builder, string folderPath,
            bool recursive = false)
        {
            return builder.WithCatalog(new DirectoryCatalog(folderPath, recursive));
        }

        public static ContainerBuilder WithAssembliesFrom(this ContainerBuilder builder,
                                                          DirectoryInfo folder,
                                                          bool recursive = false)
        {
            return builder.WithCatalog(new DirectoryCatalog(folder, recursive));
        }
    }
}