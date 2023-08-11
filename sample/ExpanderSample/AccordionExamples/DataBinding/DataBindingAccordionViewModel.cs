using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace ExpanderSample.AccordionExamples.DataBinding;

public partial class DataBindingAccordionViewModel : ObservableObject
{
    [ObservableProperty]
    private List<SomeDataModel> _items;

    public DataBindingAccordionViewModel()
    {
        Items = new List<SomeDataModel>
        {
            new()
            {
                Name = "Dan",
                Description = "Wants to be a surgeon"
            },
            new()
            {
                Name = "Dina",
                Description = "Wants to be a pirate"
            },
            new()
            {
                Name = "Dom",
                Description = "Wants to be a programmer"
            },
        };
    }

    [RelayCommand]
    private void SelectItem(SomeDataModel item)
    {
        item.Selected = !item.Selected;

        foreach (var model in Items)
        {
            if (model != item)
            {
                model.Selected = false;
            }
        }
    }
}