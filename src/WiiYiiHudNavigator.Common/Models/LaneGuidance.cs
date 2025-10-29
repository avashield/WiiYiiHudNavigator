namespace WiiYiiHudNavigator.Common.Models;

/// <summary>
/// Represents lane guidance information for HUD display
/// </summary>
public record LaneGuidance
{
	/// <summary>
	/// Maximum number of lanes supported by the HUD
	/// </summary>
	public const int MaxLanes = 12;

	/// <summary>
	/// List of lanes (up to 12)
	/// </summary>
	public IReadOnlyList<Lane> Lanes { get; init; } = Array.Empty<Lane>();

	/// <summary>
	/// Creates lane guidance from a list of lanes
	/// </summary>
	public LaneGuidance(IEnumerable<Lane> lanes)
	{
		var laneList = lanes?.Take(MaxLanes).ToList() ?? new List<Lane>();
		Lanes = laneList.AsReadOnly();
	}

	/// <summary>
	/// Creates lane guidance from individual lanes
	/// </summary>
	public LaneGuidance(params Lane[] lanes) : this(lanes.AsEnumerable())
	{
	}

	/// <summary>
	/// Converts the lane guidance to the byte array format expected by the HUD
	/// </summary>
	public byte[] ToByteArray()
	{
		var bytes = new byte[MaxLanes];
		
		for (int i = 0; i < MaxLanes; i++)
		{
			if (i < Lanes.Count)
			{
				bytes[i] = Lanes[i].ToByte();
			}
			else
			{
				// Fill remaining lanes with 0x0F (Cancel display / invalid lane)
				bytes[i] = 0x0F;
			}
		}
		
		return bytes;
	}
}

/// <summary>
/// Represents a single lane with its direction and recommendation status
/// </summary>
public record Lane
{
	/// <summary>
	/// The direction of the lane
	/// </summary>
	public LaneDirection Direction { get; init; }

	/// <summary>
	/// Whether this lane is recommended
	/// </summary>
	public bool IsRecommended { get; init; }

	public Lane(LaneDirection direction, bool isRecommended = false)
	{
		Direction = direction;
		IsRecommended = isRecommended;
	}

	/// <summary>
	/// Converts the lane to a byte value for HUD transmission
	/// </summary>
	public byte ToByte()
	{
		byte value = Direction switch
		{
			LaneDirection.Straight => 0x00,
			LaneDirection.Left => 0x01,
			LaneDirection.Right => 0x03,
			LaneDirection.TurnAroundLeft => 0x05,
			LaneDirection.SlightRight => 0x08,
			LaneDirection.Cancel => 0x0F,
			_ => 0x0F
		};

		// If recommended, set the high bit (0x80)
		if (IsRecommended && Direction != LaneDirection.Cancel)
		{
			value |= 0x80;
		}

		return value;
	}

	/// <summary>
	/// Creates a lane from a byte value
	/// </summary>
	public static Lane FromByte(byte value)
	{
		bool isRecommended = (value & 0x80) != 0;
		byte directionByte = (byte)(value & 0x7F);

		LaneDirection direction = directionByte switch
		{
			0x00 => LaneDirection.Straight,
			0x01 => LaneDirection.Left,
			0x03 => LaneDirection.Right,
			0x05 => LaneDirection.TurnAroundLeft,
			0x08 => LaneDirection.SlightRight,
			0x0F => LaneDirection.Cancel,
			_ => LaneDirection.Cancel
		};

		return new Lane(direction, isRecommended);
	}
}

/// <summary>
/// Lane direction types supported by the HUD
/// </summary>
public enum LaneDirection
{
	/// <summary>
	/// Straight lane (0x00)
	/// </summary>
	Straight = 0,

	/// <summary>
	/// Turn left lane (0x01)
	/// </summary>
	Left = 1,

	/// <summary>
	/// Turn right lane (0x03)
	/// </summary>
	Right = 3,

	/// <summary>
	/// Slight left / left merge lane (0x05)
	/// </summary>
	TurnAroundLeft = 5,

	/// <summary>
	/// Slight right / right merge lane (0x08)
	/// </summary>
	SlightRight = 8,

	/// <summary>
	/// Invalid or cancelled lane display (0x0F).
	/// Important: To remove the display of any lane data this must be used at least once after the last valid lane.
	/// </summary>
	Cancel = 15
}
