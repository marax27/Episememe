using Episememe.Application.Features.FileUpload;
using Episememe.Application.Tests.Helpers;
using FluentAssertions;
using System;
using System.Collections.Generic;
using Episememe.Application.DataTransfer;
using Xunit;

namespace Episememe.Application.Tests.FeatureTests.FileUpload
{
    public class WhenCreatingFileUploadCommand
    {
        public readonly DateTime SampleUploadDate = new DateTime(1998, 1, 15);
        public const string SampleAuthorId = "SampleAuthor";

        [Fact]
        public void GivenNullFormFile_ExceptionIsThrown()
        {
            var givenTags = new[] { "pigeons", "flying rats" };
            var givenMediaDto = new FileUploadDto(givenTags, SampleUploadDate, false);

            Action act = () => FileUploadCommand.Create(null, givenMediaDto, SampleAuthorId);
            act.Should().Throw<ArgumentNullException>();
        }

        [Fact]
        public void GivenNullTags_ExceptionIsThrown()
        {
            var givenFileName = "newFile";
            var givenFileContent = "None";
            var givenFormFile = FormFileFactory.Create(givenFileName, givenFileContent);
            var givenMediaDto = new FileUploadDto(null, SampleUploadDate, false);

            Action act = () => FileUploadCommand.Create(givenFormFile, givenMediaDto, SampleAuthorId);
            act.Should().Throw<ArgumentNullException>();
        }
    }
}
