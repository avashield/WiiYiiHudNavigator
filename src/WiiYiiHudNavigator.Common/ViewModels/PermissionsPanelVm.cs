using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using WiiYiiHudNavigator.Common.Helpers;
using WiiYiiHudNavigator.Common.UI;

namespace WiiYiiHudNavigator.Common.ViewModels;

public partial class PermissionsPanelVm : BaseViewModel
{
	[ObservableProperty]
	[NotifyPropertyChangedFor(nameof(AnyPermissionRequired))]
	private bool _permissionRequiredForBluetooth = false;

	[ObservableProperty]
	[NotifyPropertyChangedFor(nameof(AnyPermissionRequired))]
	private bool _permissionRequiredForNotification = false;

	[ObservableProperty]
	[NotifyPropertyChangedFor(nameof(AnyPermissionRequired))]
	private bool _requestRequiredIgnoringBatteryOptimization = false;

	[ObservableProperty]
	private bool _checkBluetoothPermissions = false;

	[ObservableProperty]
	private bool _checkReadNotificationPermissions = false;

	[ObservableProperty]
	private bool _checkIgnoringBatteryOptimization = false;

	public bool AnyPermissionRequired => _permissionRequiredForBluetooth || _permissionRequiredForNotification || _requestRequiredIgnoringBatteryOptimization;

	public PermissionsPanelVm()
	{
		AppEventsHelper.OnAppActivated += AppEventsHelper_OnAppActivated;
	}

	private async void AppEventsHelper_OnAppActivated()
	{
		await UpdatePermissionsStatus();
	}

	internal async Task OnViewLoaded()
	{
		await UpdatePermissionsStatus();
	}

	internal async Task OnViewUnloaded()
	{

	}

	private async Task UpdatePermissionsStatus()
	{
		PermissionRequiredForNotification = CheckReadNotificationPermissions && PermissionHelper.NotificationReaderPermissionCheckDelegate?.Invoke() == false;

		PermissionRequiredForBluetooth = CheckBluetoothPermissions && !(await PermissionHelper.CheckBluetoothStatus());

#if ANDROID
		RequestRequiredIgnoringBatteryOptimization = CheckIgnoringBatteryOptimization && !PermissionHelper.IsIgnoringBatteryOptimization();
#endif
	}


	[RelayCommand]
	private void RequestNotificationPermission()
	{
		PermissionHelper.RequestNotificationReaderAccess();
	}

	[RelayCommand]
	private async Task RequestBluetoothPermission()
	{
		try
		{
			var requestResult = await PermissionHelper.RequestBluetoothAccess();
			if (!requestResult)
			{
				await Dialogs.Alert("", "Please grant access to Bluetooth. Go to app settings to do so!");
			}
		}
		catch (Exception ex)
		{
			await Dialogs.Alert("", "Failed to request access to Bluetooth. " + ex.Message);
		}

		await UpdatePermissionsStatus();
	}

	[RelayCommand]
	private void RequestIgnoreBatteryOptimization()
	{
#if ANDROID
		PermissionHelper.RequestIgnoreBatteryOptimization();
#endif
	}
}
