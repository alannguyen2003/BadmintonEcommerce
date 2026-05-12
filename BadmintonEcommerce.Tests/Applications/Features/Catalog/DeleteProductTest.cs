using BadmintonEcommerce.Application.Abstraction.Repositories;
using BadmintonEcommerce.Application.Features.Product.Delete;
using BadmintonEcommerce.Domain.Entities.Catalog;
using BadmintonEcommerce.Tests.Helpers.Builders.Commands.Catalog;
using FluentAssertions;
using Moq;
using SharedKernel.Patterns;

namespace BadmintonEcommerce.Tests.Applications.Features.Catalog;

public class DeleteProductTest
{
    private readonly Mock<IProductRepository>
        _productRepositoryMock;

    private readonly DeleteProductCommandHandler
        _handler;

    public DeleteProductTest()
    {
        _productRepositoryMock =
            new Mock<IProductRepository>();

        _handler =
            new DeleteProductCommandHandler(
                _productRepositoryMock.Object);
    }

    [Fact]
    public async Task Handle_ProductNotFound_ShouldReturnFailure()
    {
        // Arrange
        var command = new DeleteProductCommandBuilder()
            .WithProductId(Guid.NewGuid())
            .Build();

        _productRepositoryMock
            .Setup(x => x.GetById(command.ProductId))
            .Returns((Product)null);

        // Act
        Result result = await _handler.Handle(
            command,
            CancellationToken.None);

        // Assert
        result.IsFailure.Should().BeTrue();

        _productRepositoryMock.Verify(
            x => x.Delete(It.IsAny<Product>()),
            Times.Never);

        _productRepositoryMock.Verify(
            x => x.SaveChangesAsync(),
            Times.Never);
    }

    [Fact]
    public async Task Handle_ValidProduct_ShouldDeleteProduct()
    {
        // Arrange
        var product = new Product
        {
            Id = Guid.NewGuid(),
            Name = "Yonex Astrox"
        };

        var command = new DeleteProductCommandBuilder()
            .WithProductId(Guid.NewGuid())
            .Build();

        _productRepositoryMock
            .Setup(x => x.GetById(command.ProductId))
            .Returns(product);

        // Act
        Result result = await _handler.Handle(
            command,
            CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();

        _productRepositoryMock.Verify(
            x => x.Delete(product),
            Times.Once);

        _productRepositoryMock.Verify(
            x => x.SaveChangesAsync(),
            Times.Once);
    }

    [Fact]
    public async Task Handle_ValidProduct_ShouldCallDeleteBeforeSaveChanges()
    {
        // Arrange
        var product = new Product
        {
            Id = Guid.NewGuid(),
            Name = "Victor Thruster"
        };

        var command = new DeleteProductCommandBuilder()
            .WithProductId(Guid.NewGuid())
            .Build();

        var sequence = new MockSequence();

        _productRepositoryMock
            .InSequence(sequence)
            .Setup(x => x.Delete(product))
            .Returns(Task.CompletedTask);

        _productRepositoryMock
            .InSequence(sequence)
            .Setup(x => x.SaveChangesAsync())
            .Returns(Task.CompletedTask);

        _productRepositoryMock
            .Setup(x => x.GetById(command.ProductId))
            .Returns(product);

        // Act
        await _handler.Handle(
            command,
            CancellationToken.None);

        // Assert
        _productRepositoryMock.Verify(
            x => x.Delete(product),
            Times.Once);

        _productRepositoryMock.Verify(
            x => x.SaveChangesAsync(),
            Times.Once);
    }
}