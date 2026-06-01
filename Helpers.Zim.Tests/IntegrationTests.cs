using Helpers.Zim.Models.Generated;
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

		IReadOnlyCollection<bookType> books = await _sut.GetBooksAsync(name, flavor, cts.Token)
			.ToArrayAsync(cts.Token);

		Assert.NotEmpty(books);
		Assert.DoesNotContain(null, books);

		IReadOnlyCollection<Uri> uris = await _sut.GetUrisAsync(books.Single(), cts.Token)
			.ToArrayAsync(cts.Token);

		Assert.NotEmpty(uris);
		Assert.DoesNotContain(null, uris);
		Assert.All(uris, u => Assert.True(u.IsAbsoluteUri));
	}
}
