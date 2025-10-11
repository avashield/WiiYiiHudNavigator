
namespace WiiYiiHudNavigator.NavigationIntegrationContracts;

public interface INavigationIntegrationSetup
{
	Guid Id { get; }

	void SetupServices(IServiceCollection services);

	StartNavigationIntegrationButton GetStartNavigationButton();

	INavigationIntegrationInterface GetInterface();
	
	void OnAppLoaded(IServiceProvider serviceProvider);
}

public record StartNavigationIntegrationButton(string Title, string ShortDescription, string IconFile, string PriceTagTitle);

public interface INavigationIntegrationInterface
{
	INavigationIntegrationSetup Setup {  get; }

	ContentView? GetNavigationUiContent();

	void OnAppLoaded();

	void OnAppUnloaded();
}