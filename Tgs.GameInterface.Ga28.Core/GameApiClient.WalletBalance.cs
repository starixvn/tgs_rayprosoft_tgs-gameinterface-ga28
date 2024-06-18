using System.Net;
using System.Text.Json.Nodes;
using Tgs.GameInterface.Ga28.Core.Models;

namespace Tgs.GameInterface.Ga28.Core;

internal sealed partial class GameApiClient
{
	public async ValueTask<WalletBalanceResponse> WalletBalanceAsync(Uri apiBaseUrl, string privateKey, string memberRefId, CancellationToken cancellationToken = default)
	{
		var relativeUrl = "/api/v1/wallet/balance";
		var parameters = new Dictionary<string, string>
		{
			["member_ref_id"] = memberRefId,
		};
		var logPrefix = nameof(WalletBalanceAsync);

		var (statusCode, responseContent) = await GetWithParameterAsync(
			apiBaseUrl,
			relativeUrl,
			privateKey,
			parameters,
			logPrefix,
			cancellationToken)
			.ConfigureAwait(false);

		var response = (statusCode, responseContent) switch
		{
			(HttpStatusCode.OK, string json) when JsonNode.Parse(json) is JsonNode node
				=> new WalletBalanceResponse(
					IsPlayerExists: true,
					Balance: decimal.Parse(node["balance"]!.GetValue<string>()!)),

			(HttpStatusCode.BadRequest, _) // 尚未登入創建帳號會回這個
				=> new WalletBalanceResponse(
					IsPlayerExists: false,
					Balance: 0m),

			_ => throw new HttpRequestException(HttpRequestError.InvalidResponse, statusCode: statusCode),
		};

		return response;
	}
}
