using E_CommerceAPI.API.Controllers;
using E_CommerceAPI.Application.Repositories;
using E_CommerceAPI.Application.ViewModels.Product;
using E_CommerceAPI.Domain.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace E_CommerceAPI.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TestsController : ControllerBase
    {
        private readonly IProductReadRepository _productReadRepository;
        private readonly IProductWriteRepository _productWriteRepository;
        public TestsController(IProductWriteRepository productWriteRepository, IProductReadRepository productReadRepository)
        {
            _productWriteRepository = productWriteRepository;
            _productReadRepository = productReadRepository;
        }

        [HttpGet]
        public IActionResult ProductList()
        {
            var products = _productReadRepository.GetAll(false);

            return Ok(products);
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
            await _productWriteRepository.SaveAsync();

            return StatusCode((int)HttpStatusCode.NoContent);
        }
    }
}
