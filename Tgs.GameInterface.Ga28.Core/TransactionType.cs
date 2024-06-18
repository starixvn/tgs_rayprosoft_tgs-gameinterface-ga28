namespace Tgs.GameInterface.Ga28.Core;

public enum TransactionType : byte
{
	/// <summary>
	/// 下注类 (Bet Placed)
	/// </summary>
	BetPlaced = 0,

	/// <summary>
	/// 注单为赢 (Bet Win)
	/// </summary>
	BetWin = 1,

	/// <summary>
	/// 注单为输 (Bet Lost)
	/// </summary>
	BetLost = 2,

	/// <summary>
	/// 注单为打和 (Bet Draw)
	/// </summary>
	BetDraw = 3,

	/// <summary>
	/// 注单取消 (Cancelled)
	/// </summary>
	Cancelled = 4,

	/// <summary>
	/// 注单退款 (Refunded)
	/// </summary>
	Refunded = 5,

	/// <summary>
	/// 注单回滚 (Rollback)
	/// </summary>
	Rollback = 6,

	/// <summary>
	/// 存款 (Deposit)
	/// </summary>
	Deposit = 7,

	/// <summary>
	/// 提款 (Withdraw)
	/// </summary>
	Withdraw = 8,

	/// <summary>
	/// 作废 (Void)
	/// </summary>
	Void = 9,

	/// <summary>
	/// 调整 (Adjustment)
	/// </summary>
	Adjustment = 10,

	/// <summary>
	/// 重新结算 (Reassessed)
	/// </summary>
	Reassessed = 11,

	/// <summary>
	/// 重新取消 (Re-cancelled)
	/// </summary>
	ReCancelled = 12,

	/// <summary>
	/// 奖池 (Jackpot)
	/// </summary>
	Jackpot = 13
}
