using ExpanderSample.AccordionExamples.DataBinding;
using ExpanderSample.AccordionExamples.Hybrid;
using ExpanderSample.AccordionExamples.ViewOnly;

namespace ExpanderSample
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();

            Routing.RegisterRoute(nameof(ViewOnlyAccordionPage), typeof(ViewOnlyAccordionPage));
            Routing.RegisterRoute(nameof(HybridAccordionPage), typeof(HybridAccordionPage));
            Routing.RegisterRoute(nameof(DataBindingAccordionPage), typeof(DataBindingAccordionPage));
        }
    }
}