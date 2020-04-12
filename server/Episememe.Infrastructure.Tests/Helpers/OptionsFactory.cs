using Episememe.Infrastructure.FileSystem;
using Microsoft.Extensions.Options;

namespace Episememe.Infrastructure.Tests.Helpers
{
    internal class OptionsFactory
    {
        public static IOptions<FileStorageSettings> Create(string rootDirectory = "")
        {
            return Options.Create(new FileStorageSettings
            {
                RootDirectory = rootDirectory
            });
        }
    }
}
