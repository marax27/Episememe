using Episememe.Application.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Episememe.Infrastructure.Identifiers
{
    public class MediaIdProvider : IMediaIdProvider
    {
        public string GenerateUniqueBase32Id(IReadOnlyCollection<string> existingIds)
        {
            var idExists = true;
            var newId = string.Empty;

            while (idExists)
            {
                newId = GenerateRandomBase32();

                if (!existingIds.Contains(newId))
                {
                    idExists = false;
                }
            }

            return newId;
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
