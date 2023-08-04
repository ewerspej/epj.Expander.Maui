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
    }
}