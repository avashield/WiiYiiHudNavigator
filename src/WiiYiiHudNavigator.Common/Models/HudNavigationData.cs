namespace WiiYiiHudNavigator.Common.Models;

public class HudNavigationData
{

	/// <summary>
	/// Destination name
	/// Extracted from: routeLegProgress.DestinationName
	/// </summary>
	public string? DestinationName { get; set; }

	/// <summary>
	/// Duration remaining in `seconds` (used to calculate hours and minutes)
	/// Extracted from: routeLegProgress.EtaDuration?.Duration?.TotalSeconds
	/// </summary>
	public double EtaDurationTimeRemaining { get; set; }

	/// <summary>
	/// Distance remaining in `meters`
	/// Extracted from: routeLegProgress.RemainingDistance?.GetDistanceInMeter()
	/// Used in: GetRemainingTimeAndDistance (divided by 100)
	/// </summary>
	public double EtaDistanceRemaining { get; set; }

	/// <summary>
	/// Next turn direction instruction
	/// Extracted from: routeLegProgress.NextDirection.Direction
	/// </summary>
	public DirectionInstruction? NextTurnDirection { get; set; }

	/// <summary>
	/// Second turn direction instruction
	/// Extracted from: routeLegProgress.NextDirection.Direction
	/// </summary>
	public DirectionInstruction? SecondTurnDirection { get; set; }

	/// <summary>
	/// Distance to next turn in meters
	/// Extracted from: routeLegProgress.NextDirection.Distance?.GetDistanceInMeter()
	/// </summary>
	public double NextTurnDistance { get; set; }

	/// <summary>
	/// Road name or next direction display text
	/// Extracted from: routeLegProgress.NextDirection.DisplayText
	/// Converted to Simplified Chinese before sending to HUD
	/// </summary>
	public string? NextTurnRoadName { get; set; }

	/// <summary>
	/// Indicates if route is being recalculated
	/// Extracted from: routeLegProgress.IsRerouting
	/// </summary>
	public bool IsRerouting { get; set; }
}