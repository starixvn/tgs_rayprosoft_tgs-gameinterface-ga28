using System.Net;
using System.Net.Http.Json;
using System.Text.Json.Nodes;
using Microsoft.Extensions.Logging.Abstractions;
using RichardSzalay.MockHttp;

namespace Tgs.GameInterface.Ga28.Core.UnitTest;

public sealed class GameApiClient_TicketsTest
{
	private static readonly TimeZoneInfo _TimeZone = TimeZoneInfo.CreateCustomTimeZone(
		id: "China Standard Time",
		baseUtcOffset: TimeSpan.FromHours(+8),
		displayName: "China Standard Time",
		standardDisplayName: "China Standard Time");

	[Fact]
	public async Task Win()
	{
		var apiBaseUrl = new Uri("http://partner-api.dev.ga28.cc");
		var privateKey = "8456c9de-d589-4e8a-876f-c9c8e48dda3e";
		var page = 1;
		var pageSize = 32;
		var fromModifiedTime = new DateTimeOffset(2024, 06, 13, 16, 00, 00, TimeSpan.FromHours(8));
		var toModifiedTime = new DateTimeOffset(2024, 06, 13, 16, 10, 00, TimeSpan.FromHours(8));
		var accountId = "99a6f8ef-38a6-4f31-b69a-e222c2a54771";
		var parameters = new Dictionary<string, string>
		{
			["page"] = page.ToString(),
			["page_size"] = pageSize.ToString(),
			["from_modified_date_time"] = TimeZoneInfo.ConvertTime(fromModifiedTime.ToUniversalTime(), _TimeZone).ToString("yyyy-MM-dd'T'HH:mm:sszzz"),
			["to_modified_date_time"] = TimeZoneInfo.ConvertTime(toModifiedTime.ToUniversalTime(), _TimeZone).ToString("yyyy-MM-dd'T'HH:mm:sszzz"),
			["account_id"] = accountId,
		};
		var response = JsonNode.Parse("""
		[
			{
				"sid": "190103969DD96J562D",
				"created_date_time": "2024-06-13T06:11:56.510124Z",
				"modified_date_time": "2024-06-13T06:12:34.70141Z",
				"arena_code": "ph",
				"match_day_code": "M000478",
				"fight_number": 74,
				"selection": "player",
				"odds": "1.68",
				"bet_amount": "10",
				"valid_stake": "9.85",
				"ticket_amount": "9.85",
				"ante_amount": "0.15",
				"status": 4,
				"result": "win",
				"member_ref_id": "4ucmqplxb029",
				"account_id": "99a6f8ef-38a6-4f31-b69a-e222c2a54771",
				"currency_code": "CNY",
				"win_loss_amount": "6.698",
				"odds_type": "eu",
				"settled_date_time": "2024-06-13T06:12:29.238152Z",
				"ip_address": "125.227.69.109",
				"user_agent": "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/125.0.0.0 Safari/537.36"
			}
		]
		""")!;
		var handler = new MockHttpMessageHandler();
		_ = handler
			.Expect(HttpMethod.Get, new Uri(apiBaseUrl, "/api/v1/tickets/").ToString())
			.WithHeaders("Authorization", privateKey)
			.WithExactQueryString(parameters)
			.Respond(HttpStatusCode.OK, JsonContent.Create(response));
		var fakeHttpClient = handler.ToHttpClient();
		var fakeLogger = NullLogger<GameApiClient>.Instance;
		var sut = new GameApiClient(fakeLogger, fakeHttpClient);

		var actual = await sut.TicketsAsync(
			apiBaseUrl,
			privateKey,
			page,
			pageSize,
			fromModifiedTime,
			toModifiedTime,
			accountId)
			.ToArrayAsync()
			.ConfigureAwait(true);

		Assert.Single(actual);
		var item = actual[0];
		Assert.Equal("190103969DD96J562D", item.Sid);
		Assert.Equal(DateTimeOffset.Parse("2024-06-13T06:11:56.510124Z"), item.CreatedDateTime);
		Assert.Equal(DateTimeOffset.Parse("2024-06-13T06:12:34.70141Z"), item.ModifiedDateTime);
		Assert.Equal(DateTimeOffset.Parse("2024-06-13T06:12:29.238152Z"), item.SettledDateTime);
		Assert.Equal("M000478", item.MatchDayCode);
		Assert.Equal(74, item.FightNumber);
		Assert.Equal(10m, item.BetAmount);
		Assert.Equal(9.85m, item.ValidStake);
		Assert.Equal(9.85m, item.TicketAmount);
		Assert.Equal(0.15m, item.AnteAmount);
		Assert.Equal(6.698m, item.WinLossAmount);
		Assert.Equal(ApiBetStatus.BetSettled, item.Status);
		Assert.Equal("win", item.Result);
		Assert.Equal("4ucmqplxb029", item.MemberRefId);
		Assert.Equal(response.AsArray()[0]!.ToJsonString(), item.AdditionalData);
	}

	[Fact]
	public async Task Loss()
	{
		var apiBaseUrl = new Uri("http://partner-api.dev.ga28.cc");
		var privateKey = "8456c9de-d589-4e8a-876f-c9c8e48dda3e";
		var page = 1;
		var pageSize = 32;
		var fromModifiedTime = new DateTimeOffset(2024, 06, 13, 16, 00, 00, TimeSpan.FromHours(8));
		var toModifiedTime = new DateTimeOffset(2024, 06, 13, 16, 10, 00, TimeSpan.FromHours(8));
		var accountId = "99a6f8ef-38a6-4f31-b69a-e222c2a54771";
		var parameters = new Dictionary<string, string>
		{
			["page"] = page.ToString(),
			["page_size"] = pageSize.ToString(),
			["from_modified_date_time"] = TimeZoneInfo.ConvertTime(fromModifiedTime.ToUniversalTime(), _TimeZone).ToString("yyyy-MM-dd'T'HH:mm:sszzz"),
			["to_modified_date_time"] = TimeZoneInfo.ConvertTime(toModifiedTime.ToUniversalTime(), _TimeZone).ToString("yyyy-MM-dd'T'HH:mm:sszzz"),
			["account_id"] = accountId,
		};
		var response = JsonNode.Parse("""
		[
			{
				"sid": "19010170AE3VA8I515",
				"created_date_time": "2024-06-13T05:34:23.971488Z",
				"modified_date_time": "2024-06-13T05:37:24.263361Z",
				"arena_code": "ph",
				"match_day_code": "M000478",
				"fight_number": 67,
				"selection": "banker",
				"odds": "2",
				"bet_amount": "1",
				"valid_stake": "0.985",
				"ticket_amount": "0.985",
				"ante_amount": "0.015",
				"status": 4,
				"result": "loss",
				"member_ref_id": "4ucmqplxb029",
				"account_id": "99a6f8ef-38a6-4f31-b69a-e222c2a54771",
				"currency_code": "CNY",
				"win_loss_amount": "-0.985",
				"odds_type": "eu",
				"settled_date_time": "2024-06-13T05:37:15.305415Z",
				"ip_address": "125.227.69.109",
				"user_agent": "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/125.0.0.0 Safari/537.36"
			}
		]
		""")!;
		var handler = new MockHttpMessageHandler();
		_ = handler
			.Expect(HttpMethod.Get, new Uri(apiBaseUrl, "/api/v1/tickets/").ToString())
			.WithHeaders("Authorization", privateKey)
			.WithExactQueryString(parameters)
			.Respond(HttpStatusCode.OK, JsonContent.Create(response));
		var fakeHttpClient = handler.ToHttpClient();
		var fakeLogger = NullLogger<GameApiClient>.Instance;
		var sut = new GameApiClient(fakeLogger, fakeHttpClient);

		var actual = await sut.TicketsAsync(
			apiBaseUrl,
			privateKey,
			page,
			pageSize,
			fromModifiedTime,
			toModifiedTime,
			accountId)
			.ToArrayAsync()
			.ConfigureAwait(true);

		Assert.Single(actual);
		var item = actual[0];
		Assert.Equal("19010170AE3VA8I515", item.Sid);
		Assert.Equal(DateTimeOffset.Parse("2024-06-13T05:34:23.971488Z"), item.CreatedDateTime);
		Assert.Equal(DateTimeOffset.Parse("2024-06-13T05:37:24.263361Z"), item.ModifiedDateTime);
		Assert.Equal(DateTimeOffset.Parse("2024-06-13T05:37:15.305415Z"), item.SettledDateTime);
		Assert.Equal("M000478", item.MatchDayCode);
		Assert.Equal(67, item.FightNumber);
		Assert.Equal(1m, item.BetAmount);
		Assert.Equal(0.985m, item.ValidStake);
		Assert.Equal(0.985m, item.TicketAmount);
		Assert.Equal(0.015m, item.AnteAmount);
		Assert.Equal(-0.985m, item.WinLossAmount);
		Assert.Equal(ApiBetStatus.BetSettled, item.Status);
		Assert.Equal("loss", item.Result);
		Assert.Equal("4ucmqplxb029", item.MemberRefId);
		Assert.Equal(response.AsArray()[0]!.ToJsonString(), item.AdditionalData);
	}

	[Fact]
	public async Task Unsettlement()
	{
		var apiBaseUrl = new Uri("http://partner-api.dev.ga28.cc");
		var privateKey = "8456c9de-d589-4e8a-876f-c9c8e48dda3e";
		var page = 1;
		var pageSize = 32;
		var fromModifiedTime = new DateTimeOffset(2024, 06, 13, 16, 00, 00, TimeSpan.FromHours(8));
		var toModifiedTime = new DateTimeOffset(2024, 06, 13, 16, 10, 00, TimeSpan.FromHours(8));
		var accountId = "99a6f8ef-38a6-4f31-b69a-e222c2a54771";
		var parameters = new Dictionary<string, string>
		{
			["page"] = page.ToString(),
			["page_size"] = pageSize.ToString(),
			["from_modified_date_time"] = TimeZoneInfo.ConvertTime(fromModifiedTime.ToUniversalTime(), _TimeZone).ToString("yyyy-MM-dd'T'HH:mm:sszzz"),
			["to_modified_date_time"] = TimeZoneInfo.ConvertTime(toModifiedTime.ToUniversalTime(), _TimeZone).ToString("yyyy-MM-dd'T'HH:mm:sszzz"),
			["account_id"] = accountId,
		};
		var response = JsonNode.Parse("""
		[
			{
				"sid": "19010170AE3VA8I515",
				"created_date_time": "2024-06-13T05:34:23.971488Z",
				"modified_date_time": "2024-06-13T05:34:23.971488Z",
				"arena_code": "ph",
				"match_day_code": "M000478",
				"fight_number": 67,
				"selection": "banker",
				"odds": "0",
				"bet_amount": "1",
				"valid_stake": null,
				"ticket_amount": "0.985",
				"ante_amount": "0.015",
				"status": 0,
				"result": "",
				"member_ref_id": "4ucmqplxb029",
				"account_id": "99a6f8ef-38a6-4f31-b69a-e222c2a54771",
				"currency_code": "CNY",
				"win_loss_amount": "0",
				"odds_type": "eu",
				"settled_date_time": null,
				"ip_address": "125.227.69.109",
				"user_agent": "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/125.0.0.0 Safari/537.36"
			}
		]
		""")!;
		var handler = new MockHttpMessageHandler();
		_ = handler
			.Expect(HttpMethod.Get, new Uri(apiBaseUrl, "/api/v1/tickets/").ToString())
			.WithHeaders("Authorization", privateKey)
			.WithExactQueryString(parameters)
			.Respond(HttpStatusCode.OK, JsonContent.Create(response));
		var fakeHttpClient = handler.ToHttpClient();
		var fakeLogger = NullLogger<GameApiClient>.Instance;
		var sut = new GameApiClient(fakeLogger, fakeHttpClient);

		var actual = await sut.TicketsAsync(
			apiBaseUrl,
			privateKey,
			page,
			pageSize,
			fromModifiedTime,
			toModifiedTime,
			accountId)
			.ToArrayAsync()
			.ConfigureAwait(true);

		Assert.Single(actual);
		var item = actual[0];
		Assert.Equal("19010170AE3VA8I515", item.Sid);
		Assert.Equal(DateTimeOffset.Parse("2024-06-13T05:34:23.971488Z"), item.CreatedDateTime);
		Assert.Equal(DateTimeOffset.Parse("2024-06-13T05:34:23.971488Z"), item.ModifiedDateTime);
		Assert.Equal(DateTimeOffset.UnixEpoch, item.SettledDateTime);
		Assert.Equal("M000478", item.MatchDayCode);
		Assert.Equal(67, item.FightNumber);
		Assert.Equal(1m, item.BetAmount);
		Assert.Equal(0m, item.ValidStake);
		Assert.Equal(0.985m, item.TicketAmount);
		Assert.Equal(0.015m, item.AnteAmount);
		Assert.Equal(0m, item.WinLossAmount);
		Assert.Equal(ApiBetStatus.BetConfirmed, item.Status);
		Assert.Equal("", item.Result);
		Assert.Equal("4ucmqplxb029", item.MemberRefId);
		Assert.Equal(response.AsArray()[0]!.ToJsonString(), item.AdditionalData);
	}

	[Fact]
	public async Task Void()
	{
		var apiBaseUrl = new Uri("http://partner-api.dev.ga28.cc");
		var privateKey = "8456c9de-d589-4e8a-876f-c9c8e48dda3e";
		var page = 1;
		var pageSize = 32;
		var fromModifiedTime = new DateTimeOffset(2024, 06, 18, 10, 15, 00, TimeSpan.FromHours(8));
		var toModifiedTime = new DateTimeOffset(2024, 06, 18, 10, 25, 00, TimeSpan.FromHours(8));
		var accountId = "99a6f8ef-38a6-4f31-b69a-e222c2a54771";
		var parameters = new Dictionary<string, string>
		{
			["page"] = page.ToString(),
			["page_size"] = pageSize.ToString(),
			["from_modified_date_time"] = TimeZoneInfo.ConvertTime(fromModifiedTime.ToUniversalTime(), _TimeZone).ToString("yyyy-MM-dd'T'HH:mm:sszzz"),
			["to_modified_date_time"] = TimeZoneInfo.ConvertTime(toModifiedTime.ToUniversalTime(), _TimeZone).ToString("yyyy-MM-dd'T'HH:mm:sszzz"),
			["account_id"] = accountId,
		};
		var response = JsonNode.Parse("""
		[
			{
				"sid": "190291442B0CIF1309",
				"created_date_time": "2024-06-18T02:01:52.049269Z",
				"modified_date_time": "2024-06-18T02:19:08.847127Z",
				"arena_code": "ph",
				"match_day_code": "M000485",
				"fight_number": 25,
				"selection": "player",
				"odds": "1.98",
				"bet_amount": "9.85",
				"valid_stake": null,
				"ticket_amount": "9.85",
				"ante_amount": "0",
				"status": 9,
				"result": "void",
				"member_ref_id": "4ucmqplxb029",
				"account_id": "99a6f8ef-38a6-4f31-b69a-e222c2a54771",
				"currency_code": "CNY",
				"win_loss_amount": "0",
				"odds_type": "eu",
				"settled_date_time": null,
				"ip_address": "125.227.69.109",
				"user_agent": "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/125.0.0.0 Safari/537.36"
			}
		]
		""")!;
		var handler = new MockHttpMessageHandler();
		_ = handler
			.Expect(HttpMethod.Get, new Uri(apiBaseUrl, "/api/v1/tickets/").ToString())
			.WithHeaders("Authorization", privateKey)
			.WithExactQueryString(parameters)
			.Respond(HttpStatusCode.OK, JsonContent.Create(response));
		var fakeHttpClient = handler.ToHttpClient();
		var fakeLogger = NullLogger<GameApiClient>.Instance;
		var sut = new GameApiClient(fakeLogger, fakeHttpClient);

		var actual = await sut.TicketsAsync(
			apiBaseUrl,
			privateKey,
			page,
			pageSize,
			fromModifiedTime,
			toModifiedTime,
			accountId)
			.ToArrayAsync()
			.ConfigureAwait(true);

		Assert.Single(actual);
		var item = actual[0];
		Assert.Equal("190291442B0CIF1309", item.Sid);
		Assert.Equal(DateTimeOffset.Parse("2024-06-18T02:01:52.049269Z"), item.CreatedDateTime);
		Assert.Equal(DateTimeOffset.Parse("2024-06-18T02:19:08.847127Z"), item.ModifiedDateTime);
		Assert.Equal(DateTimeOffset.UnixEpoch, item.SettledDateTime);
		Assert.Equal("M000485", item.MatchDayCode);
		Assert.Equal(25, item.FightNumber);
		Assert.Equal(9.85m, item.BetAmount);
		Assert.Equal(0.00m, item.ValidStake);
		Assert.Equal(9.85m, item.TicketAmount);
		Assert.Equal(0.00m, item.AnteAmount);
		Assert.Equal(0.00m, item.WinLossAmount);
		Assert.Equal(ApiBetStatus.BetVoided, item.Status);
		Assert.Equal("void", item.Result);
		Assert.Equal("4ucmqplxb029", item.MemberRefId);
		Assert.Equal(response.AsArray()[0]!.ToJsonString(), item.AdditionalData);
	}

	[Fact]
	public async Task Cancelled()
	{
		var apiBaseUrl = new Uri("http://partner-api.dev.ga28.cc");
		var privateKey = "8456c9de-d589-4e8a-876f-c9c8e48dda3e";
		var page = 1;
		var pageSize = 32;
		var fromModifiedTime = new DateTimeOffset(2024, 06, 18, 10, 50, 00, TimeSpan.FromHours(8));
		var toModifiedTime = new DateTimeOffset(2024, 06, 18, 11, 00, 00, TimeSpan.FromHours(8));
		var accountId = "99a6f8ef-38a6-4f31-b69a-e222c2a54771";
		var parameters = new Dictionary<string, string>
		{
			["page"] = page.ToString(),
			["page_size"] = pageSize.ToString(),
			["from_modified_date_time"] = TimeZoneInfo.ConvertTime(fromModifiedTime.ToUniversalTime(), _TimeZone).ToString("yyyy-MM-dd'T'HH:mm:sszzz"),
			["to_modified_date_time"] = TimeZoneInfo.ConvertTime(toModifiedTime.ToUniversalTime(), _TimeZone).ToString("yyyy-MM-dd'T'HH:mm:sszzz"),
			["account_id"] = accountId,
		};
		var response = JsonNode.Parse("""
		[
			{
				"sid": "190290D99C1D4UQ2E3",
				"created_date_time": "2024-06-18T01:54:35.585921Z",
				"modified_date_time": "2024-06-18T02:55:37.310049Z",
				"arena_code": "ph",
				"match_day_code": "M000485",
				"fight_number": 23,
				"selection": "banker",
				"odds": "2.21",
				"bet_amount": "10",
				"valid_stake": null,
				"ticket_amount": "9.85",
				"ante_amount": "0.15",
				"status": 7,
				"result": "cancelled",
				"member_ref_id": "4ucmqplxb029",
				"account_id": "99a6f8ef-38a6-4f31-b69a-e222c2a54771",
				"currency_code": "CNY",
				"win_loss_amount": "0",
				"odds_type": "eu",
				"settled_date_time": null,
				"ip_address": "125.227.69.109",
				"user_agent": "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/125.0.0.0 Safari/537.36"
			}
		]
		""")!;
		var handler = new MockHttpMessageHandler();
		_ = handler
			.Expect(HttpMethod.Get, new Uri(apiBaseUrl, "/api/v1/tickets/").ToString())
			.WithHeaders("Authorization", privateKey)
			.WithExactQueryString(parameters)
			.Respond(HttpStatusCode.OK, JsonContent.Create(response));
		var fakeHttpClient = handler.ToHttpClient();
		var fakeLogger = NullLogger<GameApiClient>.Instance;
		var sut = new GameApiClient(fakeLogger, fakeHttpClient);

		var actual = await sut.TicketsAsync(
			apiBaseUrl,
			privateKey,
			page,
			pageSize,
			fromModifiedTime,
			toModifiedTime,
			accountId)
			.ToArrayAsync()
			.ConfigureAwait(true);

		Assert.Single(actual);
		var item = actual[0];
		Assert.Equal("190290D99C1D4UQ2E3", item.Sid);
		Assert.Equal(DateTimeOffset.Parse("2024-06-18T01:54:35.585921Z"), item.CreatedDateTime);
		Assert.Equal(DateTimeOffset.Parse("2024-06-18T02:55:37.310049Z"), item.ModifiedDateTime);
		Assert.Equal(DateTimeOffset.UnixEpoch, item.SettledDateTime);
		Assert.Equal("M000485", item.MatchDayCode);
		Assert.Equal(23, item.FightNumber);
		Assert.Equal(10m, item.BetAmount);
		Assert.Equal(0.00m, item.ValidStake);
		Assert.Equal(9.85m, item.TicketAmount);
		Assert.Equal(0.15m, item.AnteAmount);
		Assert.Equal(0.00m, item.WinLossAmount);
		Assert.Equal(ApiBetStatus.BetCancelled, item.Status);
		Assert.Equal("cancelled", item.Result);
		Assert.Equal("4ucmqplxb029", item.MemberRefId);
		Assert.Equal(response.AsArray()[0]!.ToJsonString(), item.AdditionalData);
	}
}
