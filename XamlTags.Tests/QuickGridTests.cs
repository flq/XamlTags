using System.Collections;
using System.Windows;
using System.Windows.Controls;
using DynamicXaml.MarkupSystem;
using NUnit.Framework;
using FluentAssertions;

namespace XamlTags.Tests
{
    /// <summary>
    /// We only test for rows as the logic is the same for all three attached properties
    /// </summary>
    [TestFixture]
    public class QuickGridTests
    {
        private Grid _grid;

        private static GridLength OneStar = new GridLength(1, GridUnitType.Star);
        private static GridLength ThreeStar = new GridLength(3, GridUnitType.Star);
        private static GridLength OneHundredPixel = new GridLength(100, GridUnitType.Pixel);
        private static GridLength Auto = new GridLength(1, GridUnitType.Auto);

        [SetUp]
        public void Given()
        {
            _grid = new Grid();
        }

        [Test]
        [TestCaseSource("GridTests")]
        public void quick_grid_works_correctly(string value, LengthCollection expected)
        {
            QuickGrid.SetColumns(_grid, value);
            expected.Compare(_grid);
        }

        public static IEnumerable GridTests()
        {
            yield return new object[] { "*", new LengthCollection(OneStar) };
            yield return new object[] { "*,*", new LengthCollection(OneStar,OneStar) };
            yield return new object[] { "3*", new LengthCollection(ThreeStar) };
            yield return new object[] { "3*,*", new LengthCollection(ThreeStar, OneStar) };
            yield return new object[] { "2x*", new LengthCollection(OneStar, OneStar) };
            yield return new object[] { "2x*,3*", new LengthCollection(OneStar, OneStar, ThreeStar) };
            yield return new object[] { "100", new LengthCollection(OneHundredPixel) };
            yield return new object[] { "a", new LengthCollection(Auto) };
            yield return new object[] { "A", new LengthCollection(Auto) };
            yield return new object[] { "Auto", new LengthCollection(Auto) };
            yield return new object[] { "2x100,a,3*", new LengthCollection(OneHundredPixel, OneHundredPixel, Auto, ThreeStar) };
            yield return new object[] { "2x100,a,3*,2x*", new LengthCollection(OneHundredPixel, OneHundredPixel, Auto, ThreeStar, OneStar, OneStar) };
        }

        public class LengthCollection
        {
            private readonly GridLength[] _lengths;

            public LengthCollection(params GridLength[] lengths)
            {
                _lengths = lengths;
            }

            public void Compare(Grid g)
            {
                g.ColumnDefinitions.Should().HaveCount(_lengths.Length, "correct number of columns was created");
                int count = 0;
                foreach (var cd in g.ColumnDefinitions)
                {
                    cd.Width.GridUnitType.Should().Be(_lengths[count].GridUnitType, "Gridunittype of column " + (count+1) + " must be the same");
                    cd.Width.Value.Should().BeInRange(_lengths[count].Value, _lengths[count].Value, "Width of column " + (count+1) + " must be the same");
                    count++;
                }
            }
        }
    }

    

}