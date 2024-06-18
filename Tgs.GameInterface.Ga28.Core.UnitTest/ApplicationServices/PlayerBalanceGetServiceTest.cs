using NSubstitute;
using Tgs.GameInterface.Ga28.Core.ApplicationServices;
using Tgs.GameInterface.Ga28.Core.Models;

namespace Tgs.GameInterface.Ga28.Core.UnitTest.ApplicationServices;

public sealed class PlayerBalanceGetServiceTest
{
	[Fact]
	public async Task Normal_Process()
	{
		var query = new PlayerBalanceQuery(
			ApiUrl: new("http://partner-api.dev.ga28.cc"),
			PrivateKey: "8456c9de-d589-4e8a-876f-c9c8e48dda3e",
			PlayerCode: "4ucmqplxb029");
		var response = new WalletBalanceResponse(
			IsPlayerExist: true,
			Balance: 66.6m);
		var fakeGameApiClient = Substitute.For<IGameApiClient>();
		_ = fakeGameApiClient
			.WalletBalanceAsync(
				apiBaseUrl: Arg.Is(query.ApiUrl),
				privateKey: Arg.Is(query.PrivateKey),
				memberRefId: Arg.Is(query.PlayerCode))
			.Returns(ValueTask.FromResult(response));

		var sut = new PlayerBalanceGetService(fakeGameApiClient);

		var actual = await sut.GetAsync(query).ConfigureAwait(true);

		Assert.True(actual.IsPlayerExist);
		Assert.Equal(66.6m, actual.Balance);
	}
}
