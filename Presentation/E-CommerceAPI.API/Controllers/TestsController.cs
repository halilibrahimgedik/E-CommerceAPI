using E_CommerceAPI.API.Controllers;
using E_CommerceAPI.Application.Repositories;
using E_CommerceAPI.Domain.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

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
            var products = _productReadRepository.GetAll();

            return Ok(products);
        }

        [HttpPost]
        public async Task<IActionResult> AddProduct()
        {
            var p = new Product()
            {
                Name = "Iphone 15",
                Price = 99.99,
                Stock = 15,
            };

            await _productWriteRepository.AddAsync(p);
            await _productWriteRepository.SaveAsync();

            return Ok(p);
        }

        [HttpPut]
        public async Task<IActionResult> UpdateProduct(string productId, string name, double price, int stock)
        {
            var p = await _productReadRepository.GetByIdAsync(productId);

            p.Name = name;
            p.Price = price;
            p.Stock = stock;

            _productWriteRepository.Update(p);
            await _productWriteRepository.SaveAsync();

            return Ok(p);
        }
    }
}
