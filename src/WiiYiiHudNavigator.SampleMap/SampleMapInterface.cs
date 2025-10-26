using WiiYiiHudNavigator.NavigationIntegrationContracts;
using WiiYiiHudNavigator.SampleMap.Views;

namespace WiiYiiHudNavigator.SampleMap;

// All the code in this file is included in all platforms.

public interface ISampleMapInterface : INavigationIntegrationInterface
{

}

public class SampleMapInterface : ISampleMapInterface
{
	private IServiceProvider _serviceProvider;

	public INavigationIntegrationSetup Setup { get; private set; } = null!;

	public SampleMapInterface(IServiceProvider serviceProvider)
	{
		_serviceProvider = serviceProvider;
	}
	
	public void Initialize(INavigationIntegrationSetup setup)
	{
		Setup = setup;
	}

	public ContentView? GetNavigationUiContent()
	{
		return _serviceProvider.GetRequiredService<SampleMapView>();
	}

	public void OnAppLoaded()
	{
	}

	public void OnAppUnloaded()
	{
	}
}
