using WiiYiiHudNavigator.Common.Extensions;
using WiiYiiHudNavigator.Common.Helpers;
using WiiYiiHudNavigator.Common.ViewModels;

namespace WiiYiiHudNavigator.Common.Views.Components;

public partial class PermissionsPanel : ContentView
{
	private readonly PermissionsPanelVm _viewModel;

	public bool CheckBluetoothPermissions
	{
		get => _viewModel.CheckBluetoothPermissions;
		set
		{
			_viewModel.CheckBluetoothPermissions = value;
		}
	}

	public bool CheckReadNotificationPermissions
	{
		get => _viewModel.CheckReadNotificationPermissions;
		set
		{
			_viewModel.CheckReadNotificationPermissions = value;
		}
	}
	public bool CheckIgnoringBatteryOptimization
	{
		get => _viewModel.CheckIgnoringBatteryOptimization;
		set
		{
			_viewModel.CheckIgnoringBatteryOptimization = value;
		}
	}

	public PermissionsPanel(PermissionsPanelVm vm)
	{
		InitializeComponent();
		_viewModel = vm;
		BindingContext = _viewModel;
	}

	// Parameterless constructor for XAML instantiation
	public PermissionsPanel()
	{
		InitializeComponent();
		// Resolve the view model from the service provide

		_viewModel = Application.Current?.GetServiceProvider()?.GetService<PermissionsPanelVm>()
			?? throw new InvalidOperationException("PermissionsPanelVm service not found. Make sure it's registered in DI container.");

		BindingContext = _viewModel;
	}


	private async void ContentView_Loaded(object sender, EventArgs e)
	{
		await _viewModel.OnViewLoaded();
	}

	private async void ContentView_Unloaded(object sender, EventArgs e)
	{
		await _viewModel.OnViewUnloaded();
	}
}