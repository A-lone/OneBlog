using System;
using System.Collections;
using System.Linq;
namespace Microsoft.Security.Application.CodeCharts
{
	internal static class UpperMiddle
	{
		public static bool IsFlagSet(UpperMidCodeCharts flags, UpperMidCodeCharts flagToCheck)
		{
			return (flags & flagToCheck) != UpperMidCodeCharts.None;
		}
		public static IEnumerable CyrillicExtendedA()
		{
			return CodeChartHelper.GetRange(11744, 11775);
		}
		public static IEnumerable SupplementalPunctuation()
		{
			return CodeChartHelper.GetRange(11776, 11825);
		}
		public static IEnumerable CjkRadicalsSupplement()
		{
			return CodeChartHelper.GetRange(11904, 12019, (int i) => i == 11930);
		}
		public static IEnumerable KangxiRadicals()
		{
			return CodeChartHelper.GetRange(12032, 12245);
		}
		public static IEnumerable IdeographicDescriptionCharacters()
		{
			return CodeChartHelper.GetRange(12272, 12283);
		}
		public static IEnumerable CjkSymbolsAndPunctuation()
		{
			return CodeChartHelper.GetRange(12288, 12351);
		}
		public static IEnumerable Hiragana()
		{
			return CodeChartHelper.GetRange(12353, 12447, (int i) => i == 12439 || i == 12440);
		}
		public static IEnumerable Katakana()
		{
			return CodeChartHelper.GetRange(12448, 12543);
		}
		public static IEnumerable Bopomofo()
		{
			return CodeChartHelper.GetRange(12549, 12589);
		}
		public static IEnumerable HangulCompatibilityJamo()
		{
			return CodeChartHelper.GetRange(12593, 12686);
		}
		public static IEnumerable Kanbun()
		{
			return CodeChartHelper.GetRange(12688, 12703);
		}
		public static IEnumerable BopomofoExtended()
		{
			return CodeChartHelper.GetRange(12704, 12727);
		}
		public static IEnumerable CjkStrokes()
		{
			return CodeChartHelper.GetRange(12736, 12771);
		}
		public static IEnumerable KatakanaPhoneticExtensions()
		{
			return CodeChartHelper.GetRange(12784, 12799);
		}
		public static IEnumerable EnclosedCjkLettersAndMonths()
		{
			return CodeChartHelper.GetRange(12800, 13054, (int i) => i == 12831);
		}
		public static IEnumerable CjkCompatibility()
		{
			return CodeChartHelper.GetRange(13056, 13311);
		}
		public static IEnumerable CjkUnifiedIdeographsExtensionA()
		{
			return CodeChartHelper.GetRange(13312, 19893);
		}
		public static IEnumerable YijingHexagramSymbols()
		{
			return CodeChartHelper.GetRange(19904, 19967);
		}
		public static IEnumerable CjkUnifiedIdeographs()
		{
			return CodeChartHelper.GetRange(19968, 40907);
		}
		public static IEnumerable YiSyllables()
		{
			return CodeChartHelper.GetRange(40960, 42124);
		}
		public static IEnumerable YiRadicals()
		{
			return CodeChartHelper.GetRange(42128, 42182);
		}
		public static IEnumerable Lisu()
		{
			return CodeChartHelper.GetRange(42192, 42239);
		}
		public static IEnumerable Vai()
		{
			return CodeChartHelper.GetRange(42240, 42539);
		}
		public static IEnumerable CyrillicExtendedB()
		{
			return CodeChartHelper.GetRange(42560, 42647, (int i) => i == 42592 || i == 42593 || (i >= 42612 && i <= 42619));
		}
		public static IEnumerable Bamum()
		{
			return CodeChartHelper.GetRange(42656, 42743);
		}
		public static IEnumerable ModifierToneLetters()
		{
			return CodeChartHelper.GetRange(42752, 42783);
		}
		public static IEnumerable LatinExtendedD()
		{
			return CodeChartHelper.GetRange(42784, 42892).Concat(CodeChartHelper.GetRange(43003, 43007));
		}
		public static IEnumerable SylotiNagri()
		{
			return CodeChartHelper.GetRange(43008, 43051);
		}
		public static IEnumerable CommonIndicNumberForms()
		{
			return CodeChartHelper.GetRange(43056, 43065);
		}
		public static IEnumerable Phagspa()
		{
			return CodeChartHelper.GetRange(43072, 43127);
		}
		public static IEnumerable Saurashtra()
		{
			return CodeChartHelper.GetRange(43136, 43225, (int i) => i >= 43205 && i <= 43213);
		}
	}
}
