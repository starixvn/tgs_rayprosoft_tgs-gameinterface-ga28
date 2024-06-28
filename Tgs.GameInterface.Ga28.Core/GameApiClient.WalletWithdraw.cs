using System.Net;
using System.Text.Json.Nodes;
using Tgs.GameInterface.Ga28.Core.Models;

namespace Tgs.GameInterface.Ga28.Core;

internal sealed partial class GameApiClient
{
	public async ValueTask<WalletTransactionResponse> WalletWithdrawAsync(Uri apiBaseUrl, string privateKey, string memberRefId, decimal amount, string refId, CancellationToken cancellationToken = default)
	{
		var relativeUrl = "/api/v1/wallet/withdraw";
		var parameters = new Dictionary<string, string>
		{
			["member_ref_id"] = memberRefId,
			["amount"] = amount.ToString("0.##"),
			["ref_id"] = refId,
		};
		var logPrefix = nameof(WalletWithdrawAsync);

		var (statusCode, responseContent) = await PostAsync(
			apiBaseUrl,
			relativeUrl,
			privateKey,
			parameters,
			logPrefix,
			cancellationToken)
			.ConfigureAwait(false);

		if (statusCode == HttpStatusCode.BadRequest)
			throw new HttpRequestException(HttpRequestError.InvalidResponse, message: "餘額不足", statusCode: statusCode);

		if ((int)statusCode is < 200 or > 299)
			throw new HttpRequestException(HttpRequestError.InvalidResponse, message: responseContent, statusCode: statusCode);

		var node = JsonNode.Parse(responseContent!)!;

		var response = new WalletTransactionResponse(
			Id: node["wallet_transaction"]!["id"]!.GetValue<string>()!,
			RefId: node["wallet_transaction"]!["ref_id"]!.GetValue<string>(),
			Balance: decimal.Parse(node["balance"]!.GetValue<string>()),
			Resp: node.ToJsonString());

		return response;
	}
}
