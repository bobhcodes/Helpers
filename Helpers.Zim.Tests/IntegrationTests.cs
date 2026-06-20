using Helpers.Zim.Models;
using Helpers.Zim.Services;
using Helpers.Zim.Tests.Supplementary;
using System.Diagnostics.CodeAnalysis;

namespace Helpers.Zim.Tests;

public class IntegrationTests(Fixture fixture) : IClassFixture<Fixture>
{
	private readonly IZimService _sut = fixture.ZimService;

	[SuppressMessage("Usage", "xUnit1004:Test methods should not be skipped", Justification = "requires third-party")]
	[Theory(Skip = "requires third-party")]
	[InlineData("wikipedia_en_all", "maxi")]
	public async Task Test2(string name, string? flavor)
	{
		using var cts = new CancellationTokenSource(millisecondsDelay: 10_000);

		IReadOnlyCollection<Entry> entries = await _sut.GetEntriesAsync(name, flavor, cts.Token)
			.ToArrayAsync(cts.Token);

		Assert.NotEmpty(entries);
		Assert.DoesNotContain(default, entries);

		IReadOnlyCollection<Uri> uris = await _sut.GetUrisAsync(entries.Single(), cts.Token)
			.ToArrayAsync(cts.Token);

		Assert.NotEmpty(uris);
		Assert.DoesNotContain(null, uris);
		Assert.All(uris, u => Assert.True(u.IsAbsoluteUri));
	}
}
