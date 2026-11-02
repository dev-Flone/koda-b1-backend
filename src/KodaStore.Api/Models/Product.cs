namespace KodaStore.Api.Models;

public sealed class Product
{
    public string ProductCode { get; }
    public string Name { get; }

    public long Price { get; }
    public int Stock { get; }

    public string Category { get; }

    public Product(string productCode, string name, long price, int stock, string category)
    {
        if (string.IsNullOrWhiteSpace(productCode))
        {
            throw new ArgumentException("Product code is required.", nameof(productCode));
        }
        if (string.IsNullOrWhiteSpace(name))
        {
            throw new ArgumentException("Name is required.", nameof(name));
        }
        if (price < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(price), "Price cannot be negative.");
        }
        if (stock < 0)
        {
            throw new ArgumentException("Stock cannot be negative.", nameof(stock));
        }
        {
        }
        if (string.IsNullOrWhiteSpace(category))
        {
            throw new ArgumentException("Category is required.", nameof(category));
        }

        ProductCode = productCode;
        Name = name;
        Price = price;
        Stock = stock;
        Category = category.Trim();
    }
}