namespace Helpers.Zim.Tests.Supplementary;

public class MockingHandler : DelegatingHandler
{
	protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
	{
		var response = BuildResponse(request.RequestUri?.Host!);
		return Task.FromResult(response);
	}

	private static HttpResponseMessage BuildResponse(string host)
	{
		ArgumentException.ThrowIfNullOrEmpty(host);

		if (string.Equals(host, "browse.library.kiwix.org", StringComparison.OrdinalIgnoreCase))
		{
			var path = Path.Combine(".", "Data", "entries");
			var stream = File.Open(path, FileMode.Open, FileAccess.Read, FileShare.Read);
			return new() { Content = new StreamContent(stream), };
		}

		if (string.Equals(host, "lb.download.kiwix.org", StringComparison.OrdinalIgnoreCase))
		{
			var path = Path.Combine(".", "Data", "wikipedia_en_all_maxi_2026-02.zim.meta4");
			var stream = File.Open(path, FileMode.Open, FileAccess.Read, FileShare.Read);
			return new() { Content = new StreamContent(stream), };
		}

		throw new ArgumentOutOfRangeException(nameof(host), host, "unexpected host: " + host);
	}
}
