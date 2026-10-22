using HairBook.Models;
using System.Collections.ObjectModel;

namespace HairBook.Services;

public interface IDataService
{
    UserProfile CurrentUser { get; }
    Barber CurrentPro { get; }
    ObservableCollection<Barber> Barbers { get; }
    ObservableCollection<HairStyle> HairStyles { get; }
    ObservableCollection<Service> Services { get; }
    ObservableCollection<Appointment> Appointments { get; }
    ObservableCollection<ProBooking> ProBookings { get; }
    ObservableCollection<PlanningDay> PlanningWeek { get; }
    ObservableCollection<HistoryEntry> ProHistory { get; }
    ObservableCollection<DaySchedule> WeeklySchedule { get; }
    ObservableCollection<Review> Reviews { get; }
    ObservableCollection<PortfolioItem> PortfolioItems { get; }
    ObservableCollection<AppNotification> Notifications { get; }
    List<StyleCategoryItem> Categories { get; }
    ObservableCollection<string> FavoriteIds { get; }

    bool IsFavorite(string id);
    void ToggleFavorite(string id);
    Appointment CreateAppointment(string barberId, string serviceId, string dateLabel, string time);
    void CancelAppointment(string appointmentId);
    void AddReview(string barberId, int rating, string comment);
    void UpdateUserProfile(string firstName, string lastName, string email, string phone, string? avatar = null);
    void UpdateProProfile(string name, string bio, string phone, string address, string city, List<string> specialties, int experience);
    void AddService(string name, string description, int price, int duration);
    void UpdateService(string id, string name, string description, int price, int duration);
    void DeleteService(string id);
    void AddPortfolioItem(string title, string category, string image);
    void DeletePortfolioItem(string id);
    void UpdateProBookingStatus(string id, string status);
    void MarkAllNotificationsRead();

    Barber? GetBarber(string id);
    HairStyle? GetStyle(string id);
    Service? GetService(string id);
    Appointment? GetAppointment(string id);
}
