using System;

namespace DynamicXaml
{
    public class SetterContext
    {
        public string PropertyName { get; private set; }
        public Type PropertyType { get; private set; }
        public object Value { get; private set; }
        public XamlBuilder Builder { get; internal set; }

        public SetterContext(string propertyName, Type propertyType, object value)
        {
            PropertyName = propertyName;
            PropertyType = propertyType;
            Value = value;
        }
    }
}