using System.Text.Json.Nodes;
using Google.Protobuf.WellKnownTypes;
using Grpc.Net.Client;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection.Extensions;
using NSubstitute;
using Tgs.GameInterface.Ga28.Client;
using Tgs.GameInterface.Ga28.Core;
using Tgs.GameInterface.Ga28.Core.ApplicationServices;
using Tgs.GameInterface.Ga28.Protos;
using Tgs.ServiceUtility;

namespace Tgs.GameInterface.Ga28.IntegrationTest;

public sealed class GameInterfaceGa28GrpcServiceTests
{
	[Fact]
	public async Task CheckingService()
	{
		var factory = new WebApplicationFactory<Program>()
			.WithWebHostBuilder(builder => builder.ConfigureServices(services => services.Replace(new(typeof(IGameApiClient), Substitute.For<IGameApiClient>()))));
		var options = new GrpcChannelOptions { HttpHandler = factory.Server.CreateHandler() };
		var channel = GrpcChannel.ForAddress(factory.Server.BaseAddress, options);
		var client = new GameInterfaceGa28.GameInterfaceGa28Client(channel);
		var sut = new GameInterfaceGa28Client(client);

		var actual = await sut.CheckingServiceAsync(Environment.MachineName).ConfigureAwait(true);

		Assert.Equal(Environment.MachineName, actual);
	}

	[Fact]
	public async Task GameUrl()
	{
		var grpcRequest = new GameUrlRequest
		{
			ApiUrl = "http://partner-api.dev.ga28.cc",
			PrivateKey = "8456c9de-d589-4e8a-876f-c9c8e48dda3e",
			PartnerAccountId = "99a6f8ef-38a6-4f31-b69a-e222c2a54771",
			PlayerCode = "4ucmqplxb029",
			Language = "en",
		};
		var serviceRequest = new GameUrlQuery(
			ApiUrl: new Uri(grpcRequest.ApiUrl),
			PrivateKey: grpcRequest.PrivateKey,
			PartnerAccountId: grpcRequest.PartnerAccountId,
			PlayerCode: grpcRequest.PlayerCode,
			Language: grpcRequest.Language);
		var serviceResponse = "http://gc-dev.ga28.cc:80/launch?member_token=8d475cde-b3af-4a37-aa00-3eda04018b2d&language=en";
		var fakeService = Substitute.For<IGetService<GameUrlQuery, string>>();
		_ = fakeService.GetAsync(
			query: Arg.Is(serviceRequest),
			cancellationToken: Arg.Any<CancellationToken>())
			.Returns(ValueTask.FromResult(serviceResponse));
		var factory = new WebApplicationFactory<Program>()
			.WithWebHostBuilder(builder => builder.ConfigureServices(services =>
			{
				services.Replace(new(typeof(IGameApiClient), Substitute.For<IGameApiClient>()));
				services.Replace(new(typeof(IGetService<GameUrlQuery, string>), fakeService));
			}));
		var options = new GrpcChannelOptions { HttpHandler = factory.Server.CreateHandler() };
		var channel = GrpcChannel.ForAddress(factory.Server.BaseAddress, options);
		var client = new GameInterfaceGa28.GameInterfaceGa28Client(channel);
		var sut = new GameInterfaceGa28Client(client);

		var actual = await sut.GameUrlAsync(
			apiUrl: new Uri(grpcRequest.ApiUrl),
			privateKey: grpcRequest.PrivateKey,
			partnerAccountId: grpcRequest.PartnerAccountId,
			playerCode: grpcRequest.PlayerCode,
			language: grpcRequest.Language)
			.ConfigureAwait(true);

		Assert.Equal("http://gc-dev.ga28.cc:80/launch?member_token=8d475cde-b3af-4a37-aa00-3eda04018b2d&language=en", actual.GameUrl);
	}

	[Fact]
	public async Task PlayerBalance()
	{
		var grpcRequest = new GameUrlRequest
		{
			ApiUrl = "http://partner-api.dev.ga28.cc",
			PrivateKey = "8456c9de-d589-4e8a-876f-c9c8e48dda3e",
			PartnerAccountId = "99a6f8ef-38a6-4f31-b69a-e222c2a54771",
			PlayerCode = "4ucmqplxb029",
		};
		var serviceRequest = new PlayerBalanceQuery(
			ApiUrl: new Uri(grpcRequest.ApiUrl),
			PrivateKey: grpcRequest.PrivateKey,
			PlayerCode: grpcRequest.PlayerCode);
		var serviceResponse = new PlayerBalanceResult(
			IsPlayerExist: true,
			Balance: 12m);
		var fakeService = Substitute.For<IGetService<PlayerBalanceQuery, PlayerBalanceResult>>();
		fakeService
			.GetAsync(
				query: Arg.Is(serviceRequest),
				cancellationToken: Arg.Any<CancellationToken>())
			.Returns(ValueTask.FromResult(serviceResponse));
		var factory = new WebApplicationFactory<Program>()
			.WithWebHostBuilder(builder => builder.ConfigureServices(services =>
			{
				services.Replace(new(typeof(IGameApiClient), Substitute.For<IGameApiClient>()));
				services.Replace(new(typeof(IGetService<PlayerBalanceQuery, PlayerBalanceResult>), fakeService));
			}));
		var options = new GrpcChannelOptions { HttpHandler = factory.Server.CreateHandler() };
		var channel = GrpcChannel.ForAddress(factory.Server.BaseAddress, options);
		var client = new GameInterfaceGa28.GameInterfaceGa28Client(channel);
		var sut = new GameInterfaceGa28Client(client);

		var actual = await sut.PlayerBalanceAsync(
			apiUrl: new Uri(grpcRequest.ApiUrl),
			privateKey: grpcRequest.PrivateKey,
			playerCode: grpcRequest.PlayerCode)
			.ConfigureAwait(true);

		Assert.NotNull(actual);
		Assert.True(actual.IsPlayerExist);
		Assert.Equal(12m, actual.Balance.ToDecimal());
	}

	[Fact]
	public async Task FundIn()
	{
		var grpcRequest = new FundRequest
		{
			ApiUrl = "http://partner-api.dev.ga28.cc",
			PrivateKey = "8456c9de-d589-4e8a-876f-c9c8e48dda3e",
			PlayerCode = "4ucmqplxb029",
			OrderId = "8e341ccbad82430098715f893b1b3333",
			Amount = 16m
		};
		var serviceRequest = new FundInQuery(
			ApiUrl: new Uri(grpcRequest.ApiUrl),
			PrivateKey: grpcRequest.PrivateKey,
			PlayerCode: grpcRequest.PlayerCode,
			OrderId: grpcRequest.OrderId,
			Amount: grpcRequest.Amount);
		var apiResponse = JsonNode.Parse("""
		{
			"balance": "16",
			"wallet_transaction": {
				"id": "50d96f2ade3a43ad91eff7b801e56a83",
				"date_time_created": "2023-11-29T11:37:54.416634096Z",
				"amount": "16",
				"ref_id": "8e341ccbad82430098715f893b1b3333",
				"description": "Withdraw",
				"member_ref_id": "4ucmqplxb029",
				"type": 8
			}
		}
		""")!;
		var serviceResponse = new FundResult(
			OrderId: "8e341ccbad82430098715f893b1b3333",
			ExternalOrderId: "50d96f2ade3a43ad91eff7b801e56a83",
			Balance: 16m,
			Resp: apiResponse.ToJsonString());
		var fakeService = Substitute.For<IGetService<FundInQuery, FundResult>>();
		fakeService
			.GetAsync(
				query: Arg.Is(serviceRequest),
				cancellationToken: Arg.Any<CancellationToken>())
			.Returns(ValueTask.FromResult(serviceResponse));
		var factory = new WebApplicationFactory<Program>()
			.WithWebHostBuilder(builder => builder.ConfigureServices(services =>
			{
				services.Replace(new(typeof(IGameApiClient), Substitute.For<IGameApiClient>()));
				services.Replace(new(typeof(IGetService<FundInQuery, FundResult>), fakeService));
			}));
		var options = new GrpcChannelOptions { HttpHandler = factory.Server.CreateHandler() };
		var channel = GrpcChannel.ForAddress(factory.Server.BaseAddress, options);
		var client = new GameInterfaceGa28.GameInterfaceGa28Client(channel);
		var sut = new GameInterfaceGa28Client(client);

		var actual = await sut.FundInAsync(
			apiUrl: new Uri(grpcRequest.ApiUrl),
			privateKey: grpcRequest.PrivateKey,
			playerCode: grpcRequest.PlayerCode,
			orderId: grpcRequest.OrderId,
			amount: grpcRequest.Amount)
			.ConfigureAwait(true);

		Assert.NotNull(actual);
		Assert.Equal("8e341ccbad82430098715f893b1b3333", actual.OrderId);
		Assert.Equal("50d96f2ade3a43ad91eff7b801e56a83", actual.ExternalOrderId);
		Assert.Equal(16m, actual.Balance.ToDecimal());
		Assert.Equal(apiResponse.ToJsonString(), actual.Resp);
	}

	[Fact]
	public async Task FundOut()
	{
		var grpcRequest = new FundRequest
		{
			ApiUrl = "http://partner-api.dev.ga28.cc",
			PrivateKey = "8456c9de-d589-4e8a-876f-c9c8e48dda3e",
			PlayerCode = "4ucmqplxb029",
			OrderId = "8e341ccbad82430098715f893b1b3333",
			Amount = 18m
		};
		var serviceRequest = new FundOutQuery(
			ApiUrl: new Uri(grpcRequest.ApiUrl),
			PrivateKey: grpcRequest.PrivateKey,
			PlayerCode: grpcRequest.PlayerCode,
			OrderId: grpcRequest.OrderId,
			Amount: grpcRequest.Amount);
		var apiResponse = JsonNode.Parse("""
		{
			"balance": "18",
			"wallet_transaction": {
				"id": "50d96f2ade3a43ad91eff7b801e56a83",
				"date_time_created": "2023-11-29T11:37:54.416634096Z",
				"amount": "18",
				"ref_id": "8e341ccbad82430098715f893b1b3333",
				"description": "Deposit",
				"member_ref_id": "4ucmqplxb029",
				"type": 7
			}
		}
		""")!;
		var serviceResponse = new FundResult(
			OrderId: "8e341ccbad82430098715f893b1b3333",
			ExternalOrderId: "50d96f2ade3a43ad91eff7b801e56a83",
			Balance: 18m,
			Resp: apiResponse.ToJsonString());
		var fakeService = Substitute.For<IGetService<FundOutQuery, FundResult>>();
		fakeService
			.GetAsync(
				query: Arg.Is(serviceRequest),
				cancellationToken: Arg.Any<CancellationToken>())
			.Returns(ValueTask.FromResult(serviceResponse));
		var factory = new WebApplicationFactory<Program>()
			.WithWebHostBuilder(builder => builder.ConfigureServices(services =>
			{
				services.Replace(new(typeof(IGameApiClient), Substitute.For<IGameApiClient>()));
				services.Replace(new(typeof(IGetService<FundOutQuery, FundResult>), fakeService));
			}));
		var options = new GrpcChannelOptions { HttpHandler = factory.Server.CreateHandler() };
		var channel = GrpcChannel.ForAddress(factory.Server.BaseAddress, options);
		var client = new GameInterfaceGa28.GameInterfaceGa28Client(channel);
		var sut = new GameInterfaceGa28Client(client);

		var actual = await sut.FundOutAsync(
			apiUrl: new Uri(grpcRequest.ApiUrl),
			privateKey: grpcRequest.PrivateKey,
			playerCode: grpcRequest.PlayerCode,
			orderId: grpcRequest.OrderId,
			amount: grpcRequest.Amount)
			.ConfigureAwait(true);

		Assert.NotNull(actual);
		Assert.Equal("8e341ccbad82430098715f893b1b3333", actual.OrderId);
		Assert.Equal("50d96f2ade3a43ad91eff7b801e56a83", actual.ExternalOrderId);
		Assert.Equal(18m, actual.Balance.ToDecimal());
		Assert.Equal(apiResponse.ToJsonString(), actual.Resp);
	}

	[Fact]
	public async Task FundStatus()
	{
		var grpcRequest = new FundStatusRequest
		{
			ApiUrl = "http://partner-api.dev.ga28.cc",
			PrivateKey = "8456c9de-d589-4e8a-876f-c9c8e48dda3e",
			PlayerCode = "4ucmqplxb029",
			OrderOn = new DateTimeOffset(2024, 06, 13, 14, 59, 59, offset: TimeSpan.FromHours(8)).ToTimestamp(),
			OrderId = "8e341ccbad82430098715f893b1b3333",
		};
		var serviceRequest = new FundStatusQuery(
			ApiUrl: new Uri(grpcRequest.ApiUrl),
			PrivateKey: grpcRequest.PrivateKey,
			PlayerCode: grpcRequest.PlayerCode,
			OrderOn: grpcRequest.OrderOn.ToDateTimeOffset(),
			OrderId: grpcRequest.OrderId);
		var serviceResponse = true;
		var fakeService = Substitute.For<IGetService<FundStatusQuery, bool>>();
		fakeService
			.GetAsync(
				query: Arg.Is(serviceRequest),
				cancellationToken: Arg.Any<CancellationToken>())
			.Returns(ValueTask.FromResult(serviceResponse));
		var factory = new WebApplicationFactory<Program>()
			.WithWebHostBuilder(builder => builder.ConfigureServices(services =>
			{
				services.Replace(new(typeof(IGameApiClient), Substitute.For<IGameApiClient>()));
				services.Replace(new(typeof(IGetService<FundStatusQuery, bool>), fakeService));
			}));
		var options = new GrpcChannelOptions { HttpHandler = factory.Server.CreateHandler() };
		var channel = GrpcChannel.ForAddress(factory.Server.BaseAddress, options);
		var client = new GameInterfaceGa28.GameInterfaceGa28Client(channel);
		var sut = new GameInterfaceGa28Client(client);

		var actual = await sut.FundStatusAsync(
			apiUrl: new Uri(grpcRequest.ApiUrl),
			privateKey: grpcRequest.PrivateKey,
			playerCode: grpcRequest.PlayerCode,
			orderOn: grpcRequest.OrderOn.ToDateTimeOffset(),
			orderId: grpcRequest.OrderId)
			.ConfigureAwait(true);

		Assert.True(actual);
	}

	[Fact]
	public async Task GameRecord()
	{
		var grpcRequest = new GameRecordRequest
		{
			ApiUrl = "http://partner-api.dev.ga28.cc",
			PrivateKey = "8456c9de-d589-4e8a-876f-c9c8e48dda3e",
			PartnerAccountId = "99a6f8ef-38a6-4f31-b69a-e222c2a54771",
			StartTime = new DateTimeOffset(2024, 06, 13, 14, 00, 00, offset: TimeSpan.FromHours(8)).ToTimestamp(),
			EndTime = new DateTimeOffset(2024, 06, 13, 14, 59, 59, offset: TimeSpan.FromHours(8)).ToTimestamp(),
			Page = 1,
			PageSize = 32,
		};
		var serviceRequest = new GameRecordBetLogQuery(
			ApiUrl: new Uri(grpcRequest.ApiUrl),
			PrivateKey: grpcRequest.PrivateKey,
			PartnerAccountId: grpcRequest.PartnerAccountId,
			StartTime: grpcRequest.StartTime.ToDateTimeOffset(),
			EndTime: grpcRequest.EndTime.ToDateTimeOffset(),
			Page: grpcRequest.Page,
			PageSize: grpcRequest.PageSize);
		var apiResponse = JsonNode.Parse("""
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
		""")!;
		var serviceResponse = new GameRecordBetLogResult(
			PlayerCode: "4ucmqplxb029",
			ExternalBetId: "190103969DD96J562D",
			ExternalRoundId: "74",
			Result: RecordResult.Win,
			ExternalCreatedOn: new DateTimeOffset(2024, 06, 13, 14, 11, 56, 510, 124, TimeSpan.FromHours(8)),
			ExternalClosedOn: new DateTimeOffset(2024, 06, 13, 14, 12, 29, 238, 152, TimeSpan.FromHours(8)),
			Bet: 10m,
			ValidBet: 9.85m,
			Settle: 16.548m,
			WinLoss: 6.698m,
			AdditionalData: apiResponse.ToJsonString());
		var fakeService = Substitute.For<IQueryService<GameRecordBetLogQuery, GameRecordBetLogResult>>();
		fakeService
			.QueryAsync(
				query: Arg.Is(serviceRequest),
				cancellationToken: Arg.Any<CancellationToken>())
			.Returns(new[] { serviceResponse }.ToAsyncEnumerable());
		var factory = new WebApplicationFactory<Program>()
			.WithWebHostBuilder(builder => builder.ConfigureServices(services =>
			{
				services.Replace(new(typeof(IGameApiClient), Substitute.For<IGameApiClient>()));
				services.Replace(new(typeof(IQueryService<GameRecordBetLogQuery, GameRecordBetLogResult>), fakeService));
			}));
		var options = new GrpcChannelOptions { HttpHandler = factory.Server.CreateHandler() };
		var channel = GrpcChannel.ForAddress(factory.Server.BaseAddress, options);
		var client = new GameInterfaceGa28.GameInterfaceGa28Client(channel);
		var sut = new GameInterfaceGa28Client(client);

		var actual = await sut.GameRecordAsync(
			apiUrl: new Uri(grpcRequest.ApiUrl),
			privateKey: grpcRequest.PrivateKey,
			partnerAccountId: grpcRequest.PartnerAccountId,
			startTime: grpcRequest.StartTime.ToDateTimeOffset(),
			endTime: grpcRequest.EndTime.ToDateTimeOffset(),
			page: grpcRequest.Page,
			pageSize: grpcRequest.PageSize)
			.ToArrayAsync()
			.ConfigureAwait(true);

		Assert.Single(actual);
		var item = actual[0];
		Assert.Equal("4ucmqplxb029", item.PlayerCode);
		Assert.Equal("190103969DD96J562D", item.ExternalBetId);
		Assert.Equal("74", item.ExternalRoundId);
		Assert.Equal(RecordResult.Win, item.Result);
		Assert.Equal(new DateTimeOffset(2024, 06, 13, 14, 11, 56, 510, 124, TimeSpan.FromHours(8)), item.ExternalCreatedOn.ToDateTimeOffset());
		Assert.Equal(new DateTimeOffset(2024, 06, 13, 14, 12, 29, 238, 152, TimeSpan.FromHours(8)), item.ExternalClosedOn.ToDateTimeOffset());
		Assert.Equal(10m, item.Bet.ToDecimal());
		Assert.Equal(9.85m, item.ValidBet.ToDecimal());
		Assert.Equal(16.548m, item.Settle.ToDecimal());
		Assert.Equal(6.698m, item.WinLoss.ToDecimal());
		Assert.Equal(apiResponse.ToJsonString(), item.AdditionalData);
	}
}
