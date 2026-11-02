using KodaStore.Api.Models;

namespace KodaStore.Api.Data;

public sealed class InMemoryProductCatalogue
{
    private readonly List<Product> _products = [];

    public IReadOnlyList<Product> GetAll()
    {
        return [.. _products];
    }

    public Product? GetByCode(string productCode)
    {
        return _products.FirstOrDefault(product => string.Equals(product.ProductCode, productCode.Trim(), StringComparison.OrdinalIgnoreCase));
    }

    public void Add(Product product)
    {
        if (_products.Any(p => p.ProductCode == product.ProductCode))
        {
            throw new ArgumentException($"Theres already product with code {product.ProductCode}");
        }
        _products.Add(product);
    }

    public void Delete(string productCode)
    {
        _products.RemoveAll(product => string.Equals(product.ProductCode, productCode.Trim(), StringComparison.OrdinalIgnoreCase));
    }
}