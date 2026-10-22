namespace HairBook;

public partial class App : Application
{
    private readonly IServiceProvider _services;

    public App(IServiceProvider services)
    {
        try
        {
            InitializeComponent();
            _services = services;
        }
        catch (Exception ex)
        {
            File.WriteAllText(Path.Combine(AppContext.BaseDirectory, "crash_log.txt"), ex.ToString());
            throw;
        }
    }

    protected override Window CreateWindow(IActivationState? activationState)
    {
        try
        {
            var mainPage = _services.GetRequiredService<MainPage>();
            var window = new Window(mainPage)
            {
                Width = 430,
                Height = 880,
                Title = "HairBook - Mobile Preview"
            };
            return window;
        }
        catch (Exception ex)
        {
            File.WriteAllText(Path.Combine(AppContext.BaseDirectory, "crash_log.txt"), ex.ToString());
            throw;
        }
    }
}