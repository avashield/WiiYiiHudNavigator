using CommunityToolkit.Mvvm.ComponentModel;

namespace WiiYiiHudNavigator.Common.ViewModels;

public abstract partial class BaseViewModel : ObservableObject
{
	[ObservableProperty]
	[NotifyPropertyChangedFor(nameof(NotBusy))]
	private bool _isBusy;

	public bool NotBusy => !_isBusy;
}
