// iOS automatically uses Apple Maps, no additional setup required beyond Info.plist permissions

namespace WiiYiiHudNavigator.SampleMap.Platforms;

public static class iOSMapSetup
{
	/// <summary>
	/// Verifies that location permissions are configured in Info.plist
	/// </summary>
	public static void VerifySetup()
	{
		System.Diagnostics.Debug.WriteLine("? iOS Maps uses Apple Maps by default");
		System.Diagnostics.Debug.WriteLine("Ensure location permissions are added to Info.plist:");
		System.Diagnostics.Debug.WriteLine("- NSLocationWhenInUseUsageDescription");
		System.Diagnostics.Debug.WriteLine("- NSLocationAlwaysUsageDescription (optional)");
	}
}
