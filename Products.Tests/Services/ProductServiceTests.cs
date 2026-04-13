using AutoMapper;
using Products.Infrastructure.Contracts.Common;
using Products.Infrastructure.Contracts.Entities;
using Products.Infrastructure.Contracts.Repositories;
using Products.Library.Contracts.DTO;
using Products.Library.Implementation.Mappers.Profiles;
using Products.Library.Implementation.Services;
using FluentAssertions;
using Moq;

namespace Products.Tests.Services;

public class ProductServiceTests
{
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;
    private readonly Mock<IProductRepository> _repoMock;
    private readonly IMapper _mapper;
    private readonly ProductService _sut;

    public ProductServiceTests()
    {
        _repoMock = new Mock<IProductRepository>();
        _unitOfWorkMock = new Mock<IUnitOfWork>();
        _unitOfWorkMock.Setup(u => u.Products).Returns(_repoMock.Object);

        var mapperConfig = new MapperConfiguration(cfg => cfg.AddProfile<ProductProfile>());
        _mapper = mapperConfig.CreateMapper();

        _sut = new ProductService(_unitOfWorkMock.Object, _mapper);
    }

    [Fact]
    public async Task GetAllAsync_ReturnsAllProducts()
    {
        // Arrange
        var products = new List<Product>
        {
            new() { Id = 1, Name = "Laptop", Description = "Fast", Price = 999.99m, CreatedDate = DateTime.UtcNow },
            new() { Id = 2, Name = "Mouse",  Description = null,   Price = 29.99m,  CreatedDate = DateTime.UtcNow }
        };
        _repoMock.Setup(r => r.GetAllViaSPAsync()).ReturnsAsync(products);

        // Act
        var result = await _sut.GetAllAsync();

        // Assert
        result.Success.Should().BeTrue();
        result.Result.Should().HaveCount(2);
    }

    [Fact]
    public async Task GetByIdAsync_ExistingId_ReturnsProduct()
    {
        // Arrange
        var product = new Product { Id = 1, Name = "Laptop", Price = 999.99m, CreatedDate = DateTime.UtcNow };
        _repoMock.Setup(r => r.GetByIdViaSPAsync(1)).ReturnsAsync(product);

        // Act
        var result = await _sut.GetByIdAsync(1);

        // Assert
        result.Success.Should().BeTrue();
        result.Result!.Id.Should().Be(1);
        result.Result.Name.Should().Be("Laptop");
    }

    [Fact]
    public async Task GetByIdAsync_NonExistentId_ReturnsNotFound()
    {
        // Arrange
        _repoMock.Setup(r => r.GetByIdViaSPAsync(99)).ReturnsAsync((Product?)null);

        // Act
        var result = await _sut.GetByIdAsync(99);

        // Assert
        result.Success.Should().BeFalse();
        result.StatusCode.Should().Be(System.Net.HttpStatusCode.NotFound);
        result.Errors.Should().ContainSingle(e => e.Contains("99"));
    }

    [Fact]
    public async Task CreateAsync_ValidDto_ReturnsCreatedProduct()
    {
        // Arrange
        var dto = new CreateProductDto { Name = "Keyboard", Description = "Mechanical", Price = 89.99m };
        var created = new Product { Id = 3, Name = "Keyboard", Description = "Mechanical", Price = 89.99m, CreatedDate = DateTime.UtcNow };

        _repoMock.Setup(r => r.CreateViaSPAsync(dto.Name, dto.Description, dto.Price))
                 .Returns(Task.CompletedTask);
        _repoMock.Setup(r => r.GetFirstOrDefaultAsync(It.IsAny<System.Linq.Expressions.Expression<Func<Product, bool>>>()))
                 .ReturnsAsync(created);

        // Act
        var result = await _sut.CreateAsync(dto);

        // Assert
        result.Success.Should().BeTrue();
        result.Result!.Name.Should().Be("Keyboard");
        result.Result.Price.Should().Be(89.99m);
    }

    [Fact]
    public async Task UpdateAsync_NonExistentId_ReturnsNotFound()
    {
        // Arrange
        _repoMock.Setup(r => r.AnyAsync(It.IsAny<System.Linq.Expressions.Expression<Func<Product, bool>>>()))
                 .ReturnsAsync(false);

        var dto = new UpdateProductDto { Name = "X", Price = 10m };

        // Act
        var result = await _sut.UpdateAsync(99, dto);

        // Assert
        result.Success.Should().BeFalse();
        result.StatusCode.Should().Be(System.Net.HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task DeleteAsync_ExistingId_ReturnsTrue()
    {
        // Arrange
        _repoMock.Setup(r => r.AnyAsync(It.IsAny<System.Linq.Expressions.Expression<Func<Product, bool>>>()))
                 .ReturnsAsync(true);
        _repoMock.Setup(r => r.DeleteViaSPAsync(1)).Returns(Task.CompletedTask);

        // Act
        var result = await _sut.DeleteAsync(1);

        // Assert
        result.Success.Should().BeTrue();
        result.Result.Should().BeTrue();
    }

    [Fact]
    public async Task DeleteAsync_NonExistentId_ReturnsNotFound()
    {
        // Arrange
        _repoMock.Setup(r => r.AnyAsync(It.IsAny<System.Linq.Expressions.Expression<Func<Product, bool>>>()))
                 .ReturnsAsync(false);

        // Act
        var result = await _sut.DeleteAsync(99);

        // Assert
        result.Success.Should().BeFalse();
        result.StatusCode.Should().Be(System.Net.HttpStatusCode.NotFound);
    }
}
