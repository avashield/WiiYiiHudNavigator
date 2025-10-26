using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using Microsoft.Maui.Maps;
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

	public async Task SendNavigationToDestination(Location currentLocation, Location destinationLocation)
	{
		// Calculate actual distance using Haversine formula
		var distanceInKm = CalculateDistance(currentLocation, destinationLocation);
		var distanceInMeters = distanceInKm * 1000;
		
		// Calculate duration based on average speed (50 km/h)
		var durationInSeconds = (distanceInKm / 50) * 3600; // hours to seconds

		// Determine direction based on bearing
		var direction = GetDirectionFromBearing(currentLocation, destinationLocation);
		var secondDirection = GetRandomSecondTurn();

		// Generate random road names
		var roadNames = new[] { "Main Street", "Oak Avenue", "Park Boulevard", "River Road", "Hill Drive", "Maple Lane", "Broadway", "5th Avenue" };
		var nextRoad = roadNames[_random.Next(roadNames.Length)];
		var destinationName = $"Destination ({destinationLocation.Latitude:F4}, {destinationLocation.Longitude:F4})";

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

	private double CalculateDistance(Location start, Location end)
	{
		// Haversine formula
		var R = 6371; // Earth's radius in kilometers
		var dLat = ToRadians(end.Latitude - start.Latitude);
		var dLon = ToRadians(end.Longitude - start.Longitude);

		var a = Math.Sin(dLat / 2) * Math.Sin(dLat / 2) +
				Math.Cos(ToRadians(start.Latitude)) * Math.Cos(ToRadians(end.Latitude)) *
				Math.Sin(dLon / 2) * Math.Sin(dLon / 2);

		var c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));
		return R * c;
	}

	private double ToRadians(double degrees)
	{
		return degrees * Math.PI / 180;
	}

	private double ToDegrees(double radians)
	{
		return radians * 180 / Math.PI;
	}

	private DirectionInstruction GetDirectionFromBearing(Location start, Location end)
	{
		// Calculate bearing from start to end
		var dLon = ToRadians(end.Longitude - start.Longitude);
		var lat1 = ToRadians(start.Latitude);
		var lat2 = ToRadians(end.Latitude);

		var y = Math.Sin(dLon) * Math.Cos(lat2);
		var x = Math.Cos(lat1) * Math.Sin(lat2) - Math.Sin(lat1) * Math.Cos(lat2) * Math.Cos(dLon);
		var bearing = ToDegrees(Math.Atan2(y, x));
		
		// Normalize bearing to 0-360
		bearing = (bearing + 360) % 360;

		// Map bearing to direction instruction
		return bearing switch
		{
			>= 337.5 or < 22.5 => DirectionInstruction.Straight,      // North
			>= 22.5 and < 67.5 => DirectionInstruction.RightWide,     // Northeast
			>= 67.5 and < 112.5 => DirectionInstruction.Right, // East
			>= 112.5 and < 157.5 => DirectionInstruction.RightSharpSE, // Southeast
			>= 157.5 and < 202.5 => DirectionInstruction.Straight,    // South
			>= 202.5 and < 247.5 => DirectionInstruction.LeftSharpSE, // Southwest
			>= 247.5 and < 292.5 => DirectionInstruction.Left,        // West
			_ => DirectionInstruction.LeftWide             // Northwest
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
