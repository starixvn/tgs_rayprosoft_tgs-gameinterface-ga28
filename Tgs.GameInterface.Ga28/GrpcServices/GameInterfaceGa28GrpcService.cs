using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using Tgs.GameInterface.Ga28.Core.ApplicationServices;
using Tgs.GameInterface.Ga28.Protos;
using Tgs.ServiceUtility;

namespace Tgs.GameInterface.Ga28.GrpcServices;

internal sealed class GameInterfaceGa28GrpcService(
	ILogger<GameInterfaceGa28GrpcService> logger,
	IGetService<GameUrlQuery, string> gameUrlService,
	IGetService<PlayerBalanceQuery, PlayerBalanceResult> playerBalanceService,
	IGetService<FundInQuery, FundResult> fundInService,
	IGetService<FundOutQuery, FundResult> fundOutService,
	IGetService<FundStatusQuery, bool> fundStatusService,
	IQueryService<GameRecordBetLogQuery, GameRecordBetLogResult> gameRecordBetLogService)
	: GameInterfaceGa28.GameInterfaceGa28Base
{
	public override Task<CheckingServiceReply> CheckingService(CheckingServiceRequest request, ServerCallContext context)
	{
		logger.LogWarning("LogOn:{logOn} Message: Create a check connection.({machineName})", DateTime.Now.ToString("o"), request.MachineName);

		return Task.FromResult(new CheckingServiceReply
		{
			MachineName = Environment.MachineName,
		});
	}

	public override async Task<GameUrlReply> GameUrl(GameUrlRequest request, ServerCallContext context)
	{
		var query = new GameUrlQuery(
			ApiUrl: new Uri(request.ApiUrl),
			PrivateKey: request.PrivateKey,
			MerchantId: request.MerchantId,
			PlayerCode: request.PlayerCode);

		var result = await gameUrlService.GetAsync(
			query: query,
			cancellationToken: context.CancellationToken)
			.ConfigureAwait(false);

		return new GameUrlReply
		{
			GameUrl = result,
		};
	}

	public override async Task<PlayerBalanceReply> PlayerBalance(PlayerBalanceRequest request, ServerCallContext context)
	{
		var query = new PlayerBalanceQuery(
			ApiUrl: new Uri(request.ApiUrl),
			PrivateKey: request.PrivateKey,
			PlayerCode: request.PlayerCode);

		var result = await playerBalanceService.GetAsync(
			query: query,
			cancellationToken: context.CancellationToken)
			.ConfigureAwait(false);

		return new PlayerBalanceReply
		{
			IsPlayerExists = result.IsPlayerExists,
			Balance = result.Balance
		};
	}

	public override async Task<FundReply> FundIn(FundRequest request, ServerCallContext context)
	{
		var query = new FundInQuery(
			ApiUrl: new Uri(request.ApiUrl),
			PrivateKey: request.PrivateKey,
			PlayerCode: request.PlayerCode,
			OrderId: request.OrderId,
			Amount: request.Amount);

		var result = await fundInService.GetAsync(
			query: query,
			cancellationToken: context.CancellationToken)
			.ConfigureAwait(false);

		return new FundReply
		{
			OrderId = result.OrderId,
			ExternalOrderId = result.ExternalOrderId,
			Balance = result.Balance,
			Resp = result.Resp,
		};
	}

	public override async Task<FundReply> FundOut(FundRequest request, ServerCallContext context)
	{
		var query = new FundOutQuery(
			ApiUrl: new Uri(request.ApiUrl),
			PrivateKey: request.PrivateKey,
			PlayerCode: request.PlayerCode,
			OrderId: request.OrderId,
			Amount: request.Amount);

		var result = await fundOutService.GetAsync(
			query: query,
			cancellationToken: context.CancellationToken)
			.ConfigureAwait(false);

		return new FundReply
		{
			OrderId = result.OrderId,
			ExternalOrderId = result.ExternalOrderId,
			Balance = result.Balance,
			Resp = result.Resp,
		};
	}

	public override async Task<BaseReply> FundStatus(FundStatusRequest request, ServerCallContext context)
	{
		var query = new FundStatusQuery(
			ApiUrl: new Uri(request.ApiUrl),
			PrivateKey: request.PrivateKey,
			PlayerCode: request.PlayerCode,
			OrderOn: request.OrderOn.ToDateTimeOffset(),
			OrderId: request.OrderId);

		var result = await fundStatusService.GetAsync(
			query: query,
			cancellationToken: context.CancellationToken)
			.ConfigureAwait(false);

		return new BaseReply
		{
			Status = result,
		};
	}

	public override async Task GameRecord(GameRecordRequest request, IServerStreamWriter<GameRecordInfo> responseStream, ServerCallContext context)
	{
		var query = new GameRecordBetLogQuery(
			ApiUrl: new Uri(request.ApiUrl),
			PrivateKey: request.PrivateKey,
			MerchantId: request.MerchantId,
			StartTime: request.StartTime.ToDateTimeOffset(),
			EndTime: request.EndTime.ToDateTimeOffset(),
			request.Page,
			request.PageSize);

		var call = gameRecordBetLogService
			.QueryAsync(
				query: query,
				cancellationToken: context.CancellationToken);

		await foreach (var record in call.ConfigureAwait(false))
		{
			await responseStream.WriteAsync(new GameRecordInfo
			{
				PlayerCode = record.PlayerCode,
				ExternalBetId = record.ExternalBetId,
				ExternalRoundId = record.ExternalRoundId,
				Result = record.Result,
				ExternalCreatedOn = record.ExternalCreatedOn.ToTimestamp(),
				ExternalClosedOn = record.ExternalClosedOn.ToTimestamp(),
				Bet = record.Bet,
				ValidBet = record.ValidBet,
				Settle = record.Settle,
				WinLoss = record.WinLoss,
				AdditionalData = record.AdditionalData
			})
			.ConfigureAwait(false);
		}
	}
}
