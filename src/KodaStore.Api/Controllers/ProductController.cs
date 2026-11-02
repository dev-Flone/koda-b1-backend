using KodaStore.Api.Data;
using KodaStore.Api.Models;
using KodaStore.Api.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace KodaStore.Api.Controllers;

[ApiController]
[Route("api/v1/products")]

public sealed class ProductController(InMemoryProductCatalogue catalogue) : ControllerBase
{
    private readonly InMemoryProductCatalogue _catalogue = catalogue;

    [HttpGet]
    [ProducesResponseType(typeof(IReadOnlyList<ProductResponse>), StatusCodes.Status200OK)]
    public ActionResult<IReadOnlyList<Product>> GetAll()
    {
        var products = _catalogue.GetAll().Select(ProductResponse.FromModel).ToArray();

        return Ok(products);
    }

    [HttpGet("{productCode}")]
    [ProducesResponseType(typeof(ProductResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    public ActionResult<ProductResponse> GetByCode([FromRoute] string productCode)
    {
        var product = _catalogue.GetByCode(productCode);

        if (product is null)
        {
            return Problem(
                statusCode: StatusCodes.Status404NotFound,
                title: "Product not found.",
                detail: $"No product was found for code {productCode}"
            );
        }

        return Ok(ProductResponse.FromModel(product));
    }

    [HttpPost]
    [ProducesResponseType(typeof(ProductResponse), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    public ActionResult<ProductResponse> Create([FromBody] CreateProductRequest request)
    {
        var product = new Product(
            request.ProductCode,
            request.Name,
            request.Price,
            request.Stock,
            request.Category
        );

        _catalogue.Add(product);

        return CreatedAtAction(nameof(GetByCode), new { productCode = product.ProductCode }, ProductResponse.FromModel(product));
    }

    [HttpDelete("{productCode}")]
    [ProducesResponseType(typeof(ProductResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    public ActionResult<ProductResponse> Delete([FromRoute] string productCode)
    {
        var product = _catalogue.GetByCode(productCode);

        if (product is null)
        {
            return Problem(
                statusCode: StatusCodes.Status404NotFound,
                title: "Can't find product.",
                detail: $"There is no product with code {productCode}"
            );
        }

        _catalogue.Delete(productCode);
        return NoContent();
    }
}