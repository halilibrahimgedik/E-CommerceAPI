using E_CommerceAPI.Application.Abstraction.Storage.Local;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_CommerceAPI.Infrustructure.Services.Storage.Local
{
    public class LocalStorage : ILocalStorage
    {
        private readonly IWebHostEnvironment _webHostEnvironment;
        public LocalStorage(IWebHostEnvironment webHostEnvironment)
        {
            _webHostEnvironment = webHostEnvironment;
        }


        public async Task DeleteAsync(string path, string fileName)
            => File.Delete($"{path}/{fileName}");

        public bool HasFile(string path, string fileName)
           => File.Exists($"{path}/{fileName}");

        public List<string> GetAllFiles(string path)
        {
            DirectoryInfo directoryInfo = new DirectoryInfo(path);
            return directoryInfo.GetFiles().Select(f => f.Name).ToList();
        }

       
        public async Task<List<(string fileName, string pathOrContainerName)>> UploadAsync(string path, IFormFileCollection files)
        {
            List<(string fileName, string path)> savedFilesDatas = new();

            var directoryPathToUpload = Path.Combine(_webHostEnvironment.WebRootPath, path);

            if (!Directory.Exists(directoryPathToUpload))
                Directory.CreateDirectory(directoryPathToUpload);

            foreach (IFormFile file in files)
            {
                string fileFullPath = Path.Combine(directoryPathToUpload, file.FileName);

                bool result = await CopyFileAsync(fileFullPath, file);

                savedFilesDatas.Add((file.FileName, $"{path}/{file.FileName}")); //bir dictionary de dönebilirdik key = newFileName value = path gibi
            }

            //todo eğer yukarıdaki if geçerli değilse burada dosyaların sunucuya yüklenirken hata alındığında dair uyarıcı bir exception fırlatacağız.
            return savedFilesDatas;
        }

        async Task<bool> CopyFileAsync(string fullPath, IFormFile file)
        {
            try
            {
                await using var fileStream = new FileStream(fullPath, FileMode.Create, FileAccess.Write, FileShare.None, 1024 * 1024, useAsync: false);

                await file.CopyToAsync(fileStream);
                await fileStream.FlushAsync();

                return true;
            }
            catch (Exception ex)
            {
                //todo Log!
                throw ex;
            }
        }


    }
}
