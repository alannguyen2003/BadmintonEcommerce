using System.Linq.Expressions;
using BadmintonEcommerce.Application.Abstraction.Authentication;
using BadmintonEcommerce.Application.Abstraction.Repositories;
using BadmintonEcommerce.Application.Features.Authentication.Login;
using BadmintonEcommerce.Domain.Entities.Authentication;
using BadmintonEcommerce.Domain.Errors;
using BadmintonEcommerce.Tests.Helpers.Builders.Commands.Authentication;
using BadmintonEcommerce.Tests.Helpers.Builders.Entities;
using FluentAssertions;
using Moq;
using SharedKernel.Patterns;

namespace BadmintonEcommerce.Tests.Applications.Features.Authentication;

public class LoginTest
{
    private readonly Mock<IAccountRepository> _accountRepositoryMock;

    private readonly Mock<ITokenProvider> _tokenProviderMock;

    private readonly Mock<IPasswordHasher> _passwordHasherMock;

    private readonly LoginCommandHandler _handler;

    public LoginTest()
    {
        _accountRepositoryMock = new Mock<IAccountRepository>();

        _tokenProviderMock = new Mock<ITokenProvider>();

        _passwordHasherMock = new Mock<IPasswordHasher>();

        _handler = new LoginCommandHandler(
            _accountRepositoryMock.Object,
            _tokenProviderMock.Object,
            _passwordHasherMock.Object);
    }

    [Fact]
    public async Task Handle_ValidCredentials_ShouldReturnToken()
    {
        // Arrange
        LoginCommand command = new LoginCommandBuilder()
            .WithEmail("admin@gmail.com")
            .WithPassword("12345678")
            .Build();

        Account account = new AccountBuilder()
            .WithId(Guid.NewGuid())
            .WithEmail("admin@gmail.com")
            .WithPasswordHashed("password-hashed")
            .Build();

        List<Role> roles = new List<Role>
        {
            new Role()
            {
                Id = Guid.NewGuid(),
                Name = "Admin"
            }
        };

        const string expectedToken = "jwt-token";

        _accountRepositoryMock
            .Setup(x => x.Get(
                It.IsAny<Expression<Func<Account, bool>>>(),
                null,
                "AccountRoles",
                null,
                null))
            .ReturnsAsync(new List<Account>
            {
                account
            });

        _passwordHasherMock
            .Setup(x => x.Verify(
                command.Password,
                account.PasswordHashed))
            .Returns(true);

        _accountRepositoryMock
            .Setup(x => x.GetAccountRoles(account.Id))
            .ReturnsAsync(roles);

        _tokenProviderMock
            .Setup(x => x.Create(account, roles))
            .Returns(expectedToken);

        // Act
        Result<string> result = await _handler.Handle(
            command,
            CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();

        result.Value.Should().Be(expectedToken);

        _accountRepositoryMock.Verify(
            x => x.Get(
                It.IsAny<Expression<Func<Account, bool>>>(),
                null,
                "AccountRoles",
                null,
                null),
            Times.Once);

        _passwordHasherMock.Verify(
            x => x.Verify(
                command.Password,
                account.PasswordHashed),
            Times.Once);

        _accountRepositoryMock.Verify(
            x => x.GetAccountRoles(account.Id),
            Times.Once);

        _tokenProviderMock.Verify(
            x => x.Create(account, roles),
            Times.Once);
    }

    [Fact]
    public async Task Handle_EmailNotExists_ShouldReturnFailure()
    {
        // Arrange
        var command = new LoginCommandBuilder()
            .WithEmail("admin@gmail.com")
            .WithPassword("password-hashed")
            .Build();

        _accountRepositoryMock
            .Setup(x => x.Get(
                It.IsAny<Expression<Func<Account, bool>>>(),
                It.IsAny<Func<IQueryable<Account>,
                    IOrderedQueryable<Account>>>(),
                It.IsAny<string>(),
                It.IsAny<int?>(),
                It.IsAny<int?>()))
            .ReturnsAsync(new List<Account>());

        // Act
        Result<string> result = await _handler.Handle(
            command,
            CancellationToken.None);

        // Assert
        result.IsFailure.Should().BeTrue();

        result.Error.Should().BeEquivalentTo(
            AuthenticationError.EmailNotExists(command.Email));

        _passwordHasherMock.Verify(
            x => x.Verify(
                It.IsAny<string>(),
                It.IsAny<string>()),
            Times.Never);

        _tokenProviderMock.Verify(
            x => x.Create(
                It.IsAny<Account>(),
                It.IsAny<List<Role>>()),
            Times.Never);

        _accountRepositoryMock.Verify(
            x => x.GetAccountRoles(It.IsAny<Guid>()),
            Times.Never);
    }

    [Fact]
    public async Task Handle_InvalidPassword_ShouldReturnFailure()
    {
        // Arrange
        var command = new LoginCommandBuilder()
            .WithEmail("admin@gmail.com")
            .WithPassword("12345678")
            .Build();
        

        var account = new AccountBuilder()
            .WithId(Guid.NewGuid())
            .WithEmail("admin@gmail.com")
            .WithPasswordHashed("hashed-password")
            .Build();

        _accountRepositoryMock
            .Setup(x => x.Get(
                It.IsAny<Expression<Func<Account, bool>>>(),
                null,
                "AccountRoles",
                null,
                null))
            .ReturnsAsync(new List<Account>
            {
                account
            });

        _passwordHasherMock
            .Setup(x => x.Verify(
                command.Password,
                account.PasswordHashed))
            .Returns(false);

        // Act
        Result<string> result = await _handler.Handle(
            command,
            CancellationToken.None);

        // Assert
        result.IsFailure.Should().BeTrue();

        result.Error.Should().BeEquivalentTo(
            AuthenticationError.EmailOrPasswordIsWrong());

        _tokenProviderMock.Verify(
            x => x.Create(
                It.IsAny<Account>(),
                It.IsAny<List<Role>>()),
            Times.Never);

        _accountRepositoryMock.Verify(
            x => x.GetAccountRoles(It.IsAny<Guid>()),
            Times.Never);
    }
}