using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace ExpanderSample.AccordionExamples.DataBinding;

public partial class DataBindingAccordionViewModel : ObservableObject
{
    [ObservableProperty]
    private List<PersonModel> _items;

    public DataBindingAccordionViewModel()
    {
        Items = new List<PersonModel>
        {
            new()
            {
                Name = "Jane",
                Description = "Wants to be an actress"
            },
            new()
            {
                Name = "John",
                Description = "Wants to be a rock musician"
            },
            new()
            {
                Name = "Jasmine",
                Description = "Wants to be a heart surgeon"
            },
        };
    }

    [RelayCommand]
    private void SelectItem(PersonModel item)
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