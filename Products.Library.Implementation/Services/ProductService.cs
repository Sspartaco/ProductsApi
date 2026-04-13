using AutoMapper;
using Products.Infrastructure.Contracts.Common;
using Products.Library.Contracts.DTO;
using Products.Library.Contracts.Helpers;
using Products.Library.Contracts.Services;

namespace Products.Library.Implementation.Services;

public class ProductService(IUnitOfWork unitOfWork, IMapper mapper) : IProductService
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    private readonly IMapper _mapper = mapper;

    public async Task<OperationResult<IEnumerable<ProductDto>>> GetAllAsync()
    {
        var products = await _unitOfWork.Products.GetAllViaSPAsync();
        var dtos = _mapper.Map<IEnumerable<ProductDto>>(products);
        return new OperationResult<IEnumerable<ProductDto>>().AddResult(dtos);
    }

    public async Task<OperationResult<ProductDto>> GetByIdAsync(int id)
    {
        var product = await _unitOfWork.Products.GetByIdViaSPAsync(id);

        if (product is null)
            return new OperationResult<ProductDto>().AddNotFound($"Product with id {id} was not found.");

        return new OperationResult<ProductDto>().AddResult(_mapper.Map<ProductDto>(product));
    }

    public async Task<OperationResult<ProductDto>> CreateAsync(CreateProductDto dto)
    {
        await _unitOfWork.Products.CreateViaSPAsync(dto.Name, dto.Description, dto.Price);

        var created = await _unitOfWork.Products.GetFirstOrDefaultAsync(
            p => p.Name == dto.Name && p.Price == dto.Price);

        return new OperationResult<ProductDto>().AddResult(_mapper.Map<ProductDto>(created));
    }

    public async Task<OperationResult<ProductDto>> UpdateAsync(int id, UpdateProductDto dto)
    {
        var exists = await _unitOfWork.Products.AnyAsync(p => p.Id == id);

        if (!exists)
            return new OperationResult<ProductDto>().AddNotFound($"Product with id {id} was not found.");

        await _unitOfWork.Products.UpdateViaSPAsync(id, dto.Name, dto.Description, dto.Price);

        var updated = await _unitOfWork.Products.GetByIdViaSPAsync(id);
        return new OperationResult<ProductDto>().AddResult(_mapper.Map<ProductDto>(updated));
    }

    public async Task<OperationResult<bool>> DeleteAsync(int id)
    {
        var exists = await _unitOfWork.Products.AnyAsync(p => p.Id == id);

        if (!exists)
            return new OperationResult<bool>().AddNotFound($"Product with id {id} was not found.");

        await _unitOfWork.Products.DeleteViaSPAsync(id);
        return new OperationResult<bool>().AddResult(true);
    }
}
