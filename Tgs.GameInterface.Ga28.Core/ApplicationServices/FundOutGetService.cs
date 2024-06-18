using Tgs.ServiceUtility;

namespace Tgs.GameInterface.Ga28.Core.ApplicationServices;

public sealed class FundOutGetService(IGameApiClient apiClient) : IGetService<FundOutQuery, FundResult>
{
	public async ValueTask<FundResult> GetAsync(FundOutQuery query, CancellationToken cancellationToken = default)
	{
		var response = await apiClient.WalletDepositAsync(
			apiBaseUrl: query.ApiUrl,
			privateKey: query.PrivateKey,
			memberRefId: query.PlayerCode,
			amount: query.Amount,
			refId: query.OrderId,
			cancellationToken)
			.ConfigureAwait(false);

		return new FundResult(
			OrderId: response.RefId,
			ExternalOrderId: response.Id,
			response.Balance,
			response.Resp);
	}
}
