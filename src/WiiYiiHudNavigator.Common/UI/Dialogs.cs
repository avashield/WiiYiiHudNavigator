using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Maui.Core;
using System.Runtime.CompilerServices;
using WiiYiiHudNavigator.Common.Extensions;

namespace WiiYiiHudNavigator.Common.UI;

public static class Dialogs
{
	/// <summary>
	/// Presents a prompt dialog to the application user with an accept and a cancel button.
	/// </summary>
	/// <returns></returns>
	public static async Task<string> Prompt(string title, string message, string accept = "OK", string cancel = "Cancel", string? placeholder = null, int maxLength = -1, Keyboard? keyboard = null, string initialValue = "")
	{
		var mainPage = Application.Current.GetMainPage();
		if (mainPage == null)
			return initialValue;

		return await MainThread.InvokeOnMainThreadAsync(() =>
			mainPage.DisplayPromptAsync(title, message, accept, cancel, placeholder, maxLength, keyboard, initialValue)
		);
	}

	/// <summary>
	/// Presents an alert dialog to the application user with a single cancel button.
	/// </summary>
	/// <param name="title">The title of the alert dialog.</param>
	/// <param name="message">The body text of the alert dialog.</param>
	/// <param name="cancel">Text to be displayed on the 'Cancel' button.</param>
	/// <returns></returns>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static async Task Alert(string title, string message, string cancel = "OK")
	{
		var mainPage = Application.Current.GetMainPage();
		if (mainPage == null)
			return;

		await MainThread.InvokeOnMainThreadAsync(() =>
			mainPage.DisplayAlert(title, message, cancel)
		);
	}

	/// <summary>
	/// Presents an alert dialog to the application user with an accept and a cancel button.
	/// </summary>
	/// <param name="title">The title of the alert dialog.</param>
	/// <param name="message">The body text of the alert dialog.</param>
	/// <param name="accept">Text to be displayed on the 'Accept' button.</param>
	/// <param name="cancel">Text to be displayed on the 'Cancel' button.</param>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static async Task<bool> Alert(string title, string message, string accept, string cancel)
	{
		var mainPage = Application.Current.GetMainPage();
		if (mainPage == null)
			return false;

		return await MainThread.InvokeOnMainThreadAsync(() =>
			mainPage.DisplayAlert(title, message, accept, cancel)
		);
	}

	/// <summary>
	/// Presents an alert dialog to the application user with an accept and a cancel button.
	/// </summary>
	/// <param name="title">The title of the alert dialog.</param>
	/// <param name="message">The body text of the alert dialog.</param>
	/// <param name="accept">Text to be displayed on the 'Accept' button.</param>
	/// <param name="cancel">Text to be displayed on the 'Cancel' button.</param>
	/// <param name="flowDirection">A task that contains the user's choice as a Boolean value. true indicates that the user accepted the alert. false indicates that the user cancelled the alert.</param>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static async Task<bool> Alert(string title, string message, string accept, string cancel, FlowDirection flowDirection)
	{
		var mainPage = Application.Current.GetMainPage();
		if (mainPage == null)
			return false;

		return await MainThread.InvokeOnMainThreadAsync(() =>
			mainPage.DisplayAlert(title, message, accept, cancel, flowDirection)
		);
	}

	/// <summary>
	/// Presents an alert dialog to the application user with a single cancel button.
	/// </summary>
	/// <param name="title">The title of the alert dialog.</param>
	/// <param name="message">The body text of the alert dialog.</param>
	/// <param name="cancel">Text to be displayed on the 'Cancel' button.</param>
	/// <param name="flowDirection"></param>
	/// <returns></returns>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static async Task Alert(string title, string message, string cancel, FlowDirection flowDirection)
	{
		var mainPage = Application.Current.GetMainPage();
		if (mainPage == null)
			return;

		await MainThread.InvokeOnMainThreadAsync(() =>
			mainPage.DisplayAlert(title, message, cancel, flowDirection)
		);
	}

	/// <summary>
	/// Create new Toast
	/// </summary>
	/// <param name="message">Toast message</param>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static async Task ToastShort(string message)
	{
		using var toast = Toast.Make(message, ToastDuration.Short);
		await toast.Show();
	}

	/// <summary>
	/// Create new Toast
	/// </summary>
	/// <param name="message">Toast message</param>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static async Task ToastLong(string message)
	{
		using var toast = Toast.Make(message, ToastDuration.Long);
		await toast.Show();
	}
}
