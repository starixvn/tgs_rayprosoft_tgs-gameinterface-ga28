using System.Net;
using System.Text.Json.Nodes;

namespace Tgs.GameInterface.Ga28.Core;

internal sealed partial class GameApiClient
{
	public async ValueTask<string> MembersLoginAsync(Uri apiBaseUrl, string privateKey, string memberRefId, string accountId, CancellationToken cancellationToken = default)
	{
		var relativeUrl = "/api/v1/members/login";
		var parameters = new Dictionary<string, string>
		{
			["member_ref_id"] = memberRefId,
			["account_id"] = accountId,
		};
		var logPrefix = nameof(MembersLoginAsync);

		var (statusCode, responseContent) = await PostAsync(
			apiBaseUrl,
			relativeUrl,
			privateKey,
			parameters,
			logPrefix,
			cancellationToken)
			.ConfigureAwait(false);

		return (statusCode, responseContent) switch
		{
			(HttpStatusCode.OK, string json) when JsonNode.Parse(json) is JsonNode node
				=> node["game_client_url"]!.GetValue<string>(),

			_ => throw new HttpRequestException(HttpRequestError.InvalidResponse, message: responseContent, statusCode: statusCode),
		};
	}
}
