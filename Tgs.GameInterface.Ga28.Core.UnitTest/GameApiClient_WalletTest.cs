using System.Net;
using System.Net.Http.Json;
using System.Text.Json.Nodes;
using Microsoft.Extensions.Logging.Abstractions;
using RichardSzalay.MockHttp;

namespace Tgs.GameInterface.Ga28.Core.UnitTest;

public sealed class GameApiClient_WalletTest
{
	private static readonly TimeZoneInfo _TimeZone = TimeZoneInfo.CreateCustomTimeZone(
		id: "China Standard Time",
		baseUtcOffset: TimeSpan.FromHours(+8),
		displayName: "China Standard Time",
		standardDisplayName: "China Standard Time");

	[Fact]
	public async Task FoundOut_WhenTypeDepositRecord()
	{
		var apiBaseUrl = new Uri("http://partner-api.dev.ga28.cc");
		var privateKey = "8456c9de-d589-4e8a-876f-c9c8e48dda3e";
		var memberRefId = "4ucmqplxb029";
		var page = 1;
		var pageSize = 32;
		var refId = "01J08C6MJAGNERAPPH56G7QEDN";
		var fromCreatedTime = new DateTimeOffset(2024, 06, 13, 16, 00, 00, TimeSpan.FromHours(8));
		var toCreatedTime = new DateTimeOffset(2024, 06, 13, 16, 10, 00, TimeSpan.FromHours(8));
		var parameters = new Dictionary<string, string>
		{
			["page"] = page.ToString(),
			["page_size"] = pageSize.ToString(),
			["member_ref_id"] = memberRefId,
			["ref_id"] = refId,
			["from_created_date_time"] = TimeZoneInfo.ConvertTime(fromCreatedTime.ToUniversalTime(), _TimeZone).ToString("yyyy-MM-dd'T'HH:mm:sszzz"),
			["to_created_date_time"] = TimeZoneInfo.ConvertTime(toCreatedTime.ToUniversalTime(), _TimeZone).ToString("yyyy-MM-dd'T'HH:mm:sszzz"),
		};
		var response = JsonNode.Parse("""
		[
			{
				"id": "2b3c4a5a-1e35-412a-b741-c08235c36979",
				"date_time_created": "2024-06-13T08:50:30.908907Z",
				"amount": "10",
				"ref_id": "8bbb4c6d88064bd6a0b6e6bbc6523224",
				"description": "Deposit",
				"member_ref_id": "4ucmqplxb029",
				"type": 7
			}
		]
		""")!;
		var handler = new MockHttpMessageHandler();
		_ = handler
			.Expect(HttpMethod.Get, new Uri(apiBaseUrl, "/api/v1/wallet/").ToString())
			.WithHeaders("Authorization", privateKey)
			.WithExactQueryString(parameters)
			.Respond(HttpStatusCode.OK, JsonContent.Create(response));
		var fakeHttpClient = handler.ToHttpClient();
		var fakeLogger = NullLogger<GameApiClient>.Instance;
		var sut = new GameApiClient(fakeLogger, fakeHttpClient);

		var actual = await sut.WalletAsync(
			apiBaseUrl,
			privateKey,
			page,
			pageSize,
			memberRefId,
			refId,
			fromCreatedTime,
			toCreatedTime)
			.ConfigureAwait(true);

		Assert.True(actual);
	}

	[Fact]
	public async Task FoundOut_WhenTypeWithdrawRecord()
	{
		var apiBaseUrl = new Uri("http://partner-api.dev.ga28.cc");
		var privateKey = "8456c9de-d589-4e8a-876f-c9c8e48dda3e";
		var memberRefId = "4ucmqplxb029";
		var page = 1;
		var pageSize = 32;
		var refId = "01J08C6MJAGNERAPPH56G7QEDN";
		var fromCreatedTime = new DateTimeOffset(2024, 06, 13, 16, 00, 00, TimeSpan.FromHours(8));
		var toCreatedTime = new DateTimeOffset(2024, 06, 13, 16, 10, 00, TimeSpan.FromHours(8));
		var parameters = new Dictionary<string, string>
		{
			["page"] = page.ToString(),
			["page_size"] = pageSize.ToString(),
			["member_ref_id"] = memberRefId,
			["ref_id"] = refId,
			["from_created_date_time"] = TimeZoneInfo.ConvertTime(fromCreatedTime.ToUniversalTime(), _TimeZone).ToString("yyyy-MM-dd'T'HH:mm:sszzz"),
			["to_created_date_time"] = TimeZoneInfo.ConvertTime(toCreatedTime.ToUniversalTime(), _TimeZone).ToString("yyyy-MM-dd'T'HH:mm:sszzz"),
		};
		var response = JsonNode.Parse("""
		[
			{
				"id": "2b3c4a5a-1e35-412a-b741-c08235c36979",
				"date_time_created": "2024-06-13T08:50:30.908907Z",
				"amount": "10",
				"ref_id": "8bbb4c6d88064bd6a0b6e6bbc6523224",
				"description": "Deposit",
				"member_ref_id": "4ucmqplxb029",
				"type": 8
			}
		]
		""")!;
		var handler = new MockHttpMessageHandler();
		_ = handler
			.Expect(HttpMethod.Get, new Uri(apiBaseUrl, "/api/v1/wallet/").ToString())
			.WithHeaders("Authorization", privateKey)
			.WithExactQueryString(parameters)
			.Respond(HttpStatusCode.OK, JsonContent.Create(response));
		var fakeHttpClient = handler.ToHttpClient();
		var fakeLogger = NullLogger<GameApiClient>.Instance;
		var sut = new GameApiClient(fakeLogger, fakeHttpClient);

		var actual = await sut.WalletAsync(
			apiBaseUrl,
			privateKey,
			page,
			pageSize,
			memberRefId,
			refId,
			fromCreatedTime,
			toCreatedTime)
			.ConfigureAwait(true);

		Assert.True(actual);
	}

	[Fact]
	public async Task NotFound_WhenStatusCodeNotFound()
	{
		var apiBaseUrl = new Uri("http://partner-api.dev.ga28.cc");
		var privateKey = "8456c9de-d589-4e8a-876f-c9c8e48dda3e";
		var memberRefId = "4ucmqplxb029";
		var page = 1;
		var pageSize = 32;
		var refId = "01J08C6MJAGNERAPPH56G7QEDN";
		var fromCreatedTime = new DateTimeOffset(2024, 06, 13, 16, 00, 00, TimeSpan.FromHours(8));
		var toCreatedTime = new DateTimeOffset(2024, 06, 13, 16, 10, 00, TimeSpan.FromHours(8));
		var parameters = new Dictionary<string, string>
		{
			["page"] = page.ToString(),
			["page_size"] = pageSize.ToString(),
			["member_ref_id"] = memberRefId,
			["ref_id"] = refId,
			["from_created_date_time"] = TimeZoneInfo.ConvertTime(fromCreatedTime.ToUniversalTime(), _TimeZone).ToString("yyyy-MM-dd'T'HH:mm:sszzz"),
			["to_created_date_time"] = TimeZoneInfo.ConvertTime(toCreatedTime.ToUniversalTime(), _TimeZone).ToString("yyyy-MM-dd'T'HH:mm:sszzz"),
		};
		var handler = new MockHttpMessageHandler();
		_ = handler
			.Expect(HttpMethod.Get, new Uri(apiBaseUrl, "/api/v1/wallet/").ToString())
			.WithHeaders("Authorization", privateKey)
			.WithExactQueryString(parameters)
			.Respond(HttpStatusCode.NotFound, new StringContent(nameof(HttpStatusCode.NotFound)));
		var fakeHttpClient = handler.ToHttpClient();
		var fakeLogger = NullLogger<GameApiClient>.Instance;
		var sut = new GameApiClient(fakeLogger, fakeHttpClient);

		var actual = await sut.WalletAsync(
			apiBaseUrl,
			privateKey,
			page,
			pageSize,
			memberRefId,
			refId,
			fromCreatedTime,
			toCreatedTime)
			.ConfigureAwait(true);

		Assert.False(actual);
	}
}
