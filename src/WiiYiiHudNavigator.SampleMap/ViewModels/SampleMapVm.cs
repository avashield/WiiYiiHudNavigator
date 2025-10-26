using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using WiiYiiHudNavigator.Common.Hud;
using WiiYiiHudNavigator.Common.Models;
using WiiYiiHudNavigator.Common.ViewModels;

namespace WiiYiiHudNavigator.SampleMap.ViewModels;

internal partial class SampleMapVm : BaseViewModel
{
	private readonly IHudConnection _hudConnection;
	private readonly Random _random = new Random();

	[ObservableProperty]
	private bool _hasActiveNavigation;

	[ObservableProperty]
	private string _currentDestination = string.Empty;

	[ObservableProperty]
	private string _distanceText = "0 km";

	[ObservableProperty]
	private string _durationText = "0 min";

	[ObservableProperty]
	private string _nextTurnText = "→";

	[ObservableProperty]
	private string _currentRoad = "Main Street";

	public SampleMapVm(IHudConnection hudConnection)
	{
		_hudConnection = hudConnection;
	}

	public async Task SendNavigationToDestination(double destX, double destY, double mapWidth, double mapHeight)
	{
		// Calculate distance based on tap position (normalized to realistic values)
		var centerX = mapWidth / 2;
		var centerY = mapHeight / 2;
		var pixelDistance = Math.Sqrt(Math.Pow(destX - centerX, 2) + Math.Pow(destY - centerY, 2));
		
		// Convert pixel distance to meters (scale: 1 pixel ≈ 10 meters)
		var distanceInMeters = pixelDistance * 10;
		
		// Calculate duration based on average speed (50 km/h)
		var durationInSeconds = (distanceInMeters / 1000) * 72; // 72 seconds per km at 50 km/h

		// Determine direction based on tap position relative to center
		var direction = GetDirectionFromPosition(destX, destY, centerX, centerY);
		var secondDirection = GetRandomSecondTurn();

		// Generate random road names
		var roadNames = new[] { "Main Street", "Oak Avenue", "Park Boulevard", "River Road", "Hill Drive", "Maple Lane", "Broadway", "5th Avenue" };
		var nextRoad = roadNames[_random.Next(roadNames.Length)];
		var destinationName = $"Point at ({destX:F0}, {destY:F0})";

		// Calculate next turn distance (typically 10-30% of total distance)
		var nextTurnDistance = distanceInMeters * (_random.NextDouble() * 0.2 + 0.1);

		// Update UI
		HasActiveNavigation = true;
		CurrentDestination = destinationName;
		DistanceText = FormatDistance(distanceInMeters);
		DurationText = FormatDuration(durationInSeconds);
		NextTurnText = GetDirectionIcon(direction);
		CurrentRoad = nextRoad;

		// Create navigation data
		var navigationData = new HudNavigationData
		{
			DestinationName = destinationName,
			EtaDurationTimeRemaining = durationInSeconds,
			EtaDistanceRemaining = distanceInMeters,
			NextTurnDirection = direction,
			SecondTurnDirection = secondDirection,
			NextTurnDistance = nextTurnDistance,
			NextTurnRoadName = nextRoad,
			IsRerouting = false
		};

		// Send to HUD
		await _hudConnection.UpdateNavigationData(navigationData);
	}

	private DirectionInstruction GetDirectionFromPosition(double destX, double destY, double centerX, double centerY)
	{
		// Calculate angle from center to destination
		var angle = Math.Atan2(destY - centerY, destX - centerX) * (180 / Math.PI);
		
		// Normalize angle to 0-360
		if (angle < 0) angle += 360;

		// Map angle to direction
		return angle switch
		{
			>= 337.5 or < 22.5 => DirectionInstruction.Right,
			>= 22.5 and < 67.5 => DirectionInstruction.RightWide,
			>= 67.5 and < 112.5 => DirectionInstruction.Straight,
			>= 112.5 and < 157.5 => DirectionInstruction.LeftWide,
			>= 157.5 and < 202.5 => DirectionInstruction.Left,
			>= 202.5 and < 247.5 => DirectionInstruction.LeftSharpSE,
			>= 247.5 and < 292.5 => DirectionInstruction.Straight,
			_ => DirectionInstruction.RightSharpSE
		};
	}

	private DirectionInstruction GetRandomSecondTurn()
	{
		var directions = new[]
		{
			DirectionInstruction.Left,
			DirectionInstruction.Right,
			DirectionInstruction.Straight,
			DirectionInstruction.LeftWide,
			DirectionInstruction.RightWide
		};
		
		return directions[_random.Next(directions.Length)];
	}

	private string GetDirectionIcon(DirectionInstruction direction)
	{
		return direction switch
		{
			DirectionInstruction.Left or DirectionInstruction.LeftWide or DirectionInstruction.LeftTurnSecondRoad => "⬅",
			DirectionInstruction.Right or DirectionInstruction.RightWide or DirectionInstruction.RightTurnSecondRoad => "➡",
			DirectionInstruction.Straight => "⬆",
			DirectionInstruction.LeftSharpSE or DirectionInstruction.UTurnLeft => "↩",
			DirectionInstruction.RightSharpSE or DirectionInstruction.UTurnRight => "↪",
			DirectionInstruction.LeftFork => "↖",
			DirectionInstruction.RightFork => "↗",
			DirectionInstruction.Finish => "🏁",
			_ => "➡"
		};
	}

	private string FormatDistance(double meters)
	{
		if (meters < 1000)
			return $"{meters:F0} m";
		else
			return $"{meters / 1000:F1} km";
	}

	private string FormatDuration(double seconds)
	{
		var totalMinutes = (int)(seconds / 60);
		
		if (totalMinutes < 60)
			return $"{totalMinutes} min";
		else
		{
			var hours = totalMinutes / 60;
			var minutes = totalMinutes % 60;
			return $"{hours}h {minutes}m";
		}
	}
}
