using System;
namespace Microsoft.Security.Application
{
	[Flags]
	public enum LowerMidCodeCharts : long
	{
		None = 0L,
		Myanmar = 1L,
		Georgian = 2L,
		HangulJamo = 4L,
		Ethiopic = 8L,
		EthiopicSupplement = 16L,
		Cherokee = 32L,
		UnifiedCanadianAboriginalSyllabics = 64L,
		Ogham = 128L,
		Runic = 256L,
		Tagalog = 512L,
		Hanunoo = 1024L,
		Buhid = 2048L,
		Tagbanwa = 4096L,
		Khmer = 8192L,
		Mongolian = 16384L,
		UnifiedCanadianAboriginalSyllabicsExtended = 32768L,
		Limbu = 65536L,
		TaiLe = 131072L,
		NewTaiLue = 262144L,
		KhmerSymbols = 524288L,
		Buginese = 1048576L,
		TaiTham = 2097152L,
		Balinese = 4194304L,
		Sudanese = 8388608L,
		Lepcha = 16777216L,
		OlChiki = 33554432L,
		VedicExtensions = 67108864L,
		PhoneticExtensions = 134217728L,
		PhoneticExtensionsSupplement = 268435456L,
		CombiningDiacriticalMarksSupplement = 536870912L,
		LatinExtendedAdditional = 1073741824L
	}
}
