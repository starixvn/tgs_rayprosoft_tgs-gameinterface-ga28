using System.Reflection;
using Tgs.GameInterface.Ga28.Core;
using Tgs.GameInterface.Ga28.Core.Extensions;

namespace Microsoft.Extensions.DependencyInjection;

public static class ServiceCollectionExtensions
{
	public static IServiceCollection AddGameInterfaceCore(
		this IServiceCollection services)
		=> services
		.AddSingleton<DateTimeConverter>()
		.AddHttpClient<IGameApiClient, GameApiClient>().Services
		.RegisterAllServiceUtilityTypes(Assembly.GetExecutingAssembly(), ServiceLifetime.Transient)
		.AddSingleton(TimeProvider.System);
}
