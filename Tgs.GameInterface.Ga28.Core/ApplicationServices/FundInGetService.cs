using Tgs.ServiceUtility;

namespace Tgs.GameInterface.Ga28.Core.ApplicationServices;

public sealed class FundInGetService(IGameApiClient apiClient) : IGetService<FundInQuery, FundResult>
{
	public async ValueTask<FundResult> GetAsync(FundInQuery query, CancellationToken cancellationToken = default)
	{
		var response = await apiClient.WalletWithdrawAsync(
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
