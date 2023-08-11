using CommunityToolkit.Mvvm.ComponentModel;

namespace ExpanderSample.AccordionExamples.Hybrid;

public partial class HybridAccordionViewModel : ObservableObject
{
    [ObservableProperty]
    private List<string> _names;

    public HybridAccordionViewModel()
    {
        Names = new List<string>
        {
            "Jane",
            "John",
            "Jasmine"
        };
    }
}