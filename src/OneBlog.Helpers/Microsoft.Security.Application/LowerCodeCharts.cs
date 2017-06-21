using System;
namespace Microsoft.Security.Application
{
	[Flags]
	public enum LowerCodeCharts : long
	{
		None = 0L,
		BasicLatin = 1L,
		C1ControlsAndLatin1Supplement = 2L,
		LatinExtendedA = 4L,
		LatinExtendedB = 8L,
		IpaExtensions = 16L,
		SpacingModifierLetters = 32L,
		CombiningDiacriticalMarks = 64L,
		GreekAndCoptic = 128L,
		Cyrillic = 256L,
		CyrillicSupplement = 512L,
		Armenian = 1024L,
		Hebrew = 2048L,
		Arabic = 4096L,
		Syriac = 8192L,
		ArabicSupplement = 16384L,
		Thaana = 32768L,
		Nko = 65536L,
		Samaritan = 131072L,
		Devanagari = 262144L,
		Bengali = 524288L,
		Gurmukhi = 1048576L,
		Gujarati = 2097152L,
		Oriya = 4194304L,
		Tamil = 8388608L,
		Telugu = 16777216L,
		Kannada = 33554432L,
		Malayalam = 67108864L,
		Sinhala = 134217728L,
		Thai = 268435456L,
		Lao = 536870912L,
		Tibetan = 1073741824L,
		Default = 127L
	}
}
