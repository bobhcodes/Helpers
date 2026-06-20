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

		foreach(var o in meta.file.url)
		{
			Assert.NotNull(o);
			Assert.NotEqual(default, o.priority);
			Assert.NotEmpty(o.Value);
		}
	}
}
