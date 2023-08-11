using ExpanderSample.AccordionExamples.DataBinding;
using ExpanderSample.AccordionExamples.Hybrid;
using ExpanderSample.AccordionExamples.ViewOnly;

namespace ExpanderSample
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
        }

        private async void OnViewOnlyButtonClicked(object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync(nameof(ViewOnlyAccordionPage));
        }

        private async void OnHybridButtonClicked(object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync(nameof(HybridAccordionPage));
        }

        private async void OnDataBindingButtonClicked(object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync(nameof(DataBindingAccordionPage));
        }
    }
}