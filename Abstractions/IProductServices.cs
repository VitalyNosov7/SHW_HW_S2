using StoreMarket.Contracts.Requests;
using StoreMarket.Contracts.Responses;

namespace StoreMarket.Abstractions
{
    public interface IProductServices
    {
        public int AddProduct(ProductCreateRequest product);

        public IEnumerable<ProductResponse> GetProducts();

        public ProductResponse GetProductById(int id);
    }
}
