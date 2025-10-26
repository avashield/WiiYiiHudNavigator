using Ava.Common.Logging;
using System.Text.Json;

namespace WiiYiiHudNavigator.Common.Configurations;

public interface IAppConfig
{
	bool WelcomeCompleted { get; set; }
	string? LastConnectedDeviceName { get; set; }
	string? LastConnectedDeviceUid { get; set; }
	DynamicIntegrationsConfig[]? DynamicIntegrations { get; set; }
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

	private DynamicIntegrationsConfig[]? _dynamicIntegrations;
	public DynamicIntegrationsConfig[]? DynamicIntegrations
	{
		get
		{
			if (_dynamicIntegrations == null)
			{
				_dynamicIntegrations = GetDynamicIntegrations();
			}
			return _dynamicIntegrations;
		}
		set
		{
			_dynamicIntegrations = value;
			if (value != null)
			{
				var json = JsonSerializer.Serialize(value);
				Preferences.Set(nameof(DynamicIntegrations), json);
			}
			else
			{
				Preferences.Remove(nameof(DynamicIntegrations));
			}
		}
	}

	public static DynamicIntegrationsConfig[]? GetDynamicIntegrations()
	{
		try
		{
			var json = Preferences.Get(nameof(DynamicIntegrations), defaultValue: null);
			if (json.NotNullOrWhiteSpace())
			{
				return JsonSerializer.Deserialize<DynamicIntegrationsConfig[]>(json);
			}
		}
		catch (Exception ex)
		{
			Log.Error?.Log($"DynamicIntegrationsLoader: Failed to read EnabledDynamicIntegrations from preferences: {ex.Message}");
		}
		return null;
	}
}

public record DynamicIntegrationsConfig(string? KnownTypeName, string? DynamicDllName, bool Enabled);