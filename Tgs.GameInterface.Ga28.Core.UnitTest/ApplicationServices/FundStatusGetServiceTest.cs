using NSubstitute;
using Tgs.GameInterface.Ga28.Core.ApplicationServices;

namespace Tgs.GameInterface.Ga28.Core.UnitTest.ApplicationServices;

public sealed class FundStatusGetServiceTest
{
	[Fact]
	public async Task Normal_Process()
	{
		var query = new FundStatusQuery(
			ApiUrl: new("http://partner-api.dev.ga28.cc"),
			PrivateKey: "8456c9de-d589-4e8a-876f-c9c8e48dda3e",
			PlayerCode: "4ucmqplxb029",
			OrderOn: new DateTimeOffset(2024, 06, 14, 10, 16, 00, TimeSpan.FromHours(8)),
			OrderId: "01J0A7QY0ZRM8KDYE4BBZPZPER");
		var response = true;
		var fakeGameApiClient = Substitute.For<IGameApiClient>();
		_ = fakeGameApiClient
			.WalletAsync(
				apiBaseUrl: Arg.Is(query.ApiUrl),
				privateKey: Arg.Is(query.PrivateKey),
				page: Arg.Is(1),
				pageSize: Arg.Is(32),
				memberRefId: Arg.Is(query.PlayerCode),
				refId: Arg.Is(query.OrderId),
				fromCreatedTime: Arg.Is(query.OrderOn.Add(TimeSpan.FromHours(-6))),
				toCreatedTime: Arg.Is(query.OrderOn.Add(new(hours: 5, minutes: 59, seconds: 59))))
			.Returns(ValueTask.FromResult(response));
		var sut = new FundStatusGetService(fakeGameApiClient);

		var actual = await sut.GetAsync(query).ConfigureAwait(true);

		Assert.True(actual);
	}
}
