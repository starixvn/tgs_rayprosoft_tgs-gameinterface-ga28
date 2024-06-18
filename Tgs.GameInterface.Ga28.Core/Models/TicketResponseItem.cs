namespace Tgs.GameInterface.Ga28.Core.Models;

public sealed record class TicketResponseItem(
	string Sid,
	DateTimeOffset CreatedDateTime,
	DateTimeOffset SettledDateTime,
	DateTimeOffset ModifiedDateTime,
	string ArenaCode,
	string MatchDayCode,
	int FightNumber,
	decimal BetAmount,
	decimal ValidStake,
	decimal TicketAmount,
	decimal AnteAmount,
	decimal WinLossAmount,
	ApiBetStatus Status,
	string Result,
	string MemberRefId,
	string AdditionalData);
