namespace Tgs.GameInterface.Ga28.Core.Models;

public sealed record class WalletBalanceResponse(
	bool IsPlayerExist,
	decimal Balance);
