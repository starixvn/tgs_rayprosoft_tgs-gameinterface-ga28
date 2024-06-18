namespace Tgs.GameInterface.Ga28.Core.ApplicationServices;

public sealed record class FundInQuery(
	Uri ApiUrl,
	string PrivateKey,
	string PlayerCode,
	string OrderId,
	decimal Amount);
