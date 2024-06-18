namespace Tgs.GameInterface.Ga28.Core.ApplicationServices;

public sealed record class GameRecordBetLogResult(
	string PlayerCode,
	string ExternalBetId,
	string ExternalRoundId,
	Protos.RecordResult Result,
	DateTimeOffset ExternalCreatedOn,
	DateTimeOffset ExternalClosedOn,
	decimal Bet,
	decimal ValidBet,
	decimal Settle,
	decimal WinLoss,
	string AdditionalData);
