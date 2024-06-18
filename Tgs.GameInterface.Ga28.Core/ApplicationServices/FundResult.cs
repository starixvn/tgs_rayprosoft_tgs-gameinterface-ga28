namespace Tgs.GameInterface.Ga28.Core.ApplicationServices;

public sealed record class FundResult(
	string OrderId,
	string ExternalOrderId,
	decimal Balance,
	string Resp);
