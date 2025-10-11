using System.Runtime.CompilerServices;

namespace WiiYiiHudNavigator.Common.Extensions;
public static class ApplicationExtensions
{
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static IServiceProvider? GetServiceProvider(this Application app)
	{
		return app?.Handler?.MauiContext?.Services;
	}

	/// <summary>
	/// Gets the main window of the application.
	/// </summary>
	public static Window? GetMainWindow(this Application? app)
	{
		if (app is null || app.Windows.Count == 0)
			return null;
		return app.Windows[0];
	}

	/// <summary>
	/// Gets the main page of the application.
	/// </summary>
	public static Page? GetMainPage(this Application? app)
	{
		if (app is null || app.Windows.Count == 0)
			return null;
		return app.Windows[0].Page;
	}

	/// <summary>
	/// Replaces the main page of the application with the specified page.
	/// </summary>
	public static void ReplaceMainPage(this Application? app, Page page)
	{
		if (app is null || app.Windows.Count == 0)
			return;
		app.Windows[0].Page = page;
	}

	/// <summary>
	/// Replaces the main page of the application with the specified page.
	/// </summary>
	/// <typeparam name="T"></typeparam>
	/// <param name="app"></param>
	/// <exception cref="InvalidOperationException"></exception>
	public static void ReplaceMainPage<T>(this Application? app)
		where T : Page
	{
		if (app is null || app.Windows.Count == 0)
			return;

		var sp = app.GetServiceProvider() ?? throw new InvalidOperationException("Service provider not found");

		app.Windows[0].Page = sp.GetRequiredService<T>();
	}
}
