using System.Windows;
using DynamicXaml.ResourcesSystem;
using NUnit.Framework;
using XamlAppForTesting;

namespace XamlTags.Tests.Resources
{
    public class ResourceSystemTestContext
    {
        protected ResourceLoader _resourceLoader;

        [TestFixtureSetUp]
        public void Given()
        {
            if (Application.Current == null) new App(); //Awesome, require to make this subsystem work
            _resourceLoader = new ResourceLoader(typeof(MainWindow).Assembly);
            FixtureSetup();
        }

        protected virtual void FixtureSetup()
        {
            
        }
    }
}