using Tgs.GameInterface.Ga28.Protos;

namespace Tgs.GameInterface.Ga28.Client;

public interface IGameInterfaceGa28Client
{
	ValueTask<string> CheckingServiceAsync(string machineName, CancellationToken cancellationToken = default);

	ValueTask<GameUrlReply> GameUrlAsync(Uri apiUrl, string privateKey, string partnerAccountId, string playerCode, CancellationToken cancellationToken = default);

	ValueTask<PlayerBalanceReply> PlayerBalanceAsync(Uri apiUrl, string privateKey, string playerCode, CancellationToken cancellationToken = default);

	ValueTask<FundReply> FundInAsync(Uri apiUrl, string privateKey, string playerCode, string orderId, decimal amount, CancellationToken cancellationToken = default);

	ValueTask<FundReply> FundOutAsync(Uri apiUrl, string privateKey, string playerCode, string orderId, decimal amount, CancellationToken cancellationToken = default);

	ValueTask<bool> FundStatusAsync(Uri apiUrl, string privateKey, string playerCode, DateTimeOffset orderOn, string orderId, CancellationToken cancellationToken = default);

	IAsyncEnumerable<GameRecordInfo> GameRecordAsync(Uri apiUrl, string privateKey, string partnerAccountId, DateTimeOffset startTime, DateTimeOffset endTime, int page = 1, int pageSize = 32, CancellationToken cancellationToken = default);
}
