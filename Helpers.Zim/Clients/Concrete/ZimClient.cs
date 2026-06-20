using Helpers.Zim.Models;
using Helpers.Zim.Models.Generated;
using System.Globalization;
using System.Runtime.CompilerServices;
using System.Xml;
using System.Xml.Serialization;

namespace Helpers.Zim.Clients.Concrete
{
	public sealed class ZimClient(HttpClient httpClient, XmlSerializerFactory serializerFactory) : IZimClient
	{
		private static readonly XmlReaderSettings _xmlReaderSettings = new() { Async = true, DtdProcessing = DtdProcessing.Prohibit, };

		public async IAsyncEnumerable<Entry> GetEntriesAsync([EnumeratorCancellation] CancellationToken cancellationToken = default)
		{
			await using var stream = await httpClient.GetStreamAsync("/catalog/v2/entries?count=-1&lang=eng", cancellationToken);

			using var reader = XmlReader.Create(stream, _xmlReaderSettings);

			while (reader.ReadToFollowing("entry") && !cancellationToken.IsCancellationRequested)
			{
				using var subtree = reader.ReadSubtree();
				yield return GetEntry(subtree);
			}
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

		private static Entry GetEntry(XmlReader reader)
		{
			ArgumentNullException.ThrowIfNull(reader);

			var document = new XmlDocument();
			document.Load(reader);

			if (document.TryGetValue("id", out var id)
				&& document.TryGetValue("title", out var title)
				&& document.TryGetValue("updated", out var updatedString)
				&& document.TryGetValue("name", out var name)
				&& document.TryGetValue("flavour", out var flavor))
			{
				var updated = DateTime.Parse(updatedString, CultureInfo.InvariantCulture, DateTimeStyles.AssumeUniversal | DateTimeStyles.AdjustToUniversal);

				var link = document.GetLinks()
					.First(kvp => string.Equals(kvp.Key, "application/x-zim", StringComparison.OrdinalIgnoreCase))
					.Value;

				return new(id, title, updated, name, flavor, link);
			}

			throw new ArgumentOutOfRangeException(nameof(reader));
		}
	}
}
