#pragma warning disable IDE0130 // Namespace does not match folder structure
namespace System;
#pragma warning restore IDE0130 // Namespace does not match folder structure

public static class ListExtensions
{
	public static bool ArrayContains<T>(this T[]? array, T item)
	{
		if (array is null)
			return false;

		return Array.IndexOf(array, item) != -1;
	}
}
