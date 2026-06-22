using Helpers.Zim.Models.Generated;
using System.Runtime.CompilerServices;
using System.Xml.Serialization;

namespace Helpers.Zim.Clients.Concrete
{
	public sealed class ZimClient(HttpClient httpClient, XmlSerializerFactory serializerFactory) : IZimClient
	{
		public async IAsyncEnumerable<entryType> GetEntriesAsync([EnumeratorCancellation] CancellationToken cancellationToken = default)
		{
			var serializer = serializerFactory.CreateSerializer(typeof(feedType));
			await using var stream = await httpClient.GetStreamAsync("/catalog/v2/entries?count=-1&lang=eng", cancellationToken);
			var feed = serializer.Deserialize(stream) as feedType;
			var entries = GetEntriesAsync(feed!, cancellationToken);
			await foreach (var entry in entries) { yield return entry; }
		}

		public static async IAsyncEnumerable<entryType> GetEntriesAsync(feedType feed, [EnumeratorCancellation] CancellationToken cancellationToken = default)
		{
			ArgumentNullException.ThrowIfNull(feed);
			ArgumentNullException.ThrowIfNull(feed.entry);
			foreach (var entry in feed.entry) { yield return entry; }
		}

		public async Task<fileType> GetFileAsync(Uri uri, CancellationToken cancellationToken = default)
		{
			ArgumentNullException.ThrowIfNull(uri);
			ArgumentException.ThrowIfNullOrEmpty(uri.OriginalString);
			var serializer = serializerFactory.CreateSerializer(typeof(metalinkType));
			await using var stream = await httpClient.GetStreamAsync(uri, cancellationToken);
			var metalink = serializer.Deserialize(stream) as metalinkType;
			var file = GetFile(metalink!);
			return file;
		}

		public static fileType GetFile(metalinkType metalink)
		{
			ArgumentNullException.ThrowIfNull(metalink);
			ArgumentNullException.ThrowIfNull(metalink.file);
			return metalink.file;
		}
	}
}
