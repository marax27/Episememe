using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Internal;
using System.IO;
using System.Text;

namespace Episememe.Application.Tests.Helpers
{
    public static class FormFileFactory
    {
        public static IFormFile Create(string fileName, string fileContent)
        {
            var bytes = Encoding.UTF8.GetBytes(fileContent);

            return new FormFile(
                baseStream: new MemoryStream(bytes),
                baseStreamOffset: 0,
                length: bytes.Length,
                name: "Data",
                fileName: fileName
            );
        }
    }
}
