using System;

namespace XamlTags.Tests
{
    public class ModelForBinding
    {
        public string Text { get; set; }
        public bool Visible { get { return false; } }
    }

    public class ModelForBindingRow
    {
        public int Row { get; private set; }

        public ModelForBindingRow(int row)
        {
            Row = row;
        }
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