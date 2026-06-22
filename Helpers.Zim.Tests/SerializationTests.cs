using Helpers.Zim.Models.Generated;
using System.Xml.Serialization;

namespace Helpers.Zim.Tests;

public class SerializationTests
{
	[Theory]
	[InlineData(".", "Data", "wikipedia_en_all_maxi_2026-02.zim.meta4")]
	public void MetaDataTests(params string[] paths)
	{
		// Arrange
		var serializer = new XmlSerializer(typeof(metalinkType));
		using var stream = new FileStream(path: Path.Combine(paths), FileMode.Open, FileAccess.Read, FileShare.Read);

		// Act
		var meta = serializer.Deserialize(stream) as metalinkType;

		// Assert
		Assert.NotNull(meta);
		Assert.NotNull(meta.file);
		Assert.NotNull(meta.file.url);
		Assert.NotEmpty(meta.file.url);
		Assert.DoesNotContain(null, meta.file.url);

		foreach (var o in meta.file.url)
		{
			Assert.NotNull(o);
			Assert.NotEqual(default, o.priority);
			Assert.NotEmpty(o.Value);
		}
	}

	[Theory]
	[InlineData(".", "Data", "entries")]
	public void EntryTests(params string[] paths)
	{
		// Arrange
		var path = Path.Combine(paths);
		using var stream = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.Read);

		var serializer = new XmlSerializer(typeof(feedType));
		// Act

		var feed = serializer.Deserialize(stream) as feedType;

		// Assert
		Assert.NotNull(feed);
		Assert.NotNull(feed.entry);
		Assert.NotEmpty(feed.entry);

		foreach (var entry in feed.entry)
		{
			Assert.NotNull(entry);
			Assert.NotNull(entry.id);
			Assert.NotNull(entry.title);
			Assert.NotNull(entry.updated);
			Assert.NotNull(entry.name);
			Assert.NotNull(entry.category);
			Assert.NotNull(entry.articleCount);
			Assert.NotNull(entry.mediaCount);
			Assert.NotNull(entry.link);
		}
	}
}
