using Tgs.ServiceUtility;

namespace Tgs.GameInterface.Ga28.Core.ApplicationServices;

public sealed class FundStatusGetService(IGameApiClient apiClient) : IGetService<FundStatusQuery, bool>
{
	public async ValueTask<bool> GetAsync(FundStatusQuery query, CancellationToken cancellationToken = default)
	{
		var begin = query.OrderOn.Add(TimeSpan.FromHours(-6));
		var end = query.OrderOn.Add(new(hours: 5, minutes: 59, seconds: 59));
		var page = 1;
		var pageSize = 32;

		var response = await apiClient.WalletAsync(
			apiBaseUrl: query.ApiUrl,
			privateKey: query.PrivateKey,
			page: page,
			pageSize: pageSize,
			memberRefId: query.PlayerCode,
			refId: query.OrderId,
			fromCreatedTime: begin,
			toCreatedTime: end,
			cancellationToken)
			.ConfigureAwait(false);

		return response;
	}
}
