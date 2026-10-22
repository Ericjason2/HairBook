using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using HairBook.Services;

namespace HairBook.ViewModels;

public partial class MainViewModel : BaseViewModel
{
    private readonly Stack<string> _navigationStack = new();

    // Mode: "Client", "Pro", "Auth"
    [ObservableProperty]
    private string _currentMode = "Client";

    // Current displayed screen
    [ObservableProperty]
    private string _currentScreen = "MainClient";

    // Active bottom navigation tabs
    [ObservableProperty]
    private string _activeClientTab = "Home"; // Home, Explore, Appointments, Favorites, Profile

    [ObservableProperty]
    private string _activeProTab = "Dashboard"; // Dashboard, Planning, Services, Portfolio, Account

    // Toast Notification Banner
    [ObservableProperty]
    private string _toastMessage = string.Empty;

    [ObservableProperty]
    private bool _isToastVisible;

    // Sub ViewModels
    public HomeViewModel HomeVM { get; }
    public ExploreViewModel ExploreVM { get; }
    public StyleDetailViewModel StyleDetailVM { get; }
    public BarberProfileViewModel BarberProfileVM { get; }
    public BookingViewModel BookingVM { get; }
    public BookingSuccessViewModel BookingSuccessVM { get; }
    public AppointmentsViewModel AppointmentsVM { get; }
    public AppointmentDetailViewModel AppointmentDetailVM { get; }
    public FavoritesViewModel FavoritesVM { get; }
    public NotificationsViewModel NotificationsVM { get; }
    public ProfileViewModel ProfileVM { get; }
    public EditProfileViewModel EditProfileVM { get; }
    public AppStatesViewModel AppStatesVM { get; }

    public ProDashboardViewModel ProDashboardVM { get; }
    public ProPlanningViewModel ProPlanningVM { get; }
    public ProServicesViewModel ProServicesVM { get; }
    public ProPortfolioViewModel ProPortfolioVM { get; }
    public ProHoursViewModel ProHoursVM { get; }
    public ProProfileFormViewModel ProProfileFormVM { get; }
    public ProAccountViewModel ProAccountVM { get; }

    public LoginViewModel LoginVM { get; }
    public RegisterViewModel RegisterVM { get; }
    public OnboardingViewModel OnboardingVM { get; }

    public bool CanGoBack => _navigationStack.Count > 0;

    public MainViewModel(IDataService dataService) : base(dataService)
    {
        HomeVM = new HomeViewModel(dataService, this);
        ExploreVM = new ExploreViewModel(dataService, this);
        StyleDetailVM = new StyleDetailViewModel(dataService, this);
        BarberProfileVM = new BarberProfileViewModel(dataService, this);
        BookingVM = new BookingViewModel(dataService, this);
        BookingSuccessVM = new BookingSuccessViewModel(dataService, this);
        AppointmentsVM = new AppointmentsViewModel(dataService, this);
        AppointmentDetailVM = new AppointmentDetailViewModel(dataService, this);
        FavoritesVM = new FavoritesViewModel(dataService, this);
        NotificationsVM = new NotificationsViewModel(dataService, this);
        ProfileVM = new ProfileViewModel(dataService, this);
        EditProfileVM = new EditProfileViewModel(dataService, this);
        AppStatesVM = new AppStatesViewModel(dataService, this);

        ProDashboardVM = new ProDashboardViewModel(dataService, this);
        ProPlanningVM = new ProPlanningViewModel(dataService, this);
        ProServicesVM = new ProServicesViewModel(dataService, this);
        ProPortfolioVM = new ProPortfolioViewModel(dataService, this);
        ProHoursVM = new ProHoursViewModel(dataService, this);
        ProProfileFormVM = new ProProfileFormViewModel(dataService, this);
        ProAccountVM = new ProAccountViewModel(dataService, this);

        LoginVM = new LoginViewModel(dataService, this);
        RegisterVM = new RegisterViewModel(dataService, this);
        OnboardingVM = new OnboardingViewModel(dataService, this);

        // Auto transition from Splash to Onboarding after 2 seconds
        Task.Run(async () =>
        {
            await Task.Delay(2200);
            MainThread.BeginInvokeOnMainThread(() =>
            {
                if (CurrentScreen == "Splash")
                {
                    NavigateTo("Onboarding");
                }
            });
        });
    }

    public void NavigateTo(string screen)
    {
        if (CurrentScreen != screen)
        {
            _navigationStack.Push(CurrentScreen);
            CurrentScreen = screen;
            OnPropertyChanged(nameof(CanGoBack));
        }
    }

    [RelayCommand]
    public void GoBack()
    {
        if (_navigationStack.Count > 0)
        {
            CurrentScreen = _navigationStack.Pop();
            OnPropertyChanged(nameof(CanGoBack));

            // Refresh lists if returning to list pages
            if (CurrentScreen == "MainClient" && ActiveClientTab == "Favorites")
            {
                FavoritesVM.RefreshFavorites();
            }
            else if (CurrentScreen == "MainClient" && ActiveClientTab == "Appointments")
            {
                AppointmentsVM.RefreshList();
            }
        }
    }

    public void SwitchToClient()
    {
        _navigationStack.Clear();
        CurrentMode = "Client";
        ActiveClientTab = "Home";
        CurrentScreen = "MainClient";
        OnPropertyChanged(nameof(CanGoBack));
    }

    public void SwitchToPro()
    {
        _navigationStack.Clear();
        CurrentMode = "Pro";
        ActiveProTab = "Dashboard";
        CurrentScreen = "MainPro";
        OnPropertyChanged(nameof(CanGoBack));
    }

    public void SwitchToAuth()
    {
        _navigationStack.Clear();
        CurrentMode = "Auth";
        CurrentScreen = "Login";
        OnPropertyChanged(nameof(CanGoBack));
    }

    [RelayCommand]
    public void SetClientTab(string tab)
    {
        ActiveClientTab = tab;
        CurrentScreen = "MainClient";

        if (tab == "Favorites")
        {
            FavoritesVM.RefreshFavorites();
        }
        else if (tab == "Appointments")
        {
            AppointmentsVM.RefreshList();
        }
        else if (tab == "Explore")
        {
            ExploreVM.ApplyFilters();
        }
    }

    [RelayCommand]
    public void SetProTab(string tab)
    {
        ActiveProTab = tab;
        CurrentScreen = "MainPro";

        if (tab == "Portfolio")
        {
            ProPortfolioVM.RefreshList();
        }
    }

    public void ShowToast(string message)
    {
        ToastMessage = message;
        IsToastVisible = true;

        Task.Run(async () =>
        {
            await Task.Delay(2500);
            MainThread.BeginInvokeOnMainThread(() =>
            {
                IsToastVisible = false;
            });
        });
    }
}
