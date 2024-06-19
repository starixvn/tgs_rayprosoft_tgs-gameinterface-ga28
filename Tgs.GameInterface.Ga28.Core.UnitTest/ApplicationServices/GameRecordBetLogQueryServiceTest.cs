using System.Text.Json.Nodes;
using NSubstitute;
using Tgs.GameInterface.Ga28.Core.ApplicationServices;
using Tgs.GameInterface.Ga28.Core.Models;

namespace Tgs.GameInterface.Ga28.Core.UnitTest.ApplicationServices;

public sealed class GameRecordBetLogQueryServiceTest
{
	[Fact]
	public async Task Settled_Win()
	{
		var query = new GameRecordBetLogQuery(
			ApiUrl: new("http://partner-api.dev.ga28.cc"),
			PrivateKey: "8456c9de-d589-4e8a-876f-c9c8e48dda3e",
			PartnerAccountId: "99a6f8ef-38a6-4f31-b69a-e222c2a54771",
			StartTime: new DateTimeOffset(2024, 06, 14, 10, 16, 00, TimeSpan.FromHours(8)),
			EndTime: new DateTimeOffset(2024, 06, 14, 12, 16, 00, TimeSpan.FromHours(8)),
			Page: 1,
			PageSize: 32);
		var json = JsonNode.Parse("""
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
		var response = new TicketResponseItem(
			Sid: "190103969DD96J562D",
			CreatedDateTime: DateTimeOffset.Parse("2024-06-13T06:11:56.510124Z"),
			ModifiedDateTime: DateTimeOffset.Parse("2024-06-13T06:12:34.70141Z"),
			SettledDateTime: DateTimeOffset.Parse("2024-06-13T06:12:29.238152Z"),
			ArenaCode: "ph",
			MatchDayCode: "M000478",
			FightNumber: 74,
			BetAmount: 10m,
			ValidStake: 9.85m,
			TicketAmount: 9.85m,
			AnteAmount: 0.15m,
			WinLossAmount: 6.698m,
			Status: ApiBetStatus.BetSettled,
			Result: "win",
			MemberRefId: "4ucmqplxb029",
			AdditionalData: json.ToJsonString());
		var fakeGameApiClient = Substitute.For<IGameApiClient>();
		_ = fakeGameApiClient
			.TicketsAsync(
				apiBaseUrl: Arg.Is(query.ApiUrl),
				privateKey: Arg.Is(query.PrivateKey),
				partnerAccountId: Arg.Is(query.PartnerAccountId),
				page: Arg.Is(query.Page),
				pageSize: Arg.Is(query.PageSize),
				fromModifiedTime: Arg.Is(query.StartTime),
				toModifiedTime: Arg.Is(query.EndTime))
			.Returns(new[] { response }.ToAsyncEnumerable());
		var sut = new GameRecordBetLogQueryService(fakeGameApiClient);

		var actual = await sut.QueryAsync(query).ToArrayAsync().ConfigureAwait(true);

		Assert.Single(actual);
		var item = actual[0];
		Assert.Equal("4ucmqplxb029", item.PlayerCode);
		Assert.Equal("190103969DD96J562D", item.ExternalBetId);
		Assert.Equal("ph#74 M000478", item.ExternalRoundId);
		Assert.Equal(Protos.GameRecordResult.Win, item.Result);
		Assert.Equal(new DateTimeOffset(2024, 06, 13, 14, 11, 56, 510, 124, TimeSpan.FromHours(8)), item.ExternalCreatedOn);
		Assert.Equal(new DateTimeOffset(2024, 06, 13, 14, 12, 29, 238, 152, TimeSpan.FromHours(8)), item.ExternalClosedOn);
		Assert.Equal(10m, item.Bet);
		Assert.Equal(9.85m, item.ValidBet);
		Assert.Equal(16.548m, item.Settle);
		Assert.Equal(6.698m, item.WinLoss);
		Assert.Equal(json.ToJsonString(), item.AdditionalData);
	}

	[Fact]
	public async Task Settled_Loss()
	{
		var query = new GameRecordBetLogQuery(
			ApiUrl: new("http://partner-api.dev.ga28.cc"),
			PrivateKey: "8456c9de-d589-4e8a-876f-c9c8e48dda3e",
			PartnerAccountId: "99a6f8ef-38a6-4f31-b69a-e222c2a54771",
			StartTime: new DateTimeOffset(2024, 06, 14, 10, 16, 00, TimeSpan.FromHours(8)),
			EndTime: new DateTimeOffset(2024, 06, 14, 12, 16, 00, TimeSpan.FromHours(8)),
			Page: 1,
			PageSize: 32);
		var json = JsonNode.Parse("""
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
		var response = new TicketResponseItem(
			Sid: "19010170AE3VA8I515",
			CreatedDateTime: DateTimeOffset.Parse("2024-06-13T05:34:23.971488Z"),
			ModifiedDateTime: DateTimeOffset.Parse("2024-06-13T05:37:24.263361Z"),
			SettledDateTime: DateTimeOffset.Parse("2024-06-13T05:37:15.305415Z"),
			ArenaCode: "ph",
			MatchDayCode: "M000478",
			FightNumber: 67,
			BetAmount: 1m,
			ValidStake: 0.985m,
			TicketAmount: 0.985m,
			AnteAmount: 0.015m,
			WinLossAmount: -0.985m,
			Status: ApiBetStatus.BetSettled,
			Result: "loss",
			MemberRefId: "4ucmqplxb029",
			AdditionalData: json.ToJsonString());
		var fakeGameApiClient = Substitute.For<IGameApiClient>();
		_ = fakeGameApiClient
			.TicketsAsync(
				apiBaseUrl: Arg.Is(query.ApiUrl),
				privateKey: Arg.Is(query.PrivateKey),
				partnerAccountId: Arg.Is(query.PartnerAccountId),
				page: Arg.Is(query.Page),
				pageSize: Arg.Is(query.PageSize),
				fromModifiedTime: Arg.Is(query.StartTime),
				toModifiedTime: Arg.Is(query.EndTime))
			.Returns(new[] { response }.ToAsyncEnumerable());
		var sut = new GameRecordBetLogQueryService(fakeGameApiClient);

		var actual = await sut.QueryAsync(query).ToArrayAsync().ConfigureAwait(true);

		Assert.Single(actual);
		var item = actual[0];
		Assert.Equal("4ucmqplxb029", item.PlayerCode);
		Assert.Equal("19010170AE3VA8I515", item.ExternalBetId);
		Assert.Equal("ph#67 M000478", item.ExternalRoundId);
		Assert.Equal(Protos.GameRecordResult.Loss, item.Result);
		Assert.Equal(new DateTimeOffset(2024, 06, 13, 13, 34, 23, 971, 488, TimeSpan.FromHours(8)), item.ExternalCreatedOn);
		Assert.Equal(new DateTimeOffset(2024, 06, 13, 13, 37, 15, 305, 415, TimeSpan.FromHours(8)), item.ExternalClosedOn);
		Assert.Equal(1m, item.Bet);
		Assert.Equal(0.985m, item.ValidBet);
		Assert.Equal(0m, item.Settle);
		Assert.Equal(-0.985m, item.WinLoss);
		Assert.Equal(json.ToJsonString(), item.AdditionalData);
	}

	[Fact]
	public async Task Settled_Void()
	{
		var query = new GameRecordBetLogQuery(
			ApiUrl: new("http://partner-api.dev.ga28.cc"),
			PrivateKey: "8456c9de-d589-4e8a-876f-c9c8e48dda3e",
			PartnerAccountId: "99a6f8ef-38a6-4f31-b69a-e222c2a54771",
			StartTime: new DateTimeOffset(2024, 06, 18, 10, 15, 00, TimeSpan.FromHours(8)),
			EndTime: new DateTimeOffset(2024, 06, 18, 10, 25, 00, TimeSpan.FromHours(8)),
			Page: 1,
			PageSize: 32);
		var json = JsonNode.Parse("""
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
		var response = new TicketResponseItem(
			Sid: "190291442B0CIF1309",
			CreatedDateTime: DateTimeOffset.Parse("2024-06-18T02:01:52.049269Z"),
			ModifiedDateTime: DateTimeOffset.Parse("2024-06-18T02:19:08.847127Z"),
			SettledDateTime: DateTimeOffset.UnixEpoch,
			ArenaCode: "ph",
			MatchDayCode: "M000485",
			FightNumber: 25,
			BetAmount: 9.85m,
			ValidStake: 0.00m,
			TicketAmount: 9.85m,
			AnteAmount: 0.00m,
			WinLossAmount: -0.00m,
			Status: ApiBetStatus.BetVoided,
			Result: "void",
			MemberRefId: "4ucmqplxb029",
			AdditionalData: json.ToJsonString());
		var fakeGameApiClient = Substitute.For<IGameApiClient>();
		_ = fakeGameApiClient
			.TicketsAsync(
				apiBaseUrl: Arg.Is(query.ApiUrl),
				privateKey: Arg.Is(query.PrivateKey),
				partnerAccountId: Arg.Is(query.PartnerAccountId),
				page: Arg.Is(query.Page),
				pageSize: Arg.Is(query.PageSize),
				fromModifiedTime: Arg.Is(query.StartTime),
				toModifiedTime: Arg.Is(query.EndTime))
			.Returns(new[] { response }.ToAsyncEnumerable());
		var sut = new GameRecordBetLogQueryService(fakeGameApiClient);

		var actual = await sut.QueryAsync(query).ToArrayAsync().ConfigureAwait(true);

		Assert.Single(actual);
		var item = actual[0];
		Assert.Equal("4ucmqplxb029", item.PlayerCode);
		Assert.Equal("190291442B0CIF1309", item.ExternalBetId);
		Assert.Equal("ph#25 M000485", item.ExternalRoundId);
		Assert.Equal(Protos.GameRecordResult.Void, item.Result);
		Assert.Equal(DateTimeOffset.Parse("2024-06-18T02:01:52.049269Z"), item.ExternalCreatedOn);
		Assert.Equal(DateTimeOffset.Parse("2024-06-18T02:19:08.847127Z"), item.ExternalClosedOn);
		Assert.Equal(9.85m, item.Bet);
		Assert.Equal(0.00m, item.ValidBet);
		Assert.Equal(0.00m, item.Settle);
		Assert.Equal(0.00m, item.WinLoss);
		Assert.Equal(json.ToJsonString(), item.AdditionalData);
	}

	[Fact]
	public async Task Settled_Cancelled()
	{
		var query = new GameRecordBetLogQuery(
			ApiUrl: new("http://partner-api.dev.ga28.cc"),
			PrivateKey: "8456c9de-d589-4e8a-876f-c9c8e48dda3e",
			PartnerAccountId: "99a6f8ef-38a6-4f31-b69a-e222c2a54771",
			StartTime: new DateTimeOffset(2024, 06, 18, 10, 15, 00, TimeSpan.FromHours(8)),
			EndTime: new DateTimeOffset(2024, 06, 18, 10, 25, 00, TimeSpan.FromHours(8)),
			Page: 1,
			PageSize: 32);
		var json = JsonNode.Parse("""
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
		var response = new TicketResponseItem(
			Sid: "190290D99C1D4UQ2E3",
			CreatedDateTime: DateTimeOffset.Parse("2024-06-18T01:54:35.585921Z"),
			ModifiedDateTime: DateTimeOffset.Parse("2024-06-18T02:55:37.310049Z"),
			SettledDateTime: DateTimeOffset.UnixEpoch,
			ArenaCode: "ph",
			MatchDayCode: "M000485",
			FightNumber: 23,
			BetAmount: 10.00m,
			ValidStake: 0.00m,
			TicketAmount: 9.85m,
			AnteAmount: 0.15m,
			WinLossAmount: 0.00m,
			Status: ApiBetStatus.BetCancelled,
			Result: "cancelled",
			MemberRefId: "4ucmqplxb029",
			AdditionalData: json.ToJsonString());
		var fakeGameApiClient = Substitute.For<IGameApiClient>();
		_ = fakeGameApiClient
			.TicketsAsync(
				apiBaseUrl: Arg.Is(query.ApiUrl),
				privateKey: Arg.Is(query.PrivateKey),
				partnerAccountId: Arg.Is(query.PartnerAccountId),
				page: Arg.Is(query.Page),
				pageSize: Arg.Is(query.PageSize),
				fromModifiedTime: Arg.Is(query.StartTime),
				toModifiedTime: Arg.Is(query.EndTime))
			.Returns(new[] { response }.ToAsyncEnumerable());
		var sut = new GameRecordBetLogQueryService(fakeGameApiClient);

		var actual = await sut.QueryAsync(query).ToArrayAsync().ConfigureAwait(true);

		Assert.Single(actual);
		var item = actual[0];
		Assert.Equal("4ucmqplxb029", item.PlayerCode);
		Assert.Equal("190290D99C1D4UQ2E3", item.ExternalBetId);
		Assert.Equal("ph#23 M000485", item.ExternalRoundId);
		Assert.Equal(Protos.GameRecordResult.Cancelled, item.Result);
		Assert.Equal(DateTimeOffset.Parse("2024-06-18T01:54:35.585921Z"), item.ExternalCreatedOn);
		Assert.Equal(DateTimeOffset.Parse("2024-06-18T02:55:37.310049Z"), item.ExternalClosedOn);
		Assert.Equal(10.00m, item.Bet);
		Assert.Equal(0.00m, item.ValidBet);
		Assert.Equal(0.00m, item.Settle);
		Assert.Equal(0.00m, item.WinLoss);
		Assert.Equal(json.ToJsonString(), item.AdditionalData);
	}

	[Fact]
	public async Task StatusOpened_WhenApiBetConfirmed()
	{
		var query = new GameRecordBetLogQuery(
			ApiUrl: new("http://partner-api.dev.ga28.cc"),
			PrivateKey: "8456c9de-d589-4e8a-876f-c9c8e48dda3e",
			PartnerAccountId: "99a6f8ef-38a6-4f31-b69a-e222c2a54771",
			StartTime: new DateTimeOffset(2024, 06, 14, 10, 16, 00, TimeSpan.FromHours(8)),
			EndTime: new DateTimeOffset(2024, 06, 14, 12, 16, 00, TimeSpan.FromHours(8)),
			Page: 1,
			PageSize: 32);
		var json = JsonNode.Parse("""
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
		var response = new TicketResponseItem(
			Sid: "19010170AE3VA8I515",
			CreatedDateTime: DateTimeOffset.Parse("2024-06-13T05:34:23.971488Z"),
			ModifiedDateTime: DateTimeOffset.Parse("2024-06-13T05:34:23.971488Z"),
			SettledDateTime: DateTimeOffset.UnixEpoch,
			ArenaCode: "ph",
			MatchDayCode: "M000478",
			FightNumber: 67,
			BetAmount: 1m,
			ValidStake: 0m,
			TicketAmount: 0.985m,
			AnteAmount: 0.015m,
			WinLossAmount: 0m,
			Status: ApiBetStatus.BetConfirmed,
			Result: "",
			MemberRefId: "4ucmqplxb029",
			AdditionalData: json.ToJsonString());
		var fakeGameApiClient = Substitute.For<IGameApiClient>();
		_ = fakeGameApiClient
			.TicketsAsync(
				apiBaseUrl: Arg.Is(query.ApiUrl),
				privateKey: Arg.Is(query.PrivateKey),
				partnerAccountId: Arg.Is(query.PartnerAccountId),
				page: Arg.Is(query.Page),
				pageSize: Arg.Is(query.PageSize),
				fromModifiedTime: Arg.Is(query.StartTime),
				toModifiedTime: Arg.Is(query.EndTime))
			.Returns(new[] { response }.ToAsyncEnumerable());
		var sut = new GameRecordBetLogQueryService(fakeGameApiClient);

		var actual = await sut.QueryAsync(query).ToArrayAsync().ConfigureAwait(true);

		Assert.Single(actual);
		var item = actual[0];
		Assert.Equal("4ucmqplxb029", item.PlayerCode);
		Assert.Equal("19010170AE3VA8I515", item.ExternalBetId);
		Assert.Equal("ph#67 M000478", item.ExternalRoundId);
		Assert.Equal(Protos.GameRecordResult.None, item.Result);
		Assert.Equal(new DateTimeOffset(2024, 06, 13, 13, 34, 23, 971, 488, TimeSpan.FromHours(8)), item.ExternalCreatedOn);
		Assert.Equal(DateTimeOffset.UnixEpoch, item.ExternalClosedOn);
		Assert.Equal(1m, item.Bet);
		Assert.Equal(0m, item.ValidBet);
		Assert.Equal(0m, item.Settle);
		Assert.Equal(0m, item.WinLoss);
		Assert.Equal(json.ToJsonString(), item.AdditionalData);
	}

	[Fact]
	public async Task StatusOpened_WhenApiSettling()
	{
		var query = new GameRecordBetLogQuery(
			ApiUrl: new("http://partner-api.dev.ga28.cc"),
			PrivateKey: "8456c9de-d589-4e8a-876f-c9c8e48dda3e",
			PartnerAccountId: "99a6f8ef-38a6-4f31-b69a-e222c2a54771",
			StartTime: new DateTimeOffset(2024, 06, 14, 10, 16, 00, TimeSpan.FromHours(8)),
			EndTime: new DateTimeOffset(2024, 06, 14, 12, 16, 00, TimeSpan.FromHours(8)),
			Page: 1,
			PageSize: 32);
		var json = JsonNode.Parse("""
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
				"status": 1,
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
		var response = new TicketResponseItem(
			Sid: "19010170AE3VA8I515",
			CreatedDateTime: DateTimeOffset.Parse("2024-06-13T05:34:23.971488Z"),
			ModifiedDateTime: DateTimeOffset.Parse("2024-06-13T05:34:23.971488Z"),
			SettledDateTime: DateTimeOffset.UnixEpoch,
			ArenaCode: "ph",
			MatchDayCode: "M000478",
			FightNumber: 67,
			BetAmount: 1m,
			ValidStake: 0m,
			TicketAmount: 0.985m,
			AnteAmount: 0.015m,
			WinLossAmount: 0m,
			Status: ApiBetStatus.Settling,
			Result: "",
			MemberRefId: "4ucmqplxb029",
			AdditionalData: json.ToJsonString());
		var fakeGameApiClient = Substitute.For<IGameApiClient>();
		_ = fakeGameApiClient
			.TicketsAsync(
				apiBaseUrl: Arg.Is(query.ApiUrl),
				privateKey: Arg.Is(query.PrivateKey),
				partnerAccountId: Arg.Is(query.PartnerAccountId),
				page: Arg.Is(query.Page),
				pageSize: Arg.Is(query.PageSize),
				fromModifiedTime: Arg.Is(query.StartTime),
				toModifiedTime: Arg.Is(query.EndTime))
			.Returns(new[] { response }.ToAsyncEnumerable());
		var sut = new GameRecordBetLogQueryService(fakeGameApiClient);

		var actual = await sut.QueryAsync(query).ToArrayAsync().ConfigureAwait(true);

		Assert.Single(actual);
		var item = actual[0];
		Assert.Equal(Protos.GameRecordResult.None, item.Result);
	}

	[Fact]
	public async Task StatusOpened_WhenApiSettledAwaitingPayout()
	{
		var query = new GameRecordBetLogQuery(
			ApiUrl: new("http://partner-api.dev.ga28.cc"),
			PrivateKey: "8456c9de-d589-4e8a-876f-c9c8e48dda3e",
			PartnerAccountId: "99a6f8ef-38a6-4f31-b69a-e222c2a54771",
			StartTime: new DateTimeOffset(2024, 06, 14, 10, 16, 00, TimeSpan.FromHours(8)),
			EndTime: new DateTimeOffset(2024, 06, 14, 12, 16, 00, TimeSpan.FromHours(8)),
			Page: 1,
			PageSize: 32);
		var json = JsonNode.Parse("""
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
				"status": 2,
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
		var response = new TicketResponseItem(
			Sid: "19010170AE3VA8I515",
			CreatedDateTime: DateTimeOffset.Parse("2024-06-13T05:34:23.971488Z"),
			ModifiedDateTime: DateTimeOffset.Parse("2024-06-13T05:34:23.971488Z"),
			SettledDateTime: DateTimeOffset.UnixEpoch,
			ArenaCode: "ph",
			MatchDayCode: "M000478",
			FightNumber: 67,
			BetAmount: 1m,
			ValidStake: 0m,
			TicketAmount: 0.985m,
			AnteAmount: 0.015m,
			WinLossAmount: 0m,
			Status: ApiBetStatus.SettledAwaitingPayout,
			Result: "",
			MemberRefId: "4ucmqplxb029",
			AdditionalData: json.ToJsonString());
		var fakeGameApiClient = Substitute.For<IGameApiClient>();
		_ = fakeGameApiClient
			.TicketsAsync(
				apiBaseUrl: Arg.Is(query.ApiUrl),
				privateKey: Arg.Is(query.PrivateKey),
				partnerAccountId: Arg.Is(query.PartnerAccountId),
				page: Arg.Is(query.Page),
				pageSize: Arg.Is(query.PageSize),
				fromModifiedTime: Arg.Is(query.StartTime),
				toModifiedTime: Arg.Is(query.EndTime))
			.Returns(new[] { response }.ToAsyncEnumerable());
		var sut = new GameRecordBetLogQueryService(fakeGameApiClient);

		var actual = await sut.QueryAsync(query).ToArrayAsync().ConfigureAwait(true);

		Assert.Single(actual);
		var item = actual[0];
		Assert.Equal(Protos.GameRecordResult.None, item.Result);
	}

	[Fact]
	public async Task StatusOpened_WhenApiSettledPayoutInProgress()
	{
		var query = new GameRecordBetLogQuery(
			ApiUrl: new("http://partner-api.dev.ga28.cc"),
			PrivateKey: "8456c9de-d589-4e8a-876f-c9c8e48dda3e",
			PartnerAccountId: "99a6f8ef-38a6-4f31-b69a-e222c2a54771",
			StartTime: new DateTimeOffset(2024, 06, 14, 10, 16, 00, TimeSpan.FromHours(8)),
			EndTime: new DateTimeOffset(2024, 06, 14, 12, 16, 00, TimeSpan.FromHours(8)),
			Page: 1,
			PageSize: 32);
		var json = JsonNode.Parse("""
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
				"status": 3,
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
		var response = new TicketResponseItem(
			Sid: "19010170AE3VA8I515",
			CreatedDateTime: DateTimeOffset.Parse("2024-06-13T05:34:23.971488Z"),
			ModifiedDateTime: DateTimeOffset.Parse("2024-06-13T05:34:23.971488Z"),
			SettledDateTime: DateTimeOffset.UnixEpoch,
			ArenaCode: "ph",
			MatchDayCode: "M000478",
			FightNumber: 67,
			BetAmount: 1m,
			ValidStake: 0m,
			TicketAmount: 0.985m,
			AnteAmount: 0.015m,
			WinLossAmount: 0m,
			Status: ApiBetStatus.SettledPayoutInProgress,
			Result: "",
			MemberRefId: "4ucmqplxb029",
			AdditionalData: json.ToJsonString());
		var fakeGameApiClient = Substitute.For<IGameApiClient>();
		_ = fakeGameApiClient
			.TicketsAsync(
				apiBaseUrl: Arg.Is(query.ApiUrl),
				privateKey: Arg.Is(query.PrivateKey),
				partnerAccountId: Arg.Is(query.PartnerAccountId),
				page: Arg.Is(query.Page),
				pageSize: Arg.Is(query.PageSize),
				fromModifiedTime: Arg.Is(query.StartTime),
				toModifiedTime: Arg.Is(query.EndTime))
			.Returns(new[] { response }.ToAsyncEnumerable());
		var sut = new GameRecordBetLogQueryService(fakeGameApiClient);

		var actual = await sut.QueryAsync(query).ToArrayAsync().ConfigureAwait(true);

		Assert.Single(actual);
		var item = actual[0];
		Assert.Equal(Protos.GameRecordResult.None, item.Result);
	}

	[Fact]
	public async Task StatusOpened_WhenApiCancelledAwaitingPayout()
	{
		var query = new GameRecordBetLogQuery(
			ApiUrl: new("http://partner-api.dev.ga28.cc"),
			PrivateKey: "8456c9de-d589-4e8a-876f-c9c8e48dda3e",
			PartnerAccountId: "99a6f8ef-38a6-4f31-b69a-e222c2a54771",
			StartTime: new DateTimeOffset(2024, 06, 14, 10, 16, 00, TimeSpan.FromHours(8)),
			EndTime: new DateTimeOffset(2024, 06, 14, 12, 16, 00, TimeSpan.FromHours(8)),
			Page: 1,
			PageSize: 32);
		var json = JsonNode.Parse("""
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
				"status": 5,
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
		var response = new TicketResponseItem(
			Sid: "19010170AE3VA8I515",
			CreatedDateTime: DateTimeOffset.Parse("2024-06-13T05:34:23.971488Z"),
			ModifiedDateTime: DateTimeOffset.Parse("2024-06-13T05:34:23.971488Z"),
			SettledDateTime: DateTimeOffset.UnixEpoch,
			ArenaCode: "ph",
			MatchDayCode: "M000478",
			FightNumber: 67,
			BetAmount: 1m,
			ValidStake: 0m,
			TicketAmount: 0.985m,
			AnteAmount: 0.015m,
			WinLossAmount: 0m,
			Status: ApiBetStatus.CancelledAwaitingPayout,
			Result: "",
			MemberRefId: "4ucmqplxb029",
			AdditionalData: json.ToJsonString());
		var fakeGameApiClient = Substitute.For<IGameApiClient>();
		_ = fakeGameApiClient
			.TicketsAsync(
				apiBaseUrl: Arg.Is(query.ApiUrl),
				privateKey: Arg.Is(query.PrivateKey),
				partnerAccountId: Arg.Is(query.PartnerAccountId),
				page: Arg.Is(query.Page),
				pageSize: Arg.Is(query.PageSize),
				fromModifiedTime: Arg.Is(query.StartTime),
				toModifiedTime: Arg.Is(query.EndTime))
			.Returns(new[] { response }.ToAsyncEnumerable());
		var sut = new GameRecordBetLogQueryService(fakeGameApiClient);

		var actual = await sut.QueryAsync(query).ToArrayAsync().ConfigureAwait(true);

		Assert.Single(actual);
		var item = actual[0];
		Assert.Equal(Protos.GameRecordResult.None, item.Result);
	}

	[Fact]
	public async Task StatusOpened_WhenApiCancelledPayoutInProgress()
	{
		var query = new GameRecordBetLogQuery(
			ApiUrl: new("http://partner-api.dev.ga28.cc"),
			PrivateKey: "8456c9de-d589-4e8a-876f-c9c8e48dda3e",
			PartnerAccountId: "99a6f8ef-38a6-4f31-b69a-e222c2a54771",
			StartTime: new DateTimeOffset(2024, 06, 14, 10, 16, 00, TimeSpan.FromHours(8)),
			EndTime: new DateTimeOffset(2024, 06, 14, 12, 16, 00, TimeSpan.FromHours(8)),
			Page: 1,
			PageSize: 32);
		var json = JsonNode.Parse("""
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
				"status": 6,
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
		var response = new TicketResponseItem(
			Sid: "19010170AE3VA8I515",
			CreatedDateTime: DateTimeOffset.Parse("2024-06-13T05:34:23.971488Z"),
			ModifiedDateTime: DateTimeOffset.Parse("2024-06-13T05:34:23.971488Z"),
			SettledDateTime: DateTimeOffset.UnixEpoch,
			ArenaCode: "ph",
			MatchDayCode: "M000478",
			FightNumber: 67,
			BetAmount: 1m,
			ValidStake: 0m,
			TicketAmount: 0.985m,
			AnteAmount: 0.015m,
			WinLossAmount: 0m,
			Status: ApiBetStatus.CancelledPayoutInProgress,
			Result: "",
			MemberRefId: "4ucmqplxb029",
			AdditionalData: json.ToJsonString());
		var fakeGameApiClient = Substitute.For<IGameApiClient>();
		_ = fakeGameApiClient
			.TicketsAsync(
				apiBaseUrl: Arg.Is(query.ApiUrl),
				privateKey: Arg.Is(query.PrivateKey),
				partnerAccountId: Arg.Is(query.PartnerAccountId),
				page: Arg.Is(query.Page),
				pageSize: Arg.Is(query.PageSize),
				fromModifiedTime: Arg.Is(query.StartTime),
				toModifiedTime: Arg.Is(query.EndTime))
			.Returns(new[] { response }.ToAsyncEnumerable());
		var sut = new GameRecordBetLogQueryService(fakeGameApiClient);

		var actual = await sut.QueryAsync(query).ToArrayAsync().ConfigureAwait(true);

		Assert.Single(actual);
		var item = actual[0];
		Assert.Equal(Protos.GameRecordResult.None, item.Result);
	}

	[Fact]
	public async Task StatusOpened_WhenApiBetVoiding()
	{
		var query = new GameRecordBetLogQuery(
			ApiUrl: new("http://partner-api.dev.ga28.cc"),
			PrivateKey: "8456c9de-d589-4e8a-876f-c9c8e48dda3e",
			PartnerAccountId: "99a6f8ef-38a6-4f31-b69a-e222c2a54771",
			StartTime: new DateTimeOffset(2024, 06, 14, 10, 16, 00, TimeSpan.FromHours(8)),
			EndTime: new DateTimeOffset(2024, 06, 14, 12, 16, 00, TimeSpan.FromHours(8)),
			Page: 1,
			PageSize: 32);
		var json = JsonNode.Parse("""
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
				"status": 8,
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
		var response = new TicketResponseItem(
			Sid: "19010170AE3VA8I515",
			CreatedDateTime: DateTimeOffset.Parse("2024-06-13T05:34:23.971488Z"),
			ModifiedDateTime: DateTimeOffset.Parse("2024-06-13T05:34:23.971488Z"),
			SettledDateTime: DateTimeOffset.UnixEpoch,
			ArenaCode: "ph",
			MatchDayCode: "M000478",
			FightNumber: 67,
			BetAmount: 1m,
			ValidStake: 0m,
			TicketAmount: 0.985m,
			AnteAmount: 0.015m,
			WinLossAmount: 0m,
			Status: ApiBetStatus.BetVoiding,
			Result: "",
			MemberRefId: "4ucmqplxb029",
			AdditionalData: json.ToJsonString());
		var fakeGameApiClient = Substitute.For<IGameApiClient>();
		_ = fakeGameApiClient
			.TicketsAsync(
				apiBaseUrl: Arg.Is(query.ApiUrl),
				privateKey: Arg.Is(query.PrivateKey),
				partnerAccountId: Arg.Is(query.PartnerAccountId),
				page: Arg.Is(query.Page),
				pageSize: Arg.Is(query.PageSize),
				fromModifiedTime: Arg.Is(query.StartTime),
				toModifiedTime: Arg.Is(query.EndTime))
			.Returns(new[] { response }.ToAsyncEnumerable());
		var sut = new GameRecordBetLogQueryService(fakeGameApiClient);

		var actual = await sut.QueryAsync(query).ToArrayAsync().ConfigureAwait(true);

		Assert.Single(actual);
		var item = actual[0];
		Assert.Equal(Protos.GameRecordResult.None, item.Result);
	}

	[Fact]
	public async Task StatusOpened_WhenApiReassessedAwaitingPayout()
	{
		var query = new GameRecordBetLogQuery(
			ApiUrl: new("http://partner-api.dev.ga28.cc"),
			PrivateKey: "8456c9de-d589-4e8a-876f-c9c8e48dda3e",
			PartnerAccountId: "99a6f8ef-38a6-4f31-b69a-e222c2a54771",
			StartTime: new DateTimeOffset(2024, 06, 14, 10, 16, 00, TimeSpan.FromHours(8)),
			EndTime: new DateTimeOffset(2024, 06, 14, 12, 16, 00, TimeSpan.FromHours(8)),
			Page: 1,
			PageSize: 32);
		var json = JsonNode.Parse("""
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
				"status": 10,
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
		var response = new TicketResponseItem(
			Sid: "19010170AE3VA8I515",
			CreatedDateTime: DateTimeOffset.Parse("2024-06-13T05:34:23.971488Z"),
			ModifiedDateTime: DateTimeOffset.Parse("2024-06-13T05:34:23.971488Z"),
			SettledDateTime: DateTimeOffset.UnixEpoch,
			ArenaCode: "ph",
			MatchDayCode: "M000478",
			FightNumber: 67,
			BetAmount: 1m,
			ValidStake: 0m,
			TicketAmount: 0.985m,
			AnteAmount: 0.015m,
			WinLossAmount: 0m,
			Status: ApiBetStatus.ReassessedAwaitingPayout,
			Result: "",
			MemberRefId: "4ucmqplxb029",
			AdditionalData: json.ToJsonString());
		var fakeGameApiClient = Substitute.For<IGameApiClient>();
		_ = fakeGameApiClient
			.TicketsAsync(
				apiBaseUrl: Arg.Is(query.ApiUrl),
				privateKey: Arg.Is(query.PrivateKey),
				partnerAccountId: Arg.Is(query.PartnerAccountId),
				page: Arg.Is(query.Page),
				pageSize: Arg.Is(query.PageSize),
				fromModifiedTime: Arg.Is(query.StartTime),
				toModifiedTime: Arg.Is(query.EndTime))
			.Returns(new[] { response }.ToAsyncEnumerable());
		var sut = new GameRecordBetLogQueryService(fakeGameApiClient);

		var actual = await sut.QueryAsync(query).ToArrayAsync().ConfigureAwait(true);

		Assert.Single(actual);
		var item = actual[0];
		Assert.Equal(Protos.GameRecordResult.None, item.Result);
	}

	[Fact]
	public async Task StatusOpened_WhenApiReassessedPayoutInProgress()
	{
		var query = new GameRecordBetLogQuery(
			ApiUrl: new("http://partner-api.dev.ga28.cc"),
			PrivateKey: "8456c9de-d589-4e8a-876f-c9c8e48dda3e",
			PartnerAccountId: "99a6f8ef-38a6-4f31-b69a-e222c2a54771",
			StartTime: new DateTimeOffset(2024, 06, 14, 10, 16, 00, TimeSpan.FromHours(8)),
			EndTime: new DateTimeOffset(2024, 06, 14, 12, 16, 00, TimeSpan.FromHours(8)),
			Page: 1,
			PageSize: 32);
		var json = JsonNode.Parse("""
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
				"status": 11,
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
		var response = new TicketResponseItem(
			Sid: "19010170AE3VA8I515",
			CreatedDateTime: DateTimeOffset.Parse("2024-06-13T05:34:23.971488Z"),
			ModifiedDateTime: DateTimeOffset.Parse("2024-06-13T05:34:23.971488Z"),
			SettledDateTime: DateTimeOffset.UnixEpoch,
			ArenaCode: "ph",
			MatchDayCode: "M000478",
			FightNumber: 67,
			BetAmount: 1m,
			ValidStake: 0m,
			TicketAmount: 0.985m,
			AnteAmount: 0.015m,
			WinLossAmount: 0m,
			Status: ApiBetStatus.ReassessedPayoutInProgress,
			Result: "",
			MemberRefId: "4ucmqplxb029",
			AdditionalData: json.ToJsonString());
		var fakeGameApiClient = Substitute.For<IGameApiClient>();
		_ = fakeGameApiClient
			.TicketsAsync(
				apiBaseUrl: Arg.Is(query.ApiUrl),
				privateKey: Arg.Is(query.PrivateKey),
				partnerAccountId: Arg.Is(query.PartnerAccountId),
				page: Arg.Is(query.Page),
				pageSize: Arg.Is(query.PageSize),
				fromModifiedTime: Arg.Is(query.StartTime),
				toModifiedTime: Arg.Is(query.EndTime))
			.Returns(new[] { response }.ToAsyncEnumerable());
		var sut = new GameRecordBetLogQueryService(fakeGameApiClient);

		var actual = await sut.QueryAsync(query).ToArrayAsync().ConfigureAwait(true);

		Assert.Single(actual);
		var item = actual[0];
		Assert.Equal(Protos.GameRecordResult.None, item.Result);
	}

	[Fact]
	public async Task StatusOpened_WhenApiRecancelledAwaitingPayout()
	{
		var query = new GameRecordBetLogQuery(
			ApiUrl: new("http://partner-api.dev.ga28.cc"),
			PrivateKey: "8456c9de-d589-4e8a-876f-c9c8e48dda3e",
			PartnerAccountId: "99a6f8ef-38a6-4f31-b69a-e222c2a54771",
			StartTime: new DateTimeOffset(2024, 06, 14, 10, 16, 00, TimeSpan.FromHours(8)),
			EndTime: new DateTimeOffset(2024, 06, 14, 12, 16, 00, TimeSpan.FromHours(8)),
			Page: 1,
			PageSize: 32);
		var json = JsonNode.Parse("""
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
				"status": 12,
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
		var response = new TicketResponseItem(
			Sid: "19010170AE3VA8I515",
			CreatedDateTime: DateTimeOffset.Parse("2024-06-13T05:34:23.971488Z"),
			ModifiedDateTime: DateTimeOffset.Parse("2024-06-13T05:34:23.971488Z"),
			SettledDateTime: DateTimeOffset.UnixEpoch,
			ArenaCode: "ph",
			MatchDayCode: "M000478",
			FightNumber: 67,
			BetAmount: 1m,
			ValidStake: 0m,
			TicketAmount: 0.985m,
			AnteAmount: 0.015m,
			WinLossAmount: 0m,
			Status: ApiBetStatus.RecancelledAwaitingPayout,
			Result: "",
			MemberRefId: "4ucmqplxb029",
			AdditionalData: json.ToJsonString());
		var fakeGameApiClient = Substitute.For<IGameApiClient>();
		_ = fakeGameApiClient
			.TicketsAsync(
				apiBaseUrl: Arg.Is(query.ApiUrl),
				privateKey: Arg.Is(query.PrivateKey),
				partnerAccountId: Arg.Is(query.PartnerAccountId),
				page: Arg.Is(query.Page),
				pageSize: Arg.Is(query.PageSize),
				fromModifiedTime: Arg.Is(query.StartTime),
				toModifiedTime: Arg.Is(query.EndTime))
			.Returns(new[] { response }.ToAsyncEnumerable());
		var sut = new GameRecordBetLogQueryService(fakeGameApiClient);

		var actual = await sut.QueryAsync(query).ToArrayAsync().ConfigureAwait(true);

		Assert.Single(actual);
		var item = actual[0];
		Assert.Equal(Protos.GameRecordResult.None, item.Result);
	}

	[Fact]
	public async Task StatusOpened_WhenApiRecancelledPayoutInProgress()
	{
		var query = new GameRecordBetLogQuery(
			ApiUrl: new("http://partner-api.dev.ga28.cc"),
			PrivateKey: "8456c9de-d589-4e8a-876f-c9c8e48dda3e",
			PartnerAccountId: "99a6f8ef-38a6-4f31-b69a-e222c2a54771",
			StartTime: new DateTimeOffset(2024, 06, 14, 10, 16, 00, TimeSpan.FromHours(8)),
			EndTime: new DateTimeOffset(2024, 06, 14, 12, 16, 00, TimeSpan.FromHours(8)),
			Page: 1,
			PageSize: 32);
		var json = JsonNode.Parse("""
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
				"status": 13,
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
		var response = new TicketResponseItem(
			Sid: "19010170AE3VA8I515",
			CreatedDateTime: DateTimeOffset.Parse("2024-06-13T05:34:23.971488Z"),
			ModifiedDateTime: DateTimeOffset.Parse("2024-06-13T05:34:23.971488Z"),
			SettledDateTime: DateTimeOffset.UnixEpoch,
			ArenaCode: "ph",
			MatchDayCode: "M000478",
			FightNumber: 67,
			BetAmount: 1m,
			ValidStake: 0m,
			TicketAmount: 0.985m,
			AnteAmount: 0.015m,
			WinLossAmount: 0m,
			Status: ApiBetStatus.RecancelledPayoutInProgress,
			Result: "",
			MemberRefId: "4ucmqplxb029",
			AdditionalData: json.ToJsonString());
		var fakeGameApiClient = Substitute.For<IGameApiClient>();
		_ = fakeGameApiClient
			.TicketsAsync(
				apiBaseUrl: Arg.Is(query.ApiUrl),
				privateKey: Arg.Is(query.PrivateKey),
				partnerAccountId: Arg.Is(query.PartnerAccountId),
				page: Arg.Is(query.Page),
				pageSize: Arg.Is(query.PageSize),
				fromModifiedTime: Arg.Is(query.StartTime),
				toModifiedTime: Arg.Is(query.EndTime))
			.Returns(new[] { response }.ToAsyncEnumerable());
		var sut = new GameRecordBetLogQueryService(fakeGameApiClient);

		var actual = await sut.QueryAsync(query).ToArrayAsync().ConfigureAwait(true);

		Assert.Single(actual);
		var item = actual[0];
		Assert.Equal(Protos.GameRecordResult.None, item.Result);
	}

	[Fact]
	public async Task StatusCanceled_WhenApiBetCancelled()
	{
		var query = new GameRecordBetLogQuery(
			ApiUrl: new("http://partner-api.dev.ga28.cc"),
			PrivateKey: "8456c9de-d589-4e8a-876f-c9c8e48dda3e",
			PartnerAccountId: "99a6f8ef-38a6-4f31-b69a-e222c2a54771",
			StartTime: new DateTimeOffset(2024, 06, 14, 10, 16, 00, TimeSpan.FromHours(8)),
			EndTime: new DateTimeOffset(2024, 06, 14, 12, 16, 00, TimeSpan.FromHours(8)),
			Page: 1,
			PageSize: 32);
		var json = JsonNode.Parse("""
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
				"status": 7,
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
		var response = new TicketResponseItem(
			Sid: "19010170AE3VA8I515",
			CreatedDateTime: DateTimeOffset.Parse("2024-06-13T05:34:23.971488Z"),
			ModifiedDateTime: DateTimeOffset.Parse("2024-06-13T05:34:23.971488Z"),
			SettledDateTime: DateTimeOffset.UnixEpoch,
			ArenaCode: "ph",
			MatchDayCode: "M000478",
			FightNumber: 67,
			BetAmount: 1m,
			ValidStake: 0m,
			TicketAmount: 0.985m,
			AnteAmount: 0.015m,
			WinLossAmount: 0m,
			Status: ApiBetStatus.BetCancelled,
			Result: "",
			MemberRefId: "4ucmqplxb029",
			AdditionalData: json.ToJsonString());
		var fakeGameApiClient = Substitute.For<IGameApiClient>();
		_ = fakeGameApiClient
			.TicketsAsync(
				apiBaseUrl: Arg.Is(query.ApiUrl),
				privateKey: Arg.Is(query.PrivateKey),
				partnerAccountId: Arg.Is(query.PartnerAccountId),
				page: Arg.Is(query.Page),
				pageSize: Arg.Is(query.PageSize),
				fromModifiedTime: Arg.Is(query.StartTime),
				toModifiedTime: Arg.Is(query.EndTime))
			.Returns(new[] { response }.ToAsyncEnumerable());
		var sut = new GameRecordBetLogQueryService(fakeGameApiClient);

		var actual = await sut.QueryAsync(query).ToArrayAsync().ConfigureAwait(true);

		Assert.Single(actual);
		var item = actual[0];
		Assert.Equal(Protos.GameRecordResult.None, item.Result);
	}

	[Fact]
	public async Task StatusVoided_WhenApiBetVoided()
	{
		var query = new GameRecordBetLogQuery(
			ApiUrl: new("http://partner-api.dev.ga28.cc"),
			PrivateKey: "8456c9de-d589-4e8a-876f-c9c8e48dda3e",
			PartnerAccountId: "99a6f8ef-38a6-4f31-b69a-e222c2a54771",
			StartTime: new DateTimeOffset(2024, 06, 14, 10, 16, 00, TimeSpan.FromHours(8)),
			EndTime: new DateTimeOffset(2024, 06, 14, 12, 16, 00, TimeSpan.FromHours(8)),
			Page: 1,
			PageSize: 32);
		var json = JsonNode.Parse("""
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
				"status": 9,
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
		var response = new TicketResponseItem(
			Sid: "19010170AE3VA8I515",
			CreatedDateTime: DateTimeOffset.Parse("2024-06-13T05:34:23.971488Z"),
			ModifiedDateTime: DateTimeOffset.Parse("2024-06-13T05:34:23.971488Z"),
			SettledDateTime: DateTimeOffset.UnixEpoch,
			ArenaCode: "ph",
			MatchDayCode: "M000478",
			FightNumber: 67,
			BetAmount: 1m,
			ValidStake: 0m,
			TicketAmount: 0.985m,
			AnteAmount: 0.015m,
			WinLossAmount: 0m,
			Status: ApiBetStatus.BetVoided,
			Result: "",
			MemberRefId: "4ucmqplxb029",
			AdditionalData: json.ToJsonString());
		var fakeGameApiClient = Substitute.For<IGameApiClient>();
		_ = fakeGameApiClient
			.TicketsAsync(
				apiBaseUrl: Arg.Is(query.ApiUrl),
				privateKey: Arg.Is(query.PrivateKey),
				partnerAccountId: Arg.Is(query.PartnerAccountId),
				page: Arg.Is(query.Page),
				pageSize: Arg.Is(query.PageSize),
				fromModifiedTime: Arg.Is(query.StartTime),
				toModifiedTime: Arg.Is(query.EndTime))
			.Returns(new[] { response }.ToAsyncEnumerable());
		var sut = new GameRecordBetLogQueryService(fakeGameApiClient);

		var actual = await sut.QueryAsync(query).ToArrayAsync().ConfigureAwait(true);

		Assert.Single(actual);
		var item = actual[0];
		Assert.Equal(Protos.GameRecordResult.None, item.Result);
	}
}
