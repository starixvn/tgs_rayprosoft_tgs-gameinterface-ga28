using Tgs.ServiceUtility;

namespace Tgs.GameInterface.Ga28.Core.ApplicationServices;

public sealed class PlayerBalanceGetService(IGameApiClient apiClient) : IGetService<PlayerBalanceQuery, PlayerBalanceResult>
{
	public async ValueTask<PlayerBalanceResult> GetAsync(PlayerBalanceQuery query, CancellationToken cancellationToken = default)
	{
		var response = await apiClient.WalletBalanceAsync(
			apiBaseUrl: query.ApiUrl,
			privateKey: query.PrivateKey,
			memberRefId: query.PlayerCode,
			cancellationToken)
			.ConfigureAwait(false);

		return new PlayerBalanceResult(
			IsPlayerExists: response.IsPlayerExists,
			Balance: response.Balance);
	}
}
