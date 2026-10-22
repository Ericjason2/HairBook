using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using HairBook.Models;
using HairBook.Services;
using System.Collections.ObjectModel;

namespace HairBook.ViewModels;

public partial class HomeViewModel : BaseViewModel
{
    private readonly MainViewModel _main;

    public UserProfile CurrentUser => DataService.CurrentUser;
    public ObservableCollection<HairStyle> PopularStyles => DataService.HairStyles;
    public ObservableCollection<Barber> PopularBarbers => DataService.Barbers;
    public List<StyleCategoryItem> Categories => DataService.Categories;

    public Appointment? NextAppointment => DataService.Appointments.FirstOrDefault(a => a.Status == "confirmed");
    public int UnreadNotificationsCount => DataService.Notifications.Count(n => !n.Read);

    public HomeViewModel(IDataService dataService, MainViewModel main) : base(dataService)
    {
        _main = main;
    }

    [RelayCommand]
    private void OpenNotifications()
    {
        _main.NavigateTo("Notifications");
    }

    [RelayCommand]
    private void OpenExplore(string? category = null)
    {
        _main.ActiveClientTab = "Explore";
        if (!string.IsNullOrEmpty(category))
        {
            _main.ExploreVM.SelectCategory(category);
        }
    }

    [RelayCommand]
    private void SelectStyle(HairStyle style)
    {
        _main.StyleDetailVM.LoadStyle(style.Id);
        _main.NavigateTo("StyleDetail");
    }

    [RelayCommand]
    private void SelectBarber(Barber barber)
    {
        _main.BarberProfileVM.LoadBarber(barber.Id);
        _main.NavigateTo("BarberProfile");
    }

    [RelayCommand]
    private void OpenNextAppointment()
    {
        if (NextAppointment != null)
        {
            _main.AppointmentDetailVM.LoadAppointment(NextAppointment.Id);
            _main.NavigateTo("AppointmentDetail");
        }
    }

    [RelayCommand]
    private void ToggleFavorite(HairStyle style)
    {
        DataService.ToggleFavorite(style.Id);
    }
}

public partial class ExploreViewModel : BaseViewModel
{
    private readonly MainViewModel _main;

    [ObservableProperty]
    private string _searchQuery = string.Empty;

    [ObservableProperty]
    private string _selectedFilter = "Tous"; // Tous, Coupes, Barbe, Coiffures, Barbers

    [ObservableProperty]
    private int? _maxPriceFilter = null;

    [ObservableProperty]
    private string _sortBy = "popular"; // popular, price

    [ObservableProperty]
    private bool _isFilterSheetOpen;

    public ObservableCollection<HairStyle> FilteredStyles { get; } = new();
    public ObservableCollection<Barber> FilteredBarbers { get; } = new();

    public bool ShowBarbers => SelectedFilter == "Barbers";
    public int ResultCount => ShowBarbers ? FilteredBarbers.Count : FilteredStyles.Count;

    public ExploreViewModel(IDataService dataService, MainViewModel main) : base(dataService)
    {
        _main = main;
        ApplyFilters();
    }

    partial void OnSearchQueryChanged(string value) => ApplyFilters();
    partial void OnSelectedFilterChanged(string value)
    {
        OnPropertyChanged(nameof(ShowBarbers));
        ApplyFilters();
    }
    partial void OnMaxPriceFilterChanged(int? value) => ApplyFilters();
    partial void OnSortByChanged(string value) => ApplyFilters();

    public void SelectCategory(string cat)
    {
        SearchQuery = cat;
        SelectedFilter = "Tous";
    }

    [RelayCommand]
    public void SetFilter(string filter)
    {
        SelectedFilter = filter;
    }

    [RelayCommand]
    public void ClearFilters()
    {
        SearchQuery = string.Empty;
        SelectedFilter = "Tous";
        MaxPriceFilter = null;
        SortBy = "popular";
        IsFilterSheetOpen = false;
        ApplyFilters();
    }

    [RelayCommand]
    private void OpenFilterSheet()
    {
        IsFilterSheetOpen = true;
    }

    [RelayCommand]
    private void CloseFilterSheet()
    {
        IsFilterSheetOpen = false;
    }

    [RelayCommand]
    private void SetBudget(object? budgetParam)
    {
        if (budgetParam == null)
        {
            MaxPriceFilter = null;
        }
        else if (int.TryParse(budgetParam.ToString(), out int val))
        {
            MaxPriceFilter = val;
        }
        else
        {
            MaxPriceFilter = null;
        }
        ApplyFilters();
    }

    [RelayCommand]
    private void SetSort(string sort)
    {
        SortBy = sort;
        ApplyFilters();
    }

    [RelayCommand]
    private void SelectStyle(HairStyle style)
    {
        _main.StyleDetailVM.LoadStyle(style.Id);
        _main.NavigateTo("StyleDetail");
    }

    [RelayCommand]
    private void SelectBarber(Barber barber)
    {
        _main.BarberProfileVM.LoadBarber(barber.Id);
        _main.NavigateTo("BarberProfile");
    }

    [RelayCommand]
    private void ToggleFavorite(HairStyle style)
    {
        DataService.ToggleFavorite(style.Id);
    }

    public void ApplyFilters()
    {
        var q = SearchQuery.Trim().ToLowerInvariant();

        // 1. Styles
        var styles = DataService.HairStyles.Where(s =>
        {
            var barber = DataService.GetBarber(s.BarberId);
            bool matchQuery = string.IsNullOrEmpty(q) ||
                              s.Name.ToLowerInvariant().Contains(q) ||
                              s.Category.ToLowerInvariant().Contains(q) ||
                              s.Group.ToLowerInvariant().Contains(q) ||
                              (barber?.Name.ToLowerInvariant().Contains(q) ?? false);

            bool matchGroup = SelectedFilter == "Tous" || s.Group == SelectedFilter;
            bool matchPrice = !MaxPriceFilter.HasValue || s.Price <= MaxPriceFilter.Value;

            return matchQuery && matchGroup && matchPrice;
        });

        if (SortBy == "price")
        {
            styles = styles.OrderBy(s => s.Price);
        }

        FilteredStyles.Clear();
        foreach (var s in styles) FilteredStyles.Add(s);

        // 2. Barbers
        var barbers = DataService.Barbers.Where(b =>
            string.IsNullOrEmpty(q) ||
            b.Name.ToLowerInvariant().Contains(q) ||
            b.Specialty.ToLowerInvariant().Contains(q) ||
            b.Location.ToLowerInvariant().Contains(q));

        FilteredBarbers.Clear();
        foreach (var b in barbers) FilteredBarbers.Add(b);

        OnPropertyChanged(nameof(ResultCount));
    }
}

public partial class StyleDetailViewModel : BaseViewModel
{
    private readonly MainViewModel _main;

    [ObservableProperty]
    private HairStyle? _style;

    [ObservableProperty]
    private Barber? _barber;

    public ObservableCollection<HairStyle> MoreStylesByBarber { get; } = new();

    public StyleDetailViewModel(IDataService dataService, MainViewModel main) : base(dataService)
    {
        _main = main;
    }

    public void LoadStyle(string styleId)
    {
        Style = DataService.GetStyle(styleId) ?? DataService.HairStyles[0];
        Barber = DataService.GetBarber(Style.BarberId) ?? DataService.Barbers[0];

        MoreStylesByBarber.Clear();
        foreach (var s in DataService.HairStyles.Where(s => s.BarberId == Barber.Id && s.Id != Style.Id).Take(2))
        {
            MoreStylesByBarber.Add(s);
        }
    }

    [RelayCommand]
    private void GoBack() => _main.GoBack();

    [RelayCommand]
    private void ToggleFavorite()
    {
        if (Style != null)
        {
            DataService.ToggleFavorite(Style.Id);
            OnPropertyChanged(nameof(Style));
        }
    }

    [RelayCommand]
    private void ViewBarber()
    {
        if (Barber != null)
        {
            _main.BarberProfileVM.LoadBarber(Barber.Id);
            _main.NavigateTo("BarberProfile");
        }
    }

    [RelayCommand]
    private void StartBooking()
    {
        if (Barber != null && Style != null)
        {
            _main.BookingVM.InitializeBooking(Barber.Id, Style.ServiceId);
            _main.NavigateTo("Booking");
        }
    }
}

public partial class BarberProfileViewModel : BaseViewModel
{
    private readonly MainViewModel _main;

    [ObservableProperty]
    private Barber? _barber;

    [ObservableProperty]
    private string _activeTab = "services"; // services, portfolio, reviews

    public ObservableCollection<Service> BarberServices { get; } = new();
    public ObservableCollection<Review> BarberReviews { get; } = new();
    public ObservableCollection<PortfolioItem> BarberPortfolio => DataService.PortfolioItems;

    public BarberProfileViewModel(IDataService dataService, MainViewModel main) : base(dataService)
    {
        _main = main;
    }

    public void LoadBarber(string barberId)
    {
        Barber = DataService.GetBarber(barberId) ?? DataService.Barbers[0];
        ActiveTab = "services";

        BarberServices.Clear();
        foreach (var s in DataService.Services.Where(s => s.BarberId == Barber.Id && s.Active))
        {
            BarberServices.Add(s);
        }

        BarberReviews.Clear();
        foreach (var r in DataService.Reviews.Where(r => r.BarberId == Barber.Id))
        {
            BarberReviews.Add(r);
        }
    }

    [RelayCommand]
    private void SetTab(string tab) => ActiveTab = tab;

    [RelayCommand]
    private void GoBack() => _main.GoBack();

    [RelayCommand]
    private void ToggleFavorite()
    {
        if (Barber != null)
        {
            DataService.ToggleFavorite(Barber.Id);
            OnPropertyChanged(nameof(Barber));
        }
    }

    [RelayCommand]
    private void BookService(Service service)
    {
        if (Barber != null)
        {
            _main.BookingVM.InitializeBooking(Barber.Id, service.Id);
            _main.NavigateTo("Booking");
        }
    }

    [RelayCommand]
    private void BookAny()
    {
        if (Barber != null)
        {
            _main.BookingVM.InitializeBooking(Barber.Id, null);
            _main.NavigateTo("Booking");
        }
    }
}

public partial class CalendarDayItem : ObservableObject
{
    public int Day { get; set; }
    public bool Available { get; set; }
    public bool IsSelected { get; set; }
    public string Reason { get; set; } = string.Empty; // past, closed, full
}

public partial class BookingViewModel : BaseViewModel
{
    private readonly MainViewModel _main;

    [ObservableProperty]
    private Barber? _barber;

    [ObservableProperty]
    private Service? _selectedService;

    [ObservableProperty]
    private int? _selectedDay;

    [ObservableProperty]
    private string? _selectedTime;

    [ObservableProperty]
    private int _step; // 0: Service, 1: Date, 2: Time, 3: Confirmation

    [ObservableProperty]
    private bool _isSubmitting;

    public ObservableCollection<Service> BarberServices { get; } = new();
    public ObservableCollection<CalendarDayItem> CalendarDays { get; } = new();
    public List<string> MorningSlots { get; } = new() { "09:00", "09:30", "10:00", "10:30", "11:00" };
    public List<string> AfternoonSlots { get; } = new() { "14:00", "14:30", "15:00", "16:00" };
    public List<string> UnavailableSlots { get; } = new() { "10:00", "11:00", "14:30" };

    public string StepTitle => Step switch
    {
        0 => "Choisissez la prestation",
        1 => "Choisissez la date",
        2 => "Choisissez l’heure",
        3 => "Vérifiez votre rendez-vous",
        _ => "Réservation"
    };

    public string SelectedDateLabel => SelectedDay.HasValue ? $"Samedi {SelectedDay} octobre" : string.Empty;

    public bool CanContinue => Step switch
    {
        0 => SelectedService != null,
        1 => SelectedDay.HasValue,
        2 => !string.IsNullOrEmpty(SelectedTime),
        3 => true,
        _ => false
    };

    public BookingViewModel(IDataService dataService, MainViewModel main) : base(dataService)
    {
        _main = main;
    }

    public void InitializeBooking(string barberId, string? serviceId)
    {
        Barber = DataService.GetBarber(barberId) ?? DataService.Barbers[0];

        BarberServices.Clear();
        foreach (var s in DataService.Services.Where(s => s.BarberId == Barber.Id && s.Active))
        {
            BarberServices.Add(s);
        }

        SelectedService = BarberServices.FirstOrDefault(s => s.Id == serviceId) ?? BarberServices.FirstOrDefault();
        SelectedDay = 12; // Default to Saturday 12th as in mockup
        SelectedTime = null;
        Step = SelectedService != null ? (serviceId != null ? 1 : 0) : 0;

        BuildCalendar();
    }

    private void BuildCalendar()
    {
        CalendarDays.Clear();
        // October 31 days. Tuesday 1st (Monday=0, Tuesday=1).
        for (int d = 1; d <= 31; d++)
        {
            int weekday = (1 + (d - 1)) % 7; // Sunday = 6
            bool isPast = d < 7;
            bool isClosed = weekday == 6; // Sunday closed
            bool isFull = d == 9 || d == 15 || d == 23;

            bool available = !isPast && !isClosed && !isFull;
            string reason = isPast ? "past" : isClosed ? "closed" : isFull ? "full" : "";

            CalendarDays.Add(new CalendarDayItem
            {
                Day = d,
                Available = available,
                Reason = reason,
                IsSelected = d == SelectedDay
            });
        }
    }

    [RelayCommand]
    private void SelectService(Service service)
    {
        SelectedService = service;
        OnPropertyChanged(nameof(CanContinue));
    }

    [RelayCommand]
    private void SelectDay(CalendarDayItem dayItem)
    {
        if (!dayItem.Available) return;
        SelectedDay = dayItem.Day;
        SelectedTime = null;

        foreach (var d in CalendarDays)
        {
            d.IsSelected = d.Day == SelectedDay;
        }

        OnPropertyChanged(nameof(SelectedDateLabel));
        OnPropertyChanged(nameof(CanContinue));
    }

    [RelayCommand]
    private void SelectTime(string time)
    {
        if (UnavailableSlots.Contains(time)) return;
        SelectedTime = time;
        OnPropertyChanged(nameof(CanContinue));
    }

    [RelayCommand]
    private void NextStep()
    {
        if (Step < 3)
        {
            Step++;
            OnPropertyChanged(nameof(StepTitle));
            OnPropertyChanged(nameof(CanContinue));
        }
    }

    [RelayCommand]
    private void PreviousStep()
    {
        if (Step > 0)
        {
            Step--;
            OnPropertyChanged(nameof(StepTitle));
            OnPropertyChanged(nameof(CanContinue));
        }
        else
        {
            _main.GoBack();
        }
    }

    [RelayCommand]
    private async Task ConfirmBooking()
    {
        if (Barber == null || SelectedService == null || !SelectedDay.HasValue || string.IsNullOrEmpty(SelectedTime))
            return;

        IsSubmitting = true;
        await Task.Delay(800); // Simulate network latency

        var appt = DataService.CreateAppointment(Barber.Id, SelectedService.Id, SelectedDateLabel, SelectedTime);
        IsSubmitting = false;

        _main.BookingSuccessVM.LoadAppointment(appt.Id);
        _main.NavigateTo("BookingSuccess");
    }
}

public partial class BookingSuccessViewModel : BaseViewModel
{
    private readonly MainViewModel _main;

    [ObservableProperty]
    private Appointment? _appointment;

    public BookingSuccessViewModel(IDataService dataService, MainViewModel main) : base(dataService)
    {
        _main = main;
    }

    public void LoadAppointment(string id)
    {
        Appointment = DataService.GetAppointment(id) ?? DataService.Appointments[0];
    }

    [RelayCommand]
    private void ViewAppointment()
    {
        if (Appointment != null)
        {
            _main.AppointmentDetailVM.LoadAppointment(Appointment.Id);
            _main.NavigateTo("AppointmentDetail");
        }
    }

    [RelayCommand]
    private void GoHome()
    {
        _main.SwitchToClient();
    }
}

public partial class AppointmentsViewModel : BaseViewModel
{
    private readonly MainViewModel _main;

    [ObservableProperty]
    private string _activeTab = "upcoming"; // upcoming, pending, past

    public ObservableCollection<Appointment> FilteredAppointments { get; } = new();

    public AppointmentsViewModel(IDataService dataService, MainViewModel main) : base(dataService)
    {
        _main = main;
        RefreshList();
    }

    partial void OnActiveTabChanged(string value) => RefreshList();

    public void RefreshList()
    {
        FilteredAppointments.Clear();
        var list = ActiveTab switch
        {
            "upcoming" => DataService.Appointments.Where(a => a.Status == "confirmed"),
            "pending" => DataService.Appointments.Where(a => a.Status == "pending"),
            _ => DataService.Appointments.Where(a => a.Status == "completed" || a.Status == "cancelled")
        };

        foreach (var a in list) FilteredAppointments.Add(a);
    }

    [RelayCommand]
    private void SetTab(string tab) => ActiveTab = tab;

    [RelayCommand]
    private void SelectAppointment(Appointment appt)
    {
        _main.AppointmentDetailVM.LoadAppointment(appt.Id);
        _main.NavigateTo("AppointmentDetail");
    }

    [RelayCommand]
    private void ExploreBarbers()
    {
        _main.ActiveClientTab = "Explore";
        _main.ExploreVM.SelectedFilter = "Barbers";
    }
}

public partial class AppointmentDetailViewModel : BaseViewModel
{
    private readonly MainViewModel _main;

    [ObservableProperty]
    private Appointment? _appointment;

    [ObservableProperty]
    private bool _isCancelModalOpen;

    [ObservableProperty]
    private bool _isContactSheetOpen;

    [ObservableProperty]
    private bool _isReviewSheetOpen;

    [ObservableProperty]
    private int _reviewRating = 5;

    [ObservableProperty]
    private string _reviewComment = string.Empty;

    [ObservableProperty]
    private bool _isReviewed;

    public AppointmentDetailViewModel(IDataService dataService, MainViewModel main) : base(dataService)
    {
        _main = main;
    }

    public void LoadAppointment(string id)
    {
        Appointment = DataService.GetAppointment(id) ?? DataService.Appointments[0];
        IsCancelModalOpen = false;
        IsContactSheetOpen = false;
        IsReviewSheetOpen = false;
        IsReviewed = false;
        ReviewRating = 5;
        ReviewComment = string.Empty;
    }

    [RelayCommand]
    private void GoBack() => _main.GoBack();

    [RelayCommand]
    private void ModifyAppointment()
    {
        if (Appointment != null)
        {
            _main.BookingVM.InitializeBooking(Appointment.BarberId, Appointment.ServiceId);
            _main.NavigateTo("Booking");
        }
    }

    [RelayCommand]
    private void Rebook()
    {
        if (Appointment != null)
        {
            _main.BookingVM.InitializeBooking(Appointment.BarberId, Appointment.ServiceId);
            _main.NavigateTo("Booking");
        }
    }

    [RelayCommand]
    private void OpenCancel() => IsCancelModalOpen = true;

    [RelayCommand]
    private void CloseCancel() => IsCancelModalOpen = false;

    [RelayCommand]
    private async Task ConfirmCancel()
    {
        if (Appointment != null)
        {
            IsBusy = true;
            await Task.Delay(600);
            DataService.CancelAppointment(Appointment.Id);
            IsBusy = false;
            IsCancelModalOpen = false;
            _main.ShowToast("Rendez-vous annulé");
            OnPropertyChanged(nameof(Appointment));
        }
    }

    [RelayCommand]
    private void OpenContact() => IsContactSheetOpen = true;

    [RelayCommand]
    private void CloseContact() => IsContactSheetOpen = false;

    [RelayCommand]
    private void CallBarber()
    {
        IsContactSheetOpen = false;
        _main.ShowToast("Appel en cours…");
    }

    [RelayCommand]
    private void MessageBarber()
    {
        IsContactSheetOpen = false;
        _main.ShowToast("Conversation ouverte");
    }

    [RelayCommand]
    private void OpenReview() => IsReviewSheetOpen = true;

    [RelayCommand]
    private void CloseReview() => IsReviewSheetOpen = false;

    [RelayCommand]
    private void SetRating(object? ratingParam)
    {
        if (ratingParam != null && int.TryParse(ratingParam.ToString(), out int r))
        {
            ReviewRating = r;
        }
    }

    [RelayCommand]
    private void SubmitReview()
    {
        if (Appointment != null)
        {
            DataService.AddReview(Appointment.BarberId, ReviewRating, ReviewComment);
            IsReviewSheetOpen = false;
            IsReviewed = true;
            _main.ShowToast("Merci pour votre avis !");
        }
    }
}

public partial class FavoritesViewModel : BaseViewModel
{
    private readonly MainViewModel _main;

    [ObservableProperty]
    private string _activeTab = "styles"; // styles, barbers

    public ObservableCollection<HairStyle> FavoriteStyles { get; } = new();
    public ObservableCollection<Barber> FavoriteBarbers { get; } = new();

    public int TotalCount => FavoriteStyles.Count + FavoriteBarbers.Count;

    public FavoritesViewModel(IDataService dataService, MainViewModel main) : base(dataService)
    {
        _main = main;
        RefreshFavorites();
    }

    public void RefreshFavorites()
    {
        FavoriteStyles.Clear();
        foreach (var s in DataService.HairStyles.Where(s => DataService.IsFavorite(s.Id)))
        {
            FavoriteStyles.Add(s);
        }

        FavoriteBarbers.Clear();
        foreach (var b in DataService.Barbers.Where(b => DataService.IsFavorite(b.Id)))
        {
            FavoriteBarbers.Add(b);
        }

        OnPropertyChanged(nameof(TotalCount));
    }

    [RelayCommand]
    private void SetTab(string tab) => ActiveTab = tab;

    [RelayCommand]
    private void ToggleFavoriteStyle(HairStyle style)
    {
        DataService.ToggleFavorite(style.Id);
        RefreshFavorites();
    }

    [RelayCommand]
    private void ToggleFavoriteBarber(Barber barber)
    {
        DataService.ToggleFavorite(barber.Id);
        RefreshFavorites();
    }

    [RelayCommand]
    private void SelectStyle(HairStyle style)
    {
        _main.StyleDetailVM.LoadStyle(style.Id);
        _main.NavigateTo("StyleDetail");
    }

    [RelayCommand]
    private void SelectBarber(Barber barber)
    {
        _main.BarberProfileVM.LoadBarber(barber.Id);
        _main.NavigateTo("BarberProfile");
    }

    [RelayCommand]
    private void ExploreStyles()
    {
        _main.ActiveClientTab = "Explore";
        _main.ExploreVM.SelectedFilter = "Tous";
    }
}

public partial class NotificationsViewModel : BaseViewModel
{
    private readonly MainViewModel _main;

    public ObservableCollection<AppNotification> Notifications => DataService.Notifications;
    public int UnreadCount => Notifications.Count(n => !n.Read);

    public NotificationsViewModel(IDataService dataService, MainViewModel main) : base(dataService)
    {
        _main = main;
    }

    [RelayCommand]
    private void MarkAllAsRead()
    {
        DataService.MarkAllNotificationsRead();
        OnPropertyChanged(nameof(UnreadCount));
    }

    [RelayCommand]
    private void OpenNotification(AppNotification notif)
    {
        notif.Read = true;
        OnPropertyChanged(nameof(UnreadCount));

        if (notif.Kind == "review" || notif.Kind == "cancelled")
        {
            _main.ActiveClientTab = "Appointments";
            _main.AppointmentsVM.ActiveTab = "past";
            _main.GoBack();
        }
        else if (notif.Kind == "confirmed")
        {
            _main.AppointmentDetailVM.LoadAppointment("a2");
            _main.NavigateTo("AppointmentDetail");
        }
        else if (notif.Kind == "reminder")
        {
            _main.AppointmentDetailVM.LoadAppointment("a1");
            _main.NavigateTo("AppointmentDetail");
        }
    }

    [RelayCommand]
    private void GoBack() => _main.GoBack();
}

public partial class ProfileViewModel : BaseViewModel
{
    private readonly MainViewModel _main;

    public UserProfile User => DataService.CurrentUser;

    [ObservableProperty]
    private bool _notificationsEnabled = true;

    [ObservableProperty]
    private bool _remindersEnabled = true;

    [ObservableProperty]
    private string _selectedLanguage = "Français";

    [ObservableProperty]
    private bool _isLanguageSheetOpen;

    [ObservableProperty]
    private bool _isPasswordSheetOpen;

    [ObservableProperty]
    private bool _isLogoutModalOpen;

    [ObservableProperty]
    private string _currentPassword = string.Empty;

    [ObservableProperty]
    private string _newPassword = string.Empty;

    [ObservableProperty]
    private string _confirmPassword = string.Empty;

    public ProfileViewModel(IDataService dataService, MainViewModel main) : base(dataService)
    {
        _main = main;
    }

    [RelayCommand]
    private void OpenEditProfile() => _main.NavigateTo("EditProfile");

    [RelayCommand]
    private void SwitchToPro() => _main.SwitchToPro();

    [RelayCommand]
    private void OpenAppStates() => _main.NavigateTo("AppStates");

    [RelayCommand]
    private void OpenLanguageSheet() => IsLanguageSheetOpen = true;

    [RelayCommand]
    private void SelectLanguage(string lang)
    {
        SelectedLanguage = lang;
        IsLanguageSheetOpen = false;
        _main.ShowToast("Langue mise à jour");
    }

    [RelayCommand]
    private void OpenPasswordSheet()
    {
        CurrentPassword = string.Empty;
        NewPassword = string.Empty;
        ConfirmPassword = string.Empty;
        IsPasswordSheetOpen = true;
    }

    [RelayCommand]
    private void SavePassword()
    {
        if (NewPassword.Length >= 6 && NewPassword == ConfirmPassword)
        {
            IsPasswordSheetOpen = false;
            _main.ShowToast("Mot de passe modifié");
        }
        else
        {
            _main.ShowToast("Vérifiez vos mots de passe");
        }
    }

    [RelayCommand]
    private void OpenLogoutModal() => IsLogoutModalOpen = true;

    [RelayCommand]
    private void ConfirmLogout()
    {
        IsLogoutModalOpen = false;
        _main.SwitchToAuth();
    }

    [RelayCommand]
    private void CloseModals()
    {
        IsLanguageSheetOpen = false;
        IsPasswordSheetOpen = false;
        IsLogoutModalOpen = false;
    }

    [RelayCommand]
    private void ShowInfo(string msg) => _main.ShowToast(msg);
}

public partial class EditProfileViewModel : BaseViewModel
{
    private readonly MainViewModel _main;

    [ObservableProperty]
    private string _firstName = string.Empty;

    [ObservableProperty]
    private string _lastName = string.Empty;

    [ObservableProperty]
    private string _email = string.Empty;

    [ObservableProperty]
    private string _phone = string.Empty;

    [ObservableProperty]
    private string _avatar = string.Empty;

    public EditProfileViewModel(IDataService dataService, MainViewModel main) : base(dataService)
    {
        _main = main;
        LoadProfile();
    }

    public void LoadProfile()
    {
        var u = DataService.CurrentUser;
        FirstName = u.FirstName;
        LastName = u.LastName;
        Email = u.Email;
        Phone = u.Phone;
        Avatar = u.Avatar;
    }

    [RelayCommand]
    private void GoBack() => _main.GoBack();

    [RelayCommand]
    private async Task Save()
    {
        if (string.IsNullOrWhiteSpace(FirstName) || string.IsNullOrWhiteSpace(LastName))
        {
            _main.ShowToast("Le nom et le prénom sont requis.");
            return;
        }

        IsBusy = true;
        await Task.Delay(600);
        DataService.UpdateUserProfile(FirstName, LastName, Email, Phone, Avatar);
        IsBusy = false;
        _main.ShowToast("Profil mis à jour");
        _main.GoBack();
    }
}
