using HairBook.Models;
using System.Collections.ObjectModel;

namespace HairBook.Services;

public class DataService : IDataService
{
    public UserProfile CurrentUser { get; } = new UserProfile
    {
        FirstName = "Eric",
        LastName = "Kouamé",
        Email = "eric.kouame@mail.com",
        Phone = "+225 07 48 21 36 90",
        Avatar = "user_eric.jpg"
    };

    public Barber CurrentPro { get; private set; }

    public ObservableCollection<Barber> Barbers { get; } = new();
    public ObservableCollection<HairStyle> HairStyles { get; } = new();
    public ObservableCollection<Service> Services { get; } = new();
    public ObservableCollection<Appointment> Appointments { get; } = new();
    public ObservableCollection<ProBooking> ProBookings { get; } = new();
    public ObservableCollection<PlanningDay> PlanningWeek { get; } = new();
    public ObservableCollection<HistoryEntry> ProHistory { get; } = new();
    public ObservableCollection<DaySchedule> WeeklySchedule { get; } = new();
    public ObservableCollection<Review> Reviews { get; } = new();
    public ObservableCollection<PortfolioItem> PortfolioItems { get; } = new();
    public ObservableCollection<AppNotification> Notifications { get; } = new();
    public List<StyleCategoryItem> Categories { get; } = new();
    public ObservableCollection<string> FavoriteIds { get; } = new();

    public DataService()
    {
        SeedData();
        CurrentPro = Barbers[0]; // Chris Barber
    }

    private void SeedData()
    {
        // 1. Barbers
        var chris = new Barber
        {
            Id = "chris",
            Name = "Chris Barber",
            Specialty = "Spécialiste Fade & Taper",
            Rating = 4.9,
            ReviewCount = 128,
            Location = "Cocody, Abidjan",
            Address = "Rue des Jardins, Cocody Angré — Abidjan",
            Avatar = "barber_chris.jpg",
            Experience = 7,
            Clients = 1240,
            Works = 86,
            Hours = "Lun – Sam · 09:00 – 18:00",
            Bio = "Dégradés précis, contours nets et conseils personnalisés. Chaque coupe est pensée pour votre visage.",
            IsFavorite = true
        };

        var moussa = new Barber
        {
            Id = "moussa",
            Name = "Moussa Cuts",
            Specialty = "Afro & Barbe",
            Rating = 4.8,
            ReviewCount = 96,
            Location = "Plateau, Abidjan",
            Address = "Avenue Chardy, Plateau — Abidjan",
            Avatar = "barber_moussa.jpg",
            Experience = 10,
            Clients = 1580,
            Works = 112,
            Hours = "Mar – Dim · 10:00 – 19:00",
            Bio = "Sculpture afro, barbes dessinées au rasoir et soins. Un salon calme, sur rendez-vous uniquement.",
            IsFavorite = false
        };

        var yann = new Barber
        {
            Id = "yann",
            Name = "Yann Locks Studio",
            Specialty = "Locks & Braids",
            Rating = 4.7,
            ReviewCount = 74,
            Location = "Marcory, Abidjan",
            Address = "Boulevard VGE, Marcory Zone 4 — Abidjan",
            Avatar = "barber_yann.jpg",
            Experience = 5,
            Clients = 640,
            Works = 58,
            Hours = "Lun – Sam · 08:30 – 17:30",
            Bio = "Création et entretien de locks, tresses et coupes courtes. Produits naturels uniquement.",
            IsFavorite = false
        };

        Barbers.Add(chris);
        Barbers.Add(moussa);
        Barbers.Add(yann);

        // 2. Categories
        Categories.Add(new StyleCategoryItem { Id = "Fade", Name = "Fade", Image = "style_low_fade.jpg" });
        Categories.Add(new StyleCategoryItem { Id = "Taper", Name = "Taper", Image = "style_taper_fade.jpg" });
        Categories.Add(new StyleCategoryItem { Id = "Afro", Name = "Afro", Image = "style_afro.jpg" });
        Categories.Add(new StyleCategoryItem { Id = "Locks", Name = "Locks", Image = "style_locks.jpg" });
        Categories.Add(new StyleCategoryItem { Id = "Braids", Name = "Braids", Image = "style_braids.jpg" });
        Categories.Add(new StyleCategoryItem { Id = "Barbe", Name = "Barbe", Image = "style_beard.jpg" });
        Categories.Add(new StyleCategoryItem { Id = "Classique", Name = "Classique", Image = "style_classic.jpg" });

        // 3. Hair Styles
        HairStyles.Add(new HairStyle { Id = "low-fade", Name = "Low Fade", Description = "Une coupe moderne et propre avec un dégradé progressif.", Price = 3000, Duration = 30, Category = "Fade", Group = "Coupes", BarberId = "chris", ServiceId = "s-low-fade", Image = "style_low_fade.jpg", BarberName = "Chris Barber", IsFavorite = true });
        HairStyles.Add(new HairStyle { Id = "taper-fade", Name = "Taper Fade", Description = "Un dégradé discret sur les tempes et la nuque, pour un rendu net au quotidien.", Price = 3500, Duration = 35, Category = "Taper", Group = "Coupes", BarberId = "chris", ServiceId = "s-taper", Image = "style_taper_fade.jpg", BarberName = "Chris Barber", IsFavorite = false });
        HairStyles.Add(new HairStyle { Id = "afro", Name = "Afro sculpté", Description = "Volume maîtrisé, forme arrondie et contours parfaitement dessinés.", Price = 4000, Duration = 45, Category = "Afro", Group = "Coupes", BarberId = "moussa", ServiceId = "s-afro", Image = "style_afro.jpg", BarberName = "Moussa Cuts", IsFavorite = true });
        HairStyles.Add(new HairStyle { Id = "locks", Name = "Locks retwist", Description = "Entretien des racines et coiffage pour des locks nettes et soignées.", Price = 8000, Duration = 90, Category = "Locks", Group = "Coiffures", BarberId = "yann", ServiceId = "s-locks", Image = "style_locks.jpg", BarberName = "Yann Locks Studio", IsFavorite = true });
        HairStyles.Add(new HairStyle { Id = "beard", Name = "Barbe dessinée", Description = "Une barbe taillée au millimètre avec des contours nets au rasoir.", Price = 2000, Duration = 20, Category = "Barbe", Group = "Barbe", BarberId = "moussa", ServiceId = "s-beard", Image = "style_beard.jpg", BarberName = "Moussa Cuts", IsFavorite = false });
        HairStyles.Add(new HairStyle { Id = "braids", Name = "Braids homme", Description = "Tresses plaquées vers l’arrière, tenue longue durée.", Price = 7000, Duration = 120, Category = "Braids", Group = "Coiffures", BarberId = "yann", ServiceId = "s-braids", Image = "style_braids.jpg", BarberName = "Yann Locks Studio", IsFavorite = false });
        HairStyles.Add(new HairStyle { Id = "burst-fade", Name = "Burst Fade", Description = "Un dégradé circulaire autour de l’oreille qui met le dessus en valeur.", Price = 3500, Duration = 35, Category = "Fade", Group = "Coupes", BarberId = "moussa", ServiceId = "s-burst", Image = "style_burst_fade.jpg", BarberName = "Moussa Cuts", IsFavorite = false });
        HairStyles.Add(new HairStyle { Id = "classic", Name = "Coupe classique", Description = "Raie marquée et finition aux ciseaux, une élégance intemporelle.", Price = 2500, Duration = 30, Category = "Classique", Group = "Coupes", BarberId = "chris", ServiceId = "s-classic", Image = "style_classic.jpg", BarberName = "Chris Barber", IsFavorite = false });
        HairStyles.Add(new HairStyle { Id = "buzz", Name = "Buzz cut", Description = "Rase, uniforme et sans entretien. L’essentiel, bien fait.", Price = 2000, Duration = 20, Category = "Classique", Group = "Coupes", BarberId = "yann", ServiceId = "s-buzz", Image = "style_buzz.jpg", BarberName = "Yann Locks Studio", IsFavorite = false });

        // 4. Initial Favorites
        FavoriteIds.Add("low-fade");
        FavoriteIds.Add("afro");
        FavoriteIds.Add("locks");
        FavoriteIds.Add("chris");

        // 5. Services
        Services.Add(new Service { Id = "s-low-fade", BarberId = "chris", Name = "Low Fade", Description = "Dégradé progressif bas, contours nets à la tondeuse.", Price = 3000, Duration = 30, Active = true });
        Services.Add(new Service { Id = "s-taper", BarberId = "chris", Name = "Taper Fade", Description = "Dégradé discret sur tempes et nuque, dessus texturé.", Price = 3500, Duration = 35, Active = true });
        Services.Add(new Service { Id = "s-coupe-barbe", BarberId = "chris", Name = "Coupe + Barbe", Description = "Coupe au choix et taille de barbe dessinée au rasoir.", Price = 5000, Duration = 45, Active = true });
        Services.Add(new Service { Id = "s-classic", BarberId = "chris", Name = "Coupe classique", Description = "Coupe aux ciseaux avec raie marquée, finition soignée.", Price = 2500, Duration = 30, Active = false });
        Services.Add(new Service { Id = "s-afro", BarberId = "moussa", Name = "Afro sculpté", Description = "Mise en forme arrondie, contours et finition au peigne.", Price = 4000, Duration = 45, Active = true });
        Services.Add(new Service { Id = "s-beard", BarberId = "moussa", Name = "Barbe dessinée", Description = "Taille, contours au rasoir et serviette chaude.", Price = 2000, Duration = 20, Active = true });
        Services.Add(new Service { Id = "s-burst", BarberId = "moussa", Name = "Burst Fade", Description = "Dégradé circulaire autour de l’oreille, dessus libre.", Price = 3500, Duration = 35, Active = true });
        Services.Add(new Service { Id = "s-locks", BarberId = "yann", Name = "Locks retwist", Description = "Entretien des racines et coiffage, soin inclus.", Price = 8000, Duration = 90, Active = true });
        Services.Add(new Service { Id = "s-braids", BarberId = "yann", Name = "Braids homme", Description = "Tresses plaquées vers l’arrière, motif au choix.", Price = 7000, Duration = 120, Active = true });
        Services.Add(new Service { Id = "s-buzz", BarberId = "yann", Name = "Buzz cut", Description = "Coupe rase uniforme à la tondeuse, contours propres.", Price = 2000, Duration = 20, Active = true });

        // 6. Appointments
        Appointments.Add(new Appointment
        {
            Id = "a1",
            BarberId = "chris",
            ServiceId = "s-low-fade",
            ServiceName = "Low Fade",
            DateLabel = "Samedi 12 octobre",
            Time = "15:00",
            Duration = 30,
            Price = 3000,
            Status = "confirmed",
            BarberName = "Chris Barber",
            BarberAvatar = "barber_chris.jpg",
            BarberAddress = "Rue des Jardins, Cocody Angré — Abidjan",
            BarberSpecialty = "Spécialiste Fade & Taper",
            BarberRating = 4.9
        });

        Appointments.Add(new Appointment
        {
            Id = "a2",
            BarberId = "moussa",
            ServiceId = "s-beard",
            ServiceName = "Barbe dessinée",
            DateLabel = "Mercredi 16 octobre",
            Time = "10:30",
            Duration = 20,
            Price = 2000,
            Status = "confirmed",
            BarberName = "Moussa Cuts",
            BarberAvatar = "barber_moussa.jpg",
            BarberAddress = "Avenue Chardy, Plateau — Abidjan",
            BarberSpecialty = "Afro & Barbe",
            BarberRating = 4.8
        });

        Appointments.Add(new Appointment
        {
            Id = "a3",
            BarberId = "yann",
            ServiceId = "s-locks",
            ServiceName = "Locks retwist",
            DateLabel = "Vendredi 18 octobre",
            Time = "09:00",
            Duration = 90,
            Price = 8000,
            Status = "pending",
            BarberName = "Yann Locks Studio",
            BarberAvatar = "barber_yann.jpg",
            BarberAddress = "Boulevard VGE, Marcory Zone 4 — Abidjan",
            BarberSpecialty = "Locks & Braids",
            BarberRating = 4.7
        });

        Appointments.Add(new Appointment
        {
            Id = "a4",
            BarberId = "chris",
            ServiceId = "s-taper",
            ServiceName = "Taper Fade",
            DateLabel = "Samedi 28 septembre",
            Time = "11:00",
            Duration = 35,
            Price = 3500,
            Status = "completed",
            BarberName = "Chris Barber",
            BarberAvatar = "barber_chris.jpg",
            BarberAddress = "Rue des Jardins, Cocody Angré — Abidjan",
            BarberSpecialty = "Spécialiste Fade & Taper",
            BarberRating = 4.9
        });

        Appointments.Add(new Appointment
        {
            Id = "a5",
            BarberId = "moussa",
            ServiceId = "s-afro",
            ServiceName = "Afro sculpté",
            DateLabel = "Mardi 10 septembre",
            Time = "14:00",
            Duration = 45,
            Price = 4000,
            Status = "completed",
            BarberName = "Moussa Cuts",
            BarberAvatar = "barber_moussa.jpg",
            BarberAddress = "Avenue Chardy, Plateau — Abidjan",
            BarberSpecialty = "Afro & Barbe",
            BarberRating = 4.8
        });

        Appointments.Add(new Appointment
        {
            Id = "a6",
            BarberId = "chris",
            ServiceId = "s-coupe-barbe",
            ServiceName = "Coupe + Barbe",
            DateLabel = "Jeudi 5 septembre",
            Time = "16:00",
            Duration = 45,
            Price = 5000,
            Status = "cancelled",
            BarberName = "Chris Barber",
            BarberAvatar = "barber_chris.jpg",
            BarberAddress = "Rue des Jardins, Cocody Angré — Abidjan",
            BarberSpecialty = "Spécialiste Fade & Taper",
            BarberRating = 4.9
        });

        // 7. Pro Today Bookings
        ProBookings.Add(new ProBooking { Id = "pb1", Client = "Kevin Aka", Service = "Taper Fade", Time = "10:30", Duration = 35, Price = 3500, Status = "pending", Phone = "+225 07 12 44 80 21" });
        ProBookings.Add(new ProBooking { Id = "pb2", Client = "Serge Koffi", Service = "Low Fade", Time = "11:30", Duration = 30, Price = 3000, Status = "accepted", Phone = "+225 05 66 10 32 47" });
        ProBookings.Add(new ProBooking { Id = "pb3", Client = "Ibrahim Touré", Service = "Coupe + Barbe", Time = "14:00", Duration = 45, Price = 5000, Status = "pending", Phone = "+225 01 23 98 76 54" });
        ProBookings.Add(new ProBooking { Id = "pb4", Client = "Junior Mensah", Service = "Low Fade", Time = "16:00", Duration = 30, Price = 3000, Status = "accepted", Phone = "+225 07 90 55 12 03" });

        // 8. Planning Week
        var mon = new PlanningDay { Id = "d1", ShortName = "Lun", Label = "Lundi 7 octobre", Date = 7 };
        mon.Slots.Add(new PlanningSlot { Id = "w1", Start = "10:30", Duration = 35, Client = "Kevin Aka", Service = "Taper Fade", Status = "pending" });
        mon.Slots.Add(new PlanningSlot { Id = "w2", Start = "11:30", Duration = 30, Client = "Serge Koffi", Service = "Low Fade", Status = "confirmed" });
        mon.Slots.Add(new PlanningSlot { Id = "w3", Start = "14:00", Duration = 45, Client = "Ibrahim Touré", Service = "Coupe + Barbe", Status = "pending" });
        mon.Slots.Add(new PlanningSlot { Id = "w4", Start = "16:00", Duration = 30, Client = "Junior Mensah", Service = "Low Fade", Status = "confirmed" });

        var tue = new PlanningDay { Id = "d2", ShortName = "Mar", Label = "Mardi 8 octobre", Date = 8 };
        tue.Slots.Add(new PlanningSlot { Id = "w5", Start = "09:00", Duration = 30, Client = "Aristide B.", Service = "Low Fade", Status = "confirmed" });
        tue.Slots.Add(new PlanningSlot { Id = "w6", Start = "15:00", Duration = 45, Client = "Paul E.", Service = "Coupe + Barbe", Status = "confirmed" });

        var wed = new PlanningDay { Id = "d3", ShortName = "Mer", Label = "Mercredi 9 octobre", Date = 9 };
        wed.Slots.Add(new PlanningSlot { Id = "w7", Start = "09:00", Duration = 35, Client = "Yves T.", Service = "Taper Fade", Status = "confirmed" });
        wed.Slots.Add(new PlanningSlot { Id = "w8", Start = "10:00", Duration = 30, Client = "Marc D.", Service = "Low Fade", Status = "confirmed" });
        wed.Slots.Add(new PlanningSlot { Id = "w9", Start = "11:00", Duration = 45, Client = "Cédric O.", Service = "Coupe + Barbe", Status = "confirmed" });
        wed.Slots.Add(new PlanningSlot { Id = "w10", Start = "14:00", Duration = 30, Client = "Loïc A.", Service = "Low Fade", Status = "confirmed" });
        wed.Slots.Add(new PlanningSlot { Id = "w11", Start = "15:00", Duration = 35, Client = "Henri K.", Service = "Taper Fade", Status = "confirmed" });
        wed.Slots.Add(new PlanningSlot { Id = "w12", Start = "16:30", Duration = 45, Client = "Tony M.", Service = "Coupe + Barbe", Status = "confirmed" });

        var thu = new PlanningDay { Id = "d4", ShortName = "Jeu", Label = "Jeudi 10 octobre", Date = 10 };
        thu.Slots.Add(new PlanningSlot { Id = "w13", Start = "11:00", Duration = 30, Client = "Franck S.", Service = "Low Fade", Status = "pending" });

        var fri = new PlanningDay { Id = "d5", ShortName = "Ven", Label = "Vendredi 11 octobre", Date = 11 };
        fri.Slots.Add(new PlanningSlot { Id = "w14", Start = "09:30", Duration = 35, Client = "Alain G.", Service = "Taper Fade", Status = "confirmed" });
        fri.Slots.Add(new PlanningSlot { Id = "w15", Start = "14:30", Duration = 45, Client = "Willy B.", Service = "Coupe + Barbe", Status = "confirmed" });
        fri.Slots.Add(new PlanningSlot { Id = "w16", Start = "16:00", Duration = 30, Client = "Eddy N.", Service = "Low Fade", Status = "pending" });

        var sat = new PlanningDay { Id = "d6", ShortName = "Sam", Label = "Samedi 12 octobre", Date = 12 };
        sat.Slots.Add(new PlanningSlot { Id = "w17", Start = "09:00", Duration = 30, Client = "Rémi K.", Service = "Low Fade", Status = "confirmed" });
        sat.Slots.Add(new PlanningSlot { Id = "w18", Start = "10:00", Duration = 35, Client = "Boris A.", Service = "Taper Fade", Status = "confirmed" });
        sat.Slots.Add(new PlanningSlot { Id = "w19", Start = "11:00", Duration = 45, Client = "Steve Y.", Service = "Coupe + Barbe", Status = "confirmed" });
        sat.Slots.Add(new PlanningSlot { Id = "w20", Start = "14:30", Duration = 30, Client = "Gildas P.", Service = "Low Fade", Status = "confirmed" });
        sat.Slots.Add(new PlanningSlot { Id = "w21", Start = "15:00", Duration = 30, Client = "Eric Kouamé", Service = "Low Fade", Status = "confirmed" });

        var sun = new PlanningDay { Id = "d7", ShortName = "Dim", Label = "Dimanche 13 octobre", Date = 13, Closed = true };

        PlanningWeek.Add(mon);
        PlanningWeek.Add(tue);
        PlanningWeek.Add(wed);
        PlanningWeek.Add(thu);
        PlanningWeek.Add(fri);
        PlanningWeek.Add(sat);
        PlanningWeek.Add(sun);

        // 9. Pro History
        ProHistory.Add(new HistoryEntry { Id = "h1", Client = "Eric Kouamé", Service = "Taper Fade", DateLabel = "Sam. 28 sept.", Price = 3500, Status = "completed" });
        ProHistory.Add(new HistoryEntry { Id = "h2", Client = "Kevin Aka", Service = "Low Fade", DateLabel = "Sam. 28 sept.", Price = 3000, Status = "completed" });
        ProHistory.Add(new HistoryEntry { Id = "h3", Client = "Serge Koffi", Service = "Coupe + Barbe", DateLabel = "Ven. 27 sept.", Price = 5000, Status = "completed" });
        ProHistory.Add(new HistoryEntry { Id = "h4", Client = "Paul E.", Service = "Low Fade", DateLabel = "Jeu. 26 sept.", Price = 3000, Status = "cancelled" });
        ProHistory.Add(new HistoryEntry { Id = "h5", Client = "Junior Mensah", Service = "Taper Fade", DateLabel = "Mer. 25 sept.", Price = 3500, Status = "completed" });

        // 10. Weekly Schedule
        WeeklySchedule.Add(new DaySchedule { Day = "Lundi", Enabled = true, Open = "09:00", Close = "18:00", HasBreak = true, BreakStart = "13:00", BreakEnd = "14:00" });
        WeeklySchedule.Add(new DaySchedule { Day = "Mardi", Enabled = true, Open = "09:00", Close = "18:00", HasBreak = true, BreakStart = "13:00", BreakEnd = "14:00" });
        WeeklySchedule.Add(new DaySchedule { Day = "Mercredi", Enabled = true, Open = "09:00", Close = "18:00", HasBreak = true, BreakStart = "13:00", BreakEnd = "14:00" });
        WeeklySchedule.Add(new DaySchedule { Day = "Jeudi", Enabled = true, Open = "09:00", Close = "18:00", HasBreak = false, BreakStart = "13:00", BreakEnd = "14:00" });
        WeeklySchedule.Add(new DaySchedule { Day = "Vendredi", Enabled = true, Open = "09:00", Close = "19:00", HasBreak = true, BreakStart = "13:00", BreakEnd = "14:00" });
        WeeklySchedule.Add(new DaySchedule { Day = "Samedi", Enabled = true, Open = "08:30", Close = "17:00", HasBreak = false, BreakStart = "13:00", BreakEnd = "14:00" });
        WeeklySchedule.Add(new DaySchedule { Day = "Dimanche", Enabled = false, Open = "10:00", Close = "14:00", HasBreak = false, BreakStart = "12:00", BreakEnd = "12:30" });

        // 11. Reviews
        Reviews.Add(new Review { Id = "r1", BarberId = "chris", Author = "Kevin Aka", Rating = 5, Comment = "Dégradé parfait, très propre. Chris prend le temps de comprendre ce qu’on veut.", Date = "28 sept." });
        Reviews.Add(new Review { Id = "r2", BarberId = "chris", Author = "Serge Koffi", Rating = 5, Comment = "Ponctuel, salon calme et résultat impeccable. Je ne vais plus ailleurs.", Date = "21 sept." });
        Reviews.Add(new Review { Id = "r3", BarberId = "chris", Author = "Ibrahim Touré", Rating = 4, Comment = "Très bonne coupe, juste un peu d’attente à l’arrivée.", Date = "14 sept." });
        Reviews.Add(new Review { Id = "r4", BarberId = "chris", Author = "Junior Mensah", Rating = 5, Comment = "Le meilleur taper de Cocody, sans hésiter.", Date = "2 sept." });
        Reviews.Add(new Review { Id = "r5", BarberId = "moussa", Author = "Fabrice N’Guessan", Rating = 5, Comment = "Afro sculpté comme je voulais, barbe nickel.", Date = "30 sept." });
        Reviews.Add(new Review { Id = "r6", BarberId = "moussa", Author = "Ange Yao", Rating = 4, Comment = "Travail soigné et conseils utiles pour l’entretien.", Date = "18 sept." });
        Reviews.Add(new Review { Id = "r7", BarberId = "yann", Author = "Patrick Diomandé", Rating = 5, Comment = "Retwist propre et durable, Yann est très minutieux.", Date = "25 sept." });
        Reviews.Add(new Review { Id = "r8", BarberId = "yann", Author = "Hervé Kouassi", Rating = 4, Comment = "Belles tresses, ambiance détendue.", Date = "9 sept." });

        // 12. Portfolio
        PortfolioItems.Add(new PortfolioItem { Id = "p1", Image = "style_low_fade.jpg", Title = "Low fade texturé", Category = "Fade" });
        PortfolioItems.Add(new PortfolioItem { Id = "p2", Image = "style_taper_fade.jpg", Title = "Taper naturel", Category = "Taper" });
        PortfolioItems.Add(new PortfolioItem { Id = "p3", Image = "style_burst_fade.jpg", Title = "Burst fade twists", Category = "Fade" });
        PortfolioItems.Add(new PortfolioItem { Id = "p4", Image = "style_beard.jpg", Title = "Barbe + line-up", Category = "Barbe" });
        PortfolioItems.Add(new PortfolioItem { Id = "p5", Image = "style_classic.jpg", Title = "Raie marquée", Category = "Classique" });
        PortfolioItems.Add(new PortfolioItem { Id = "p6", Image = "style_afro.jpg", Title = "Afro arrondi", Category = "Afro" });
        PortfolioItems.Add(new PortfolioItem { Id = "p7", Image = "style_buzz.jpg", Title = "Buzz cut net", Category = "Classique" });
        PortfolioItems.Add(new PortfolioItem { Id = "p8", Image = "style_locks.jpg", Title = "Locks entretien", Category = "Locks" });
        PortfolioItems.Add(new PortfolioItem { Id = "p9", Image = "style_braids.jpg", Title = "Cornrows", Category = "Braids" });

        // 13. Notifications
        Notifications.Add(new AppNotification { Id = "n1", Kind = "reminder", Title = "Rappel de rendez-vous", Body = "Low Fade avec Chris Barber samedi à 15:00. Pensez à arriver 5 min en avance.", Time = "Il y a 10 min", Group = "Aujourd'hui", Read = false });
        Notifications.Add(new AppNotification { Id = "n2", Kind = "confirmed", Title = "Réservation confirmée", Body = "Moussa Cuts a confirmé votre Barbe dessinée du mercredi 16 octobre.", Time = "Il y a 2 h", Group = "Aujourd'hui", Read = false });
        Notifications.Add(new AppNotification { Id = "n3", Kind = "message", Title = "Nouveau message", Body = "Yann Locks Studio : « Bonjour Eric, je vous confirme dès ce soir. »", Time = "Il y a 4 h", Group = "Aujourd'hui", Read = true });
        Notifications.Add(new AppNotification { Id = "n4", Kind = "review", Title = "Avis disponible", Body = "Comment s’est passé votre Taper Fade avec Chris Barber ? Laissez un avis.", Time = "Lundi", Group = "Cette semaine", Read = false });
        Notifications.Add(new AppNotification { Id = "n5", Kind = "cancelled", Title = "Rendez-vous annulé", Body = "Votre Coupe + Barbe du jeudi 5 septembre a été annulée.", Time = "Dimanche", Group = "Cette semaine", Read = true });
    }

    public bool IsFavorite(string id) => FavoriteIds.Contains(id);

    public void ToggleFavorite(string id)
    {
        if (FavoriteIds.Contains(id))
        {
            FavoriteIds.Remove(id);
        }
        else
        {
            FavoriteIds.Add(id);
        }

        // Update items in collections
        var s = HairStyles.FirstOrDefault(x => x.Id == id);
        if (s != null) s.IsFavorite = FavoriteIds.Contains(id);

        var b = Barbers.FirstOrDefault(x => x.Id == id);
        if (b != null) b.IsFavorite = FavoriteIds.Contains(id);
    }

    public Appointment CreateAppointment(string barberId, string serviceId, string dateLabel, string time)
    {
        var barber = GetBarber(barberId) ?? Barbers[0];
        var service = GetService(serviceId) ?? Services[0];

        var appt = new Appointment
        {
            Id = $"a{DateTimeOffset.UtcNow.ToUnixTimeMilliseconds()}",
            BarberId = barber.Id,
            ServiceId = service.Id,
            ServiceName = service.Name,
            DateLabel = dateLabel,
            Time = time,
            Duration = service.Duration,
            Price = service.Price,
            Status = "confirmed",
            BarberName = barber.Name,
            BarberAvatar = barber.Avatar,
            BarberAddress = barber.Address,
            BarberSpecialty = barber.Specialty,
            BarberRating = barber.Rating
        };

        Appointments.Insert(0, appt);
        return appt;
    }

    public void CancelAppointment(string appointmentId)
    {
        var appt = Appointments.FirstOrDefault(a => a.Id == appointmentId);
        if (appt != null)
        {
            appt.Status = "cancelled";
        }
    }

    public void AddReview(string barberId, int rating, string comment)
    {
        Reviews.Insert(0, new Review
        {
            Id = $"r{DateTimeOffset.UtcNow.ToUnixTimeMilliseconds()}",
            BarberId = barberId,
            Author = CurrentUser.FullName,
            Rating = rating,
            Comment = comment,
            Date = "Aujourd'hui"
        });
    }

    public void UpdateUserProfile(string firstName, string lastName, string email, string phone, string? avatar = null)
    {
        CurrentUser.FirstName = firstName;
        CurrentUser.LastName = lastName;
        CurrentUser.Email = email;
        CurrentUser.Phone = phone;
        if (!string.IsNullOrEmpty(avatar))
        {
            CurrentUser.Avatar = avatar;
        }
    }

    public void UpdateProProfile(string name, string bio, string phone, string address, string city, List<string> specialties, int experience)
    {
        CurrentPro.Name = name;
        CurrentPro.Bio = bio;
        CurrentPro.Address = $"{address} — {city}";
        CurrentPro.Location = $"{city}, Abidjan";
        CurrentPro.Experience = experience;
        if (specialties.Count > 0)
        {
            CurrentPro.Specialty = string.Join(" & ", specialties.Take(2));
        }
    }

    public void AddService(string name, string description, int price, int duration)
    {
        Services.Add(new Service
        {
            Id = $"s{DateTimeOffset.UtcNow.ToUnixTimeMilliseconds()}",
            BarberId = CurrentPro.Id,
            Name = name,
            Description = description,
            Price = price,
            Duration = duration,
            Active = true
        });
    }

    public void UpdateService(string id, string name, string description, int price, int duration)
    {
        var s = Services.FirstOrDefault(x => x.Id == id);
        if (s != null)
        {
            s.Name = name;
            s.Description = description;
            s.Price = price;
            s.Duration = duration;
        }
    }

    public void DeleteService(string id)
    {
        var s = Services.FirstOrDefault(x => x.Id == id);
        if (s != null)
        {
            Services.Remove(s);
        }
    }

    public void AddPortfolioItem(string title, string category, string image)
    {
        PortfolioItems.Insert(0, new PortfolioItem
        {
            Id = $"p{DateTimeOffset.UtcNow.ToUnixTimeMilliseconds()}",
            Title = title,
            Category = category,
            Image = image
        });
    }

    public void DeletePortfolioItem(string id)
    {
        var p = PortfolioItems.FirstOrDefault(x => x.Id == id);
        if (p != null)
        {
            PortfolioItems.Remove(p);
        }
    }

    public void UpdateProBookingStatus(string id, string status)
    {
        var b = ProBookings.FirstOrDefault(x => x.Id == id);
        if (b != null)
        {
            b.Status = status;
        }
    }

    public void MarkAllNotificationsRead()
    {
        foreach (var n in Notifications)
        {
            n.Read = true;
        }
    }

    public Barber? GetBarber(string id) => Barbers.FirstOrDefault(b => b.Id == id);
    public HairStyle? GetStyle(string id) => HairStyles.FirstOrDefault(s => s.Id == id);
    public Service? GetService(string id) => Services.FirstOrDefault(s => s.Id == id);
    public Appointment? GetAppointment(string id) => Appointments.FirstOrDefault(a => a.Id == id);
}
