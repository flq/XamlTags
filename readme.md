## DynamicXaml

XamlTags is aimed to be what [HtmlTags][1] is to Html. Obviously, Xaml ain't Html, but
once in a while there may be a need to generate a Xaml Object tree programmatically. 

This is what this project is aimed at. Check out this example:

    var _builder = new XamBuilder();
    var button = 
      _builder.Start<Button>()
        .Margin("5,0")
        .WidthAndHeight(200d,30d)
        .Create();

You can

* Set several properties by combining them with __And__. In this case you must provide as many arguments as targetted properties
* Use e.g. a string to set a thickness object. DynamicXaml will attempt to find the fitting TypeConverter and use it to get the proper type

### Factory

This is how you get yourself a _Factory_:

    var b = new XamlBuilder();
    XamlFactory<Button> f = b.Start<Button>()
                             .MinWidth(100d)
                             .BindContent("Text")
                             .CreateFactory();
And then

`var button = f.Create(new ModelForBinding { Text = "A" });`

### Binding

As you can see, when you prefix a property with __Bind__, DynamicXaml will attempt to bind this property to the property of the DataContext you specify. You can also specify a converter:

`_xaml.BindVisibility("Visible", converter: new BooleanToVisibilityConverter());`

This expects a bool-field named __Visible__ on the object in the DataContext.

You can bind __directly to the DataContext__ by not specifying a path:

    // Binds straight to the DataContext
    _xaml.BindText();

If you need __one-way binding__, just say so:

    // Binds to the read-only property Text on the DataContext
    builder.Start<TextBox>().IsEnabled(false).OneWayBindText("Text");
    // Alternative syntax:
    builder.Start<TextBox>().IsEnabled(false).BindText("Text", oneway:true);
    
### Nesting

Nesting works by providing certain __Func__ types to a property which in turn produce a __Xaml__ object. There are two helper static methods to provide the correct type: __X.N__ (for nested), __X.NM__ (for nested-multi).

    var stackPanelContents = X.NM(b => new Xaml[]
    {
        b.Start<Image>().Width(124d),
        b.Start<TextBlock>().Text("Hello")
    });
    builder.Start<Button>()
           .Content(X.N(b => b.Start<StackPanel>().Children(stackPanelContents)));

### Referencing resources

Even though using resources to fill properties is somewhat limited compared to XAML-Usage, support is available. You can provide the assemblies to the XamlBuilder which you want to be scanned for Resource Dictionaries. Objects form those dictionaries can be referenced by using the __Static__-prefix in the dynamic call:

    _builder.GetResourcesFrom(typeof(App).Assembly);
    _builder.Start<Button>().Content(X.N(b => b.Start<Rectangle>().WidthAndHeight(100d,100d).StaticFill("red")));
    
You can also add resources to a WPF-Object you build:

    _builder.Start<Button>().AddResource("color", value);

### Support of attached properties

You can use attached properties within the DynamicXAML API with a call to __Attach__:

    _builder.Start<Button>()
        .Attach(Grid.RowProperty, 2)
        .Attach(Grid.ColumnProperty, 3);

If you want to bind an attached property to some value of the underlying DataContext, specify a __path__ as such:

`_builder.Start<Button>().Attach(Grid.RowProperty, path:"Row")`


Check out the tests as this project is _fully Test-driven_, so you can see which features are possible.

  [1]: http://htmltags.fubu-project.org/what-is-htmltags/