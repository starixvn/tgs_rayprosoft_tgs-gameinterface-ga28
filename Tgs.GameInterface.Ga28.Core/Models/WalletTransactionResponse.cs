namespace Tgs.GameInterface.Ga28.Core.Models;

public sealed record class WalletTransactionResponse(
	string Id,
	string RefId,
	decimal Balance,
	string Resp);
