using System.Collections.Generic;
using System.Linq;
using System.Windows;
using DynamicXaml.Extensions;

namespace DynamicXaml.MarkupSystem
{
    public static partial class QuickGrid
    {
        public class GridLengthBuilder
        {
            private readonly string _input;

            public GridLengthBuilder(string input)
            {
                _input = input;
            }

            public IEnumerable<GridLength> GetLengths()
            {
                var items = _input.Split(',');
                return items.SelectMany(Parse);
            }

            private static IEnumerable<GridLength> Parse(string item)
            {
                return EnumerableExtensions.Concat(
                    StarItem(item), 
                    RatioStarItem(item), 
                    PixelItem(item), 
                    AutoItem(item),
                    MultiplierItem(item));
            }

            private static IEnumerable<GridLength> StarItem(string item)
            {
                if (item.Equals("*"))
                    yield return new GridLength(1, GridUnitType.Star);
            }

            private static IEnumerable<GridLength> AutoItem(string item)
            {
                if (item.InvariantEquals("a") || item.InvariantEquals("auto"))
                    yield return new GridLength(1, GridUnitType.Auto);
            }

            private static IEnumerable<GridLength> RatioStarItem(string item)
            {
                int value;
                if (item.EndsWith("*") && int.TryParse(item.Substring(0, item.Length -1), out value))
                    yield return new GridLength(value, GridUnitType.Star);
            }

            private static IEnumerable<GridLength> MultiplierItem(string item)
            {
                if (!item.Contains("x"))
                    return Enumerable.Empty<GridLength>();
                var values = item.Split('x');
                var multiplier = int.Parse(values[0]);

                return Parse(values[1]).Multiply(multiplier, l => new GridLength(l.Value, l.GridUnitType));
            }

            private static IEnumerable<GridLength> PixelItem(string item)
            {
                int value;
                if (int.TryParse(item, out value))
                    yield return new GridLength(value);
            }
        }
    }
}