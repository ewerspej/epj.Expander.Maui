using System.Diagnostics;
using System.Windows.Input;

namespace epj.Expander.Maui;

public partial class Expander : ContentView
{
    private static bool _animationsEnabled;

    public View HeaderContent
    {
        get => (View)GetValue(HeaderContentProperty);
        set => SetValue(HeaderContentProperty, value);
    }

    public View BodyContent => GetTemplateChild("ExpanderContent") as ContentPresenter;

    private bool _isExpanded;
    public bool IsExpanded
    {
        get => _isExpanded;
        set
        {
            if (_isExpanded == value) return;
            _isExpanded = value;
            OnIsExpandedChanged();
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
    public static readonly BindableProperty HeaderContentProperty = BindableProperty.Create(nameof(HeaderContent), typeof(View), typeof(Expander));

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

    private async void OnIsExpandedChanged()
    {
        try
        {
            if (!CanAnimate() || BodyContent == null)
            {
                return;
            }

            var size = BodyContent.Measure(double.PositiveInfinity, double.PositiveInfinity);

            if (IsExpanded)
            {
                OnPropertyChanged(nameof(IsExpanded));

                BodyContent.HeightRequest = 0;
                BodyContent.TranslationY = -size.Minimum.Height;

                await Task.WhenAll(new List<Task>
                {
                    BodyContent.AnimateHeightAsync(0, size.Minimum.Height, Easing.CubicIn),
                    BodyContent.AnimateTranslationYAsync(-size.Minimum.Height, 0, Easing.CubicIn),
                });
            }
            else
            {
                await Task.WhenAll(new List<Task>
                {
                    BodyContent.AnimateTranslationYAsync(0, -size.Minimum.Height, Easing.CubicOut),
                    BodyContent.AnimateHeightAsync(size.Minimum.Height, 0, Easing.CubicOut),
                });

                BodyContent.HeightRequest = size.Minimum.Height;
            }
        }
        catch (Exception ex)
        {
            Debug.WriteLine(ex);
        }
        finally
        {
            OnPropertyChanged(nameof(IsExpanded));
            IsExpandedChanged?.Invoke(this, new ExpandedEventArgs { Expanded = IsExpanded });
        }
    }

    private bool CanAnimate() => _animationsEnabled && Animated;

    /// <summary>
    /// Animations are experimental at the moment, call this once in App.xaml.cs or MauiProgram.cs to enable animation. 
    /// Note: Animations are only supported on Android and iOS at the moment.
    /// </summary>
    /// <param name="enable"></param>
    public static void EnableAnimations(bool enable = true) => _animationsEnabled = enable;
}