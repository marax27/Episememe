using Episememe.Application.Interfaces;
using System;

namespace Episememe.Infrastructure.Identifiers
{
    public class MediaIdProvider : IMediaIdProvider
    {
        public string Generate()
        {
            return GenerateRandomBase32();
        }

        private string GenerateRandomBase32()
        {
            const string chars = "abcdefghijklmnopqrstuvwxyz2345678";
            var stringChars = new char[8];
            var random = new Random();

            for (var i = 0; i < stringChars.Length; i++)
            {
                stringChars[i] = chars[random.Next(chars.Length)];
            }

            var finalString = new string(stringChars);
            return finalString;
        }
    }
}
