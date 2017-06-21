using System;
namespace Microsoft.Security.Application
{
	[Flags]
	public enum UpperCodeCharts
	{
		None = 0,
		DevanagariExtended = 1,
		KayahLi = 2,
		Rejang = 4,
		HangulJamoExtendedA = 8,
		Javanese = 16,
		Cham = 32,
		MyanmarExtendedA = 64,
		TaiViet = 128,
		MeeteiMayek = 256,
		HangulSyllables = 512,
		HangulJamoExtendedB = 1024,
		CjkCompatibilityIdeographs = 2048,
		AlphabeticPresentationForms = 4096,
		ArabicPresentationFormsA = 8192,
		VariationSelectors = 16384,
		VerticalForms = 32768,
		CombiningHalfMarks = 65536,
		CjkCompatibilityForms = 131072,
		SmallFormVariants = 262144,
		ArabicPresentationFormsB = 524288,
		HalfWidthAndFullWidthForms = 1048576,
		Specials = 2097152
	}
}
