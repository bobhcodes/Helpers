using Helpers.Zim.Clients;
using Helpers.Zim.Tests.Supplementary;

namespace Helpers.Zim.Tests;

public class ClientTests(MockingFixture fixture) : IClassFixture<MockingFixture>
{
	private readonly IZimClient _sut = fixture.ZimClient;

	[Fact]
	public async Task GetBooksTests()
	{
		// Act
		var entries = await _sut.GetEntriesAsync().ToArrayAsync();

		// Assert
		Assert.NotNull(entries);
		Assert.NotEmpty(entries);

		foreach (var entry in entries)
		{
			Assert.NotEqual(default, entry);
			Assert.NotNull(entry.Id);
			Assert.NotEqual(default, entry.Updated);
			Assert.NotNull(entry.Name);
			Assert.NotNull(entry.Title);
			Assert.NotNull(entry.Link);
		}
	}

	[Theory]
	[InlineData("https://lb.download.kiwix.org/zim/wikipedia/wikipedia_en_all_maxi_2026-02.zim.meta4")]
	public async Task GetFileTests(string uriString)
	{
		// Arrange
		var uri = new Uri(uriString);

		// Act
		var file = await _sut.GetFileAsync(uri);

		// Assert
		Assert.NotNull(file);
		Assert.NotNull(file.url);
		Assert.NotEmpty(file.url);
		Assert.DoesNotContain(null, file.url);

		foreach (var o in file.url)
		{
			Assert.NotNull(o);
			Assert.NotEqual(default, o.priority);
			Assert.NotEmpty(o.Value);
		}
	}
}
