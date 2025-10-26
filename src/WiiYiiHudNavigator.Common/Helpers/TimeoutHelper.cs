namespace WiiYiiHudNavigator.Common.Helpers;


public static class TimeoutHelper
{
	/// <summary>
	/// Mimics JS setTimeout(callback, delay)
	/// </summary>
	public static Timer SetTimeout(Action action, int delayMilliseconds)
	{
		Timer? timer = null;
		timer = new Timer(_ =>
		{
			// Stop the timer once executed
			timer?.Dispose();
			action();
		}, null, delayMilliseconds, Timeout.Infinite);

		return timer;
	}

	/// <summary>
	/// Mimics JS clearTimeout(timerId)
	/// </summary>
	public static void ClearTimeout(Timer? timer)
	{
		timer?.Dispose();
	}
}