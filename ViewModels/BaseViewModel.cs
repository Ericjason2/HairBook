using CommunityToolkit.Mvvm.ComponentModel;
using HairBook.Services;

namespace HairBook.ViewModels;

public abstract partial class BaseViewModel : ObservableObject
{
    public IDataService DataService { get; }

    [ObservableProperty]
    private bool _isBusy;

    [ObservableProperty]
    private string _title = string.Empty;

    public BaseViewModel(IDataService dataService)
    {
        DataService = dataService;
    }
}
