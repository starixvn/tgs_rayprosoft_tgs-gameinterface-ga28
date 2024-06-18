using System.Runtime.CompilerServices;
using Google.Protobuf.WellKnownTypes;
using Tgs.GameInterface.Ga28.Protos;

namespace Tgs.GameInterface.Ga28.Client;

internal sealed class GameInterfaceGa28Client(
	GameInterfaceGa28.GameInterfaceGa28Client client) : IGameInterfaceGa28Client
{
	public async ValueTask<string> CheckingServiceAsync(string machineName, CancellationToken cancellationToken = default)
	{
		var request = new CheckingServiceRequest
		{
			MachineName = machineName,
		};

		var reply = await client.CheckingServiceAsync(
				request: request,
				cancellationToken: cancellationToken)
			.ConfigureAwait(false);

		return reply.MachineName;
	}

	public async ValueTask<GameUrlReply> GameUrlAsync(Uri apiUrl, string privateKey, string partnerAccountId, string playerCode, string language, CancellationToken cancellationToken = default)
	{
		var request = new GameUrlRequest
		{
			ApiUrl = apiUrl.ToString(),
			PrivateKey = privateKey,
			PartnerAccountId = partnerAccountId,
			PlayerCode = playerCode,
			Language = language,
		};

		return await client.GameUrlAsync(
			request: request,
			cancellationToken: cancellationToken)
			.ConfigureAwait(false);
	}

	public async ValueTask<PlayerBalanceReply> PlayerBalanceAsync(Uri apiUrl, string privateKey, string playerCode, CancellationToken cancellationToken = default)
	{
		var request = new PlayerBalanceRequest
		{
			ApiUrl = apiUrl.ToString(),
			PrivateKey = privateKey,
			PlayerCode = playerCode,
		};

		return await client.PlayerBalanceAsync(
			request: request,
			cancellationToken: cancellationToken)
			.ConfigureAwait(false);
	}

	public async ValueTask<FundReply> FundInAsync(Uri apiUrl, string privateKey, string playerCode, string orderId, decimal amount, CancellationToken cancellationToken = default)
	{
		var request = new FundRequest
		{
			ApiUrl = apiUrl.ToString(),
			PrivateKey = privateKey,
			PlayerCode = playerCode,
			OrderId = orderId,
			Amount = amount,
		};
		return await client.FundInAsync(
			request: request,
			cancellationToken: cancellationToken)
			.ConfigureAwait(false);
	}

	public async ValueTask<FundReply> FundOutAsync(Uri apiUrl, string privateKey, string playerCode, string orderId, decimal amount, CancellationToken cancellationToken = default)
	{
		var request = new FundRequest
		{
			ApiUrl = apiUrl.ToString(),
			PrivateKey = privateKey,
			PlayerCode = playerCode,
			OrderId = orderId,
			Amount = amount,
		};
		return await client.FundOutAsync(
			request: request,
			cancellationToken: cancellationToken)
			.ConfigureAwait(false);
	}

	public async ValueTask<bool> FundStatusAsync(Uri apiUrl, string privateKey, string playerCode, DateTimeOffset orderOn, string orderId, CancellationToken cancellationToken = default)
	{
		var request = new FundStatusRequest
		{
			ApiUrl = apiUrl.ToString(),
			PrivateKey = privateKey,
			PlayerCode = playerCode,
			OrderOn = orderOn.ToTimestamp(),
			OrderId = orderId,
		};
		var reply = await client.FundStatusAsync(
			request: request,
			cancellationToken: cancellationToken)
			.ConfigureAwait(false);

		return reply.Status;
	}

	public async IAsyncEnumerable<GameRecordInfo> GameRecordAsync(Uri apiUrl, string privateKey, string partnerAccountId, DateTimeOffset startTime, DateTimeOffset endTime, int page = 1, int pageSize = 32, [EnumeratorCancellation] CancellationToken cancellationToken = default)
	{
		var request = new GameRecordRequest
		{
			ApiUrl = apiUrl.ToString(),
			PrivateKey = privateKey,
			PartnerAccountId = partnerAccountId,
			StartTime = startTime.ToTimestamp(),
			EndTime = endTime.ToTimestamp(),
			Page = page,
			PageSize = pageSize,
		};

		var call = client.GameRecord(
			request: request,
			cancellationToken: cancellationToken);

		while (await call.ResponseStream.MoveNext(cancellationToken).ConfigureAwait(false))
			yield return call.ResponseStream.Current;
	}
}
