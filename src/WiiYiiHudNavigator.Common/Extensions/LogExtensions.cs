using Ava.Common.Logging;
using System.Diagnostics.CodeAnalysis;

namespace WiiYiiHudNavigator.Common.Extensions;

public static class LogExtensions
{
	[SuppressMessage("Style", "IDE0060:Remove unused parameter", Justification = "dotnet 10 will use static extensions")]
	public static void LogException(Exception ex, string? message = null)
	{
		if (message is not null)
		{
			Log.Error?.Log($"{message}: {ex.Message}");
			Log.Debug?.Log($"{message}: {ex.ToString()}");
		}
		else
		{
			Log.Error?.Log($"An exception occurred: {ex.Message}");
			Log.Debug?.Log($"An exception occurred: {ex.ToString()}");
		}
	}
}