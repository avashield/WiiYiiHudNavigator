# Sample Map Navigation Integration

This module provides an interactive map interface using Microsoft.Maui.Controls.Maps for navigation testing.

## Features

- ??? **Real Map Integration**: Uses Google Maps (Android) and Apple Maps (iOS)
- ?? **Location Selection**: Tap anywhere on the map to set a destination
- ?? **Smart Direction Calculation**: Automatically calculates bearing and direction instructions
- ?? **Navigation Data**: Sends realistic navigation data to HUD including:
  - Distance and duration
  - Turn directions based on bearing
  - Road names (simulated)
  - ETA calculations

## Setup Instructions

### 1. Main App Configuration

In your main app's `MauiProgram.cs`, add the following:

```csharp
using WiiYiiHudNavigator.SampleMap;

public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();
    builder
 .UseMauiApp<App>()
            // Add this line to enable maps:
   .UseMauiMaps();
        
  // ... rest of your configuration
        
     return builder.Build();
 }
}
```

### 2. Android Configuration

#### AndroidManifest.xml

Add the following permissions and metadata:

```xml
<manifest>
    <!-- Location permissions -->
    <uses-permission android:name="android.permission.ACCESS_COARSE_LOCATION" />
<uses-permission android:name="android.permission.ACCESS_FINE_LOCATION" />
    <uses-feature android:name="android.hardware.location" android:required="false" />
    <uses-feature android:name="android.hardware.location.gps" android:required="false" />
    <uses-feature android:name="android.hardware.location.network" android:required="false" />

    <application>
    <!-- Google Maps API Key -->
        <meta-data 
            android:name="com.google.android.geo.API_KEY" 
    android:value="YOUR_GOOGLE_MAPS_API_KEY_HERE" />
    </application>
</manifest>
```

#### Get Google Maps API Key:

1. Go to [Google Cloud Console](https://console.cloud.google.com/)
2. Create a new project or select existing
3. Enable "Maps SDK for Android"
4. Create credentials ? API Key
5. Restrict the API key to Android apps
6. Add your app's package name and SHA-1 certificate fingerprint

### 3. iOS Configuration

#### Info.plist

Add location usage descriptions:

```xml
<key>NSLocationWhenInUseUsageDescription</key>
<string>This app needs access to your location to show you on the map and calculate navigation routes.</string>
<key>NSLocationAlwaysUsageDescription</key>
<string>This app needs access to your location for navigation features.</string>
```

### 4. Windows Configuration (Optional)

For Windows, you'll need a Bing Maps API key:

#### Add to MauiProgram.cs:

```csharp
#if WINDOWS
builder.ConfigureEssentials(essentials =>
{
    essentials.MapServiceToken = "YOUR_BING_MAPS_API_KEY_HERE";
});
#endif
```

Get a Bing Maps key from: https://www.bingmapsportal.com/

## Usage

The module automatically:

1. Shows the user's current location (or defaults to San Francisco)
2. Displays an interactive map
3. Allows tapping to set destinations
4. Draws route lines between current location and destination
5. Calculates realistic navigation data:
   - Distance using Haversine formula
   - Duration based on 50 km/h average speed
   - Direction based on geographic bearing
   - Random road names and turn sequences

## Permissions

The app will automatically request location permissions when loaded. Make sure to handle permission denials gracefully in production.

## Dependencies

- `Microsoft.Maui.Controls.Maps` (v9.0.120+)
- Platform-specific map services (Google Maps for Android, Apple Maps for iOS)

## Notes

- The module generates simulated road names for testing purposes
- Actual turn-by-turn navigation requires integration with a routing service (Google Directions API, etc.)
- Distance and duration calculations are estimates based on straight-line distance
- For production use, consider integrating with a real routing service
