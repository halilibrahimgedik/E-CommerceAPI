using E_CommerceAPI.Application.RequestParameters;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_CommerceAPI.Application.Services
{
    public interface IFileService
    {
        // path =>  ..../images/product-images  || ..../images/category-images
        // files => yüklenicek dosyalar
        // geri dönüş değeri bir dictionary olarak kaydedilen dosya adı ve dosyanın path'i döndürelecek,


        /// <param name="path"> gönderilen dosyaların kaydedileceği dizin / klasör </param>
        /// <param name="files">The collection of files to be uploaded.</param>
        ///<returns>A dictionary where the key is the file name and the value is the full path of the uploaded file.</returns>
        Task<Dictionary<string, string>> UploadAsync(string path, IFormFileCollection files);

        Task<bool> SaveFileAsync(string fullPath, IFormFile file);
    }
}
