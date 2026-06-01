using Helpers.Zim.Models.Generated;

namespace Helpers.Zim.Services;

public interface IZimService
{
	IAsyncEnumerable<bookType> GetBooksAsync(CancellationToken cancellationToken = default);
	IAsyncEnumerable<bookType> GetBooksAsync(string name, string? flavor = null, CancellationToken cancellationToken = default);
	IAsyncEnumerable<Uri> GetUrisAsync(bookType book, CancellationToken cancellationToken = default);
	IAsyncEnumerable<Uri> GetUrisAsync(Uri uri, CancellationToken cancellationToken = default);
}
