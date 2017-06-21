using System;
using System.Collections;
namespace Microsoft.Security.Application.CodeCharts
{
	internal static class Middle
	{
		public static bool IsFlagSet(MidCodeCharts flags, MidCodeCharts flagToCheck)
		{
			return (flags & flagToCheck) != MidCodeCharts.None;
		}
		public static IEnumerable GreekExtended()
		{
			return CodeChartHelper.GetRange(7936, 8190, (int i) => i == 7958 || i == 7959 || i == 7966 || i == 7967 || i == 8006 || i == 8007 || i == 8014 || i == 8015 || i == 8024 || i == 8026 || i == 8028 || i == 8030 || i == 8062 || i == 8063 || i == 8117 || i == 8133 || i == 8148 || i == 8149 || i == 8156 || i == 8176 || i == 8177 || i == 8181);
		}
		public static IEnumerable GeneralPunctuation()
		{
			return CodeChartHelper.GetRange(8192, 8303, (int i) => i >= 8293 && i <= 8297);
		}
		public static IEnumerable SuperscriptsAndSubscripts()
		{
			return CodeChartHelper.GetRange(8304, 8340, (int i) => i == 8306 || i == 8307 || i == 8335);
		}
		public static IEnumerable CurrencySymbols()
		{
			return CodeChartHelper.GetRange(8352, 8376);
		}
		public static IEnumerable CombiningDiacriticalMarksForSymbols()
		{
			return CodeChartHelper.GetRange(8400, 8432);
		}
		public static IEnumerable LetterlikeSymbols()
		{
			return CodeChartHelper.GetRange(8448, 8527);
		}
		public static IEnumerable NumberForms()
		{
			return CodeChartHelper.GetRange(8528, 8585);
		}
		public static IEnumerable Arrows()
		{
			return CodeChartHelper.GetRange(8592, 8703);
		}
		public static IEnumerable MathematicalOperators()
		{
			return CodeChartHelper.GetRange(8704, 8959);
		}
		public static IEnumerable MiscellaneousTechnical()
		{
			return CodeChartHelper.GetRange(8960, 9192);
		}
		public static IEnumerable ControlPictures()
		{
			return CodeChartHelper.GetRange(9216, 9254);
		}
		public static IEnumerable OpticalCharacterRecognition()
		{
			return CodeChartHelper.GetRange(9280, 9290);
		}
		public static IEnumerable EnclosedAlphanumerics()
		{
			return CodeChartHelper.GetRange(9312, 9471);
		}
		public static IEnumerable BoxDrawing()
		{
			return CodeChartHelper.GetRange(9472, 9599);
		}
		public static IEnumerable BlockElements()
		{
			return CodeChartHelper.GetRange(9600, 9631);
		}
		public static IEnumerable GeometricShapes()
		{
			return CodeChartHelper.GetRange(9632, 9727);
		}
		public static IEnumerable MiscellaneousSymbols()
		{
			return CodeChartHelper.GetRange(9728, 9983, (int i) => i == 9934 || i == 9954 || (i >= 9956 && i <= 9959));
		}
		public static IEnumerable Dingbats()
		{
			return CodeChartHelper.GetRange(9985, 10174, (int i) => i == 9989 || i == 9994 || i == 9995 || i == 10024 || i == 10060 || i == 10062 || i == 10067 || i == 10068 || i == 10069 || i == 10079 || i == 10080 || i == 10133 || i == 10134 || i == 10135 || i == 10160);
		}
		public static IEnumerable MiscellaneousMathematicalSymbolsA()
		{
			return CodeChartHelper.GetRange(10176, 10223, (int i) => i == 10187 || i == 10189 || i == 10190 || i == 10191);
		}
		public static IEnumerable SupplementalArrowsA()
		{
			return CodeChartHelper.GetRange(10224, 10239);
		}
		public static IEnumerable BraillePatterns()
		{
			return CodeChartHelper.GetRange(10240, 10495);
		}
		public static IEnumerable SupplementalArrowsB()
		{
			return CodeChartHelper.GetRange(10496, 10623);
		}
		public static IEnumerable MiscellaneousMathematicalSymbolsB()
		{
			return CodeChartHelper.GetRange(10624, 10751);
		}
		public static IEnumerable SupplementalMathematicalOperators()
		{
			return CodeChartHelper.GetRange(10752, 11007);
		}
		public static IEnumerable MiscellaneousSymbolsAndArrows()
		{
			return CodeChartHelper.GetRange(11008, 11097, (int i) => i == 11085 || i == 11086 || i == 11087);
		}
		public static IEnumerable Glagolitic()
		{
			return CodeChartHelper.GetRange(11264, 11358, (int i) => i == 11311);
		}
		public static IEnumerable LatinExtendedC()
		{
			return CodeChartHelper.GetRange(11360, 11391);
		}
		public static IEnumerable Coptic()
		{
			return CodeChartHelper.GetRange(11392, 11519, (int i) => i >= 11506 && i <= 11512);
		}
		public static IEnumerable GeorgianSupplement()
		{
			return CodeChartHelper.GetRange(11520, 11557);
		}
		public static IEnumerable Tifinagh()
		{
			return CodeChartHelper.GetRange(11568, 11631, (int i) => i >= 11622 && i <= 11630);
		}
		public static IEnumerable EthiopicExtended()
		{
			return CodeChartHelper.GetRange(11648, 11742, (int i) => (i >= 11671 && i <= 11679) || i == 11687 || i == 11695 || i == 11703 || i == 11711 || i == 11719 || i == 11727 || i == 11735 || i == 11743);
		}
	}
}
