using System.IO.Abstractions.TestingHelpers;

namespace Episememe.Infrastructure.Tests.Helpers
{
    public static class MockFileSystemExtensions
    {
        public static MockFileSystem WithFile(
            this MockFileSystem fileSystem, string filename, string fileContent)
        {
            var fileData = new MockFileData(fileContent);
            fileSystem.AddFile(filename, fileContent);
            return fileSystem;
        }
    }
}
