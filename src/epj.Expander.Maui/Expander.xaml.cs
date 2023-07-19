namespace epj.Expander.Maui;

public partial class Expander : ContentView
{
    private View _headerContent;
    public View HeaderContent
    {
        get => _headerContent ??= new ContentView();
        set
        {
            if (_headerContent == value) return;
            _headerContent = value;
            OnPropertyChanged();
        }
    }

    public bool IsExpanded
    {
        get => (bool)GetValue(IsExpandedProperty);
        set => SetValue(IsExpandedProperty, value);
    }

    public static readonly BindableProperty IsExpandedProperty = BindableProperty.Create(nameof(IsExpanded), typeof(bool), typeof(Expander));

    public event EventHandler<ExpandedEventArgs> IsExpandedChanged;

    public Expander()
    {
        InitializeComponent();
    }

    private void OnHeaderContentTapped(object sender, TappedEventArgs e)
    {
        IsExpanded = !IsExpanded;
        IsExpandedChanged?.Invoke(this, new ExpandedEventArgs { Expanded = IsExpanded });
    }
}