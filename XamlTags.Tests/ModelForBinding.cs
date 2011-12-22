namespace XamlTags.Tests
{
    public class ModelForBinding
    {
        public string Text { get; set; }
        public bool Visible { get { return false; } }
    }

    public class ModelForBindingReadOnly
    {
        public ModelForBindingReadOnly(string text)
        {
            Text = text;
        }

        public string Text { get; private set; }
    }
}