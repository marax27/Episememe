using System;
using System.Collections.Generic;
using System.Text;
using Episememe.Application.Interfaces;
using System.Web.Mvc;

namespace Episememe.Application.DataTransfer
{
    public class LoadInstanceDto
    {
        public string Id { get; }
        
        public FileStreamResult LoadedFile {get;} 

        public LoadInstanceDto(string id, FileStreamResult loadedFile)
        {
            Id = id;
            LoadedFile = loadedFile;
        }


    }
}