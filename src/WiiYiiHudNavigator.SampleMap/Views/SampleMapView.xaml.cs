using WiiYiiHudNavigator.SampleMap.ViewModels;

namespace WiiYiiHudNavigator.SampleMap.Views;

public partial class SampleMapView : ContentView
{
	private SampleMapVm? _viewModel;
	
	public SampleMapView()
	{
		InitializeComponent();
	}

	private void ContentView_Loaded(object sender, EventArgs e)
	{
		_viewModel = BindingContext as SampleMapVm;
	}

	private void ContentView_Unloaded(object sender, EventArgs e)
	{
		_viewModel = null;
	}

	private async void OnMapTapped(object? sender, TappedEventArgs e)
	{
		if (_viewModel == null || MapCanvas == null)
			return;

		// Get the tap position
		var tapPosition = e.GetPosition(MapCanvas);
		if (tapPosition == null)
			return;

		// Get the current location marker position (center of the map)
		var mapWidth = MapCanvas.Width;
		var mapHeight = MapCanvas.Height;
		
		var startX = mapWidth * 0.5;
		var startY = mapHeight * 0.5;

		// Set destination marker at tap position
		var destX = tapPosition.Value.X - 16; // Offset for marker center
		var destY = tapPosition.Value.Y - 16;

		AbsoluteLayout.SetLayoutBounds(DestinationMarker, new Rect(destX, destY, -1, -1));
		DestinationMarker.IsVisible = true;

		// Draw route line between current location and destination
		var lineStartX = startX;
		var lineStartY = startY;
		var lineEndX = tapPosition.Value.X;
		var lineEndY = tapPosition.Value.Y;

		// Calculate line length and angle
		var deltaX = lineEndX - lineStartX;
		var deltaY = lineEndY - lineStartY;
		var distance = Math.Sqrt(deltaX * deltaX + deltaY * deltaY);
		var angle = Math.Atan2(deltaY, deltaX) * (180 / Math.PI);

		// Position and rotate the line
		AbsoluteLayout.SetLayoutBounds(RouteLine, new Rect(lineStartX, lineStartY, distance, 3));
		RouteLine.Rotation = angle;
		RouteLine.AnchorX = 0;
		RouteLine.AnchorY = 0.5;
		RouteLine.IsVisible = true;

		// Animate the markers
		await Task.WhenAll(
			DestinationMarker.ScaleTo(1.2, 150),
			CurrentLocationMarker.ScaleTo(1.2, 150)
		);
		
		await Task.WhenAll(
			DestinationMarker.ScaleTo(1.0, 150),
			CurrentLocationMarker.ScaleTo(1.0, 150)
		);

		// Send navigation data to HUD
		await _viewModel.SendNavigationToDestination(tapPosition.Value.X, tapPosition.Value.Y, mapWidth, mapHeight);
	}
}