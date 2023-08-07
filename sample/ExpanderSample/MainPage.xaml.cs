using epj.Expander.Maui;

namespace ExpanderSample
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();

            var viewModel = new MainViewModel();
            _ = viewModel.LoadAsync();
            BindingContext = viewModel;
        }

        private void Expander_OnHeaderTapped(object sender, ExpandedEventArgs e)
        {
            // uncomment this to enable accordion-style functionality
            // note: this is just a demo implementation, there are other ways to do this

            //if (sender is not Expander expander)
            //{
            //    return;
            //}
            //
            //foreach (var child in ExpanderLayout.Children)
            //{
            //    if (child is not Expander other)
            //    {
            //        continue;
            //    }
            //
            //    if (other != expander)
            //    {
            //        other.IsExpanded = false;
            //    }
            //}
        }
    }
}