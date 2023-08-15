using CommunityToolkit.Mvvm.ComponentModel;

namespace ExpanderSample.AccordionExamples.Hybrid;

public class PersonTuple : Tuple<string, string>
{
    public PersonTuple(string item1, string item2) : base(item1, item2) { }
}

public partial class HybridAccordionViewModel : ObservableObject
{
    [ObservableProperty]
    private List<PersonTuple> _people;

    public HybridAccordionViewModel()
    {
        People = new List<PersonTuple>
        {
            new("Jane", "Wants to be an actress"),
            new("John", "Wants to be a rock musician"),
            new("Jasmine", "Wants to be a heart surgeon")
        };
    }
}