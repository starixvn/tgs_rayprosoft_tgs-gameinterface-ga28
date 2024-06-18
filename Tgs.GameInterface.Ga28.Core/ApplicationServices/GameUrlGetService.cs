using Tgs.ServiceUtility;

namespace Tgs.GameInterface.Ga28.Core.ApplicationServices;

public sealed class GameUrlGetService(IGameApiClient apiClient) : IGetService<GameUrlQuery, string>
{
	public async ValueTask<string> GetAsync(GameUrlQuery query, CancellationToken cancellationToken = default)
	{
		var response = await apiClient.MembersLoginAsync(
			apiBaseUrl: query.ApiUrl,
			privateKey: query.PrivateKey,
			memberRefId: query.PlayerCode,
			accountId: query.MerchantId,
			cancellationToken).
			ConfigureAwait(false);

		return response;
	}
}
