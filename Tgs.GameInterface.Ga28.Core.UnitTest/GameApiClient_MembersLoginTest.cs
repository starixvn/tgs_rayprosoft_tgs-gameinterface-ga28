using System.Net;
using System.Net.Http.Json;
using System.Text.Json.Nodes;
using Microsoft.Extensions.Logging.Abstractions;
using RichardSzalay.MockHttp;

namespace Tgs.GameInterface.Ga28.Core.UnitTest;

public sealed class GameApiClient_MembersLoginTest
{
	[Fact]
	public async Task Normal_Process()
	{
		var apiBaseUrl = new Uri("http://partner-api.dev.ga28.cc");
		var privateKey = "8456c9de-d589-4e8a-876f-c9c8e48dda3e";
		var memberRefId = "4ucmqplxb029";
		var partnerAccountId = "99a6f8ef-38a6-4f31-b69a-e222c2a54771";
		var parameters = new Dictionary<string, string>
		{
			["member_ref_id"] = memberRefId,
			["account_id"] = partnerAccountId,
		};
		var response = JsonNode.Parse("""
		{
			"token": "ea23026e-80bc-482a-9465-8f89c4e8a38d",
			"game_client_url": "http://gc-dev.ga28.cc/launch?member_token=8d475cde-b3af-4a37-aa00-3eda04018b2d",
			"member": {
				"member_ref_id": "4ucmqplxb029",
				"display_name": "4ucmqplxb029",
				"account_id": "99a6f8ef-38a6-4f31-b69a-e222c2a54771",
				"enable": true,
				"freeze": false
			}
		}
		""")!;
		var handler = new MockHttpMessageHandler();
		_ = handler
			.Expect(HttpMethod.Post, new Uri(apiBaseUrl, "/api/v1/members/login").ToString())
			.WithHeaders("Authorization", privateKey)
			.WithJsonContent(parameters)
			.Respond(HttpStatusCode.OK, JsonContent.Create(response));
		var fakeHttpClient = handler.ToHttpClient();
		var fakeLogger = NullLogger<GameApiClient>.Instance;
		var sut = new GameApiClient(fakeLogger, fakeHttpClient);

		var actual = await sut.MembersLoginAsync(
			apiBaseUrl,
			privateKey,
			partnerAccountId,
			memberRefId)
			.ConfigureAwait(true);

		Assert.Equal("http://gc-dev.ga28.cc/launch?member_token=8d475cde-b3af-4a37-aa00-3eda04018b2d", actual);
	}
}
