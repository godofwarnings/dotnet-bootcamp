using ProductApiEF.Models;
using System.Collections.Generic;

namespace ProductApiEF.Services
{
    public interface IProductService
    {
        List<Product> GetProducts();
        Product? GetProductById(int productId);
    }
}
