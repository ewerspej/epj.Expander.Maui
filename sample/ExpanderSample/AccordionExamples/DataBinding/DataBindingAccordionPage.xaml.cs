namespace ExpanderSample.AccordionExamples.DataBinding;

public partial class DataBindingAccordionPage : ContentPage
{
    public DataBindingAccordionPage()
    {
        InitializeComponent();
        BindingContext = new DataBindingAccordionViewModel();
    }
}