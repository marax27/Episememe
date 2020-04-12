using System.IO;

namespace Episememe.Application.Interfaces
{
    public interface IFileStorage
    {
        Stream CreateNew(string filename);
        Stream Read(string filename);
        void Delete(string filename);
    }
}
