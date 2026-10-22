using Microsoft.Extensions.Logging;
using HairBook.Services;
using HairBook.ViewModels;

namespace HairBook;

public static class MauiProgram
{
	public static MauiApp CreateMauiApp()
	{
		AppDomain.CurrentDomain.UnhandledException += (s, e) =>
		{
			try { File.WriteAllText(Path.Combine(AppContext.BaseDirectory, "crash_log.txt"), e.ExceptionObject?.ToString()); } catch { }
		};

		var builder = MauiApp.CreateBuilder();
		builder
			.UseMauiApp<App>()
			.ConfigureFonts(fonts =>
			{
				fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
				fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
				fonts.AddFont("DMSerifDisplay-Regular.ttf", "DMSerifDisplay");
				fonts.AddFont("lucide.ttf", "Lucide");
			});

		// Core Services & ViewModels
		builder.Services.AddSingleton<IDataService, DataService>();
		builder.Services.AddSingleton<MainViewModel>();
		builder.Services.AddTransient<MainPage>();

#if DEBUG
		builder.Logging.AddDebug();
#endif

		return builder.Build();
	}
}
