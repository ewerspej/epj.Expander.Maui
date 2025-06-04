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
            try
            {
                await Shell.Current.GoToAsync(nameof(ViewOnlyAccordionPage));
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception);
            }
        }

        private async void OnHybridButtonClicked(object sender, EventArgs e)
        {
            try
            {
                await Shell.Current.GoToAsync(nameof(HybridAccordionPage));
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception);
            }
        }

        private async void OnDataBindingButtonClicked(object sender, EventArgs e)
        {
            try
            {
                await Shell.Current.GoToAsync(nameof(DataBindingAccordionPage));
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception);
            }
        }
    }
}