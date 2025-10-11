namespace WiiYiiHudNavigator.Common.Configurations;

public interface IAppConfig
{
	bool WelcomeCompleted { get; set; }
	string? LastConnectedDeviceName { get; set; }
	string? LastConnectedDeviceUid { get; set; }
}

public class AppConfig : IAppConfig
{
	private bool? _welcomeCompleted;

	public bool WelcomeCompleted
	{
		get
		{
			if (_welcomeCompleted == null)
			{
				_welcomeCompleted = Preferences.Get(nameof(WelcomeCompleted), defaultValue: false);
			}
			return _welcomeCompleted.Value;
		}
		set
		{
			_welcomeCompleted = value;
			Preferences.Set(nameof(WelcomeCompleted), value);
		}
	}

	private string? _lastConnectedDeviceName;
	public string? LastConnectedDeviceName
	{
		get
		{
			if (_lastConnectedDeviceName == null)
			{
				_lastConnectedDeviceName = Preferences.Get(nameof(LastConnectedDeviceName), defaultValue: null);
			}
			return _lastConnectedDeviceName;
		}
		set
		{
			_lastConnectedDeviceName = value;
			Preferences.Set(nameof(LastConnectedDeviceName), value);
		}
	}

	private string? _lastConnectedDeviceUid;
	public string? LastConnectedDeviceUid
	{
		get
		{
			if (_lastConnectedDeviceUid == null)
			{
				_lastConnectedDeviceUid = Preferences.Get(nameof(LastConnectedDeviceUid), defaultValue: null);
			}
			return _lastConnectedDeviceUid;
		}
		set
		{
			_lastConnectedDeviceUid = value;
			Preferences.Set(nameof(LastConnectedDeviceUid), value);
		}
	}
}
