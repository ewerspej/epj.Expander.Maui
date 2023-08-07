# epj.Expander.Maui

![License](https://img.shields.io/github/license/ewerspej/epj.Expander.Maui)
[![Nuget](https://img.shields.io/nuget/v/epj.Expander.Maui)](https://www.nuget.org/packages/epj.Expander.Maui/)

A simple Expander control for .NET MAUI

## Summary

This is a simple Expander control for .NET MAUI which can be used standalone as well as like accordion. The header and body are fully customizable and the expanding and collapsing of the body can be animated.

***Note**: Animations are experimental at the moment and require an explicit opt-in.*

<div style="display: flex; justify-content: center">
    <img src="https://github.com/ewerspej/epj.Expander.Maui/blob/main/assets/sample_00.PNG" width="360" />
</div>

## Platforms

| Platform    | Supported  |
|-------------|------------|
| Android     | Yes        |
| iOS         | Yes        |
| Windows     | Yes        |
| MacCatalyst | *Untested* |

## Usage

### Registration *(not required at this moment)*

Registration of the control strictly isn't required, because it only uses standard controls natively supported by .NET MAUI at the moment. However, a registration method is provided in case that some future version requires to do so:

```c#
public static MauiApp CreateMauiApp()
{
    var builder = MauiApp.CreateBuilder();
    builder
        .UseMauiApp<App>()
        .UseExpander() // optional: add this
        .ConfigureFonts(fonts =>
        {
            fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
            fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
        });

    return builder.Build();
}
```

### XAML

```xml
<maui:Expander
  Command="{Binding Source={RelativeSource AncestorType={x:Type expanderSample:MainViewModel}}, Path=PrintCommand}"
  CommandParameter="{Binding Name}"
  HeaderTapped="Expander_OnHeaderTapped">

  <!-- add header content -->
  <maui:Expander.HeaderContent>
    <Grid
      HeightRequest="80"
      BackgroundColor="Orange">
      <Label
        Text="{Binding Name}"
        VerticalOptions="Center"
        HorizontalOptions="Center" />
    </Grid>
  </maui:Expander.HeaderContent>

  <!-- add body content -->
  <Grid
    HeightRequest="200"
    BackgroundColor="SkyBlue">
    <Label
      Text="{Binding Name}"
      VerticalOptions="Center"
      HorizontalOptions="Center" />
  </Grid>
</maui:Expander>
```

## Properties

Most of these properties are bindable for MVVM goodness. Please let me know if something is missing or not working as expected.

| Type       | Property             | Description                                                             | Default Value |
|------------|----------------------|-------------------------------------------------------------------------|---------------|
| View       | HeaderContent        | Use to set the header content, can be any type of View or Layout        | null          |
| View       | BodyContent          | Use to set the body content, can be any type of View or Layout          | null          |
| ICommand   | Command              | The command to be invoked when the header is tapped                     | null          |
| object     | CommandParameter     | The optional command parameter                                          | null          |
| bool       | IsExpanded           | Use to get or set whether the Expander is expanded.                     | false         |
| bool       | Animate              | Use to enable animations - requires opt-in, see below under [Animation](https://github.com/ewerspej/epj.Expander.Maui#animations) | false         |
| Easing     | ExpandEasing         | Use to set the type of [Easing](https://learn.microsoft.com/dotnet/api/microsoft.maui.easing) for the expand animation             | CubicIn       |
| Easing     | CollapseEasing       | Use to set the type of [Easing](https://learn.microsoft.com/dotnet/api/microsoft.maui.easing) for the collapse animation           | CubicOut      |
| uint       | ExpandDuration       | Use to set the duration of the expand animation in milliseconds         | 250           |
| uint       | CollapseDuration     | Use to set the duration of the collapse animation in milliseconds       | 250           |

## Events

The control exposes two events:

* `HeaderTapped`: Fires when the header is tapped by the user
* `IsExpandedChanged`: Fires whenever the `IsExpanded` property has changed

## Animations

Animations are currently experimental, the implementation is not well tested and may be subject to change in the future. Generally, animations come with a performance penalty and when used extensively, they may slow down the application.

Therefore, animations at this time require an explicit opt-in. Anywhere in your code, but ideally in *MauiProgram.cs*, add the following line of code:


```c#
Expander.EnableAnimations();
```

After that, setting the `Animate` property to `True` will take effect:

```xml
<maui:Expander
  Animate="True"
  CollapseDuration="100"
  ExpandDuration="200"
  CollapseEasing="{x:Static Easing.SpringOut}"
  ExpandEasing="{x:Static Easing.SpringIn}">
  <maui:Expander.HeaderContent>
    <!-- header -->
  </maui:Expander.HeaderContent>
  <!-- body -->
</maui:Expander>
```

# Examples

If you are looking for a more extensive example for the potential usage of the control, e.g. if you are looking for a demo of the accordion-style functionality, have a look at the code in this repository. The sample app provides some starting points.
