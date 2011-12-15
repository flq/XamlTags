## XamlTags

XamlTags is aimed to be what [HtmlTags][1] is to Html. Obviously, Xaml ain't Html, but
once in a while there may be a need to generate a Xaml Object tree programmatically. 

This is what this project is aimed at. Check out this example:

    var _builder = new XamBuilder();
    var button = 
      _builder.Start<Button>()
        .Margin("5,0")
        .WidthAndHeight(200d,30d)
        .Create();

Check out the tests as this project is _fully Test-driven_, as it is also a project for me to learn F#

  [1]: http://htmltags.fubu-project.org/what-is-htmltags/