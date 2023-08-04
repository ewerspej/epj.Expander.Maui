using CommunityToolkit.Mvvm.ComponentModel;
using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.Input;

namespace ExpanderSample;

public partial class MainViewModel : ObservableObject
{
    [ObservableProperty]
    private ObservableCollection<MyClass> _items;

    public async Task LoadAsync()
    {
        await Task.Delay(TimeSpan.FromSeconds(0.5));
        Items = new ObservableCollection<MyClass>
        {
            new()
            {
                Name = "Expander One"
            },
            new()
            {
                Name = "Expander Two"
            },
            new()
            {
                Name = "Expander Three"
            }
        };
    }

    [RelayCommand]
    private void Print(string text)
    {
        Console.WriteLine(text);
    }
}