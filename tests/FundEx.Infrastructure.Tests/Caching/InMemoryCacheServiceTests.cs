namespace FundEx.Infrastructure.Tests.Caching;

using FluentAssertions;
using FundEx.Infrastructure.Caching;
using Microsoft.Extensions.Caching.Memory;

public class InMemoryCacheServiceTests
{
    private readonly InMemoryCacheService _sut;

    public InMemoryCacheServiceTests()
    {
        var cache = new MemoryCache(new MemoryCacheOptions());
        _sut = new InMemoryCacheService(cache);
    }

    [Fact]
    public async Task GetAsync_Should_Return_Null_When_Key_Not_Found()
    {
        var result = await _sut.GetAsync<string>("nonexistent");

        result.Should().BeNull();
    }

    [Fact]
    public async Task SetAsync_And_GetAsync_Should_Roundtrip()
    {
        await _sut.SetAsync("test-key", "test-value", TimeSpan.FromMinutes(5));

        var result = await _sut.GetAsync<string>("test-key");

        result.Should().Be("test-value");
    }

    [Fact]
    public async Task RemoveAsync_Should_Remove_Key()
    {
        await _sut.SetAsync("to-remove", "value");

        await _sut.RemoveAsync("to-remove");

        var result = await _sut.GetAsync<string>("to-remove");
        result.Should().BeNull();
    }
}
