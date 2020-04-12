using System;
using System.IO;
using System.IO.Abstractions;
using System.Linq;
using Episememe.Application.Exceptions;
using Episememe.Application.Interfaces;

namespace Episememe.Infrastructure.FileSystem
{
    public class FileStorage : IFileStorage
    {
        private readonly IFileSystem _fileSystem;
        private readonly string _rootDirectory;

        public FileStorage(string rootDirectory, IFileSystem fileSystem)
        {
            _rootDirectory = rootDirectory;
            _fileSystem = fileSystem;
        }

        public Stream CreateNew(string filename)
        {
            var validatedPath = GetValidatedPath(filename);
            EnsureDirectoryExists(_fileSystem.Path.GetDirectoryName(validatedPath));

            try
            {
                return _fileSystem.File.Open(validatedPath, FileMode.CreateNew, FileAccess.Write);
            }
            catch (IOException)
            {
                if(_fileSystem.File.Exists(validatedPath))
                    throw new FileExistsException($"Cannot create '{filename}': the file already exists.");
                throw;
            }
        }

        public Stream Read(string filename)
        {
            var validatedPath = GetValidatedPath(filename);

            if (!_fileSystem.File.Exists(validatedPath))
                throw new FileDoesNotExistException($"Cannot read from '{filename}': the file does not exist.");

            return _fileSystem.File.OpenRead(validatedPath);
        }

        public void Delete(string filename)
        {
            var validatedPath = GetValidatedPath(filename);

            if (!_fileSystem.File.Exists(validatedPath))
                throw new FileDoesNotExistException($"Cannot delete '{filename}': the file does not exist.");

            _fileSystem.File.Delete(validatedPath);
        }

        protected void EnsureDirectoryExists(string directoryPath)
        {
            if (!_fileSystem.Directory.Exists(directoryPath))
                _fileSystem.Directory.CreateDirectory(directoryPath);
        }

        protected string GetValidatedPath(string filename)
        {
            if (filename.Any(ch => !char.IsLetterOrDigit(ch)))
            {
                throw new ArgumentException($"Filename '{filename}' contains non-alphanumeric characters.");
            }

            if (filename.Length <= 2)
            {
                throw new ArgumentException($"Filename '{filename}' is too short (<= 2)");
            }

            return Path.Combine(_rootDirectory, filename[0].ToString(), filename[1].ToString(), filename[2..]);
        }
    }
}
