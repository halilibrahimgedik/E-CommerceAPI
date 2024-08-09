using E_CommerceAPI.Application.Abstraction.Storage;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_CommerceAPI.Infrustructure.Services.Storage
{
    // mimarinin'In kullanacağı servis
    public class StorageService : IStorageService
    {
        private readonly IStorage _storage;
        public StorageService(IStorage storage)
        {
            _storage = storage;
        }

        // Dosyalardaki Storage property'sini merkezi olarak yönetip değerini verebilmek için, mimaride o anda hangi storage servisi kullanılıyorsa
        // onun ismini dönderecek bize
        public string StorageName { get => _storage.GetType().Name; }

        public Task DeleteAsync(string pathOrContainerName, string fileName)
            => _storage.DeleteAsync(pathOrContainerName, fileName);

        public List<string> GetAllFiles(string pathOrContainerName)
            => _storage.GetAllFiles(pathOrContainerName);

        public bool HasFile(string pathOrContainerName, string fileName)
            => _storage.HasFile(pathOrContainerName, fileName);

        public Task<List<(string fileName, string pathOrContainerName)>> UploadAsync(string pathOrContainerName, IFormFileCollection files)
            => _storage.UploadAsync(pathOrContainerName, files);
    }
}
