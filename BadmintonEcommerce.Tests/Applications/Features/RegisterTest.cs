using System.Linq.Expressions;
using BadmintonEcommerce.Application.Abstraction.Authentication;
using BadmintonEcommerce.Application.Abstraction.Repositories;
using BadmintonEcommerce.Application.Features.Authentication.Register;
using BadmintonEcommerce.Domain.Abstraction;
using BadmintonEcommerce.Domain.Entities.Authentication;
using BadmintonEcommerce.Domain.Errors;
using BadmintonEcommerce.Mapper.Abstractions;
using BadmintonEcommerce.Tests.Helpers.Builders.Commands.Authentication;
using BadmintonEcommerce.Tests.Helpers.Builders.Entities;
using FluentAssertions;
using Moq;
using SharedKernel.Patterns;
using SharedKernel.Services;

namespace BadmintonEcommerce.Tests.Applications.Features;

public class RegisterTest
{
    private readonly Mock<IAccountRepository> _accountRepositoryMock;

    private readonly Mock<IRoleRepository> _roleRepositoryMock;

    private readonly Mock<IPasswordHasher> _passwordHasherMock;

    private readonly Mock<IMapper> _mapperMock;

    private readonly Mock<IDateTimeProvider> _dateTimeProviderMock;

    private readonly Mock<ITokenProvider> _tokenProviderMock;

    private readonly RegisterCommandHandler _handler;

    public RegisterTest()
    {
        _accountRepositoryMock = new Mock<IAccountRepository>();

        _roleRepositoryMock = new Mock<IRoleRepository>();

        _passwordHasherMock = new Mock<IPasswordHasher>();

        _mapperMock = new Mock<IMapper>();

        _dateTimeProviderMock = new Mock<IDateTimeProvider>();

        _tokenProviderMock = new Mock<ITokenProvider>();

        _handler = new RegisterCommandHandler(
            _accountRepositoryMock.Object,
            _roleRepositoryMock.Object,
            _passwordHasherMock.Object,
            _mapperMock.Object,
            _dateTimeProviderMock.Object,
            _tokenProviderMock.Object);
    }

    [Fact]
    public async Task Handle_EmailAlreadyExists_ShouldReturnFailure()
    {
        // Arrange
        var command = new RegisterCommandBuilder().ValidCommand();
        
        var existingAccount = new AccountBuilder()
            .WithId(Guid.NewGuid())
            .WithEmail(command.Email)
            .WithUsername(command.Username).Build();

        _accountRepositoryMock
            .Setup(x => x.Get(
                It.IsAny<Expression<Func<Account, bool>>>(),
                It.IsAny<Func<IQueryable<Account>,
                    IOrderedQueryable<Account>>>(),
                It.IsAny<string>(),
                It.IsAny<int?>(),
                It.IsAny<int?>()))
            .ReturnsAsync(new List<Account>
            {
                existingAccount
            });

        // Act
        Result<string> result = await _handler.Handle(
            command,
            CancellationToken.None);

        // Assert
        result.IsFailure.Should().BeTrue();

        result.Error.Should().BeEquivalentTo(
            AuthenticationError.EmailExists(command.Email));

        _accountRepositoryMock.Verify(
            x => x.Insert(It.IsAny<Account>()),
            Times.Never);

        _accountRepositoryMock.Verify(
            x => x.SaveChangesAsync(),
            Times.Never);

        _tokenProviderMock.Verify(
            x => x.Create(
                It.IsAny<Account>(),
                It.IsAny<List<Role>>()),
            Times.Never);
    }

    [Fact]
    public async Task Handle_ValidRequest_ShouldRegisterSuccessfully()
    {
        // Arrange
        var command = new RegisterCommandBuilder().ValidCommand();

        var roles = new List<Role>
        {
            new RoleBuilder()
                .WithId(Guid.NewGuid())
                .WithName(Roles.User)
                .Build()
        };

        var mappedAccount = new AccountBuilder()
            .WithId(Guid.NewGuid())
            .WithEmail(command.Email)
            .WithUsername(command.Username)
            .WithAccountRoles(new List<AccountRole>())
            .Build();

        DateTime utcNow = DateTime.UtcNow;

        const string hashedPassword = "hashed-password";

        const string expectedToken = "jwt-token";

        _roleRepositoryMock
            .Setup(x => x.Get(
                It.IsAny<Expression<Func<Role, bool>>>(),
                It.IsAny<Func<IQueryable<Role>,
                    IOrderedQueryable<Role>>>(),
                It.IsAny<string>(),
                It.IsAny<int?>(),
                It.IsAny<int?>()))
            .ReturnsAsync(roles);

        _accountRepositoryMock
            .Setup(x => x.Get(
                It.IsAny<Expression<Func<Account, bool>>>(),
                It.IsAny<Func<IQueryable<Account>,
                    IOrderedQueryable<Account>>>(),
                It.IsAny<string>(),
                It.IsAny<int?>(),
                It.IsAny<int?>()))
            .ReturnsAsync(new List<Account>());

        _mapperMock
            .Setup(x => x.Map<Account>(command))
            .Returns(mappedAccount);

        _passwordHasherMock
            .Setup(x => x.Hash(command.Password))
            .Returns(hashedPassword);

        _dateTimeProviderMock
            .Setup(x => x.UtcNow)
            .Returns(utcNow);

        _tokenProviderMock
            .Setup(x => x.Create(
                It.IsAny<Account>(),
                It.IsAny<List<Role>>()))
            .Returns(expectedToken);

        // Act
        Result<string> result = await _handler.Handle(
            command,
            CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();

        result.Value.Should().Be(expectedToken);

        mappedAccount.PasswordHashed
            .Should().Be(hashedPassword);

        mappedAccount.CreatedOnUtc
            .Should().Be(utcNow);

        mappedAccount.AccountRoles
            .Should().HaveCount(1);

        mappedAccount.AccountRoles.First().RoleId
            .Should().Be(roles.First().Id);

        _passwordHasherMock.Verify(
            x => x.Hash(command.Password),
            Times.Once);

        _accountRepositoryMock.Verify(
            x => x.Insert(mappedAccount),
            Times.Once);

        _accountRepositoryMock.Verify(
            x => x.SaveChangesAsync(),
            Times.Once);

        _tokenProviderMock.Verify(
            x => x.Create(
                It.IsAny<Account>(),
                It.IsAny<List<Role>>()),
            Times.Once);
    }
}