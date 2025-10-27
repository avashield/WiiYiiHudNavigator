using Microsoft.Maui.Controls.Maps;
using Microsoft.Maui.Maps;
using WiiYiiHudNavigator.Plugin.SampleMap.ViewModels;

namespace WiiYiiHudNavigator.Plugin.SampleMap.Views;

public partial class SampleMapView : ContentView
{
	private SampleMapVm? _viewModel;
	private Pin? _currentLocationPin;
	private Pin? _destinationPin;
	private Polyline? _routeLine;

	public SampleMapView()
	{
		InitializeComponent();
	}

	private async void ContentView_Loaded(object sender, EventArgs e)
	{
		_viewModel = BindingContext as SampleMapVm;

		// Set initial map location (default to a major city or user location)
		// You can change this to the user's actual location
		var initialLocation = new Location(37.7749, -122.4194); // San Francisco
		MapControl.MoveToRegion(MapSpan.FromCenterAndRadius(initialLocation, Distance.FromKilometers(5)));

		// Try to get user's actual location
		try
		{
			var location = await Geolocation.GetLastKnownLocationAsync();
			if (location != null)
			{
				initialLocation = location;
				MapControl.MoveToRegion(MapSpan.FromCenterAndRadius(initialLocation, Distance.FromKilometers(5)));
			}
		}
		catch
		{
			// Use default location if unable to get user location
		}

		// Add current location pin
		_currentLocationPin = new Pin
		{
			Label = "Current Location",
			Address = "You are here",
			Type = PinType.Place,
			Location = initialLocation
		};
		MapControl.Pins.Add(_currentLocationPin);
	}

	private void ContentView_Unloaded(object sender, EventArgs e)
	{
		_viewModel = null;
		MapControl.Pins.Clear();
		MapControl.MapElements.Clear();
	}

	private async void OnMapClicked(object? sender, MapClickedEventArgs e)
	{
		if (_viewModel == null || _currentLocationPin == null)
			return;

		var destinationLocation = e.Location;

		// Remove existing destination pin if any
		if (_destinationPin != null)
		{
			MapControl.Pins.Remove(_destinationPin);
		}

		// Add new destination pin
		_destinationPin = new Pin
		{
			Label = "Destination",
			Address = $"Lat: {destinationLocation.Latitude:F4}, Lon: {destinationLocation.Longitude:F4}",
			Type = PinType.Place,
			Location = destinationLocation
		};
		MapControl.Pins.Add(_destinationPin);

		// Remove existing route line if any
		if (_routeLine != null)
		{
			MapControl.MapElements.Remove(_routeLine);
		}

		// Draw route line
		_routeLine = new Polyline
		{
			StrokeColor = Colors.Blue,
			StrokeWidth = 8
		};
		_routeLine.Geopath.Add(_currentLocationPin.Location);
		_routeLine.Geopath.Add(destinationLocation);
		MapControl.MapElements.Add(_routeLine);

		// Adjust map to show both pins
		var positions = new[] { _currentLocationPin.Location, destinationLocation };
		MapControl.MoveToRegion(MapSpan.FromCenterAndRadius(
			new Location(
				(positions[0].Latitude + positions[1].Latitude) / 2,
				(positions[0].Longitude + positions[1].Longitude) / 2
			),
			Distance.FromKilometers(CalculateDistance(_currentLocationPin.Location, destinationLocation) * 0.7)
		));

		// Send navigation data to HUD
		await _viewModel.SendNavigationToDestination(
			_currentLocationPin.Location,
			destinationLocation
		);
	}

	private double CalculateDistance(Location start, Location end)
	{
		// Haversine formula to calculate distance between two coordinates
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
}