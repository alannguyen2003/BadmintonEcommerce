using System.Linq.Expressions;
using BadmintonEcommerce.Application.Abstraction.Repositories;
using BadmintonEcommerce.Application.Features.Product.ChangePrimaryImage;
using BadmintonEcommerce.Domain.Entities.Catalog;
using BadmintonEcommerce.Domain.Errors;
using BadmintonEcommerce.Tests.Helpers.Builders.Commands.Catalog;
using BadmintonEcommerce.Tests.Helpers.Builders.Entities;
using FluentAssertions;
using Moq;
using SharedKernel.Patterns;

namespace BadmintonEcommerce.Tests.Applications.Features.Catalog;

public class ChangePrimaryImageTest
{
    private readonly Mock<IProductRepository>
        _productRepositoryMock;

    private readonly Mock<IProductImageRepository>
        _productImageRepositoryMock;

    private readonly ChangePrimaryImageCommandHandler
        _handler;

    public ChangePrimaryImageTest()
    {
        _productRepositoryMock =
            new Mock<IProductRepository>();

        _productImageRepositoryMock =
            new Mock<IProductImageRepository>();

        _handler =
            new ChangePrimaryImageCommandHandler(
                _productRepositoryMock.Object,
                _productImageRepositoryMock.Object);
    }

    [Fact]
    public async Task Handle_ImageNotFound_ShouldReturnFailure()
    {
        // Arrange
        Guid productId = Guid.NewGuid();
        Guid imageId = Guid.NewGuid();

        var command = new ChangePrimaryImageCommandBuilder()
            .WithProductId(productId)
            .WithImageId(imageId)
            .Build();

        _productImageRepositoryMock
            .Setup(x => x.GetById(imageId))
            .Returns((ProductImage?)null);

        // Act
        Result result =
            await _handler.Handle(
                command,
                CancellationToken.None);

        // Assert
        result.IsFailure.Should().BeTrue();

        result.Error.Should().BeEquivalentTo(
            ProductImageError.NotFound(imageId));

        _productImageRepositoryMock.Verify(
            x => x.SaveChangesAsync(),
            Times.Never);
    }

    [Fact]
    public async Task Handle_ValidImage_ShouldUpdatePrimaryImageSuccessfully()
    {
        // Arrange
        Guid productId = Guid.NewGuid();

        Guid primaryImageId = Guid.NewGuid();

        Guid secondaryImageId = Guid.NewGuid();

        var command = new ChangePrimaryImageCommandBuilder()
            .WithProductId(productId)
            .WithImageId(primaryImageId)
            .Build();

        var primaryImage = new ProductImageBuilder()
            .WithId(primaryImageId)
            .WithProductId(productId)
            .WithIsPrimary(false)
            .Build();

        var secondaryImage = new ProductImageBuilder()
            .WithId(secondaryImageId)
            .WithId(productId)
            .WithIsPrimary(true)
            .Build();

        var images = new List<ProductImage>
        {
            primaryImage,
            secondaryImage
        };

        _productImageRepositoryMock
            .Setup(x => x.GetById(primaryImageId))
            .Returns(primaryImage);

        _productImageRepositoryMock
            .Setup(x => x.Get(
                It.IsAny<
                    Expression<Func<ProductImage, bool>>>(),
                null,
                "",
                null,
                null))
            .ReturnsAsync(images);

        // Act
        Result result =
            await _handler.Handle(
                command,
                CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();

        primaryImage.IsPrimary.Should().BeTrue();

        secondaryImage.IsPrimary.Should().BeFalse();

        _productImageRepositoryMock.Verify(
            x => x.SaveChangesAsync(),
            Times.Once);
    }

    [Fact]
    public async Task Handle_WhenOnlyOneImage_ShouldSetItAsPrimary()
    {
        // Arrange
        Guid productId = Guid.NewGuid();

        Guid imageId = Guid.NewGuid();

        var command = new ChangePrimaryImageCommandBuilder()
            .WithProductId(productId)
            .WithImageId(imageId)
            .Build();
        
        var image = new ProductImageBuilder()
            .WithId(imageId)
            .WithProductId(productId)
            .WithIsPrimary(false)
            .Build();

        var images = new List<ProductImage>
        {
            image
        };

        _productImageRepositoryMock
            .Setup(x => x.GetById(imageId))
            .Returns(image);

        _productImageRepositoryMock
            .Setup(x => x.Get(
                It.IsAny<
                    Expression<Func<ProductImage, bool>>>(),
                null,
                "",
                null,
                null))
            .ReturnsAsync(images);

        // Act
        Result result =
            await _handler.Handle(
                command,
                CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();

        image.IsPrimary.Should().BeTrue();

        _productImageRepositoryMock.Verify(
            x => x.SaveChangesAsync(),
            Times.Once);
    }

    [Fact]
    public async Task Handle_ShouldSetOnlyOnePrimaryImage()
    {
        // Arrange
        Guid productId = Guid.NewGuid();

        Guid selectedImageId = Guid.NewGuid();

        var images = new List<ProductImage>
        {
            new ProductImageBuilder()
                .WithId(selectedImageId)
                .WithProductId(productId)
                .WithIsPrimary(false)
                .Build(),
            new ProductImageBuilder()
                .WithId(Guid.NewGuid())
                .WithProductId(productId)
                .WithIsPrimary(true)
                .Build(),
            new ProductImageBuilder()
                .WithId(Guid.NewGuid())
                .WithProductId(productId)
                .WithIsPrimary(true)
                .Build()
        };
        
        var command = new ChangePrimaryImageCommandBuilder()
            .WithProductId(productId)
            .WithImageId(selectedImageId)
            .Build();

        _productImageRepositoryMock
            .Setup(x => x.GetById(selectedImageId))
            .Returns(images.First());

        _productImageRepositoryMock
            .Setup(x => x.Get(
                It.IsAny<
                    Expression<Func<ProductImage, bool>>>(),
                null,
                "",
                null,
                null))
            .ReturnsAsync(images);

        // Act
        Result result =
            await _handler.Handle(
                command,
                CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();

        images.Count(x => x.IsPrimary)
            .Should().Be(1);

        images.First(x => x.Id == selectedImageId)
            .IsPrimary.Should().BeTrue();
    }
}