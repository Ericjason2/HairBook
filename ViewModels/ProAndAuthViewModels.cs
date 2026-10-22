using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using HairBook.Models;
using HairBook.Services;
using System.Collections.ObjectModel;

namespace HairBook.ViewModels;

public partial class ProDashboardViewModel : BaseViewModel
{
    private readonly MainViewModel _main;

    public Barber CurrentPro => DataService.CurrentPro;
    public ObservableCollection<ProBooking> Bookings => DataService.ProBookings;

    public int ActiveCount => Bookings.Count(b => b.Status != "declined");
    public int PendingCount => Bookings.Count(b => b.Status == "pending");
    public ProBooking? NextBooking => Bookings.FirstOrDefault(b => b.Status != "declined");

    [ObservableProperty]
    private ProBooking? _selectedBooking;

    [ObservableProperty]
    private bool _isDetailSheetOpen;

    [ObservableProperty]
    private ProBooking? _declineTarget;

    [ObservableProperty]
    private bool _isDeclineModalOpen;

    public ProDashboardViewModel(IDataService dataService, MainViewModel main) : base(dataService)
    {
        _main = main;
    }

    [RelayCommand]
    private void AcceptBooking(ProBooking booking)
    {
        DataService.UpdateProBookingStatus(booking.Id, "accepted");
        _main.ShowToast($"Réservation de {booking.Client.Split(' ')[0]} acceptée");
        OnPropertyChanged(nameof(PendingCount));
    }

    [RelayCommand]
    private void DeclineBooking(ProBooking booking)
    {
        DeclineTarget = booking;
        IsDeclineModalOpen = true;
    }

    [RelayCommand]
    private void ConfirmDecline()
    {
        if (DeclineTarget != null)
        {
            DataService.UpdateProBookingStatus(DeclineTarget.Id, "declined");
            IsDeclineModalOpen = false;
            _main.ShowToast("Réservation refusée");
            OnPropertyChanged(nameof(ActiveCount));
            OnPropertyChanged(nameof(PendingCount));
            OnPropertyChanged(nameof(NextBooking));
        }
    }

    [RelayCommand]
    private void CancelDecline()
    {
        IsDeclineModalOpen = false;
        DeclineTarget = null;
    }

    [RelayCommand]
    private void ViewBookingDetails(ProBooking booking)
    {
        SelectedBooking = booking;
        IsDetailSheetOpen = true;
    }

    [RelayCommand]
    private void CloseDetailSheet()
    {
        IsDetailSheetOpen = false;
        SelectedBooking = null;
    }

    [RelayCommand]
    private void CallClient()
    {
        IsDetailSheetOpen = false;
        _main.ShowToast("Appel en cours…");
    }

    [RelayCommand]
    private void OpenPlanning()
    {
        _main.ActiveProTab = "Planning";
    }

    [RelayCommand]
    private void OpenProAccount()
    {
        _main.ActiveProTab = "Account";
    }
}

public partial class ProPlanningViewModel : BaseViewModel
{
    private readonly MainViewModel _main;

    [ObservableProperty]
    private string _activeView = "day"; // day, week, history

    [ObservableProperty]
    private PlanningDay? _selectedDay;

    [ObservableProperty]
    private PlanningSlot? _selectedSlot;

    [ObservableProperty]
    private bool _isSlotDetailOpen;

    public ObservableCollection<PlanningDay> PlanningWeek => DataService.PlanningWeek;
    public ObservableCollection<HistoryEntry> ProHistory => DataService.ProHistory;

    public ProPlanningViewModel(IDataService dataService, MainViewModel main) : base(dataService)
    {
        _main = main;
        SelectedDay = PlanningWeek.FirstOrDefault();
    }

    [RelayCommand]
    public void SetView(string view) => ActiveView = view;

    [RelayCommand]
    private void SelectDay(PlanningDay day)
    {
        SelectedDay = day;
    }

    [RelayCommand]
    private void SelectSlot(PlanningSlot slot)
    {
        SelectedSlot = slot;
        IsSlotDetailOpen = true;
    }

    [RelayCommand]
    private void CloseSlotDetail()
    {
        IsSlotDetailOpen = false;
        SelectedSlot = null;
    }

    [RelayCommand]
    private void AcceptSlot()
    {
        if (SelectedSlot != null)
        {
            SelectedSlot.Status = "confirmed";
            IsSlotDetailOpen = false;
            _main.ShowToast("Réservation acceptée");
        }
    }

    [RelayCommand]
    private void DeclineSlot()
    {
        if (SelectedSlot != null)
        {
            SelectedDay?.Slots.Remove(SelectedSlot);
            IsSlotDetailOpen = false;
            _main.ShowToast("Réservation refusée");
        }
    }

    [RelayCommand]
    private void OpenHours() => _main.NavigateTo("ProHours");
}

public partial class ProServicesViewModel : BaseViewModel
{
    private readonly MainViewModel _main;

    public ObservableCollection<Service> Services => DataService.Services;

    [ObservableProperty]
    private bool _isFormSheetOpen;

    [ObservableProperty]
    private Service? _editingService;

    [ObservableProperty]
    private string _formName = string.Empty;

    [ObservableProperty]
    private string _formDescription = string.Empty;

    [ObservableProperty]
    private int _formPrice = 3000;

    [ObservableProperty]
    private int _formDuration = 30;

    [ObservableProperty]
    private bool _isDeleteModalOpen;

    [ObservableProperty]
    private Service? _serviceToDelete;

    public ProServicesViewModel(IDataService dataService, MainViewModel main) : base(dataService)
    {
        _main = main;
    }

    [RelayCommand]
    private void OpenAddService()
    {
        EditingService = null;
        FormName = string.Empty;
        FormDescription = string.Empty;
        FormPrice = 3000;
        FormDuration = 30;
        IsFormSheetOpen = true;
    }

    [RelayCommand]
    private void OpenEditService(Service service)
    {
        EditingService = service;
        FormName = service.Name;
        FormDescription = service.Description;
        FormPrice = service.Price;
        FormDuration = service.Duration;
        IsFormSheetOpen = true;
    }

    [RelayCommand]
    private void CloseForm()
    {
        IsFormSheetOpen = false;
    }

    [RelayCommand]
    private void SaveService()
    {
        if (string.IsNullOrWhiteSpace(FormName))
        {
            _main.ShowToast("Le nom de la prestation est requis.");
            return;
        }

        if (EditingService != null)
        {
            DataService.UpdateService(EditingService.Id, FormName, FormDescription, FormPrice, FormDuration);
            _main.ShowToast("Prestation modifiée");
        }
        else
        {
            DataService.AddService(FormName, FormDescription, FormPrice, FormDuration);
            _main.ShowToast("Prestation ajoutée");
        }

        IsFormSheetOpen = false;
    }

    [RelayCommand]
    private void PromptDelete(Service service)
    {
        ServiceToDelete = service;
        IsDeleteModalOpen = true;
    }

    [RelayCommand]
    private void ConfirmDelete()
    {
        if (ServiceToDelete != null)
        {
            DataService.DeleteService(ServiceToDelete.Id);
            IsDeleteModalOpen = false;
            _main.ShowToast("Prestation supprimée");
        }
    }

    [RelayCommand]
    private void CancelDelete()
    {
        IsDeleteModalOpen = false;
        ServiceToDelete = null;
    }

    [RelayCommand]
    private void ToggleActive(Service service)
    {
        service.Active = !service.Active;
        _main.ShowToast(service.Active ? "Prestation visible" : "Prestation masquée");
    }
}

public partial class ProPortfolioViewModel : BaseViewModel
{
    private readonly MainViewModel _main;

    [ObservableProperty]
    private string _selectedFilter = "Toutes";

    [ObservableProperty]
    private bool _isAddPhotoSheetOpen;

    [ObservableProperty]
    private string _newPhotoTitle = string.Empty;

    [ObservableProperty]
    private string _newPhotoCategory = "Fade";

    [ObservableProperty]
    private bool _isDeleteModalOpen;

    [ObservableProperty]
    private PortfolioItem? _itemToDelete;

    public ObservableCollection<PortfolioItem> FilteredItems { get; } = new();
    public List<string> Categories { get; } = new() { "Toutes", "Fade", "Taper", "Afro", "Locks", "Braids", "Barbe", "Classique" };

    public ProPortfolioViewModel(IDataService dataService, MainViewModel main) : base(dataService)
    {
        _main = main;
        RefreshList();
    }

    partial void OnSelectedFilterChanged(string value) => RefreshList();

    public void RefreshList()
    {
        FilteredItems.Clear();
        var items = SelectedFilter == "Toutes"
            ? DataService.PortfolioItems
            : DataService.PortfolioItems.Where(i => i.Category == SelectedFilter);

        foreach (var i in items) FilteredItems.Add(i);
    }

    [RelayCommand]
    private void SetFilter(string filter) => SelectedFilter = filter;

    [RelayCommand]
    private void OpenAddPhoto()
    {
        NewPhotoTitle = string.Empty;
        NewPhotoCategory = "Fade";
        IsAddPhotoSheetOpen = true;
    }

    [RelayCommand]
    private void CloseAddPhoto() => IsAddPhotoSheetOpen = false;

    [RelayCommand]
    private void SaveNewPhoto()
    {
        if (string.IsNullOrWhiteSpace(NewPhotoTitle))
        {
            _main.ShowToast("Veuillez indiquer un titre.");
            return;
        }

        DataService.AddPortfolioItem(NewPhotoTitle, NewPhotoCategory, "style_low_fade.jpg");
        IsAddPhotoSheetOpen = false;
        RefreshList();
        _main.ShowToast("Photo publiée");
    }

    [RelayCommand]
    private void PromptDelete(PortfolioItem item)
    {
        ItemToDelete = item;
        IsDeleteModalOpen = true;
    }

    [RelayCommand]
    private void ConfirmDelete()
    {
        if (ItemToDelete != null)
        {
            DataService.DeletePortfolioItem(ItemToDelete.Id);
            IsDeleteModalOpen = false;
            RefreshList();
            _main.ShowToast("Photo supprimée");
        }
    }

    [RelayCommand]
    private void CancelDelete()
    {
        IsDeleteModalOpen = false;
        ItemToDelete = null;
    }
}

public partial class ProHoursViewModel : BaseViewModel
{
    private readonly MainViewModel _main;

    public ObservableCollection<DaySchedule> Schedule => DataService.WeeklySchedule;

    public ProHoursViewModel(IDataService dataService, MainViewModel main) : base(dataService)
    {
        _main = main;
    }

    [RelayCommand]
    private void GoBack() => _main.GoBack();

    [RelayCommand]
    private async Task SaveHours()
    {
        IsBusy = true;
        await Task.Delay(600);
        IsBusy = false;
        _main.ShowToast("Horaires enregistrés");
        _main.GoBack();
    }
}

public partial class ProProfileFormViewModel : BaseViewModel
{
    private readonly MainViewModel _main;

    [ObservableProperty]
    private string _name = string.Empty;

    [ObservableProperty]
    private string _bio = string.Empty;

    [ObservableProperty]
    private string _phone = string.Empty;

    [ObservableProperty]
    private string _address = string.Empty;

    [ObservableProperty]
    private string _city = string.Empty;

    [ObservableProperty]
    private int _experience = 7;

    public List<string> SpecialtiesList { get; } = new() { "Fade", "Taper", "Afro", "Locks", "Braids", "Barbe", "Classique" };
    public ObservableCollection<string> SelectedSpecialties { get; } = new();

    public ProProfileFormViewModel(IDataService dataService, MainViewModel main) : base(dataService)
    {
        _main = main;
        LoadProProfile();
    }

    public void LoadProProfile()
    {
        var p = DataService.CurrentPro;
        Name = p.Name;
        Bio = p.Bio;
        Phone = "+225 07 00 11 22 33";
        Address = "Rue des Jardins, Cocody Angré";
        City = "Abidjan";
        Experience = p.Experience;

        SelectedSpecialties.Clear();
        SelectedSpecialties.Add("Fade");
        SelectedSpecialties.Add("Taper");
        SelectedSpecialties.Add("Barbe");
    }

    [RelayCommand]
    private void ToggleSpecialty(string spec)
    {
        if (SelectedSpecialties.Contains(spec))
            SelectedSpecialties.Remove(spec);
        else
            SelectedSpecialties.Add(spec);
    }

    [RelayCommand]
    private void DecreaseExp()
    {
        if (Experience > 0) Experience--;
    }

    [RelayCommand]
    private void IncreaseExp()
    {
        if (Experience < 50) Experience++;
    }

    [RelayCommand]
    private void GoBack() => _main.GoBack();

    [RelayCommand]
    private async Task Save()
    {
        if (string.IsNullOrWhiteSpace(Name))
        {
            _main.ShowToast("Le nom est requis.");
            return;
        }

        IsBusy = true;
        await Task.Delay(600);
        DataService.UpdateProProfile(Name, Bio, Phone, Address, City, SelectedSpecialties.ToList(), Experience);
        IsBusy = false;
        _main.ShowToast("Profil professionnel enregistré");
        _main.GoBack();
    }
}

public partial class ProAccountViewModel : BaseViewModel
{
    private readonly MainViewModel _main;

    public Barber CurrentPro => DataService.CurrentPro;

    [ObservableProperty]
    private bool _notificationsEnabled = true;

    [ObservableProperty]
    private bool _isLogoutModalOpen;

    public ProAccountViewModel(IDataService dataService, MainViewModel main) : base(dataService)
    {
        _main = main;
    }

    [RelayCommand]
    private void ViewPublicProfile()
    {
        _main.BarberProfileVM.LoadBarber(CurrentPro.Id);
        _main.NavigateTo("BarberProfile");
    }

    [RelayCommand]
    private void OpenProProfile() => _main.NavigateTo("ProProfileForm");

    [RelayCommand]
    private void OpenProHours() => _main.NavigateTo("ProHours");

    [RelayCommand]
    private void OpenProHistory()
    {
        _main.ActiveProTab = "Planning";
        _main.ProPlanningVM.SetView("history");
    }

    [RelayCommand]
    private void SwitchToClientMode()
    {
        _main.SwitchToClient();
    }

    [RelayCommand]
    private void OpenLogout() => IsLogoutModalOpen = true;

    [RelayCommand]
    private void ConfirmLogout()
    {
        IsLogoutModalOpen = false;
        _main.SwitchToAuth();
    }

    [RelayCommand]
    private void CancelLogout() => IsLogoutModalOpen = false;

    [RelayCommand]
    private void ShowToast(string msg) => _main.ShowToast(msg);
}

public partial class LoginViewModel : BaseViewModel
{
    private readonly MainViewModel _main;

    [ObservableProperty]
    private string _role = "client"; // client, pro

    [ObservableProperty]
    private string _identifier = "eric.kouame@mail.com";

    [ObservableProperty]
    private string _password = "hairbook2026";

    [ObservableProperty]
    private bool _isForgotSheetOpen;

    [ObservableProperty]
    private string _forgotEmail = string.Empty;

    public LoginViewModel(IDataService dataService, MainViewModel main) : base(dataService)
    {
        _main = main;
    }

    [RelayCommand]
    private void SetRole(string r)
    {
        Role = r;
        Identifier = r == "client" ? "eric.kouame@mail.com" : "chris.barber@mail.com";
    }

    [RelayCommand]
    private async Task SubmitLogin()
    {
        if (string.IsNullOrWhiteSpace(Identifier) || Password.Length < 6)
        {
            _main.ShowToast("Vérifiez vos identifiants (mot de passe min 6 caractères).");
            return;
        }

        IsBusy = true;
        await Task.Delay(600);
        IsBusy = false;

        if (Role == "client")
            _main.SwitchToClient();
        else
            _main.SwitchToPro();
    }

    [RelayCommand]
    private void OpenRegister() => _main.NavigateTo("Register");

    [RelayCommand]
    private void OpenForgotSheet() => IsForgotSheetOpen = true;

    [RelayCommand]
    private void CloseForgotSheet() => IsForgotSheetOpen = false;

    [RelayCommand]
    private void SendResetLink()
    {
        IsForgotSheetOpen = false;
        _main.ShowToast("Lien de réinitialisation envoyé");
    }
}

public partial class RegisterViewModel : BaseViewModel
{
    private readonly MainViewModel _main;

    [ObservableProperty]
    private string _role = "client"; // client, pro

    [ObservableProperty]
    private string _name = string.Empty;

    [ObservableProperty]
    private string _email = string.Empty;

    [ObservableProperty]
    private string _phone = string.Empty;

    [ObservableProperty]
    private string _password = string.Empty;

    [ObservableProperty]
    private string _confirmPassword = string.Empty;

    public RegisterViewModel(IDataService dataService, MainViewModel main) : base(dataService)
    {
        _main = main;
    }

    [RelayCommand]
    private void SetRole(string r) => Role = r;

    [RelayCommand]
    private void GoBack() => _main.GoBack();

    [RelayCommand]
    private async Task SubmitRegister()
    {
        if (string.IsNullOrWhiteSpace(Name) || !Email.Contains("@") || Password.Length < 6 || Password != ConfirmPassword)
        {
            _main.ShowToast("Veuillez remplir correctement tous les champs.");
            return;
        }

        IsBusy = true;
        await Task.Delay(700);
        IsBusy = false;

        _main.ShowToast("Compte créé avec succès");
        if (Role == "client")
            _main.SwitchToClient();
        else
            _main.SwitchToPro();
    }
}

public class OnboardingSlideItem
{
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string Image { get; set; } = string.Empty;
}

public partial class OnboardingViewModel : BaseViewModel
{
    private readonly MainViewModel _main;

    [ObservableProperty]
    private int _currentIndex;

    public List<OnboardingSlideItem> Slides { get; } = new()
    {
        new OnboardingSlideItem
        {
            Title = "Trouvez votre style",
            Description = "Découvrez les meilleurs styles de coiffure et trouvez l’inspiration pour votre prochaine coupe.",
            Image = "style_taper_fade.jpg"
        },
        new OnboardingSlideItem
        {
            Title = "Trouvez votre barber",
            Description = "Découvrez les coiffeurs et barbiers disponibles autour de vous.",
            Image = "barber_work.jpg"
        },
        new OnboardingSlideItem
        {
            Title = "Réservez en quelques secondes",
            Description = "Choisissez votre prestation, votre date et votre heure, puis confirmez votre rendez-vous.",
            Image = "shop_interior.jpg"
        }
    };

    public OnboardingSlideItem CurrentSlide => Slides[CurrentIndex];
    public bool IsLastSlide => CurrentIndex == Slides.Count - 1;

    public OnboardingViewModel(IDataService dataService, MainViewModel main) : base(dataService)
    {
        _main = main;
    }

    [RelayCommand]
    private void Next()
    {
        if (IsLastSlide)
        {
            Finish();
        }
        else
        {
            CurrentIndex++;
            OnPropertyChanged(nameof(CurrentSlide));
            OnPropertyChanged(nameof(IsLastSlide));
        }
    }

    [RelayCommand]
    private void Skip() => Finish();

    [RelayCommand]
    private void Finish()
    {
        _main.NavigateTo("Login");
    }
}

public partial class AppStatesViewModel : BaseViewModel
{
    private readonly MainViewModel _main;

    [ObservableProperty]
    private string _selectedEmptyKey = "rdv";

    [ObservableProperty]
    private string _errorSimState = "error"; // error, loading, ok

    [ObservableProperty]
    private bool _isConfirmModalOpen;

    public AppStatesViewModel(IDataService dataService, MainViewModel main) : base(dataService)
    {
        _main = main;
    }

    [RelayCommand]
    private void GoBack() => _main.GoBack();

    [RelayCommand]
    private void SetEmpty(string key) => SelectedEmptyKey = key;

    [RelayCommand]
    private async Task RetryError()
    {
        ErrorSimState = "loading";
        await Task.Delay(1000);
        ErrorSimState = "ok";
    }

    [RelayCommand]
    private void ResetError() => ErrorSimState = "error";

    [RelayCommand]
    private void TriggerToast(string msg) => _main.ShowToast(msg);

    [RelayCommand]
    private void OpenConfirm() => IsConfirmModalOpen = true;

    [RelayCommand]
    private void ConfirmAction()
    {
        IsConfirmModalOpen = false;
        _main.ShowToast("Rendez-vous annulé");
    }

    [RelayCommand]
    private void CancelConfirm() => IsConfirmModalOpen = false;
}
