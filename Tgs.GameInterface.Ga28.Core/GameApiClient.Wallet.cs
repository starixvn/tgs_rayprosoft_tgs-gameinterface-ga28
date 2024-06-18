using System.Net;
using System.Text.Json.Nodes;

namespace Tgs.GameInterface.Ga28.Core;

internal sealed partial class GameApiClient
{
	public async ValueTask<bool> WalletAsync(Uri apiBaseUrl, string privateKey, int page, int pageSize, string memberRefId, string refId, DateTimeOffset fromCreatedTime, DateTimeOffset toCreatedTime, CancellationToken cancellationToken = default)
	{
		var relativeUrl = "/api/v1/wallet/";
		var parameters = new Dictionary<string, string>
		{
			["page"] = page.ToString(),
			["page_size"] = pageSize.ToString(),
			["member_ref_id"] = memberRefId,
			["ref_id"] = refId,
			["from_created_date_time"] = _dateTimeConverter.DatetimeConvertToString(fromCreatedTime),
			["to_created_date_time"] = _dateTimeConverter.DatetimeConvertToString(toCreatedTime),
		};
		var logPrefix = nameof(WalletAsync);

		var (statusCode, responseContent) = await GetWithParameterAsync(
			apiBaseUrl,
			relativeUrl,
			privateKey,
			parameters,
			logPrefix,
			cancellationToken)
			.ConfigureAwait(false);

		var found = (statusCode, responseContent) switch
		{
			(HttpStatusCode.OK, string json) when JsonNode.Parse(json) is JsonArray { Count: 1 } array
				=> array[0]?["type"]?.GetValue<int>() is 7 or 8,

			(HttpStatusCode.NotFound, _)
				=> false,

			_ => throw new HttpRequestException(HttpRequestError.InvalidResponse, statusCode: statusCode),
		};

		return found;
	}
}
