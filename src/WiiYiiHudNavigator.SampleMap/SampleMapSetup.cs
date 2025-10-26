using WiiYiiHudNavigator.NavigationIntegrationContracts;
using WiiYiiHudNavigator.SampleMap.ViewModels;
using WiiYiiHudNavigator.SampleMap.Views;

namespace WiiYiiHudNavigator.SampleMap;

public class SampleMapSetup : INavigationIntegrationSetup
{
	private IServiceProvider? _serviceProvider;
	private INavigationIntegrationInterface? _interface;

	public Guid Id { get; } = Guid.NewGuid();

	public StartNavigationIntegrationButton GetStartNavigationButton() =>
		new(
			title: "Sample Map - Free",
			shortDescription: "Just a Sample App",
			priceTagTitle: "FREE",
			iconFileFromMainApp: null,
			iconImageFromSource: ImageSource.FromResource($"{typeof(SampleMapSetup).Namespace}.Resources.Images.map_icon.png", typeof(SampleMapSetup).Assembly)
		);

	public void SetupServices(IServiceCollection services)
	{
		services.AddTransient<SampleMapView>();
		services.AddSingleton<ISampleMapInterface, SampleMapInterface>();

		services.AddSingleton<SampleMapVm>();
		services.AddSingleton<SampleMapView>();
	}

	public INavigationIntegrationInterface GetInterface()
	{
		return _interface ??=
			_serviceProvider!.GetRequiredService<ISampleMapInterface>();
	}

	private void RaiseOnAppLoaded()
	{
		if (_interface is null)
			// not loaded yet
			return;
		GetInterface().OnAppLoaded();
	}

	public void OnAppLoaded(IServiceProvider serviceProvider)
	{
		_serviceProvider = serviceProvider;
		RaiseOnAppLoaded();
	}
}