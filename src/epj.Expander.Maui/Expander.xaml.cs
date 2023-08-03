using System.Diagnostics;
using System.Windows.Input;

namespace epj.Expander.Maui;

public partial class Expander : ContentView
{
    public IView HeaderContent
    {
        get => (IView)GetValue(HeaderContentProperty);
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
            _ = OnIsExpandedAsync();
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

    private async Task OnIsExpandedAsync()
    {
        try
        {
            if (!Animated)
            {
                return;
            }

            var size = BodyContent.Measure(0, 0);

            if (IsExpanded)
            {
                OnPropertyChanged(nameof(IsExpanded));

                BodyContent.HeightRequest = 0;
                BodyContent.TranslationY = -size.Minimum.Height;

                await Task.WhenAll(new List<Task>
                {
                    BodyContent.AnimateHeightAsync(0, size.Minimum.Height),
                    BodyContent.AnimateTranslationYAsync(-size.Minimum.Height, 0)
                });
            }
            else
            {
                await Task.WhenAll(new List<Task>
                {
                    BodyContent.AnimateHeightAsync(size.Minimum.Height, 0),
                    BodyContent.AnimateTranslationYAsync(0, -size.Minimum.Height)
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
}