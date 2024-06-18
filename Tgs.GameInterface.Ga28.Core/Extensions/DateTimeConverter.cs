namespace Tgs.GameInterface.Ga28.Core.Extensions;

internal sealed class DateTimeConverter
{
	private readonly TimeZoneInfo _timeZone = TimeZoneInfo.CreateCustomTimeZone(
		id: "China Standard Time",
		baseUtcOffset: TimeSpan.FromHours(+8),
		displayName: "China Standard Time",
		standardDisplayName: "China Standard Time");

	private readonly string _toStringformat = "yyyy-MM-dd'T'HH:mm:sszzz";

	public string DatetimeConvertToString(DateTimeOffset time)
		=> TimeZoneInfo.ConvertTime(time.ToUniversalTime(), _timeZone).ToString(_toStringformat);
}
