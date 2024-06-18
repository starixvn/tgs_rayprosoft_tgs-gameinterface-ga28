using System.Net;
using System.Net.Http.Json;
using System.Text.Json.Nodes;
using Microsoft.Extensions.Logging.Abstractions;
using RichardSzalay.MockHttp;

namespace Tgs.GameInterface.Ga28.Core.UnitTest;

public sealed class GameApiClient_WalletDepositTest
{
	[Fact]
	public async Task Normal_Process()
	{
		var apiBaseUrl = new Uri("http://partner-api.dev.ga28.cc");
		var privateKey = "8456c9de-d589-4e8a-876f-c9c8e48dda3e";
		var memberRefId = "4ucmqplxb029";
		var amount = 16m;
		var refId = "85a32de0-883b-4bd1-b44a-43e703f6137d";
		var parameters = new Dictionary<string, string>
		{
			["member_ref_id"] = memberRefId,
			["amount"] = amount.ToString("0.##"),
			["ref_id"] = refId,
		};
		var response = JsonNode.Parse("""
		{
			"balance": "1920.98",
			"wallet_transaction": {
				"id": "01J0A5AC9DRN2ZNXEQF0QPY68J",
				"date_time_created": "2023-11-29T11:37:54.416634096Z",
				"amount": "16",
				"ref_id": "85a32de0-883b-4bd1-b44a-43e703f6137d",
				"description": "Deposit",
				"member_ref_id": "4ucmqplxb029",
				"type": 7
			}
		}
		""")!;
		var handler = new MockHttpMessageHandler();
		_ = handler
			.Expect(HttpMethod.Post, new Uri(apiBaseUrl, "/api/v1/wallet/deposit").ToString())
			.WithHeaders("Authorization", privateKey)
			.WithJsonContent(parameters)
			.Respond(HttpStatusCode.Created, JsonContent.Create(response));
		var fakeHttpClient = handler.ToHttpClient();
		var fakeLogger = NullLogger<GameApiClient>.Instance;
		var sut = new GameApiClient(fakeLogger, fakeHttpClient);

		var actual = await sut.WalletDepositAsync(
			apiBaseUrl,
			privateKey,
			memberRefId,
			amount,
			refId)
			.ConfigureAwait(true);

		Assert.Equal("01J0A5AC9DRN2ZNXEQF0QPY68J", actual.Id);
		Assert.Equal("85a32de0-883b-4bd1-b44a-43e703f6137d", actual.RefId);
		Assert.Equal(1920.98m, actual.Balance);
		Assert.Equal(response.ToJsonString(), actual.Resp);
	}
}
