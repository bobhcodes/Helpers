using Xunit;

namespace Helpers.OldhamCouncil.Tests;

public sealed class ServiceTests : IClassFixture<Fixtures.ServiceFixture>
{
	private readonly IService _sut;

	public ServiceTests(Fixtures.ServiceFixture fixture)
	{
		_sut = fixture.Service;
	}

	[Theory]
	[InlineData("422000112981")]
	public async Task GetBinCollections(string uprn)
	{
		var dictionary = await _sut.GetBinCollectionsAsync(uprn, TestContext.Current.CancellationToken)
			.ToDictionaryAsync(kvp => kvp.Key, kvp => kvp.Value, cancellationToken: TestContext.Current.CancellationToken);

		Assert.NotNull(dictionary);
		Assert.NotEmpty(dictionary);
		Assert.All(dictionary.Keys, dt => Assert.NotEqual(default, dt));
		Assert.All(dictionary.Values, e => Assert.NotEqual(OldhamCouncil.Models.BinTypes.None, e));
	}

	[Theory]
	[InlineData("OL1 1UT", default)]
	public async Task PostcodeAndHouseNumber(string postcode, string? houseNumber)
	{
		var dictionary = await _sut.GetBinCollectionsAsync(postcode, houseNumber, TestContext.Current.CancellationToken)
			.ToDictionaryAsync(kvp => kvp.Key, kvp => kvp.Value, cancellationToken: TestContext.Current.CancellationToken);

		Assert.NotNull(dictionary);
		Assert.NotEmpty(dictionary);
		Assert.All(dictionary.Keys, dt => Assert.NotEqual(default, dt));
		Assert.All(dictionary.Values, e => Assert.NotEqual(OldhamCouncil.Models.BinTypes.None, e));
	}
}
