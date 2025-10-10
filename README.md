# WiiYii HUD Companion App – Navigation Extension Repo

The **WiiYii HUD Companion App** is an **unofficial companion application** for **WiiYii Smart HUD** devices.  
It connects to WiiYii head-up display (HUD) units via **Bluetooth**, transmitting **real-time navigation data** from your phone directly to your car’s HUD — so you can drive safely with directions always in view.

This repository contains the **open-source plugin and extension framework** that powers the app’s navigation integrations.

---

## 🧭 About the App

The app uses your existing **Google Maps navigation** — meaning there’s **no need for a separate navigation system**.  
However, its plugin-based design allows developers to add **support for additional navigation services**, such as:

- 🗺️ **MapBox**
- 🚗 **Waze**
- 🌏 **Here WeGo**
- 🧭 **OpenStreetMap (OSM)** and others

---

## 🔌 Plugin / Extension System

The app is designed with a **modular plugin architecture**.  
Each plugin acts as a bridge between a navigation provider and the WiiYii HUD display protocol.

### Plugin Responsibilities

A plugin should:
1. Receive navigation data (e.g., direction, distance, ETA, street name).
2. Format and send this data to the main app via the plugin API.
3. Allow the app to forward it to the connected HUD device over Bluetooth.

Plugins can be built in **.NET (C#)** as independent assemblies and loaded dynamically at runtime.

---

## ⚙️ Plugin Development Guide

### Requirements
- .NET 9.0 (or higher)
- .NET MAUI

### Steps
1. Clone this repository:
   ```bash
   git clone https://github.com/AvaShieldAdmin/WiiYiiHudNavigator.git
   ````

2. Create a new plugin project (e.g., `WiiYiiHudNavigator.Waze`).

3. Implement the shared interfaces:

   ```csharp
   public interface INavigationIntegrationSetup
   {
       Guid Id { get; }
       void SetupServices(IServiceCollection services);
       StartNavigationIntegrationButton GetStartNavigationButton();
       INavigationIntegrationInterface GetInterface();
       void OnAppLoaded(IServiceProvider serviceProvider);
   }

   public interface INavigationIntegrationInterface
   {
       INavigationIntegrationSetup Setup { get; }
       ContentView? GetNavigationUiContent();
       void OnAppLoaded();
       void OnAppUnloaded();
   }

   public record StartNavigationIntegrationButton(
       string Title,
       string ShortDescription,
       string IconFile,
       string PriceTagTitle
   );
   ```

4. Build and test your plugin with the main app.

5. Submit a **Pull Request** to share your plugin with the community!

---

## 📡 Communication Protocol

The app communicates with WiiYii HUD devices via Bluetooth.

Each plugin can update the HUD in real time by using the shared `IHudConnection` interface, which is registered in the dependency injection container.

Inject it into your class as follows:

```csharp
public class MyNavigationIntegration : INavigationIntegrationInterface
{
    private readonly IHudConnection _hud;

    public MyNavigationIntegration(IHudConnection hud)
    {
        _hud = hud;
    }

    public async Task SendUpdateAsync(NavigationData data)
    {
        var hudData = new HudNavigationData
        {
            Direction = data.Direction,
            DistanceToTurn = data.Distance,
            StreetName = data.Street,
            Eta = data.Eta,
            Speed = data.Speed
        };

        await _hud.UpdateNavigationData(hudData);
    }

    public void Clear()
    {
        _hud.NoNavigationData();
    }
}
```

### Interface Definition

```csharp
public interface IHudConnection
{
    void NoNavigationData();
    Task UpdateNavigationData(HudNavigationData navigationData, bool hasNoChange = false);
}
```

Use `UpdateNavigationData()` whenever your plugin has new navigation info to send to the HUD,
and call `NoNavigationData()` when no route is active.

---

## 🧠 Example: Waze Plugin Concept

A Waze integration could:

* Listen for Android **Intent broadcasts** from Waze (for next-turn info)
* Parse direction, street name, and ETA
* Convert to `HudNavigationData`
* Update the HUD via `IHudConnection`

This allows real-time Waze navigation to appear on the WiiYii HUD, just like with Google Maps.

---

## 🤝 Contributing

We welcome contributions from the community!

If you’d like to:

* Add support for another navigation provider
* Improve Bluetooth performance
* Enhance the plugin SDK
  please follow these steps:

1. Fork the repository
2. Create a feature branch
3. Commit and push your changes
4. Open a Pull Request

Please follow the project’s **code style**, **naming conventions**, and **commit message format**.

---

## 🛡️ Disclaimer

This is an **unofficial** project, created by an independent developer to enhance the functionality of **WiiYii Smart HUD** devices.
It is **not affiliated with or endorsed by WiiYii** or its official partners.

Use at your own discretion.

---

## 📄 License

This project is licensed under the **AGPL License**.
You are free to use, modify, and distribute the code with proper attribution.

