# Lane Guidance Usage Examples

## Basic Usage

### Example 1: Three lanes (left turn, straight recommended, right turn)
```csharp
var navigationData = new HudNavigationData
{
    LaneGuidance = new LaneGuidance(
new Lane(LaneDirection.Left),
        new Lane(LaneDirection.Straight, isRecommended: true),
        new Lane(LaneDirection.Right)
    )
};
```

### Example 2: Four lanes with multiple directions
```csharp
var lanes = new[]
{
    new Lane(LaneDirection.Left),
    new Lane(LaneDirection.Left),
    new Lane(LaneDirection.Straight, isRecommended: true),
    new Lane(LaneDirection.Right)
};

var navigationData = new HudNavigationData
{
    LaneGuidance = new LaneGuidance(lanes)
};
```

### Example 3: Complex intersection with slight turns
```csharp
var navigationData = new HudNavigationData
{
    LaneGuidance = new LaneGuidance(
        new Lane(LaneDirection.Left),
        new Lane(LaneDirection.TurnAroundLeft),
    new Lane(LaneDirection.Straight, isRecommended: true),
        new Lane(LaneDirection.SlightRight),
    new Lane(LaneDirection.Right)
    )
};
```

## Lane Direction Types

- `LaneDirection.Straight` - Straight lane (0x00)
- `LaneDirection.Left` - Turn left lane (0x01)
- `LaneDirection.Right` - Turn right lane (0x03)
- `LaneDirection.TurnAroundLeft` - Turn around left / U-turn lane (0x05)
- `LaneDirection.SlightRight` - Slight right / right merge lane (0x08)
- `LaneDirection.Cancel` - Cancel/remove lane display (0x0F)

## Important Notes

### ?? Removing Lane Guidance Display
**IMPORTANT**: To remove the display of lane data on the HUD, you must send `LaneDirection.Cancel` at least once after the last valid lane. The HUD will not automatically clear lane guidance when navigation ends.

```csharp
// Example: Clearing lane guidance after navigation ends
var navigationData = new HudNavigationData
{
    LaneGuidance = new LaneGuidance(
        new Lane(LaneDirection.Cancel)
  )
};

await hudConnection.UpdateNavigationData(navigationData);
```

### ?? Removing Camera Display
**IMPORTANT**: To remove the camera icon from the HUD display, you must send `CameraDistance = 0` at least once after showing the camera. The HUD will not automatically clear the camera icon.

```csharp
// Example: Clearing camera display
var navigationData = new HudNavigationData
{
    CameraDistance = 0  // This removes the camera icon
};

await hudConnection.UpdateNavigationData(navigationData);

// Or use null to not send camera data at all
var navigationData = new HudNavigationData
{
    CameraDistance = null  // Camera data not sent
};
```

## Features

### Type Safety
The `LaneGuidance` record provides type-safe lane configuration with compile-time checking.

### Automatic Byte Conversion
The record automatically converts lane information to the byte format expected by the HUD:
- Handles recommended lane marking (sets high bit 0x80)
- Fills unused lanes with 0x0F (invalid)
- Limits to maximum 12 lanes

### Immutability
Records are immutable by default, preventing accidental modifications.

### Conversion Methods

#### To Byte Array
```csharp
LaneGuidance guidance = new LaneGuidance(/* lanes */);
byte[] bytes = guidance.ToByteArray();
```

#### From Byte Array
```csharp
byte laneData = 0x81; // Left turn, recommended
Lane lane = Lane.FromByte(laneData);
// lane.Direction == LaneDirection.Left
// lane.IsRecommended == true
```

## HUD Behavior

- The HUD supports up to 12 lanes
- Only one lane should be marked as recommended (high bit set)
- Unused lane positions are filled with 0x0F
- The recommended lane is visually highlighted on the HUD display
- **Lane guidance persists until explicitly cleared with `LaneDirection.Cancel`**
- **Camera icon persists until explicitly cleared with `CameraDistance = 0`**

## Example: Complete Navigation Data

```csharp
var navigationData = new HudNavigationData
{
    NextTurnDirection = DirectionInstruction.Right,
    NextTurnDistance = 500,
    NextTurnRoadName = "Main Street",
    EtaDurationTimeRemaining = 1800, // 30 minutes
    EtaDistanceRemaining = 15000, // 15 km
    CameraDistance = 200,
LaneGuidance = new LaneGuidance(
        new Lane(LaneDirection.Left),
        new Lane(LaneDirection.Straight),
   new Lane(LaneDirection.Right, isRecommended: true)
    )
};

await hudConnection.UpdateNavigationData(navigationData);
```

## Example: Clearing All HUD Displays

```csharp
// When navigation ends, clear all displays
var clearData = new HudNavigationData
{
    CameraDistance = 0,  // Clear camera icon
    LaneGuidance = new LaneGuidance(
        new Lane(LaneDirection.Cancel)// Clear lane guidance
    )
};

await hudConnection.UpdateNavigationData(clearData);

// Then call NoNavigationData to stop navigation completely
hudConnection.NoNavigationData();
```

## Best Practices

1. **Always clear displays**: When navigation ends or guidance is no longer needed, explicitly clear camera and lane displays
2. **Single recommended lane**: Only mark one lane as recommended per guidance update
3. **Use Cancel appropriately**: Send `LaneDirection.Cancel` when lane guidance is no longer needed
4. **Camera management**: Set `CameraDistance = 0` to remove camera icon after passing the camera location
5. **Keep alive**: Send at least one navigation update per second to keep the HUD display active
