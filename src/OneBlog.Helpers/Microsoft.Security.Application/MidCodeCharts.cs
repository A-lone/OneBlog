using System;
namespace Microsoft.Security.Application
{
	[Flags]
	public enum MidCodeCharts : long
	{
		None = 0L,
		GreekExtended = 1L,
		GeneralPunctuation = 2L,
		SuperscriptsAndSubscripts = 4L,
		CurrencySymbols = 8L,
		CombiningDiacriticalMarksForSymbols = 16L,
		LetterlikeSymbols = 32L,
		NumberForms = 64L,
		Arrows = 128L,
		MathematicalOperators = 256L,
		MiscellaneousTechnical = 512L,
		ControlPictures = 1024L,
		OpticalCharacterRecognition = 2048L,
		EnclosedAlphanumerics = 4096L,
		BoxDrawing = 8192L,
		BlockElements = 16384L,
		GeometricShapes = 32768L,
		MiscellaneousSymbols = 65536L,
		Dingbats = 131072L,
		MiscellaneousMathematicalSymbolsA = 262144L,
		SupplementalArrowsA = 524288L,
		BraillePatterns = 1048576L,
		SupplementalArrowsB = 2097152L,
		MiscellaneousMathematicalSymbolsB = 4194304L,
		SupplementalMathematicalOperators = 8388608L,
		MiscellaneousSymbolsAndArrows = 16777216L,
		Glagolitic = 33554432L,
		LatinExtendedC = 67108864L,
		Coptic = 134217728L,
		GeorgianSupplement = 268435456L,
		Tifinagh = 536870912L,
		EthiopicExtended = 16384L
	}
}
