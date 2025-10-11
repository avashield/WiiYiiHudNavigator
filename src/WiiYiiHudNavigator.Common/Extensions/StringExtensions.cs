using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Runtime.CompilerServices;

#pragma warning disable IDE0130 // Namespace does not match folder structure
namespace System;
#pragma warning restore IDE0130 // Namespace does not match folder structure

public static class StringExtensions
{
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static string CleanUnicodeLineBreaks(this string str)
	{
		return str.Replace("\u00A0", "");
	}

	/// <summary>
	/// Consistent hash-code between different run of the applications.
	/// DO NOT USE THIS as a replacement for unique codes.
	/// </summary>
	/// <source>
	/// https://andrewlock.net/why-is-string-gethashcode-different-each-time-i-run-my-program-in-net-core/
	/// </source>
	/// <returns>
	/// Semi-unique hash-code
	/// </returns>
	public static int GetConsistentHashcode(this string str)
	{
		unchecked
		{
			int hash1 = (5381 << 16) + 5381;
			int hash2 = hash1;

			for (int i = 0; i < str.Length; i += 2)
			{
				hash1 = (hash1 << 5) + hash1 ^ str[i];
				if (i == str.Length - 1)
					break;
				hash2 = (hash2 << 5) + hash2 ^ str[i + 1];
			}

			return hash1 + hash2 * 1566083941;
		}
	}

	/// <summary>
	/// Consistent hash-code between different run of the applications.
	/// DO NOT USE THIS as a replacement for unique codes.
	/// </summary>
	/// <source>
	/// https://andrewlock.net/why-is-string-gethashcode-different-each-time-i-run-my-program-in-net-core/
	/// </source>
	/// <param name="str"></param>
	/// <returns>
	/// Semi-unique hash-code in hexadecimal format
	/// </returns>
	public static string GetConsistentHashcodeString(this string str) =>
		str.GetConsistentHashcode().ToString("x", CultureInfo.InvariantCulture);

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool IsNullOrEmpty([NotNullWhen(false)] this string? str) =>
		str is null || str.Length == 0;

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool NotNullOrEmpty([NotNullWhen(true)] this string? str) =>
		str is not null && str.Length != 0;

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool IsNullOrWhiteSpace([NotNullWhen(false)] this string? str) =>
		string.IsNullOrWhiteSpace(str);

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool NotNullOrWhiteSpace([NotNullWhen(true)] this string? str) =>
		!string.IsNullOrWhiteSpace(str);

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool ContainsOrdinalIgnoreCase([NotNullWhen(true)] this string? str, string value) =>
		str?.Contains(value, StringComparison.OrdinalIgnoreCase) == true;

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool ContainsInvariantIgnoreCase([NotNullWhen(true)] this string? str, string value) =>
		str?.Contains(value, StringComparison.InvariantCultureIgnoreCase) == true;

	/// <summary>
	/// Get string that is not null or empty, returns fallback if either is true
	/// </summary>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static string GetNonEmptyWithFallback([NotNullWhen(false)] this string? str, string fallback) =>
		str.IsNullOrWhiteSpace() ? fallback : str;

	/// <summary>
	/// Get string that is not null but could be empty, returns fallback if it is null
	/// </summary>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static string GetWithFallback([NotNullWhen(false)] this string? str, string fallback) =>
		str.IsNullOrEmpty() ? fallback : str;

	/// <summary>
	/// Returns a string that represents the current number, using CultureInfo.InvariantCulture.
	/// </summary>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static string ToStringInvariant(this byte num) =>
		num.ToString(CultureInfo.InvariantCulture);

	/// <summary>
	/// Returns a string that represents the current number, using CultureInfo.InvariantCulture.
	/// </summary>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static string ToStringInvariant(this int num) =>
		num.ToString(CultureInfo.InvariantCulture);

	/// <summary>
	/// Returns a string that represents the current number, using CultureInfo.InvariantCulture.
	/// </summary>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static string ToStringInvariant(this uint num) =>
		num.ToString(CultureInfo.InvariantCulture);

	/// <summary>
	/// Returns a string that represents the current number, using CultureInfo.InvariantCulture.
	/// </summary>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static string ToStringInvariant(this long num) =>
		num.ToString(CultureInfo.InvariantCulture);

	/// <summary>
	/// Returns a string that represents the current number, using CultureInfo.InvariantCulture.
	/// </summary>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static string ToStringInvariant(this ulong num) =>
		num.ToString(CultureInfo.InvariantCulture);

	/// <summary>
	/// Returns a string that represents the current number, using CultureInfo.InvariantCulture.
	/// </summary>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static string ToStringInvariant(this ushort num) =>
		num.ToString(CultureInfo.InvariantCulture);
}
