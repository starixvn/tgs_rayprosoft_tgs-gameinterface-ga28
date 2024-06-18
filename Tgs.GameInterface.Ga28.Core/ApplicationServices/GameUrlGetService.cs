using System.Web;
using Tgs.ServiceUtility;

namespace Tgs.GameInterface.Ga28.Core.ApplicationServices;

public sealed class GameUrlGetService(IGameApiClient apiClient) : IGetService<GameUrlQuery, string>
{
	public async ValueTask<string> GetAsync(GameUrlQuery query, CancellationToken cancellationToken = default)
	{
		var gameUrl = await apiClient.MembersLoginAsync(
			apiBaseUrl: query.ApiUrl,
			privateKey: query.PrivateKey,
			partnerAccountId: query.PartnerAccountId,
			memberRefId: query.PlayerCode,
			cancellationToken).
			ConfigureAwait(false);

		var path = new Uri(gameUrl);
		var builder = new UriBuilder(path);
		var parseQuery = HttpUtility.ParseQueryString(builder.Query);
		parseQuery["language"] = query.Language;
		builder.Query = parseQuery.ToString();

		return HttpUtility.UrlDecode(builder.ToString());
	}
}
