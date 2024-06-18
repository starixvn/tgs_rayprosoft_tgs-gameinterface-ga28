namespace Tgs.GameInterface.Ga28.Core;

public enum ApiBetStatus : byte
{
	/// <summary>
	/// 注单确定 (Bet Confirmed)
	/// </summary>
	BetConfirmed = 0,

	/// <summary>
	/// 结算中 (Settling)
	/// </summary>
	Settling = 1,

	/// <summary>
	/// 结算了不过在等派彩 (Settled but Awaiting Payout)
	/// </summary>
	SettledAwaitingPayout = 2,

	/// <summary>
	/// 结算了不过派彩正在进行中 (Settled but Payout in Progress)
	/// </summary>
	SettledPayoutInProgress = 3,

	/// <summary>
	/// 注单结算了 (Bet Settled)
	/// </summary>
	BetSettled = 4,

	/// <summary>
	/// 取消不过在等派彩 (Cancelled but Awaiting Payout)
	/// </summary>
	CancelledAwaitingPayout = 5,

	/// <summary>
	/// 取消了不过派彩正在进行中 (Cancelled but Payout in Progress)
	/// </summary>
	CancelledPayoutInProgress = 6,

	/// <summary>
	/// 注单取消 (Bet Cancelled)
	/// </summary>
	BetCancelled = 7,

	/// <summary>
	/// 注单作废中 (Bet Voiding)
	/// </summary>
	BetVoiding = 8,

	/// <summary>
	/// 注单作废了 (Bet Voided)
	/// </summary>
	BetVoided = 9,

	/// <summary>
	/// 重新结算不过在等派彩 (Reassessed but Awaiting Payout)
	/// </summary>
	ReassessedAwaitingPayout = 10,

	/// <summary>
	/// 重新结算不过派彩正在进行中 (Reassessed but Payout in Progress)
	/// </summary>
	ReassessedPayoutInProgress = 11,

	/// <summary>
	/// 重新取消单，正在等派彩 (Re-cancelled, Awaiting Payout)
	/// </summary>
	RecancelledAwaitingPayout = 12,

	/// <summary>
	/// 重新取消单了，派彩正在处理中 (Re-cancelled, Payout in Progress)
	/// </summary>
	RecancelledPayoutInProgress = 13
}
