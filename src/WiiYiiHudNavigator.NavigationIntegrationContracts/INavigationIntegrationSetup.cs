namespace WiiYiiHudNavigator.NavigationIntegrationContracts;

public interface INavigationIntegrationSetup
{
	Guid Id { get; }

	void SetupServices(MauiAppBuilder builder, IServiceCollection services);

	StartNavigationIntegrationButton GetStartNavigationButton();

	INavigationIntegrationInterface GetInterface();

	void OnAppLoaded(IServiceProvider serviceProvider);
}

public class StartNavigationIntegrationButton(
	string title,
	string shortDescription,
	string? iconFileFromMainApp,
	ImageSource? iconImageFromSource,
	string priceTagTitle)
{
	public string Title { get; init; } = title;

	public string ShortDescription { get; init; } = shortDescription;

	/// <summary>
	/// Icon file from main app as MauiImage, has higher priority over IconImageFromSource
	/// </summary>
	public string? IconFileFromMainApp { get; init; } = iconFileFromMainApp;

	/// <summary>
	/// Image source that optionally defined by the integration
	/// </summary>
	public ImageSource? IconImageFromSource { get; init; } = iconImageFromSource;

	public string PriceTagTitle { get; init; } = priceTagTitle;
}

public interface INavigationIntegrationInterface
{
	INavigationIntegrationSetup Setup { get; }

	ContentView? GetNavigationUiContent();

	void OnAppLoaded();

	void OnAppUnloaded();
}