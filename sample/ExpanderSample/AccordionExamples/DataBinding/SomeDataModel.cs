﻿using CommunityToolkit.Mvvm.ComponentModel;

namespace ExpanderSample.AccordionExamples.DataBinding;

public partial class SomeDataModel : ObservableObject
{
    [ObservableProperty]
    private string _name;

    [ObservableProperty]
    private string _description;

    [ObservableProperty]
    private bool _selected;
}