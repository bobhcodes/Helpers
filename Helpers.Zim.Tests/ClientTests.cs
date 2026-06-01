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
		var books = await _sut.GetBooksAsync().ToArrayAsync();

		// Assert
		Assert.NotNull(books);
		Assert.NotEmpty(books);
		Assert.DoesNotContain(null, books);

		foreach (var book in books)
		{
			Assert.NotNull(book);
			Assert.NotNull(book.id);
			Assert.NotEqual(default, book.date);
			Assert.NotNull(book.name);
			Assert.NotNull(book.title);
			Assert.NotNull(book.url);
		}
	}

	[Theory]
	[InlineData("https://lbo.download.kiwix.org/zim/wikipedia/wikipedia_en_all_maxi_2026-02.zim.meta4")]
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
