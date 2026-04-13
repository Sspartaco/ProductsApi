using Products.Library.Contracts.DTO;
using Products.Library.Contracts.Helpers;

namespace Products.Library.Contracts.Services;

public interface IProductService
{
    Task<OperationResult<IEnumerable<ProductDto>>> GetAllAsync();
    Task<OperationResult<ProductDto>> GetByIdAsync(int id);
    Task<OperationResult<ProductDto>> CreateAsync(CreateProductDto dto);
    Task<OperationResult<ProductDto>> UpdateAsync(int id, UpdateProductDto dto);
    Task<OperationResult<bool>> DeleteAsync(int id);
}
