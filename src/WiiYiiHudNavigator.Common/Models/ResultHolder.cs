namespace WiiYiiHudNavigator.Common.Models;

public class ResultHolder : IComparable<bool>, IEquatable<bool>
{
	public static readonly ResultHolder Successful = new ResultHolder(true);

	public ResultHolder()
	{
	}

	public ResultHolder(bool success)
	{
		Success = success;
	}

	public ResultHolder(bool success, string message)
	{
		Success = success;
		Message = message;
	}

	public bool Success { get; }

	public string? Message { get; }

	public static ResultHolder Failed(string message) =>
		new(false, message);

	public static ResultHolder Failed() =>
		new(false);

	public int CompareTo(bool other)
	{
		return Success.CompareTo(other);
	}

	public bool Equals(bool other)
	{
		return Success == other;
	}
}

public class ResultHolder<T> : ResultHolder
{
	public ResultHolder(bool success, string message) 
		:base(success, message)
	{
	}
	public ResultHolder(bool success)
		: base(success)
	{
	}

	public ResultHolder(string message)
		: base(false,message)
	{
	}

	public ResultHolder()
		:base()
	{
	}

	public T? Result { get; set; }

	public static new ResultHolder<T> Failed(string message) =>
		new(false, message);

	public static new ResultHolder<T> Failed() =>
		new(false);

}
