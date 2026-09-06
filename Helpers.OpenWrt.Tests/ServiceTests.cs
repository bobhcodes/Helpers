using System.Linq;
using Xunit;

namespace Helpers.OpenWrt.Tests;

public sealed class ServiceTests : IClassFixture<Fixtures.ServiceFixture>
{
	private readonly IService _sut;

	public ServiceTests(Fixtures.ServiceFixture fixture)
	{
		_sut = fixture.Service;
	}

	[Theory]
	[InlineData("77.68.0.0/17")]
	[InlineData("77.68.11.211")]
	public async Task AddBlackhole(string s)
	{
		var prefix = Networking.Models.AddressPrefix.Parse(s, provider: null);

		// see if already exists
		var exists = await _sut.GetBlackholesAsync(TestContext.Current.CancellationToken).AnyAsync(b => b == prefix, TestContext.Current.CancellationToken);

		// if so, remove it
		if (exists) await _sut.DeleteBlackholeAsync(prefix, TestContext.Current.CancellationToken);

		// Assert it was removed
		Assert.False(await _sut.GetBlackholesAsync(TestContext.Current.CancellationToken).AnyAsync(b => b == prefix, TestContext.Current.CancellationToken));

		// add it
		await _sut.AddBlackholeAsync(prefix, TestContext.Current.CancellationToken);

		// Assert it was added
		Assert.True(await _sut.GetBlackholesAsync(TestContext.Current.CancellationToken).AnyAsync(b => b == prefix, TestContext.Current.CancellationToken));

		// delete it
		await _sut.DeleteBlackholeAsync(prefix, TestContext.Current.CancellationToken);

		// Assert it was deleted
		Assert.False(await _sut.GetBlackholesAsync(TestContext.Current.CancellationToken).AnyAsync(b => b == prefix, TestContext.Current.CancellationToken));

		// if it existed previously, put it back
		if (exists) await _sut.AddBlackholeAsync(prefix, TestContext.Current.CancellationToken);
	}

	[Theory]
	[InlineData("IPAddresses.csv")]
	public async Task AddManyBlackholes(string filename)
	{
		var path = Path.Combine(".", "Data", filename);
		var lines = await File.ReadAllLinesAsync(path, TestContext.Current.CancellationToken);

		Assert.NotNull(lines);
		Assert.NotEmpty(lines);
		Assert.DoesNotContain(default, lines);

		var prefixes = (
			from line in lines
			let prefix = Helpers.Networking.Models.AddressPrefix.Parse(line, provider: null)
			select prefix
		).ToList();

		Assert.NotEmpty(prefixes);
		Assert.DoesNotContain(default, prefixes);

		await _sut.AddBlackholesAsync(prefixes, TestContext.Current.CancellationToken);
	}
}
