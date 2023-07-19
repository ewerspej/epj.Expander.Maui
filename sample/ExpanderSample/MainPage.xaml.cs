namespace ExpanderSample
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
            BindingContext = new MainViewModel();

            // uncomment the following to enable accordion-like sample functionality

            //ExpanderOne.HeaderTapped += (_, e) =>
            //{
            //    if (e.Expanded)
            //    {
            //        ExpanderTwo.IsExpanded = false;
            //    }
            //};

            //ExpanderTwo.HeaderTapped += (_, e) =>
            //{
            //    if (e.Expanded)
            //    {
            //        ExpanderOne.IsExpanded = false;
            //    }
            //};
        }
    }
}