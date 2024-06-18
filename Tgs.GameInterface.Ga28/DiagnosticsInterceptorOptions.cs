using System.Diagnostics;

namespace Tgs.GameInterface.Ga28;

public class DiagnosticsInterceptorOptions
{
	public ActivitySource InterceptorActivitySource { get; set; } =
		new ActivitySource(typeof(DiagnosticsInterceptorOptions).FullName!);
}
