using E_CommerceAPI.API.Controllers;
using E_CommerceAPI.Application.Repositories;
using E_CommerceAPI.Application.RequestParameters;
using E_CommerceAPI.Application.ViewModels.Product;
using E_CommerceAPI.Domain.Entities;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Net;

namespace E_CommerceAPI.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TestsController : ControllerBase
    {
        private readonly IProductReadRepository _productReadRepository;
        private readonly IProductWriteRepository _productWriteRepository;
        private readonly IWebHostEnvironment _webHostEnvironment;
        public TestsController(
            IProductWriteRepository productWriteRepository, 
            IProductReadRepository productReadRepository,
            IWebHostEnvironment webHostEnvironment)
        {
            _productWriteRepository = productWriteRepository;
            _productReadRepository = productReadRepository;
            this._webHostEnvironment = webHostEnvironment;
        }

        [HttpGet]
        public async Task<IActionResult> ProductList([FromQuery]Pagination pagination)
        {
            var products = await _productReadRepository.GetAll(false).Select( p=> new
            {
                p.Id,
                p.Name,
                p.Stock,
                p.Price,
                p.CreatedDate,
                p.UpdatedDate,
            }).Skip(pagination.Page * pagination.SizePerPage).Take(pagination.SizePerPage).ToListAsync();

            var totalCount = _productReadRepository.GetAll(false).Count();

            return Ok(new { products, totalCount });
        }

        [HttpGet("{id}")]
        public IActionResult GetProductById(string id)
        {
            var products = _productReadRepository.GetByIdAsync(id, false);

            return Ok(products);
        }

        [HttpPost]
        public async Task<IActionResult> AddProduct(CreateProductVM createProductVM)
        {
            await _productWriteRepository.AddAsync(new()
            {
                Name = createProductVM.Name,
                Stock = createProductVM.Stock,
                Price = createProductVM.Price,
            });

            await _productWriteRepository.SaveAsync();

            return StatusCode((int)HttpStatusCode.Created);
        }

        [HttpPut]
        public async Task<IActionResult> UpdateProduct(UpdateProductVM updateProductVM)
        {
            var p = await _productReadRepository.GetByIdAsync(updateProductVM.Id);

            p.Name = updateProductVM.Name;
            p.Price = updateProductVM.Price;
            p.Stock = updateProductVM.Stock;

            await _productWriteRepository.SaveAsync();

            return Ok(p);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProduct(string id)
        {
            await _productWriteRepository.RemoveAsync(id);
            var a = await _productWriteRepository.SaveAsync();

            return Ok();
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> Upload()
        {
            //wwwroot/resource/product-images
            string uploadPath = Path.Combine(_webHostEnvironment.WebRootPath, "images/product-images");

            if (!Directory.Exists(uploadPath))
                Directory.CreateDirectory(uploadPath);

            Random r = new();
            foreach (IFormFile file in Request.Form.Files)
            {
                string fullPath = Path.Combine(uploadPath, $"{r.Next()}{Path.GetExtension(file.FileName)}");

                using FileStream fileStream = new(fullPath, FileMode.Create, FileAccess.Write, FileShare.None, 1024 * 1024, useAsync: false);
                await file.CopyToAsync(fileStream);
                await fileStream.FlushAsync();
            }
            return Ok();
        }
    }
}
