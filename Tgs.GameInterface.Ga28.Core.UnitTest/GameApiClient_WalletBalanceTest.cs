using System.Net;
using System.Net.Http.Json;
using System.Text.Json.Nodes;
using Microsoft.Extensions.Logging.Abstractions;
using RichardSzalay.MockHttp;

namespace Tgs.GameInterface.Ga28.Core.UnitTest;

public sealed class GameApiClient_WalletBalanceTest
{
	[Fact]
	public async Task Normal_Process()
	{
		var apiBaseUrl = new Uri("http://partner-api.dev.ga28.cc");
		var privateKey = "8456c9de-d589-4e8a-876f-c9c8e48dda3e";
		var memberRefId = "4ucmqplxb029";
		var parameters = new Dictionary<string, string>
		{
			["member_ref_id"] = memberRefId,
		};
		var response = JsonNode.Parse("""
		{
			"balance": "10.5"
		}
		""")!;
		var handler = new MockHttpMessageHandler();
		_ = handler
			.Expect(HttpMethod.Get, new Uri(apiBaseUrl, "/api/v1/wallet/balance").ToString())
			.WithHeaders("Authorization", privateKey)
			.WithExactQueryString(parameters)
			.Respond(HttpStatusCode.OK, JsonContent.Create(response));
		var fakeHttpClient = handler.ToHttpClient();
		var fakeLogger = NullLogger<GameApiClient>.Instance;
		var sut = new GameApiClient(fakeLogger, fakeHttpClient);

		var actual = await sut.WalletBalanceAsync(
			apiBaseUrl,
			privateKey,
			memberRefId)
			.ConfigureAwait(true);

		Assert.True(actual.IsPlayerExists);
		Assert.Equal(10.5m, actual.Balance);
	}

	[Fact]
	public async Task PlayerNotExists_WhenStatusCodeBadRequest()
	{
		var apiBaseUrl = new Uri("http://partner-api.dev.ga28.cc");
		var privateKey = "8456c9de-d589-4e8a-876f-c9c8e48dda3e";
		var memberRefId = "4ucmqplxb029";
		var parameters = new Dictionary<string, string>
		{
			["member_ref_id"] = memberRefId,
		};
		var handler = new MockHttpMessageHandler();
		_ = handler
			.Expect(HttpMethod.Get, new Uri(apiBaseUrl, "/api/v1/wallet/balance").ToString())
			.WithHeaders("Authorization", privateKey)
			.WithExactQueryString(parameters)
			.Respond(HttpStatusCode.BadRequest, new StringContent(nameof(HttpStatusCode.BadRequest)));
		var fakeHttpClient = handler.ToHttpClient();
		var fakeLogger = NullLogger<GameApiClient>.Instance;
		var sut = new GameApiClient(fakeLogger, fakeHttpClient);

		var actual = await sut.WalletBalanceAsync(
			apiBaseUrl,
			privateKey,
			memberRefId)
			.ConfigureAwait(true);

		Assert.False(actual.IsPlayerExists);
		Assert.Equal(0m, actual.Balance);
	}
}
