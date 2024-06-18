namespace Tgs.GameInterface.Ga28.Core.ApplicationServices;

public sealed record class GameRecordBetLogQuery(
	Uri ApiUrl,
	string PrivateKey,
	string MerchantId,
	DateTimeOffset StartTime,
	DateTimeOffset EndTime,
	int Page,
	int PageSize);
