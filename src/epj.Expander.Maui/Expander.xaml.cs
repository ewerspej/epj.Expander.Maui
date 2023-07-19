using System.Windows.Input;

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

    private bool _isExpanded;
    public bool IsExpanded
    {
        get => _isExpanded;
        set
        {
            if (_isExpanded == value) return;
            _isExpanded = value;
            OnPropertyChanged();

            IsExpandedChanged?.Invoke(this, new ExpandedEventArgs { Expanded = value });
        }
    }

    public ICommand Command
    {
        get => (ICommand)GetValue(CommandProperty);
        set => SetValue(CommandProperty, value);
    }

    public object CommandParameter
    {
        get => GetValue(CommandParameterProperty);
        set => SetValue(CommandParameterProperty, value);
    }

    public static readonly BindableProperty IsExpandedProperty = BindableProperty.Create(nameof(IsExpanded), typeof(bool), typeof(Expander), propertyChanged: OnIsExpandedPropertyChanged);
    public static readonly BindableProperty CommandProperty = BindableProperty.Create(nameof(Command), typeof(ICommand), typeof(Expander));
    public static readonly BindableProperty CommandParameterProperty = BindableProperty.Create(nameof(CommandParameter), typeof(object), typeof(Expander));

    private static void OnIsExpandedPropertyChanged(BindableObject bindable, object oldValue, object newValue)
    {
        ((Expander)bindable).IsExpanded = (bool)newValue;
    }

    public event EventHandler<ExpandedEventArgs> IsExpandedChanged;
    public event EventHandler<ExpandedEventArgs> HeaderTapped;

    public Expander()
    {
        InitializeComponent();
    }

    private void OnHeaderContentTapped(object sender, TappedEventArgs e)
    {
        IsExpanded = !IsExpanded;
        HeaderTapped?.Invoke(this, new ExpandedEventArgs { Expanded = IsExpanded });
        Command?.Execute(CommandParameter);
    }
}