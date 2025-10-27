# Quick Setup Guide for Main App Integration

## Step 1: Enable Maps in MauiProgram.cs

```csharp
var builder = MauiApp.CreateBuilder();
builder
  .UseMauiApp<App>()
    .UseMauiMaps() // ? Add this line
    .ConfigureFonts(fonts =>
    {
      // your font configuration
    });
```

## Step 2: Android Setup

### Get Google Maps API Key

1. Visit: https://console.cloud.google.com/
2. Create/select a project
3. Enable "Maps SDK for Android"
4. Create API Key (Credentials ? Create Credentials ? API Key)
5. Restrict key to Android apps
6. Add your package name and SHA-1 fingerprint

### Add to AndroidManifest.xml

```xml
<?xml version="1.0" encoding="utf-8"?>
<manifest xmlns:android="http://schemas.android.com/apk/res/android">
    
    <!-- Add this inside <application> tag -->
    <application>
   <meta-data 
            android:name="com.google.android.geo.API_KEY" 
       android:value="AIza..." /> <!-- Your actual API key here -->
  </application>
    
</manifest>
```

The location permissions are already declared in the SampleMap module.

## Step 3: iOS Setup

Add to Info.plist:

```xml
<key>NSLocationWhenInUseUsageDescription</key>
<string>We need your location to show you on the map and calculate routes.</string>
```

## Step 4: (Optional) Verify Setup

In your MainActivity (Android) or AppDelegate (iOS), you can verify the setup:

```csharp
// Android MainActivity.OnCreate
#if ANDROID
WiiYiiHudNavigator.Plugin.SampleMap.Platforms.AndroidMapSetup.VerifySetup();
#endif

// iOS AppDelegate.FinishedLaunching
#if IOS
WiiYiiHudNavigator.Plugin.SampleMap.Platforms.iOSMapSetup.VerifySetup();
#endif
```

## Testing

1. Run the app
2. Navigate to "Sample Map - Free"
3. Allow location permissions when prompted
4. Tap anywhere on the map to set a destination
5. View navigation data in the bottom panel
6. Check HUD receives the navigation updates

## Troubleshooting

### "Map shows but location doesn't work"
- Check location permissions are granted
- Verify Info.plist (iOS) or AndroidManifest.xml (Android) has location permissions

### "Android map is blank/gray"
- Verify Google Maps API Key is correct
- Ensure API Key is enabled for "Maps SDK for Android"
- Check package name matches your app
- Verify SHA-1 fingerprint is added to API key restrictions

### "iOS map works but crashes on tap"
- Ensure location permissions are granted
- Check Info.plist has NSLocationWhenInUseUsageDescription

## API Key Security

?? **Important**: In production:
- Use different API keys for debug/release builds
- Restrict API keys properly
- Consider using backend proxy for API calls
- Don't commit API keys to public repositories
