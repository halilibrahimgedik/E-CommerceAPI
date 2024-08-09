using E_CommerceAPI.Application.RequestParameters;
using E_CommerceAPI.Infrustructure.Operations;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_CommerceAPI.Infrustructure.Services
{
    public class FileService
    {
        private readonly IWebHostEnvironment _webHostEnvironment;

        public FileService(IWebHostEnvironment webHostEnvironment)
        {
            _webHostEnvironment = webHostEnvironment;
        }


        private string EditFileNameAsync(string fileName, string directoryPathToUpload)
        {
            string extension = Path.GetExtension(fileName);
            string currentFileName = Path.GetFileNameWithoutExtension(fileName);

            // dosyanın ismini düzeltelim (her dosya adı benzersiz olmalı aksi takdirde sistemde aynı isimde dosya varmı kontrol etmelisin)
            var time = DateTime.UtcNow.AddHours(3).ToString("ddMMyyyyHHmmssfff");
            var fixedName = FixingNameOperation.fixCharacters(currentFileName);
            var newFileName = fixedName + time + extension;

            // Buradan sonra yeni ulşan dosya ile aynı isme sahip bir dosya var mı kontrolüdür. Var ise adına count değeri ekleniyor.
            // normal de dosyaya yeni isimlendirme yapıp newFileName değişkenine atarken yukarıda aynı dosya ismine sahip dosya olmaz çünkü milisaniye cinsinden zamanı da newFileName'e dahil ediyoruz. Fakat Script ile isteklerde aynı milisaniyede gelebilir. O yüzden ne olur olmaz biz işimizi sağlama alalım.
            var pathToSearch = Path.Combine(directoryPathToUpload, newFileName);

            int count = 1;
            while (File.Exists(pathToSearch))
            {
                // Dosya mevcutsa, count ekleyerek ve yeni bir isim oluştur
                newFileName = $"{fixedName}{time}_{count}{extension}";
                pathToSearch = Path.Combine(directoryPathToUpload, newFileName);
                count++;
            }

            return newFileName;
        }

        public async Task<bool> SaveFileAsync(string fullPath, IFormFile file)
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

        public async Task<Dictionary<string, string>> UploadAsync(string path, IFormFileCollection files)
        {
            Dictionary<string, string> SavedFilesInfo = new();
            List<bool> results = new();

            var directoryPathToUpload = Path.Combine(_webHostEnvironment.WebRootPath, path); // ..../images/product-images gibi

            if (!Directory.Exists(directoryPathToUpload))
                Directory.CreateDirectory(directoryPathToUpload);

            foreach (IFormFile file in files)
            {

                // burada dosya ismi alınıp, seo uyumlu isim işlemi gerçekleştirilecek ve yeni dosya ismi döndürelecek
                string newFileName = EditFileNameAsync(file.FileName, directoryPathToUpload);

                string fileFullPath = Path.Combine(directoryPathToUpload, newFileName);

                bool result = await SaveFileAsync(fileFullPath, file);

                //  dosyanın ismini ve path'ini Dictionary olarak kaydedelim ki kullanıcıya kaydedilen dosya adını ve yolunu döndürelim,
                //SavedFilesInfo.Add(newFileName, fileFullPath);
                SavedFilesInfo.Add(newFileName, path+"/"+newFileName); //!! 2. parametre images/product-images/kirimiziaraba090820240049125.jpg gibi olucak
                results.Add(result);
            }

            if (results.TrueForAll(r => r.Equals(true)))
                return SavedFilesInfo;

            //todo eğer yukarıdaki if geçerli değilse burada dosyaların sunucuya yüklenirken hata alındığında dair uyarıcı bir exception fırlatacağız.
            return null;
        }

    }
}
