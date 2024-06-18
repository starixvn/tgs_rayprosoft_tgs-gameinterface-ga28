using System.Net;
using System.Runtime.CompilerServices;
using System.Text.Json.Nodes;
using Tgs.GameInterface.Ga28.Core.Models;

namespace Tgs.GameInterface.Ga28.Core;

internal sealed partial class GameApiClient
{
	public async IAsyncEnumerable<TicketResponseItem> TicketsAsync(Uri apiBaseUrl, string privateKey, int page, int pageSize, DateTimeOffset fromModifiedTime, DateTimeOffset toModifiedTime, string accountId, [EnumeratorCancellation] CancellationToken cancellationToken = default)
	{
		var relativeUrl = "/api/v1/tickets/";
		var parameters = new Dictionary<string, string>
		{
			["page"] = page.ToString(),
			["page_size"] = pageSize.ToString(),
			["from_modified_date_time"] = _dateTimeConverter.DatetimeConvertToString(fromModifiedTime),
			["to_modified_date_time"] = _dateTimeConverter.DatetimeConvertToString(toModifiedTime),
			["account_id"] = accountId,
		};
		var logPrefix = nameof(TicketsAsync);

		var (statusCode, responseContent) = await GetWithParameterAsync(
			apiBaseUrl,
			relativeUrl,
			privateKey,
			parameters,
			logPrefix,
			cancellationToken)
			.ConfigureAwait(false);

		if (statusCode == HttpStatusCode.NotFound)
			yield break;

		if (statusCode != HttpStatusCode.OK || responseContent is null || JsonNode.Parse(responseContent) is not JsonNode jsonNode)
			throw new HttpRequestException(HttpRequestError.InvalidResponse, message: responseContent, statusCode: statusCode);

		foreach (var nullableNode in jsonNode.AsArray())
		{
			var node = nullableNode!;

			var sid = node["sid"]!.GetValue<string>();
			var createdDateTime = DateTimeOffset.Parse(node["created_date_time"]!.GetValue<string>());
			var modifiedDateTime = DateTimeOffset.Parse(node["modified_date_time"]!.GetValue<string>());

			var arenaCode = node["arena_code"]!.GetValue<string>();
			var matchDayCode = node["match_day_code"]!.GetValue<string>();
			var fightNumber = node["fight_number"]!.GetValue<int>();

			var betAmount = decimal.Parse(node["bet_amount"]!.GetValue<string>());
			var ticketAmount = decimal.Parse(node["ticket_amount"]!.GetValue<string>());
			var anteAmount = decimal.Parse(node["ante_amount"]!.GetValue<string>());
			var winLossAmount = decimal.Parse(node["win_loss_amount"]!.GetValue<string>());

			var status = (ApiBetStatus)node["status"]!.GetValue<byte>();

			var memberRefId = node["member_ref_id"]!.GetValue<string>();

			// 未結算前會為 "" 的欄位
			var result = node["result"]!.GetValue<string>();

			// 未結算前會為 null 的欄位
			var settledDateTime = node["settled_date_time"] is { } sdtNode ? DateTimeOffset.Parse(sdtNode!.GetValue<string>()) : DateTimeOffset.UnixEpoch;
			var validStake = node["valid_stake"] is { } vsNode ? decimal.Parse(vsNode!.GetValue<string>()) : 0m;

			var item = new TicketResponseItem(
				Sid: sid,
				CreatedDateTime: createdDateTime,
				ModifiedDateTime: modifiedDateTime,
				SettledDateTime: settledDateTime,
				ArenaCode: arenaCode,
				MatchDayCode: matchDayCode,
				FightNumber: fightNumber,
				BetAmount: betAmount,
				ValidStake: validStake,
				TicketAmount: ticketAmount,
				AnteAmount: anteAmount,
				WinLossAmount: winLossAmount,
				Status: status,
				Result: result,
				MemberRefId: memberRefId,
				AdditionalData: node.ToJsonString());

			yield return item;
		}
	}
}
