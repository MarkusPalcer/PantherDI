using System.IO;
using PantherDI.Registry.Catalog;

namespace PantherDI.Extensions
{
    public static class CatalogBuilderExtensions
    {
        public static ContainerCreation.ContainerBuilder WithAssembliesFrom(this ContainerCreation.ContainerBuilder builder, string folderPath,
            bool recursive = false)
        {
            return builder.WithCatalog(new DirectoryCatalog(folderPath, recursive));
        }

        public static ContainerCreation.ContainerBuilder WithAssembliesFrom(this ContainerCreation.ContainerBuilder builder,
                                                          DirectoryInfo folder,
                                                          bool recursive = false)
        {
            return builder.WithCatalog(new DirectoryCatalog(folder, recursive));
        }
    }
}