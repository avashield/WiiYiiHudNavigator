namespace WiiYiiHudNavigator.Common.Configurations;

public static class WiiYiiConfig
{
	public const string DefaultBluetoothDeviceName = "WYHUD";

	//public static readonly Guid HudBluetoothServiceCharacteristicReadUUID = Guid.Parse("d44bc439-abfd-45a2-b575-925416129601");
	//public static readonly Guid HudBluetoothServiceCharacteristicWriteUUID = Guid.Parse("d44bc439-abfd-45a2-b575-925416129600");
	//public static readonly Guid HudBluetoothServiceUUID = Guid.Parse("0000fee9-0000-1000-8000-00805f9b34fb");

	public static readonly string HudBluetoothServiceCharacteristicReadUUID = "d44bc439-abfd-45a2-b575-925416129601";
	public static readonly string HudBluetoothServiceCharacteristicWriteUUID = "d44bc439-abfd-45a2-b575-925416129600";
	public static readonly Guid HudBluetoothServiceCharacteristicWriteUUIDGuid = Guid.Parse(HudBluetoothServiceCharacteristicWriteUUID);
	public static readonly string HudBluetoothServiceUUID = "0000fee9-0000-1000-8000-00805f9b34fb";
	public static readonly Guid HudBluetoothServiceUUIDGuid = Guid.Parse(HudBluetoothServiceUUID);

	public static readonly TimeSpan DeviceScanTimeout = TimeSpan.FromSeconds(3);
	public static readonly TimeSpan DeviceBatchScanTimeout = TimeSpan.FromSeconds(7);
	public static readonly TimeSpan ConnectTimeout = TimeSpan.FromSeconds(10);
	public static readonly int ConnectRetryTimes = 3;
	public static readonly TimeSpan ConnectRetryDelay = TimeSpan.FromMilliseconds(500);
	public static readonly TimeSpan OperationTimeout = TimeSpan.FromSeconds(10);
	public static readonly int OperationTimeoutMs = (int)OperationTimeout.TotalMilliseconds;
}
