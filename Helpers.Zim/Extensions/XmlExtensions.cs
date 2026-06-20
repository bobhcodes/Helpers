using System.Diagnostics.CodeAnalysis;

namespace System.Xml;

public static class XmlExtensions
{
	extension(XmlDocument document)
	{
		public bool TryGetValue(string name, [NotNullWhen(true)] out string value)
		{
			ArgumentException.ThrowIfNullOrWhiteSpace(name);

			var elements = document.GetElementsByTagName(name);

			if (elements.Count > 0)
			{
				value = elements[0]?.ChildNodes[0]?.Value!;
				return true;
			}

			value = null!;
			return false;
		}

		public IEnumerable<KeyValuePair<string, Uri>> GetLinks()
		{
			foreach (XmlNode node in document.GetElementsByTagName("link"))
			{
				if (node.TryGetAttribute("type", out var type)
					&& node.TryGetAttribute("href", out var hrefString)
					&& Uri.TryCreate(hrefString, UriKind.RelativeOrAbsolute, out var href))
				{
					yield return new(type, href);
				}
			}
		}
	}

	extension(XmlNode node)
	{
		public bool TryGetAttribute(string name, [NotNullWhen(true)] out string value)
		{
			ArgumentException.ThrowIfNullOrWhiteSpace(name);
			var attribute = node.Attributes?.GetNamedItem(name);
			value = attribute?.Value!;
			return attribute is not null;
		}
	}
}
