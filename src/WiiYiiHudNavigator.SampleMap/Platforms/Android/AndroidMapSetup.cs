using Android.App;
using Android.Content.PM;

[assembly: UsesPermission(Android.Manifest.Permission.AccessCoarseLocation)]
[assembly: UsesPermission(Android.Manifest.Permission.AccessFineLocation)]
[assembly: UsesFeature("android.hardware.location", Required = false)]
[assembly: UsesFeature("android.hardware.location.gps", Required = false)]
[assembly: UsesFeature("android.hardware.location.network", Required = false)]

// Note: Add your Google Maps API Key in AndroidManifest.xml:
// <meta-data android:name="com.google.android.geo.API_KEY" android:value="YOUR_API_KEY_HERE" />

namespace WiiYiiHudNavigator.SampleMap.Platforms;

public static class AndroidMapSetup
{
	/// <summary>
	/// Verifies that the necessary permissions are declared.
	/// Call this from your MainActivity.OnCreate or Application.OnCreate to check setup.
	/// </summary>
	public static void VerifySetup()
	{
		var context = Android.App.Application.Context;
		var packageManager = context.PackageManager;
		var packageName = context.PackageName;

		try
		{
			var appInfo = packageManager?.GetApplicationInfo(packageName ?? "", PackageInfoFlags.MetaData);
			var metaData = appInfo?.MetaData;

			if (metaData == null || !metaData.ContainsKey("com.google.android.geo.API_KEY"))
			{
				System.Diagnostics.Debug.WriteLine("?? WARNING: Google Maps API Key not found in AndroidManifest.xml");
				System.Diagnostics.Debug.WriteLine("Add the following to your AndroidManifest.xml:");
				System.Diagnostics.Debug.WriteLine("<meta-data android:name=\"com.google.android.geo.API_KEY\" android:value=\"YOUR_API_KEY\" />");
			}
			else
			{
				System.Diagnostics.Debug.WriteLine("? Google Maps API Key found");
			}
		}
		catch (Exception ex)
		{
			System.Diagnostics.Debug.WriteLine($"Error verifying Google Maps setup: {ex.Message}");
		}
	}
}
