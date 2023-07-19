using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace ExpanderSample;

public partial class MainViewModel : ObservableObject
{
    [RelayCommand]
    private void DoSomething(string arg)
    {
        Console.WriteLine(arg);
    }
}