namespace WiiYiiHudNavigator.Common.Helpers;

public static class AppEventsHelper
{
	public static event Action? OnAppActivated;
	public static event Action? OnAppDestroyed;

	public static void RaiseOnAppActivated()
	{
		OnAppActivated?.Invoke();
	}

	public static void RaiseOnAppDestroyed()
	{
		OnAppDestroyed?.Invoke();
	}
}
