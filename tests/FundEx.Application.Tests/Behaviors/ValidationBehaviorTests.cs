namespace FundEx.Application.Tests.Behaviors;

using FluentAssertions;
using FluentValidation;
using FluentValidation.Results;
using FundEx.Application.Common.Behaviors;
using MediatR;
using Moq;

public class ValidationBehaviorTests
{
    [Fact]
    public async Task Should_Call_Next_When_No_Validators()
    {
        var validators = Enumerable.Empty<IValidator<TestRequest>>();
        var behavior = new ValidationBehavior<TestRequest, TestResponse>(validators);
        var nextCalled = false;

        var result = await behavior.Handle(
            new TestRequest(),
            (ct) =>
            {
                nextCalled = true;
                return Task.FromResult(new TestResponse());
            },
            CancellationToken.None);

        nextCalled.Should().BeTrue();
    }

    [Fact]
    public async Task Should_Throw_When_Validation_Fails()
    {
        var validator = new Mock<IValidator<TestRequest>>();
        validator.Setup(v => v.ValidateAsync(It.IsAny<ValidationContext<TestRequest>>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new ValidationResult(new[] { new ValidationFailure("Prop", "Error") }));

        var behavior = new ValidationBehavior<TestRequest, TestResponse>(new[] { validator.Object });

        var act = () => behavior.Handle(
            new TestRequest(),
            (ct) => Task.FromResult(new TestResponse()),
            CancellationToken.None);

        await act.Should().ThrowAsync<ValidationException>();
    }

    public record TestRequest : IRequest<TestResponse>;
    public record TestResponse;
}
