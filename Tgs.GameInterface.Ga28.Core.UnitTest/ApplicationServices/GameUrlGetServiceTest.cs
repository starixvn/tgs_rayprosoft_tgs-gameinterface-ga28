using NSubstitute;
using Tgs.GameInterface.Ga28.Core.ApplicationServices;

namespace Tgs.GameInterface.Ga28.Core.UnitTest.ApplicationServices;

public sealed class GameUrlGetServiceTest
{
	[Fact]
	public async Task Normal_Process()
	{
		var query = new GameUrlQuery(
			ApiUrl: new("http://partner-api.dev.ga28.cc"),
			PrivateKey: "8456c9de-d589-4e8a-876f-c9c8e48dda3e",
			PartnerAccountId: "99a6f8ef-38a6-4f31-b69a-e222c2a54771",
			PlayerCode: "4ucmqplxb029");
		var response = "https://xxx.xxx.xxx/xxx?xxx=xxx";
		var fakeGameApiClient = Substitute.For<IGameApiClient>();
		_ = fakeGameApiClient
			.MembersLoginAsync(
				apiBaseUrl: Arg.Is(query.ApiUrl),
				privateKey: Arg.Is(query.PrivateKey),
				partnerAccountId: Arg.Is(query.PartnerAccountId),
				memberRefId: Arg.Is(query.PlayerCode))
			.Returns(ValueTask.FromResult(response));
		var sut = new GameUrlGetService(fakeGameApiClient);

		var actual = await sut.GetAsync(query).ConfigureAwait(true);

		Assert.Equal("https://xxx.xxx.xxx/xxx?xxx=xxx", actual);
	}
}
