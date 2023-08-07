using System.Diagnostics;
using System.Windows.Input;

namespace epj.Expander.Maui;

[ContentProperty(nameof(BodyContent))]
public class Expander : ContentView
{
    private static bool _animationsEnabled;

    private readonly SemaphoreSlim _semaphoreSlim = new(1, 1);

    private Grid HeaderGrid { get; }
    private Grid BodyGrid { get; }

    /// <summary>
    /// Use to set the header content of the expander
    /// </summary>
    public View HeaderContent
    {
        get => (View)GetValue(HeaderContentProperty);
        set => SetValue(HeaderContentProperty, value);
    }

    /// <summary>
    /// Use to set the body content of the expander
    /// </summary>
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
            if (_isExpanded == value)
            {
                return;
            }
            _isExpanded = value;
            OnIsExpandedChanged();
        }
    }

    /// <summary>
    /// Animations are experimental at the moment. You need to call Expander.EnableAnimations() first before setting this to true.
    /// </summary>
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

    public Easing ExpandEasing
    {
        get => (Easing)GetValue(ExpandEasingProperty);
        set => SetValue(ExpandEasingProperty, value);
    }

    public Easing CollapseEasing
    {
        get => (Easing)GetValue(CollapseEasingProperty);
        set => SetValue(CollapseEasingProperty, value);
    }

    private uint _expandDuration = 250;
    public uint ExpandDuration
    {
        get => _expandDuration;
        set
        {
            if (_expandDuration == value)
            {
                return;
            }
            _expandDuration = value;
            OnPropertyChanged();
        }
    }

    private uint _collapseDuration = 250;
    public uint CollapseDuration
    {
        get => _collapseDuration;
        set
        {
            if (_collapseDuration == value)
            {
                return;
            }
            _collapseDuration = value;
            OnPropertyChanged();
        }
    }

    public static readonly BindableProperty IsExpandedProperty = BindableProperty.Create(nameof(IsExpanded), typeof(bool), typeof(Expander), propertyChanged: OnIsExpandedPropertyChanged);
    public static readonly BindableProperty CommandProperty = BindableProperty.Create(nameof(Command), typeof(ICommand), typeof(Expander));
    public static readonly BindableProperty CommandParameterProperty = BindableProperty.Create(nameof(CommandParameter), typeof(object), typeof(Expander));
    public static readonly BindableProperty HeaderContentProperty = BindableProperty.Create(nameof(HeaderContent), typeof(View), typeof(Expander), propertyChanged: OnHeaderContentPropertyChanged);
    public static readonly BindableProperty BodyContentProperty = BindableProperty.Create(nameof(BodyContent), typeof(View), typeof(Expander), propertyChanged: OnContentPropertyChanged);
    public static readonly BindableProperty ExpandEasingProperty = BindableProperty.Create(nameof(ExpandEasing), typeof(Easing), typeof(Expander), defaultValue: Easing.CubicIn);
    public static readonly BindableProperty CollapseEasingProperty = BindableProperty.Create(nameof(CollapseEasing), typeof(Easing), typeof(Expander), defaultValue: Easing.CubicOut);
    public static readonly BindableProperty ExpandDurationProperty = BindableProperty.Create(nameof(ExpandDuration), typeof(int), typeof(Expander), defaultValue: 250, propertyChanged: OnExpandDurationPropertyChanged);
    public static readonly BindableProperty CollapseDurationProperty = BindableProperty.Create(nameof(CollapseDuration), typeof(int), typeof(Expander), defaultValue: 250, propertyChanged: OnCollapseDurationPropertyChanged);

    private static void OnIsExpandedPropertyChanged(BindableObject bindable, object oldValue, object newValue)
    {
        ((Expander)bindable).IsExpanded = (bool)newValue;
    }

    private static void OnExpandDurationPropertyChanged(BindableObject bindable, object oldValue, object newValue)
    {
        var expandDuration = (int)newValue;

        if (expandDuration <= 0)
        {
            throw new ArgumentOutOfRangeException($"Value for {nameof(ExpandDuration)} must be larger than 0");
        }

        ((Expander)bindable).ExpandDuration = (uint)expandDuration;
    }

    private static void OnCollapseDurationPropertyChanged(BindableObject bindable, object oldValue, object newValue)
    {
        var collapseDuration = (int)newValue;

        if (collapseDuration <= 0)
        {
            throw new ArgumentOutOfRangeException($"Value for {nameof(CollapseDuration)} must be larger than 0");
        }

        ((Expander)bindable).CollapseDuration = (uint)collapseDuration;
    }

    private static void OnHeaderContentPropertyChanged(BindableObject bindable, object oldValue, object newValue)
    {
        var expander = (Expander)bindable;

        if (expander.HeaderGrid is not { } grid || newValue is not View view)
        {
            return;
        }

        if (oldValue is View oldView)
        {
            grid.Remove(oldView);
        }

        grid.Add(view);
    }

    private static void OnContentPropertyChanged(BindableObject bindable, object oldValue, object newValue)
    {
        var expander = (Expander)bindable;

        if (expander.BodyGrid is not { } grid || newValue is not View view)
        {
            return;
        }

        if (oldValue is View oldView)
        {
            grid.Remove(oldView);
        }

        grid.Add(view);
    }

    public event EventHandler<ExpandedEventArgs> IsExpandedChanged;
    public event EventHandler<ExpandedEventArgs> HeaderTapped;

    public Expander()
    {
        HeaderGrid = new Grid
        {
            ZIndex = 500,
            HorizontalOptions = LayoutOptions.Fill,
            VerticalOptions = LayoutOptions.Start
        };

        var tapGestureRecognizer = new TapGestureRecognizer();
        tapGestureRecognizer.Tapped += OnHeaderContentTapped;
        HeaderGrid.GestureRecognizers.Add(tapGestureRecognizer);

        BodyGrid = new Grid
        {
            ZIndex = 0,
            HorizontalOptions = LayoutOptions.Fill,
            VerticalOptions = LayoutOptions.Start,
            IsClippedToBounds = true
        };

        BodyGrid.SetBinding(Grid.IsVisibleProperty, new Binding(nameof(IsExpanded), source: this));

        var expanderGrid = new Grid
        {
            RowDefinitions =
            {
                new RowDefinition(GridLength.Auto),
                new RowDefinition(GridLength.Auto),
            }
        };

        expanderGrid.Add(HeaderGrid);
        expanderGrid.SetRow(HeaderGrid, 0);

        expanderGrid.Add(BodyGrid);
        expanderGrid.SetRow(BodyGrid, 1);

        Content = expanderGrid;
    }

    private void OnHeaderContentTapped(object sender, TappedEventArgs e)
    {
        IsExpanded = !IsExpanded;
        HeaderTapped?.Invoke(this, new ExpandedEventArgs { Expanded = IsExpanded });
        Command?.Execute(CommandParameter);
    }

    private async void OnIsExpandedChanged()
    {
        var notified = false;

        try
        {
            await _semaphoreSlim.WaitAsync();

            if (!CanAnimate() || BodyContent == null)
            {
                return;
            }

            var size = BodyContent.Measure(double.PositiveInfinity, double.PositiveInfinity);

            if (IsExpanded)
            {
                BodyContent.HeightRequest = 0;
                BodyContent.TranslationY = -size.Minimum.Height;

                OnPropertyChanged(nameof(IsExpanded));
                notified = true;

                await Task.WhenAll(new List<Task>
                {
                    BodyContent.AnimateHeightAsync(0, size.Minimum.Height, ExpandEasing, ExpandDuration),
                    BodyContent.AnimateTranslationYAsync(-size.Minimum.Height, 0, ExpandEasing, ExpandDuration)
                });
            }
            else
            {
                await Task.WhenAll(new List<Task>
                {
                    BodyContent.AnimateTranslationYAsync(0, -size.Minimum.Height, CollapseEasing, CollapseDuration),
                    BodyContent.AnimateHeightAsync(size.Minimum.Height, 0, CollapseEasing, CollapseDuration)
                });

                OnPropertyChanged(nameof(IsExpanded));
                notified = true;

                BodyContent.HeightRequest = size.Minimum.Height;
            }
        }
        catch (Exception ex)
        {
            Debug.WriteLine(ex);
        }
        finally
        {
            if (!notified)
            {
                OnPropertyChanged(nameof(IsExpanded));
            }
            IsExpandedChanged?.Invoke(this, new ExpandedEventArgs { Expanded = IsExpanded });

            _semaphoreSlim.Release();
        }
    }

    private bool CanAnimate() => _animationsEnabled && Animated;

    /// <summary>
    /// Animations are experimental at the moment, call this once in App.xaml.cs or MauiProgram.cs to enable animation. 
    /// </summary>
    /// <param name="enable"></param>
    public static void EnableAnimations(bool enable = true) => _animationsEnabled = enable;
}