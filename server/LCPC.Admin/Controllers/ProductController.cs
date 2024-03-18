using LCPC.Domain.Commands;
using LCPC.Domain.Entities;
using LCPC.Domain.QueriesDtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LCPC.Admin.Controllers;

[ApiExplorerSettings(GroupName ="产品服务")]
[Route("api/[controller]")]
[ApiController]
[Authorize]
public class ProductController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly IProductQueries _productQueries;

    public ProductController(IMediator mediator, IProductQueries productQueries)
    {
        _mediator = mediator;
        _productQueries = productQueries;
    }

    [HttpGet("getProductCates")]
    public async Task<ReturnResult<List<ProductCatesDto>>> GetProductCates()
        => await _productQueries.GetProductCates();

    [HttpPost("getProductPages")]
    public async Task<ReturnResult<List<ProductDto>>> GetProductPages([FromBody] ProductSearch search)
        => await _productQueries.GetProductPages(search);

    [HttpPost("createProduct")]
    public async Task<ReturnResult> CreateProduct([FromBody] CreateProductCommand command)
        => await _mediator.Send(command);

    [HttpPost("updateProduct")]
    public async Task<ReturnResult> UpdateProduct([FromBody] UpdateProductCommand command)
        => await _mediator.Send(command);

    [HttpDelete("deleteProduct")]
    public async Task<ReturnResult> DeleteProduct([FromBody] string[] ids)
    {
        var cmd = new DeleteProductCommand();
        cmd.AddIds(ids);
        return await _mediator.Send(cmd);
    }

    [HttpPost("updateProductStatus/{id}")]
    public async Task<ReturnResult> UpdateProductStatus(string id)
    {
        var cmd = new UpdateProductStatusCommand();
        cmd.AddId(id);
        return await _mediator.Send(cmd);
    }

    [HttpPost("getProductSellPage")]
    public async Task<ReturnResult<List<ProduceSellDto>>> GetProductSellPage([FromBody]DataSearch search)
        => await _productQueries.GetProductSellPage(search);

    [HttpPost("checkProduct")]
    public async Task<ReturnResult> CheckProductCount([FromBody]List<ProductCheck> checks)
        => await _productQueries.CheckProductCount(checks);

    [HttpGet("searchProduct")]
   public async Task<ReturnResult<List<ProductForInpush>>> GetEnableProdcutSqls(string name = "")
        => await _productQueries.GetEnableProdcutSqls(name);
}