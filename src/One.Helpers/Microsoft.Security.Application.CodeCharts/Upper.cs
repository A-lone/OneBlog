using System;
using System.Collections;
using System.Linq;
namespace Microsoft.Security.Application.CodeCharts
{
	internal static class Upper
	{
		public static bool IsFlagSet(UpperCodeCharts flags, UpperCodeCharts flagToCheck)
		{
			return (flags & flagToCheck) != UpperCodeCharts.None;
		}
		public static IEnumerable DevanagariExtended()
		{
			return CodeChartHelper.GetRange(43232, 43259);
		}
		public static IEnumerable KayahLi()
		{
			return CodeChartHelper.GetRange(43264, 43311);
		}
		public static IEnumerable Rejang()
		{
			return CodeChartHelper.GetRange(43312, 43347).Concat(new int[]
			{
				43359
			});
		}
		public static IEnumerable HangulJamoExtendedA()
		{
			return CodeChartHelper.GetRange(43360, 43388);
		}
		public static IEnumerable Javanese()
		{
			return CodeChartHelper.GetRange(43392, 43487, (int i) => i == 43470 || (i >= 43482 && i <= 43485));
		}
		public static IEnumerable Cham()
		{
			return CodeChartHelper.GetRange(43520, 43615, (int i) => (i >= 43575 && i <= 43583) || i == 43598 || i == 43599 || i == 43610 || i == 43611);
		}
		public static IEnumerable MyanmarExtendedA()
		{
			return CodeChartHelper.GetRange(43616, 43643);
		}
		public static IEnumerable TaiViet()
		{
			return CodeChartHelper.GetRange(43648, 43714).Concat(CodeChartHelper.GetRange(43739, 43743));
		}
		public static IEnumerable MeeteiMayek()
		{
			return CodeChartHelper.GetRange(43968, 44025, (int i) => i == 44014 || i == 44015);
		}
		public static IEnumerable HangulSyllables()
		{
			return CodeChartHelper.GetRange(44032, 55203);
		}
		public static IEnumerable HangulJamoExtendedB()
		{
			return CodeChartHelper.GetRange(55216, 55291, (int i) => i == 55239 || i == 55240 || i == 55241 || i == 55242);
		}
		public static IEnumerable CjkCompatibilityIdeographs()
		{
			return CodeChartHelper.GetRange(63744, 64217, (int i) => i == 64046 || i == 64047 || i == 64110 || i == 64111);
		}
		public static IEnumerable AlphabeticPresentationForms()
		{
			return CodeChartHelper.GetRange(64256, 64335, (int i) => (i >= 64263 && i <= 64274) || (i >= 64280 && i <= 64284) || i == 64311 || i == 64317 || i == 64319 || i == 64322 || i == 64325);
		}
		public static IEnumerable ArabicPresentationFormsA()
		{
			return CodeChartHelper.GetRange(64336, 65021, (int i) => (i >= 64434 && i <= 64466) || (i >= 64832 && i <= 64847) || i == 64912 || i == 64913 || (i >= 64968 && i <= 65007));
		}
		public static IEnumerable VariationSelectors()
		{
			return CodeChartHelper.GetRange(65024, 65039);
		}
		public static IEnumerable VerticalForms()
		{
			return CodeChartHelper.GetRange(65040, 65049);
		}
		public static IEnumerable CombiningHalfMarks()
		{
			return CodeChartHelper.GetRange(65056, 65062);
		}
		public static IEnumerable CjkCompatibilityForms()
		{
			return CodeChartHelper.GetRange(65072, 65103);
		}
		public static IEnumerable SmallFormVariants()
		{
			return CodeChartHelper.GetRange(65104, 65131, (int i) => i == 65107 || i == 65127);
		}
		public static IEnumerable ArabicPresentationFormsB()
		{
			return CodeChartHelper.GetRange(65136, 65276, (int i) => i == 65141);
		}
		public static IEnumerable HalfWidthAndFullWidthForms()
		{
			return CodeChartHelper.GetRange(65281, 65518, (int i) => i == 65471 || i == 65472 || i == 65473 || i == 65480 || i == 65481 || i == 65488 || i == 65489 || i == 65496 || i == 65497 || i == 65501 || i == 65502 || i == 65503 || i == 65511);
		}
		public static IEnumerable Specials()
		{
			return CodeChartHelper.GetRange(65529, 65533);
		}
	}
}
