using CommunityToolkit.Mvvm.ComponentModel;
using System.Collections.ObjectModel;

namespace HairBook.Models;

public partial class Barber : ObservableObject
{
    [ObservableProperty]
    private string _id = string.Empty;

    [ObservableProperty]
    private string _name = string.Empty;

    [ObservableProperty]
    private string _specialty = string.Empty;

    [ObservableProperty]
    private double _rating;

    [ObservableProperty]
    private int _reviewCount;

    [ObservableProperty]
    private string _location = string.Empty;

    [ObservableProperty]
    private string _address = string.Empty;

    [ObservableProperty]
    private string _avatar = string.Empty;

    [ObservableProperty]
    private int _experience;

    [ObservableProperty]
    private int _clients;

    [ObservableProperty]
    private int _works;

    [ObservableProperty]
    private string _hours = string.Empty;

    [ObservableProperty]
    private string _bio = string.Empty;

    [ObservableProperty]
    private bool _isFavorite;

    public string ShortLocation => Location.Split(',')[0].Trim();
    public string RatingFormatted => Rating.ToString("0.0");
    public string FormattedClients => $"{Clients:N0} clients".Replace(",", " ");
    public string FormattedWorks => $"{Works} réalisations";
}

public partial class HairStyle : ObservableObject
{
    [ObservableProperty]
    private string _id = string.Empty;

    [ObservableProperty]
    private string _name = string.Empty;

    [ObservableProperty]
    private string _description = string.Empty;

    [ObservableProperty]
    private int _price;

    [ObservableProperty]
    private int _duration;

    [ObservableProperty]
    private string _category = string.Empty; // Fade, Taper, Afro, Locks, Braids, Barbe, Classique

    [ObservableProperty]
    private string _group = string.Empty; // Coupes, Barbe, Coiffures

    [ObservableProperty]
    private string _barberId = string.Empty;

    [ObservableProperty]
    private string _serviceId = string.Empty;

    [ObservableProperty]
    private string _image = string.Empty;

    [ObservableProperty]
    private string _barberName = string.Empty;

    [ObservableProperty]
    private bool _isFavorite;

    public string FormattedPrice => $"{Price:N0} FCFA".Replace(",", " ");
    public string FormattedDuration => Duration < 60 ? $"{Duration} min" : $"{Duration / 60} h {Duration % 60:D2}".TrimEnd('0').TrimEnd(' ');
    public string StartingPriceFormatted => $"dès {FormattedPrice}";
}

public partial class Service : ObservableObject
{
    [ObservableProperty]
    private string _id = string.Empty;

    [ObservableProperty]
    private string _barberId = string.Empty;

    [ObservableProperty]
    private string _name = string.Empty;

    [ObservableProperty]
    private string _description = string.Empty;

    [ObservableProperty]
    private int _price;

    [ObservableProperty]
    private int _duration;

    [ObservableProperty]
    private bool _active = true;

    public string FormattedPrice => $"{Price:N0} FCFA".Replace(",", " ");
    public string FormattedDuration => Duration < 60 ? $"{Duration} min" : $"{Duration / 60} h {Duration % 60:D2}".TrimEnd('0').TrimEnd(' ');
}

public partial class Appointment : ObservableObject
{
    [ObservableProperty]
    private string _id = string.Empty;

    [ObservableProperty]
    private string _barberId = string.Empty;

    [ObservableProperty]
    private string _serviceId = string.Empty;

    [ObservableProperty]
    private string _serviceName = string.Empty;

    [ObservableProperty]
    private string _dateLabel = string.Empty;

    [ObservableProperty]
    private string _time = string.Empty;

    [ObservableProperty]
    private int _duration;

    [ObservableProperty]
    private int _price;

    [ObservableProperty]
    private string _status = "confirmed"; // confirmed, pending, completed, cancelled

    [ObservableProperty]
    private string _barberName = string.Empty;

    [ObservableProperty]
    private string _barberAvatar = string.Empty;

    [ObservableProperty]
    private string _barberAddress = string.Empty;

    [ObservableProperty]
    private string _barberSpecialty = string.Empty;

    [ObservableProperty]
    private double _barberRating = 4.9;

    public string FormattedPrice => $"{Price:N0} FCFA".Replace(",", " ");
    public string FormattedDuration => Duration < 60 ? $"{Duration} min" : $"{Duration / 60} h {Duration % 60:D2}".TrimEnd('0').TrimEnd(' ');
    public bool IsConfirmed => Status == "confirmed";
    public bool IsPending => Status == "pending";
    public bool IsCompleted => Status == "completed";
    public bool IsCancelled => Status == "cancelled";
    public bool IsActive => IsConfirmed || IsPending;
}

public partial class ProBooking : ObservableObject
{
    [ObservableProperty]
    private string _id = string.Empty;

    [ObservableProperty]
    private string _client = string.Empty;

    [ObservableProperty]
    private string _service = string.Empty;

    [ObservableProperty]
    private string _time = string.Empty;

    [ObservableProperty]
    private int _duration;

    [ObservableProperty]
    private int _price;

    [ObservableProperty]
    private string _status = "pending"; // pending, accepted, declined

    [ObservableProperty]
    private string _phone = string.Empty;

    public string FormattedPrice => $"{Price:N0} FCFA".Replace(",", " ");
    public string FormattedDuration => $"{Duration} min";
    public bool IsPending => Status == "pending";
    public bool IsAccepted => Status == "accepted";
    public bool IsDeclined => Status == "declined";
}

public partial class PlanningSlot : ObservableObject
{
    [ObservableProperty]
    private string _id = string.Empty;

    [ObservableProperty]
    private string _start = string.Empty;

    [ObservableProperty]
    private int _duration;

    [ObservableProperty]
    private string _client = string.Empty;

    [ObservableProperty]
    private string _service = string.Empty;

    [ObservableProperty]
    private string _status = "confirmed"; // confirmed, pending

    public string EndTime
    {
        get
        {
            var parts = Start.Split(':');
            if (parts.Length == 2 && int.TryParse(parts[0], out int h) && int.TryParse(parts[1], out int m))
            {
                var total = h * 60 + m + Duration;
                return $"{total / 60:D2}:{total % 60:D2}";
            }
            return Start;
        }
    }
}

public partial class PlanningDay : ObservableObject
{
    [ObservableProperty]
    private string _id = string.Empty;

    [ObservableProperty]
    private string _shortName = string.Empty;

    [ObservableProperty]
    private string _label = string.Empty;

    [ObservableProperty]
    private int _date;

    [ObservableProperty]
    private bool _closed;

    [ObservableProperty]
    private ObservableCollection<PlanningSlot> _slots = new();

    public int TotalBookedMinutes => Slots.Sum(s => s.Duration);
    public int FreeMinutes => Math.Max(0, 480 - TotalBookedMinutes); // Assuming 8-hour workday = 480 mins
    public double OccupancyRatio => Closed ? 0 : Math.Min(1.0, (double)TotalBookedMinutes / 480.0);
    public int OccupancyPercentage => (int)Math.Round(OccupancyRatio * 100);
    public string FreeMinutesFormatted => $"{FreeMinutes / 60} h {FreeMinutes % 60:D2}".TrimEnd('0').TrimEnd(' ') + " libres";
}

public partial class HistoryEntry : ObservableObject
{
    [ObservableProperty]
    private string _id = string.Empty;

    [ObservableProperty]
    private string _client = string.Empty;

    [ObservableProperty]
    private string _service = string.Empty;

    [ObservableProperty]
    private string _dateLabel = string.Empty;

    [ObservableProperty]
    private int _price;

    [ObservableProperty]
    private string _status = "completed"; // completed, cancelled

    public string FormattedPrice => $"{Price:N0} FCFA".Replace(",", " ");
}

public partial class DaySchedule : ObservableObject
{
    [ObservableProperty]
    private string _day = string.Empty;

    [ObservableProperty]
    private bool _enabled = true;

    [ObservableProperty]
    private string _open = "09:00";

    [ObservableProperty]
    private string _close = "18:00";

    [ObservableProperty]
    private bool _hasBreak;

    [ObservableProperty]
    private string _breakStart = "13:00";

    [ObservableProperty]
    private string _breakEnd = "14:00";
}

public partial class Review : ObservableObject
{
    [ObservableProperty]
    private string _id = string.Empty;

    [ObservableProperty]
    private string _barberId = string.Empty;

    [ObservableProperty]
    private string _author = string.Empty;

    [ObservableProperty]
    private int _rating = 5;

    [ObservableProperty]
    private string _comment = string.Empty;

    [ObservableProperty]
    private string _date = string.Empty;
}

public partial class PortfolioItem : ObservableObject
{
    [ObservableProperty]
    private string _id = string.Empty;

    [ObservableProperty]
    private string _image = string.Empty;

    [ObservableProperty]
    private string _title = string.Empty;

    [ObservableProperty]
    private string _category = string.Empty;
}

public partial class AppNotification : ObservableObject
{
    [ObservableProperty]
    private string _id = string.Empty;

    [ObservableProperty]
    private string _kind = string.Empty; // reminder, confirmed, message, review, cancelled

    [ObservableProperty]
    private string _title = string.Empty;

    [ObservableProperty]
    private string _body = string.Empty;

    [ObservableProperty]
    private string _time = string.Empty;

    [ObservableProperty]
    private string _group = "Aujourd'hui"; // Aujourd'hui, Cette semaine

    [ObservableProperty]
    private bool _read;
}

public class StyleCategoryItem
{
    public string Id { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string Image { get; set; } = string.Empty;
}

public partial class UserProfile : ObservableObject
{
    [ObservableProperty]
    private string _firstName = "Eric";

    [ObservableProperty]
    private string _lastName = "Kouamé";

    [ObservableProperty]
    private string _email = "eric.kouame@mail.com";

    [ObservableProperty]
    private string _phone = "+225 07 48 21 36 90";

    [ObservableProperty]
    private string _avatar = "user_eric.jpg";

    public string FullName => $"{FirstName} {LastName}".Trim();
}
