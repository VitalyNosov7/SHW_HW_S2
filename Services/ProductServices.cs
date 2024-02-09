using AutoMapper;
using Microsoft.Extensions.Caching.Memory;
using StoreMarket.Abstractions;
using StoreMarket.Contexts;
using StoreMarket.Contracts.Requests;
using StoreMarket.Contracts.Responses;
using StoreMarket.Models;

namespace StoreMarket.Services
{
    public class ProductServices : IProductServices
    {
        private readonly StoreContext _storeContext;
        private readonly IMapper _mapper;
        private readonly IMemoryCache _memoryCache;


        public ProductServices(StoreContext storeContext, IMapper mapper, IMemoryCache memoryCache)
        {
            _storeContext = storeContext;
            _mapper = mapper;
            _memoryCache = memoryCache;
        }

        

        public int AddProduct(ProductCreateRequest product)
        {
            var mappEntity = _mapper.Map<Product>(product);

            _storeContext.Products.Add(mappEntity);

            _storeContext.SaveChanges();

            _memoryCache.Remove("products");

            return mappEntity.Id;
        }

        public ProductResponse GetProductById(int id)
        {
            var product = _storeContext.Products.FirstOrDefault(p => p.Id == id);

            if (product == null)
            {
                return null;
            }
            return _mapper.Map<ProductResponse>(product);
        }

        public IEnumerable<ProductResponse> GetProducts()
        {


            if (_memoryCache.TryGetValue("products", out IEnumerable<ProductResponse> prods ) )
            {
                return prods;
            }

            IEnumerable<ProductResponse> products = _storeContext.Products.Select(_mapper.Map<ProductResponse>).ToList();

            _memoryCache.Set("products", products, TimeSpan.FromMinutes(30));

            return products;
        }



    }
}
