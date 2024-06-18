namespace Tgs.GameInterface.Ga28.Core.ApplicationServices;

public sealed record class FundStatusQuery(
	Uri ApiUrl,
	string PrivateKey,
	string PlayerCode,
	DateTimeOffset OrderOn,
	string OrderId);
