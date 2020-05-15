using Episememe.Application.Features.FileUpload;
using Episememe.Application.Tests.Helpers;
using FluentAssertions;
using System;
using System.Collections.Generic;
using Xunit;

namespace Episememe.Application.Tests.FeatureTests.FileUpload
{
    public class WhenCreatingFileUploadCommand
    {
        [Fact]
        public void GivenNullFormFile_ExceptionIsThrown()
        {
            var givenTags = new List<string>()
            {
                "pigeons", "flying rats"
            };

            Action act = () => FileUploadCommand.Create(null, givenTags, string.Empty);
            act.Should().Throw<ArgumentNullException>();
        }

        [Fact]
        public void GivenNullTags_ExceptionIsThrown()
        {
            var givenFileName = "newFile";
            var givenFileContent = "None";
            var givenFormFile = FormFileFactory.Create(givenFileName, givenFileContent);

            Action act = () => FileUploadCommand.Create(givenFormFile, null, string.Empty);
            act.Should().Throw<ArgumentNullException>();
        }
    }
}
