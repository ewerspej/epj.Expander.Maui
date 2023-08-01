using System.Windows.Input;

namespace epj.Expander.Maui;

public partial class Expander : ContentView
{
    public IView HeaderContent
    {
        get => (IView)GetValue(HeaderContentProperty);
        set => SetValue(HeaderContentProperty, value);
    }

    public View BodyContent
    {
        get => (View)GetValue(BodyContentProperty);
        set => SetValue(BodyContentProperty, value);
    }

    private bool _isExpanded;
    public bool IsExpanded
    {
        get => _isExpanded;
        set
        {
            if (_isExpanded == value) return;
            _isExpanded = value;
            Animate();
            OnPropertyChanged();
            IsExpandedChanged?.Invoke(this, new ExpandedEventArgs { Expanded = value });
        }
    }

    public bool Animated { get; set; }

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
    public static readonly BindableProperty HeaderContentProperty = BindableProperty.Create(nameof(HeaderContent), typeof(IView), typeof(Expander));
    public static readonly BindableProperty BodyContentProperty = BindableProperty.Create(nameof(BodyContent), typeof(View), typeof(Expander), defaultBindingMode: BindingMode.TwoWay);

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

    private void Animate()
    {
        if (!IsExpanded || !Animated)
        {
            return;
        }

        var size = BodyContent.Measure(0, 0);

        BodyContent.HeightRequest = 0;
        var animation1 = new Animation(h => BodyContent.HeightRequest = h, 0, size.Minimum.Height);

        BodyContent.TranslationY = -size.Minimum.Height;
        var animation2 = new Animation(h => BodyContent.TranslationY = h, -size.Minimum.Height, 0);

        animation1.Commit(this, "BodyContentHeight");
        animation2.Commit(this, "BodyContentTranslationY");
    }
}