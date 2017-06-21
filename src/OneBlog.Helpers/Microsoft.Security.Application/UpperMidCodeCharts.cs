using System;
namespace Microsoft.Security.Application
{
	[Flags]
	public enum UpperMidCodeCharts : long
	{
		None = 0L,
		CyrillicExtendedA = 1L,
		SupplementalPunctuation = 2L,
		CjkRadicalsSupplement = 4L,
		KangxiRadicals = 8L,
		IdeographicDescriptionCharacters = 16L,
		CjkSymbolsAndPunctuation = 32L,
		Hiragana = 64L,
		Katakana = 128L,
		Bopomofo = 256L,
		HangulCompatibilityJamo = 512L,
		Kanbun = 1024L,
		BopomofoExtended = 2048L,
		CjkStrokes = 4096L,
		KatakanaPhoneticExtensions = 8192L,
		EnclosedCjkLettersAndMonths = 16384L,
		CjkCompatibility = 32768L,
		CjkUnifiedIdeographsExtensionA = 65536L,
		YijingHexagramSymbols = 131072L,
		CjkUnifiedIdeographs = 262144L,
		YiSyllables = 524288L,
		YiRadicals = 1048576L,
		Lisu = 2097152L,
		Vai = 4194304L,
		CyrillicExtendedB = 8388608L,
		Bamum = 16777216L,
		ModifierToneLetters = 33554432L,
		LatinExtendedD = 67108864L,
		SylotiNagri = 134217728L,
		CommonIndicNumberForms = 268435456L,
		Phagspa = 536870912L,
		Saurashtra = 1073741824L
	}
}
