using Helpers.Zim.Models.Generated;
using System.Runtime.CompilerServices;
using System.Xml.Serialization;

namespace Helpers.Zim.Clients.Concrete;

public class ZimClient(HttpClient httpClient, XmlSerializerFactory serializerFactory) : IZimClient
{

	public async IAsyncEnumerable<bookType> GetBooksAsync([EnumeratorCancellation] CancellationToken cancellationToken = default)
	{
		var serializer = serializerFactory.CreateSerializer(typeof(libraryType));
		await using var stream = await httpClient.GetStreamAsync("/library/library_zim.xml", cancellationToken);
		var library = serializer.Deserialize(stream) as libraryType;
		IReadOnlyCollection<bookType> books = library?.book ?? throw new Exception();
		foreach (var book in books) { yield return book; }
	}

	public async Task<fileType> GetFileAsync(Uri uri, CancellationToken cancellationToken = default)
	{
		ArgumentNullException.ThrowIfNull(uri);
		ArgumentException.ThrowIfNullOrEmpty(uri.OriginalString);

		var serializer = serializerFactory.CreateSerializer(typeof(metalinkType));
		await using var stream = await httpClient.GetStreamAsync(uri, cancellationToken);
		var metalink = serializer.Deserialize(stream) as metalinkType;
		return metalink?.file ?? throw new Exception();
	}
}
