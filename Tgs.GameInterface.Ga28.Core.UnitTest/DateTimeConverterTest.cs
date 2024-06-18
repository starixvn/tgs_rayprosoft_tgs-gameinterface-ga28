using Tgs.GameInterface.Ga28.Core.Extensions;

namespace Tgs.GameInterface.Ga28.Core.UnitTest;

public sealed class DateTimeConverterTest
{
	[Fact]
	public void TestDatetimeConvertToString()
	{
		var sut = new DateTimeConverter();

		var actual = sut.DatetimeConvertToString(new DateTimeOffset(2024, 06, 04, 13, 12, 11, offset: TimeSpan.FromHours(8)));

		Assert.Equal("2024-06-04T13:12:11+08:00", actual);
	}
}
