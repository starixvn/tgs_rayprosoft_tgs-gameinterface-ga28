namespace Tgs.GameInterface.Ga28.Core.ApplicationServices;

public sealed record class GameUrlQuery(
	Uri ApiUrl,
	string PrivateKey,
	string PartnerAccountId,
	string PlayerCode);
