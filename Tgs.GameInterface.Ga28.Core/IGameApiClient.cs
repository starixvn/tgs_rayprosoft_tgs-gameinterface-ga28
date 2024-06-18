using System.Security.Principal;
using Tgs.GameInterface.Ga28.Core.Models;

namespace Tgs.GameInterface.Ga28.Core;

public interface IGameApiClient
{
	ValueTask<string> MembersLoginAsync(Uri apiBaseUrl, string privateKey, string partnerAccountId, string memberRefId, CancellationToken cancellationToken = default);

	IAsyncEnumerable<TicketResponseItem> TicketsAsync(Uri apiBaseUrl, string privateKey, string partnerAccountId, int page, int pageSize, DateTimeOffset fromModifiedTime, DateTimeOffset toModifiedTime, CancellationToken cancellationToken = default);

	ValueTask<WalletBalanceResponse> WalletBalanceAsync(Uri apiBaseUrl, string privateKey, string memberRefId, CancellationToken cancellationToken = default);

	ValueTask<WalletTransactionResponse> WalletDepositAsync(Uri apiBaseUrl, string privateKey, string memberRefId, decimal amount, string refId, CancellationToken cancellationToken = default);

	ValueTask<WalletTransactionResponse> WalletWithdrawAsync(Uri apiBaseUrl, string privateKey, string memberRefId, decimal amount, string refId, CancellationToken cancellationToken = default);

	ValueTask<bool> WalletAsync(Uri apiBaseUrl, string privateKey, int page, int pageSize, string memberRefId, string refId, DateTimeOffset fromCreatedTime, DateTimeOffset toCreatedTime, CancellationToken cancellationToken = default);
}
