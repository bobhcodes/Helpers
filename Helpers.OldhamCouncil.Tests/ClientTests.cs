using Xunit;

namespace Helpers.OldhamCouncil.Tests;

public sealed class ClientTests : IClassFixture<Fixtures.ClientFixture>
{
	private readonly IClient _sut;

	public ClientTests(Fixtures.ClientFixture fixture)
	{
		_sut = fixture.Client;
	}

	[Theory]
	[InlineData("ol11aa", "422000069073", "21  OLDHAM DELIVERY OFFICE HAMILTON STREET OLDHAM OL1 1AA")]
	[InlineData("ol1 1ut", "422000112981", "CIVIC CENTRE WEST STREET OLDHAM OL1 1UT")]
	public async Task GetAddresses(string postcode, string uprn, string address)
	{
		var addresses = await _sut.GetAddressesAsync(postcode, TestContext.Current.CancellationToken)
			.ToListAsync(TestContext.Current.CancellationToken);

		Assert.NotNull(addresses);
		Assert.NotEmpty(addresses);
		Assert.NotNull(addresses);
		Assert.NotEmpty(addresses);
		Assert.Contains(uprn, addresses.Select(a => a.Uprn));
		Assert.Contains(address, addresses.Select(a => a.FullAddress));
	}

	[Theory]
	[InlineData("422000112981")]
	public async Task GetBinCollections(string uprn)
	{
		var collections = await _sut.GetBinCollectionsAsync(uprn, TestContext.Current.CancellationToken)
			.ToListAsync(TestContext.Current.CancellationToken);

		Assert.NotEmpty(collections);
	}
}
