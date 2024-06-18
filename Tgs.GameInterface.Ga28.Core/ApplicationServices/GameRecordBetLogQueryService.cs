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
			var result = Protos.RecordResult.None;
			var externalRoundId = string.Concat(item.ArenaCode, "#", item.FightNumber, " ", item.MatchDayCode);
			var externalClosedOn = DateTimeOffset.UnixEpoch;

			if (!string.IsNullOrWhiteSpace(item.Result))
			{
				result = item.Result.ToLower() switch
				{
					"win" => Protos.RecordResult.Win,
					"loss" => Protos.RecordResult.Loss,
					"draw" => Protos.RecordResult.Draw,
					"cancelled" => Protos.RecordResult.Cancelled,
					_ => Protos.RecordResult.Void,
				};

				externalClosedOn = result is not Protos.RecordResult.Cancelled and not Protos.RecordResult.Void ? item.SettledDateTime : item.ModifiedDateTime;
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
				Settle: item.ValidStake + item.WinLossAmount,
				WinLoss: item.WinLossAmount,
				AdditionalData: item.AdditionalData);
		}
	}
}
