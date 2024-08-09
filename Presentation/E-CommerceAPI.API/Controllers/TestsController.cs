using E_CommerceAPI.Application.Repositories;
using E_CommerceAPI.Application.RequestParameters;
using E_CommerceAPI.Application.Services;
using E_CommerceAPI.Application.ViewModels.Product;
using E_CommerceAPI.Domain.Entities;
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
        private readonly IFileService _fileService;

        private readonly IFileReadRepository _fileReadRepository;
        private readonly IFileWriteRepository _fileWriteRepository;
        private readonly IInvoiceFileReadRepository _invoiceFileReadRepository;
        private readonly IFileWriteRepository _invoiceFileWriteRepository;
        private readonly IProductImageFileReadRepository _productImageFileReadRepository;
        private readonly IProductImageFileWriteRepository _productImageFileWriteRepository;
        public TestsController(
            IProductWriteRepository productWriteRepository,
            IProductReadRepository productReadRepository,
            IWebHostEnvironment webHostEnvironment,
            IFileService fileService,
            IFileReadRepository fileReadRepository,
            IFileWriteRepository fileWriteRepository,
            IInvoiceFileReadRepository invoiceFileReadRepository,
            IFileWriteRepository invoiceFileWriteRepository,
            IProductImageFileReadRepository productImageFileReadRepository,
            IProductImageFileWriteRepository productImageFileWriteRepository)
        {
            _productWriteRepository = productWriteRepository;
            _productReadRepository = productReadRepository;
            _fileService = fileService;

            _fileReadRepository = fileReadRepository;
            _fileWriteRepository = fileWriteRepository;
            _invoiceFileReadRepository = invoiceFileReadRepository;
            _invoiceFileWriteRepository = invoiceFileWriteRepository;
            _productImageFileReadRepository = productImageFileReadRepository;
            _productImageFileWriteRepository = productImageFileWriteRepository;
        }

        [HttpGet]
        public async Task<IActionResult> ProductList([FromQuery] Pagination pagination)
        {
            var products = await _productReadRepository.GetAll(false).Select(p => new
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
            var datas = await _fileService.UploadAsync("images/product-images", Request.Form.Files);

            await _productImageFileWriteRepository.AddRangeAsync(datas.Select(data => new ProductImageFile()
            {
                FileName = data.Key,
                Path = data.Value,
            }).ToList());

            await _productWriteRepository.SaveAsync();
            return Ok(datas);
        }
    }
}
