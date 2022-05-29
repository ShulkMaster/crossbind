﻿namespace CrossBind.Model.Components;

public class ComponentModel : BindModel
{
    public readonly Extendable Extends;
    public string Body { get; init; }
    
    public ComponentModel(Extendable extends): base(ModelType.Component)
    {
        Extends = extends;
    }

}

public enum Extendable {
    Button,
    Select,
    TextBox,
    Component
}