namespace Tgs.GameInterface.Ga28.Core.ApplicationServices;

public sealed record class PlayerBalanceQuery(
	Uri ApiUrl,
	string PrivateKey,
	string PlayerCode);
