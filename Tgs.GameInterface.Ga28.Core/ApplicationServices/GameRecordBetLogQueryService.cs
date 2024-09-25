using System.Runtime.CompilerServices;
using Tgs.ServiceUtility;

namespace Tgs.GameInterface.Ga28.Core.ApplicationServices;

public sealed class GameRecordBetLogQueryService(IGameApiClient apiClient) : IQueryService<GameRecordBetLogQuery, GameRecordBetLogResult>
{
	public async IAsyncEnumerable<GameRecordBetLogResult> QueryAsync(GameRecordBetLogQuery query, [EnumeratorCancellation] CancellationToken cancellationToken = default)
	{
		var response = apiClient.TicketsAsync(
			apiBaseUrl: query.ApiUrl,
			privateKey: query.PrivateKey,
			partnerAccountId: query.PartnerAccountId,
			page: query.Page,
			pageSize: query.PageSize,
			fromModifiedTime: query.StartTime,
			toModifiedTime: query.EndTime,
			cancellationToken);

		await foreach (var item in response)
		{
			var result = Protos.GameRecordResult.None;
			var externalRoundId = string.Concat(item.ArenaCode, "#", item.FightNumber, " ", item.MatchDayCode);
			var externalClosedOn = DateTimeOffset.UnixEpoch;

			if (!string.IsNullOrWhiteSpace(item.Result))
			{
				result = item.Result.ToLower() switch
				{
					"win" => Protos.GameRecordResult.Win,
					"loss" => Protos.GameRecordResult.Loss,
					"draw" => Protos.GameRecordResult.Draw,
					"cancelled" => Protos.GameRecordResult.Cancelled,
					_ => Protos.GameRecordResult.Void,
				};

				externalClosedOn = result is not Protos.GameRecordResult.Cancelled and not Protos.GameRecordResult.Void ? item.SettledDateTime : item.ModifiedDateTime;
			}

			yield return new GameRecordBetLogResult(
				PlayerCode: item.MemberRefId,
				ExternalBetId: item.Sid,
				ExternalRoundId: externalRoundId,
				Result: result,
				ExternalCreatedOn: item.CreatedDateTime,
				ExternalClosedOn: externalClosedOn,
				Bet: item.BetAmount,
				ValidBet: item.ValidStake,
				Settle: (result != Protos.GameRecordResult.Loss) ? item.ValidStake + item.WinLossAmount : 0.0m,
				WinLoss: item.WinLossAmount,
				AdditionalData: item.AdditionalData);
		}
	}
}
