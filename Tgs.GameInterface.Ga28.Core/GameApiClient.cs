using System.Diagnostics;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;
using System.Web;
using Microsoft.Extensions.Logging;
using Tgs.GameInterface.Ga28.Core.Extensions;
using Result = (System.Net.HttpStatusCode statusCode, string? responseContent);

namespace Tgs.GameInterface.Ga28.Core;

internal sealed partial class GameApiClient(ILogger<GameApiClient> logger, HttpClient httpClient) : IGameApiClient
{
	private readonly DateTimeConverter _dateTimeConverter = new();

	private async ValueTask<Result> GetWithParameterAsync(Uri apiBaseUrl, string relativeUrl, string privateKey, IDictionary<string, string> parameters, string logPrefix, CancellationToken cancellationToken = default)
	{
		logPrefix = $"{nameof(GameApiClient).ToLower()}.{logPrefix}";

		var apiUrl = new Uri(apiBaseUrl, relativeUrl);

		if (parameters.Any())
		{
			var builder = new UriBuilder(apiUrl);
			var query = HttpUtility.ParseQueryString(builder.Query);
			foreach (var parameter in parameters)
				query[parameter.Key] = parameter.Value;
			builder.Query = query.ToString();
			apiUrl = builder.Uri;
		}

		var request = new HttpRequestMessage(HttpMethod.Get, apiUrl);
		request.Headers.Authorization = AuthenticationHeaderValue.Parse(privateKey);

		var sw = Stopwatch.StartNew();

		var response = await httpClient.SendAsync(
			request: request,
			cancellationToken: cancellationToken)
			.ConfigureAwait(false);

		sw.Stop();

		var responseContent = await response.Content.ReadAsStringAsync(cancellationToken).ConfigureAwait(false);

		var apiLogEvent = new
		{
			Path = request?.RequestUri?.AbsoluteUri.NoLongerThan(512) ?? string.Empty,
			Method = request?.Method.ToString().NoLongerThan(8) ?? string.Empty,
			Prefix = logPrefix,
			ServerName = Environment.MachineName.NoLongerThan(128),
			Parameters = JsonSerializer.Serialize(parameters),
			RequestHeaders = request?.Headers + request?.Content?.Headers?.ToString(),
			RequestContent = string.Empty,
			ResponseHeaders = response.Headers.ToString(),
			ResponseContent = responseContent.NoLongerThan(4096),
			ResponseCode = (short)response.StatusCode,
			ExecutionTimeMs = (int)sw.ElapsedMilliseconds,
			RecordedOn = DateTimeOffset.UtcNow
		};
		logger.LogInformation("{apiLogEvent}", JsonSerializer.Serialize(apiLogEvent));

		return (response.StatusCode, responseContent);
	}

	private async ValueTask<Result> GetAsync(Uri apiBaseUrl, string relativeUrl, string privateKey, string logPrefix, CancellationToken cancellationToken = default)
	{
		logPrefix = $"{nameof(GameApiClient).ToLower()}.{logPrefix}";

		var apiUrl = new Uri(apiBaseUrl, relativeUrl);

		var sw = Stopwatch.StartNew();

		var request = new HttpRequestMessage(HttpMethod.Get, apiUrl);
		request.Headers.Authorization = AuthenticationHeaderValue.Parse(privateKey);

		var response = await httpClient.SendAsync(
			request: request,
			cancellationToken: cancellationToken)
			.ConfigureAwait(false);

		sw.Stop();

		var responseContent = await response.Content.ReadAsStringAsync(cancellationToken).ConfigureAwait(false);

		var apiLogEvent = new
		{
			Path = request?.RequestUri?.AbsoluteUri.NoLongerThan(512) ?? string.Empty,
			Method = request?.Method.ToString().NoLongerThan(8) ?? string.Empty,
			Prefix = logPrefix,
			ServerName = Environment.MachineName.NoLongerThan(128),
			RequestHeaders = request?.Headers + request?.Content?.Headers?.ToString(),
			RequestContent = string.Empty,
			ResponseHeaders = response.Headers.ToString(),
			ResponseContent = responseContent.NoLongerThan(4096),
			ResponseCode = (short)response.StatusCode,
			ExecutionTimeMs = (int)sw.ElapsedMilliseconds,
			RecordedOn = DateTimeOffset.UtcNow
		};
		logger.LogInformation("{apiLogEvent}", JsonSerializer.Serialize(apiLogEvent));

		return (response.StatusCode, responseContent);
	}

	private async ValueTask<Result> PostAsync(Uri apiBaseUrl, string relativeUrl, string privateKey, IDictionary<string, string> parameters, string logPrefix, CancellationToken cancellationToken = default)
	{
		logPrefix = $"{nameof(GameApiClient).ToLower()}.{logPrefix}";

		var apiUrl = new Uri(apiBaseUrl, relativeUrl);

		var requestContent = JsonContent.Create(parameters);

		var request = new HttpRequestMessage(HttpMethod.Post, apiUrl)
		{
			Content = requestContent,
		};
		request.Headers.Authorization = AuthenticationHeaderValue.Parse(privateKey);

		var sw = Stopwatch.StartNew();

		var response = await httpClient.SendAsync(
			request: request,
			cancellationToken: cancellationToken)
			.ConfigureAwait(false);

		sw.Stop();

		var responseContent = await response.Content.ReadAsStringAsync(cancellationToken).ConfigureAwait(false);

		var apiLogEvent = new
		{
			Path = request?.RequestUri?.AbsoluteUri.NoLongerThan(512) ?? string.Empty,
			Method = request?.Method.ToString().NoLongerThan(8) ?? string.Empty,
			Prefix = logPrefix,
			ServerName = Environment.MachineName.NoLongerThan(128),
			Parameters = JsonSerializer.Serialize(parameters),
			RequestHeaders = request?.Headers + request?.Content?.Headers?.ToString(),
			RequestContent = (await requestContent.ReadAsStringAsync(cancellationToken).ConfigureAwait(false)).NoLongerThan(4096),
			ResponseHeaders = response.Headers.ToString(),
			ResponseContent = responseContent.NoLongerThan(4096),
			ResponseCode = (short)response.StatusCode,
			ExecutionTimeMs = (int)sw.ElapsedMilliseconds,
			RecordedOn = DateTimeOffset.UtcNow
		};
		logger.LogInformation("{apiLogEvent}", JsonSerializer.Serialize(apiLogEvent));

		return (response.StatusCode, responseContent);
	}
}
