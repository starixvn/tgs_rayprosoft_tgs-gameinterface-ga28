using System.Text.Json.Nodes;
using NSubstitute;
using Tgs.GameInterface.Ga28.Core.ApplicationServices;
using Tgs.GameInterface.Ga28.Core.Models;

namespace Tgs.GameInterface.Ga28.Core.UnitTest.ApplicationServices;

public sealed class FundInGetServiceTest
{
	[Fact]
	public async Task Normal_Process()
	{
		var query = new FundInQuery(
			ApiUrl: new("http://partner-api.dev.ga28.cc"),
			PrivateKey: "8456c9de-d589-4e8a-876f-c9c8e48dda3e",
			PlayerCode: "4ucmqplxb029",
			OrderId: "01J0A7QY0ZRM8KDYE4BBZPZPER",
			Amount: 4m);
		var json = new JsonObject
		{
			["id"] = "01J0A86KKR5458KM7Y7WGM7X69",
			["ref_id"] = "01J0A7QY0ZRM8KDYE4BBZPZPER",
			["balance"] = "8"
		};
		var response = new WalletTransactionResponse(
			Id: "01J0A86KKR5458KM7Y7WGM7X69",
			RefId: "01J0A7QY0ZRM8KDYE4BBZPZPER",
			Balance: 8m,
			Resp: json.ToJsonString());
		var fakeGameApiClient = Substitute.For<IGameApiClient>();
		_ = fakeGameApiClient
			.WalletWithdrawAsync(
				apiBaseUrl: Arg.Is(query.ApiUrl),
				privateKey: Arg.Is(query.PrivateKey),
				memberRefId: Arg.Is(query.PlayerCode),
				amount: Arg.Is(query.Amount),
				refId: Arg.Is(query.OrderId))
			.Returns(ValueTask.FromResult(response));
		var sut = new FundInGetService(fakeGameApiClient);

		var actual = await sut.GetAsync(query).ConfigureAwait(true);

		Assert.NotNull(actual);
		Assert.Equal("01J0A7QY0ZRM8KDYE4BBZPZPER", actual.OrderId);
		Assert.Equal("01J0A86KKR5458KM7Y7WGM7X69", actual.ExternalOrderId);
		Assert.Equal(8m, actual.Balance);
		Assert.Equal(json.ToJsonString(), actual.Resp);
	}
}
