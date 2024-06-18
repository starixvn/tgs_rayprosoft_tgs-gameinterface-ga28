using Microsoft.AspNetCore.Mvc;
using Tgs.GameInterface.Ga28.Core.ApplicationServices;
using Tgs.ServiceUtility;

namespace Tgs.GameInterface.Ga28.Controllers;

[Route("[controller]/[action]")]
[ApiController]
public sealed class Ga28Controller : ControllerBase
{
	/// <remarks>
	/// <b>Example</b>
	///
	/// Post
	/// {
	///     "apiUrl": "http://partner-api.dev.ga28.cc",
	///     "privateKey": "8456c9de-d589-4e8a-876f-c9c8e48dda3e",
	///     "partnerAccountId": "99a6f8ef-38a6-4f31-b69a-e222c2a54771",
	///     "playerCode": "4ucmqplxb029",
	///     "language": "en"
	/// }
	/// </remarks>
	[HttpPost]
	public ValueTask<string> GameUrl(
		[FromServices] IGetService<GameUrlQuery, string> service,
		[FromBody] GameUrlQuery query)
		=> service.GetAsync(query);

	/// <remarks>
	/// <b>Example</b>
	///
	/// Post
	/// {
	///     "apiUrl": "http://partner-api.dev.ga28.cc",
	///     "privateKey": "8456c9de-d589-4e8a-876f-c9c8e48dda3e",
	///     "playerCode": "4ucmqplxb029"
	/// }
	/// </remarks>
	[HttpPost]
	public ValueTask<PlayerBalanceResult> PlayerBalance(
		[FromServices] IGetService<PlayerBalanceQuery, PlayerBalanceResult> service,
		[FromBody] PlayerBalanceQuery query)
		=> service.GetAsync(query);

	/// <remarks>
	/// <b>Example</b>
	///
	/// Post
	/// {
	///     "apiUrl": "http://partner-api.dev.ga28.cc",
	///     "privateKey": "8456c9de-d589-4e8a-876f-c9c8e48dda3e",
	///     "playerCode": "4ucmqplxb029",
	///     "orderId": "XXXXX",
	///     "amount": 10
	/// }
	/// </remarks>
	[HttpPost]
	public ValueTask<FundResult> FundIn(
		[FromServices] IGetService<FundInQuery, FundResult> service,
		[FromBody] FundInQuery query)
		=> service.GetAsync(query);

	/// <remarks>
	/// <b>Example</b>
	///
	/// Post
	/// {
	///     "apiUrl": "http://partner-api.dev.ga28.cc",
	///     "privateKey": "8456c9de-d589-4e8a-876f-c9c8e48dda3e",
	///     "playerCode": "4ucmqplxb029",
	///     "orderId": "XXXXX",
	///     "amount": 10
	/// }
	/// </remarks>
	[HttpPost]
	public ValueTask<FundResult> FundOut(
		[FromServices] IGetService<FundOutQuery, FundResult> service,
		[FromBody] FundOutQuery query)
		=> service.GetAsync(query);

	/// <remarks>
	/// <b>Example</b>
	///
	/// Post
	/// {
	///     "apiUrl": "http://partner-api.dev.ga28.cc",
	///     "privateKey": "8456c9de-d589-4e8a-876f-c9c8e48dda3e",
	///     "playerCode": "4ucmqplxb029",
	///     "orderOn": "2024-06-03T17:00:00+08:00",
	///     "orderId": "XXXXX"
	/// }
	/// </remarks>
	[HttpPost]
	public ValueTask<bool> FundStatus(
		[FromServices] IGetService<FundStatusQuery, bool> service,
		[FromBody] FundStatusQuery query)
		=> service.GetAsync(query);

	/// <remarks>
	/// <b>Example</b>
	///
	/// Post
	/// {
	///     "apiUrl": "http://partner-api.dev.ga28.cc",
	///     "privateKey": "8456c9de-d589-4e8a-876f-c9c8e48dda3e",
	///     "partnerAccountId": "99a6f8ef-38a6-4f31-b69a-e222c2a54771",
	///     "startTime": "2024-06-03T17:00:00+08:00",
	///     "endTime": "2024-06-03T18:00:00+08:00",
	///     "page": 1,
	///     "pageSize": 32
	/// }
	/// </remarks>
	[HttpPost]
	public ValueTask<GameRecordBetLogResult[]> GameRecord(
		[FromServices] IQueryService<GameRecordBetLogQuery, GameRecordBetLogResult> service,
		[FromBody] GameRecordBetLogQuery query)
		=> service.QueryAsync(query).ToArrayAsync();
}
