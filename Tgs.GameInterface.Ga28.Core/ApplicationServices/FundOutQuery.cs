namespace Tgs.GameInterface.Ga28.Core.ApplicationServices;

public sealed record class FundOutQuery(
	Uri ApiUrl,
	string PrivateKey,
	string PlayerCode,
	string OrderId,
	decimal Amount);
