using Ava.Common.Logging;
using System.Diagnostics.CodeAnalysis;
using WiiYiiHudNavigator.Common.Bluetooth;
using WiiYiiHudNavigator.Common.Extensions;

namespace WiiYiiHudNavigator.Common.Helpers;

public static class PermissionHelper
{

	#region Bluetooth Permission
	private static readonly BluetoothPermissions _bluetoothPermissions
#if ANDROID
		//= new(scan: true, connect: true, advertise: false, bluetoothLocation: false);
		= new(scan: true, connect: true, advertise: false, bluetoothLocation: true);
#else
		= new();
#endif

	public static async Task<bool> CheckBluetoothStatus()
	{
		try
		{
			var requestStatus = await _bluetoothPermissions.CheckStatusAsync();
			return requestStatus == PermissionStatus.Granted;
		}
		catch (Exception)
		{
			// logger.LogError(ex);
			return false;
		}
	}

	public static async Task<bool> RequestBluetoothAccess()
	{
		try
		{
			var requestStatus = await _bluetoothPermissions.RequestAsync();
			return requestStatus == PermissionStatus.Granted;
		}
		catch (Exception)
		{
			// logger.LogError(ex);
			return false;
		}
	}
	#endregion

	#region Android NotificationReader

	public static Action? RequestNotificationReaderCheckDelegate { get; set; }

	public static Func<bool>? NotificationReaderPermissionCheckDelegate { get; set; }

	public static bool CheckNotificationReaderAccess()
	{
		if (NotificationReaderPermissionCheckDelegate == null)
		{
			// not available in this platform
			return true;
		}
		return NotificationReaderPermissionCheckDelegate();
	}

	public static void RequestNotificationReaderAccess()
	{
		RequestNotificationReaderCheckDelegate?.Invoke();
	}
	#endregion

	#region BatteryOptimization


	/// <summary>
	/// Checks if battery optimization is disabled for this app.
	/// </summary>
	/// <returns>True if battery optimization is disabled (good), false if enabled (needs to be disabled)</returns>
	public static bool IsIgnoringBatteryOptimization()
	{
#if ANDROID
		var context = Platform.CurrentActivity ?? global::Android.App.Application.Context;
		if (context == null)
		{
			return false;
		}
		return IsIgnoringBatteryOptimization(context);
#else
		// Not applicable on this platform
		return true;
#endif
	}

#if ANDROID
	/// <summary>
	/// Checks if battery optimization is disabled for this app.
	/// </summary>
	/// <param name="context">Android context</param>
	/// <returns>True if battery optimization is disabled (good), false if enabled (needs to be disabled)</returns>
	[SuppressMessage("Interoperability", "CA1422:Validate platform compatibility", Justification = "We check API level")]
	public static bool IsIgnoringBatteryOptimization(Android.Content.Context context)
	{
		try
		{
			if (Android.OS.Build.VERSION.SdkInt >= Android.OS.BuildVersionCodes.M)
			{
				var powerManager = (Android.OS.PowerManager?)context.GetSystemService(Android.Content.Context.PowerService);
				return powerManager?.IsIgnoringBatteryOptimizations(context.PackageName) ?? false;
			}
			else
			{
				// Battery optimization not present before API 23, so we consider it "ignored"
				return true;
			}
		}
		catch (Exception ex)
		{
			Log.Debug?.Log($"Failed to check battery optimization: {ex.Message}", "IsolatedServiceHelpers");
		}

		return false;
	}

	/// <summary>
	/// Requests to ignore battery optimization for this app.
	/// Critical for notification listener services on devices with aggressive battery optimization.
	/// </summary>
	/// <returns>True if the request dialog was shown</returns>
	[SuppressMessage("Interoperability", "CA1422:Validate platform compatibility", Justification = "We check API level")]
	public static bool RequestIgnoreBatteryOptimization()
	{
		var context = Platform.CurrentActivity ?? global::Android.App.Application.Context;
		if (context == null)
		{
			return false;
		}
		return RequestIgnoreBatteryOptimization(context);
	}

	/// <summary>
	/// Requests to ignore battery optimization for this app.
	/// Critical for notification listener services on devices with aggressive battery optimization.
	/// </summary>
	/// <param name="context">Android context</param>
	/// <returns>True if the request dialog was shown</returns>
	[SuppressMessage("Interoperability", "CA1422:Validate platform compatibility", Justification = "We check API level")]
	public static bool RequestIgnoreBatteryOptimization(Android.Content.Context context)
	{
		try
		{
			if (Android.OS.Build.VERSION.SdkInt >= Android.OS.BuildVersionCodes.M)
			{
				var powerManager = (Android.OS.PowerManager?)context.GetSystemService(Android.Content.Context.PowerService);
				if (powerManager != null && !powerManager.IsIgnoringBatteryOptimizations(context.PackageName))
				{
					var intent = new Android.Content.Intent(Android.Provider.Settings.ActionRequestIgnoreBatteryOptimizations);
					intent.SetData(Android.Net.Uri.Parse($"package:{context.PackageName}"));
					intent.SetFlags(Android.Content.ActivityFlags.NewTask);
					context.StartActivity(intent);

					Log.Debug?.Log("Requested battery optimization exemption", "IsolatedServiceHelpers");
					return true;
				}
			}
			else
			{
				// Battery optimization not present before API 23
				return true;
			}
		}
		catch (Exception ex)
		{
			LogExtensions.LogException(ex, message: "Failed to request battery optimization exemption");
		}

		return false;
	}
#endif
	#endregion
}
