﻿using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_CommerceAPI.Application.Abstraction.Storage
{
    public interface IStorage
    {
        
        Task<List<(string fileName, string pathOrContainerName)>> UploadAsync(string pathOrContainerName, IFormFileCollection files);

        Task DeleteAsync(string pathOrContainerName, string fileName);

        List<string> GetAllFiles(string pathOrContainerName);

        bool HasFile(string pathOrContainerName, string fileName);
    }
}
