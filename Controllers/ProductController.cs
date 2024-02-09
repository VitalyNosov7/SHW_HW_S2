using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using StoreMarket.Abstractions;
using StoreMarket.Contexts;
using StoreMarket.Contracts.Requests;
using StoreMarket.Contracts.Responses;
using StoreMarket.Models;
using System;
using System.Text;
using System.Text.Json;

namespace StoreMarket.Controllers
{

    [ApiController]
    [Route("[controller]")]
    public class ProductController : ControllerBase
    {
        private StoreContext _context;
        private readonly IProductServices _productServices;
        //private IDistributedCache _cache;

        //public ProductController(StoreContext context,IProductServices productServices, IDistributedCache cache)
        public ProductController(StoreContext context, IProductServices productServices)
        {
            _context = context;
            _productServices = productServices;
            //  _cache = cache;
        }

        [HttpGet]
        [Route("products/{id}")]
        public ActionResult<ProductResponse> GetProductById(int id)
        {
            var product = _productServices.GetProductById(id);

            return Ok(product);
        }

        [HttpGet]
        [Route("products")]
        public ActionResult<IEnumerable<ProductResponse>> GetProducts()
        {
            var products = _productServices.GetProducts();

            return Ok(products);
        }

        [HttpPost]
        [Route("create")]
        public ActionResult<int> AddProducts(ProductCreateRequest request)
        {
            try
            {
                var id = _productServices.AddProduct(request);
                return Ok(id);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }

        private string GetCsv(IEnumerable<Product> products)
        {
            StringBuilder sb = new StringBuilder();

            foreach (var product in products)
            {
                sb.AppendLine($"{product.Name} " +
                              $"; {product.Price}" +
                              $" ; {product.Description}" +
                              $" ; {product.Category}\n");
            }

            return sb.ToString();
        }

        [HttpGet]
        [Route("Csv")]
        public FileContentResult GetProductsCsv()
        {

            using (_context)
            {
                var products = _context.Products.Select(x =>
                                                    new Product
                                                    {
                                                        Name = x.Name,
                                                        Description = x.Description,
                                                        Price = x.Price
                                                    }).ToList();

                var content = GetCsv(products);
                return File(new System.Text.UTF8Encoding().GetBytes(content), "text/csv", "report.csv");
            }
        }
    }
}
