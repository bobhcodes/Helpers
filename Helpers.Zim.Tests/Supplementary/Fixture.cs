using Helpers.Zim.Clients;
using Helpers.Zim.Clients.Concrete;
using Helpers.Zim.Services;
using Helpers.Zim.Services.Concrete;
using Microsoft.Extensions.DependencyInjection;
using System.Xml.Serialization;

namespace Helpers.Zim.Tests.Supplementary;

public sealed class Fixture : IDisposable
{
	private readonly ServiceProvider _serviceProvider;

	public Fixture()
	{
		_serviceProvider = new ServiceCollection()
			.AddTransient<HttpMessageHandler>(_ => new HttpClientHandler { AllowAutoRedirect = false, })
			.AddSingleton(new XmlSerializerFactory())
			.AddHttpClient<IZimClient, ZimClient>(c => c.BaseAddress = new Uri("https://download.kiwix.org/"))
				.ConfigurePrimaryHttpMessageHandler<HttpMessageHandler>()
				.Services
			.AddTransient<IZimService, ZimService>()
			.BuildServiceProvider();

		ZimClient = _serviceProvider.GetRequiredService<IZimClient>();
		ZimService = _serviceProvider.GetRequiredService<IZimService>();
	}

	public void Dispose() => _serviceProvider.Dispose();

	public IZimClient ZimClient { get; }
	public IZimService ZimService { get; }
}
