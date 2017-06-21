using Microsoft.Security.Application.CodeCharts;
using System;
using System.Collections;
using System.Globalization;
namespace Microsoft.Security.Application
{
	internal static class SafeList
	{
		internal delegate char[] GenerateSafeValue(int value);
		internal static char[][] Generate(int length, SafeList.GenerateSafeValue generateSafeValue)
		{
			char[][] array = new char[length + 1][];
			for (int i = 0; i <= length; i++)
			{
				array[i] = generateSafeValue(i);
			}
			return array;
		}
		internal static void PunchUnicodeThrough(ref char[][] safeList, LowerCodeCharts lowerCodeCharts, LowerMidCodeCharts lowerMidCodeCharts, MidCodeCharts midCodeCharts, UpperMidCodeCharts upperMidCodeCharts, UpperCodeCharts upperCodeCharts)
		{
			if (lowerCodeCharts != LowerCodeCharts.None)
			{
				SafeList.PunchCodeCharts(ref safeList, lowerCodeCharts);
			}
			if (lowerMidCodeCharts != LowerMidCodeCharts.None)
			{
				SafeList.PunchCodeCharts(ref safeList, lowerMidCodeCharts);
			}
			if (midCodeCharts != MidCodeCharts.None)
			{
				SafeList.PunchCodeCharts(ref safeList, midCodeCharts);
			}
			if (upperMidCodeCharts != UpperMidCodeCharts.None)
			{
				SafeList.PunchCodeCharts(ref safeList, upperMidCodeCharts);
			}
			if (upperCodeCharts != UpperCodeCharts.None)
			{
				SafeList.PunchCodeCharts(ref safeList, upperCodeCharts);
			}
		}
		internal static void PunchSafeList(ref char[][] safeList, IEnumerable whiteListedCharacters)
		{
			SafeList.PunchHolesIfNeeded(ref safeList, true, whiteListedCharacters);
		}
		internal static char[] HashThenValueGenerator(int value)
		{
			return SafeList.StringToCharArrayWithHashPrefix(value.ToString(CultureInfo.InvariantCulture));
		}
		internal static char[] HashThenHexValueGenerator(int value)
		{
			return SafeList.StringToCharArrayWithHashPrefix(value.ToString("X2", CultureInfo.InvariantCulture));
		}
		internal static char[] PercentThenHexValueGenerator(int value)
		{
			return SafeList.StringToCharArrayWithPercentPrefix(value.ToString("x2", CultureInfo.InvariantCulture));
		}
		internal static char[] SlashThenHexValueGenerator(int value)
		{
			return SafeList.StringToCharArrayWithSlashPrefix(value.ToString("x2", CultureInfo.InvariantCulture));
		}
		internal static char[] SlashThenSixDigitHexValueGenerator(int value)
		{
			return SafeList.StringToCharArrayWithSlashPrefix(value.ToString("X6", CultureInfo.InvariantCulture));
		}
		private static char[] StringToCharArrayWithHashPrefix(string value)
		{
			return SafeList.StringToCharArrayWithPrefix(value, '#');
		}
		private static char[] StringToCharArrayWithPercentPrefix(string value)
		{
			return SafeList.StringToCharArrayWithPrefix(value, '%');
		}
		private static char[] StringToCharArrayWithSlashPrefix(string value)
		{
			return SafeList.StringToCharArrayWithPrefix(value, '\\');
		}
		private static char[] StringToCharArrayWithPrefix(string value, char prefix)
		{
			int length = value.Length;
			char[] array = new char[length + 1];
			array[0] = prefix;
			for (int i = 0; i < length; i++)
			{
				array[i + 1] = value[i];
			}
			return array;
		}
		private static void PunchCodeCharts(ref char[][] safeList, LowerCodeCharts codeCharts)
		{
			SafeList.PunchHolesIfNeeded(ref safeList, Lower.IsFlagSet(codeCharts, LowerCodeCharts.BasicLatin), Lower.BasicLatin());
			SafeList.PunchHolesIfNeeded(ref safeList, Lower.IsFlagSet(codeCharts, LowerCodeCharts.C1ControlsAndLatin1Supplement), Lower.Latin1Supplement());
			SafeList.PunchHolesIfNeeded(ref safeList, Lower.IsFlagSet(codeCharts, LowerCodeCharts.LatinExtendedA), Lower.LatinExtendedA());
			SafeList.PunchHolesIfNeeded(ref safeList, Lower.IsFlagSet(codeCharts, LowerCodeCharts.LatinExtendedB), Lower.LatinExtendedB());
			SafeList.PunchHolesIfNeeded(ref safeList, Lower.IsFlagSet(codeCharts, LowerCodeCharts.IpaExtensions), Lower.IpaExtensions());
			SafeList.PunchHolesIfNeeded(ref safeList, Lower.IsFlagSet(codeCharts, LowerCodeCharts.SpacingModifierLetters), Lower.SpacingModifierLetters());
			SafeList.PunchHolesIfNeeded(ref safeList, Lower.IsFlagSet(codeCharts, LowerCodeCharts.CombiningDiacriticalMarks), Lower.CombiningDiacriticalMarks());
			SafeList.PunchHolesIfNeeded(ref safeList, Lower.IsFlagSet(codeCharts, LowerCodeCharts.GreekAndCoptic), Lower.GreekAndCoptic());
			SafeList.PunchHolesIfNeeded(ref safeList, Lower.IsFlagSet(codeCharts, LowerCodeCharts.Cyrillic), Lower.Cyrillic());
			SafeList.PunchHolesIfNeeded(ref safeList, Lower.IsFlagSet(codeCharts, LowerCodeCharts.CyrillicSupplement), Lower.CyrillicSupplement());
			SafeList.PunchHolesIfNeeded(ref safeList, Lower.IsFlagSet(codeCharts, LowerCodeCharts.Armenian), Lower.Armenian());
			SafeList.PunchHolesIfNeeded(ref safeList, Lower.IsFlagSet(codeCharts, LowerCodeCharts.Hebrew), Lower.Hebrew());
			SafeList.PunchHolesIfNeeded(ref safeList, Lower.IsFlagSet(codeCharts, LowerCodeCharts.Arabic), Lower.Arabic());
			SafeList.PunchHolesIfNeeded(ref safeList, Lower.IsFlagSet(codeCharts, LowerCodeCharts.Syriac), Lower.Syriac());
			SafeList.PunchHolesIfNeeded(ref safeList, Lower.IsFlagSet(codeCharts, LowerCodeCharts.ArabicSupplement), Lower.ArabicSupplement());
			SafeList.PunchHolesIfNeeded(ref safeList, Lower.IsFlagSet(codeCharts, LowerCodeCharts.Thaana), Lower.Thaana());
			SafeList.PunchHolesIfNeeded(ref safeList, Lower.IsFlagSet(codeCharts, LowerCodeCharts.Nko), Lower.Nko());
			SafeList.PunchHolesIfNeeded(ref safeList, Lower.IsFlagSet(codeCharts, LowerCodeCharts.Samaritan), Lower.Samaritan());
			SafeList.PunchHolesIfNeeded(ref safeList, Lower.IsFlagSet(codeCharts, LowerCodeCharts.Devanagari), Lower.Devanagari());
			SafeList.PunchHolesIfNeeded(ref safeList, Lower.IsFlagSet(codeCharts, LowerCodeCharts.Bengali), Lower.Bengali());
			SafeList.PunchHolesIfNeeded(ref safeList, Lower.IsFlagSet(codeCharts, LowerCodeCharts.Gurmukhi), Lower.Gurmukhi());
			SafeList.PunchHolesIfNeeded(ref safeList, Lower.IsFlagSet(codeCharts, LowerCodeCharts.Gujarati), Lower.Gujarati());
			SafeList.PunchHolesIfNeeded(ref safeList, Lower.IsFlagSet(codeCharts, LowerCodeCharts.Oriya), Lower.Oriya());
			SafeList.PunchHolesIfNeeded(ref safeList, Lower.IsFlagSet(codeCharts, LowerCodeCharts.Tamil), Lower.Tamil());
			SafeList.PunchHolesIfNeeded(ref safeList, Lower.IsFlagSet(codeCharts, LowerCodeCharts.Telugu), Lower.Telugu());
			SafeList.PunchHolesIfNeeded(ref safeList, Lower.IsFlagSet(codeCharts, LowerCodeCharts.Kannada), Lower.Kannada());
			SafeList.PunchHolesIfNeeded(ref safeList, Lower.IsFlagSet(codeCharts, LowerCodeCharts.Malayalam), Lower.Malayalam());
			SafeList.PunchHolesIfNeeded(ref safeList, Lower.IsFlagSet(codeCharts, LowerCodeCharts.Sinhala), Lower.Sinhala());
			SafeList.PunchHolesIfNeeded(ref safeList, Lower.IsFlagSet(codeCharts, LowerCodeCharts.Thai), Lower.Thai());
			SafeList.PunchHolesIfNeeded(ref safeList, Lower.IsFlagSet(codeCharts, LowerCodeCharts.Lao), Lower.Lao());
			SafeList.PunchHolesIfNeeded(ref safeList, Lower.IsFlagSet(codeCharts, LowerCodeCharts.Tibetan), Lower.Tibetan());
		}
		private static void PunchCodeCharts(ref char[][] safeList, LowerMidCodeCharts codeCharts)
		{
			SafeList.PunchHolesIfNeeded(ref safeList, LowerMiddle.IsFlagSet(codeCharts, LowerMidCodeCharts.Myanmar), LowerMiddle.Myanmar());
			SafeList.PunchHolesIfNeeded(ref safeList, LowerMiddle.IsFlagSet(codeCharts, LowerMidCodeCharts.Georgian), LowerMiddle.Georgian());
			SafeList.PunchHolesIfNeeded(ref safeList, LowerMiddle.IsFlagSet(codeCharts, LowerMidCodeCharts.HangulJamo), LowerMiddle.HangulJamo());
			SafeList.PunchHolesIfNeeded(ref safeList, LowerMiddle.IsFlagSet(codeCharts, LowerMidCodeCharts.Ethiopic), LowerMiddle.Ethiopic());
			SafeList.PunchHolesIfNeeded(ref safeList, LowerMiddle.IsFlagSet(codeCharts, LowerMidCodeCharts.EthiopicSupplement), LowerMiddle.EthiopicSupplement());
			SafeList.PunchHolesIfNeeded(ref safeList, LowerMiddle.IsFlagSet(codeCharts, LowerMidCodeCharts.Cherokee), LowerMiddle.Cherokee());
			SafeList.PunchHolesIfNeeded(ref safeList, LowerMiddle.IsFlagSet(codeCharts, LowerMidCodeCharts.UnifiedCanadianAboriginalSyllabics), LowerMiddle.UnifiedCanadianAboriginalSyllabics());
			SafeList.PunchHolesIfNeeded(ref safeList, LowerMiddle.IsFlagSet(codeCharts, LowerMidCodeCharts.Ogham), LowerMiddle.Ogham());
			SafeList.PunchHolesIfNeeded(ref safeList, LowerMiddle.IsFlagSet(codeCharts, LowerMidCodeCharts.Runic), LowerMiddle.Runic());
			SafeList.PunchHolesIfNeeded(ref safeList, LowerMiddle.IsFlagSet(codeCharts, LowerMidCodeCharts.Tagalog), LowerMiddle.Tagalog());
			SafeList.PunchHolesIfNeeded(ref safeList, LowerMiddle.IsFlagSet(codeCharts, LowerMidCodeCharts.Hanunoo), LowerMiddle.Hanunoo());
			SafeList.PunchHolesIfNeeded(ref safeList, LowerMiddle.IsFlagSet(codeCharts, LowerMidCodeCharts.Buhid), LowerMiddle.Buhid());
			SafeList.PunchHolesIfNeeded(ref safeList, LowerMiddle.IsFlagSet(codeCharts, LowerMidCodeCharts.Tagbanwa), LowerMiddle.Tagbanwa());
			SafeList.PunchHolesIfNeeded(ref safeList, LowerMiddle.IsFlagSet(codeCharts, LowerMidCodeCharts.Khmer), LowerMiddle.Khmer());
			SafeList.PunchHolesIfNeeded(ref safeList, LowerMiddle.IsFlagSet(codeCharts, LowerMidCodeCharts.Mongolian), LowerMiddle.Mongolian());
			SafeList.PunchHolesIfNeeded(ref safeList, LowerMiddle.IsFlagSet(codeCharts, LowerMidCodeCharts.UnifiedCanadianAboriginalSyllabicsExtended), LowerMiddle.UnifiedCanadianAboriginalSyllabicsExtended());
			SafeList.PunchHolesIfNeeded(ref safeList, LowerMiddle.IsFlagSet(codeCharts, LowerMidCodeCharts.Limbu), LowerMiddle.Limbu());
			SafeList.PunchHolesIfNeeded(ref safeList, LowerMiddle.IsFlagSet(codeCharts, LowerMidCodeCharts.TaiLe), LowerMiddle.TaiLe());
			SafeList.PunchHolesIfNeeded(ref safeList, LowerMiddle.IsFlagSet(codeCharts, LowerMidCodeCharts.NewTaiLue), LowerMiddle.NewTaiLue());
			SafeList.PunchHolesIfNeeded(ref safeList, LowerMiddle.IsFlagSet(codeCharts, LowerMidCodeCharts.KhmerSymbols), LowerMiddle.KhmerSymbols());
			SafeList.PunchHolesIfNeeded(ref safeList, LowerMiddle.IsFlagSet(codeCharts, LowerMidCodeCharts.Buginese), LowerMiddle.Buginese());
			SafeList.PunchHolesIfNeeded(ref safeList, LowerMiddle.IsFlagSet(codeCharts, LowerMidCodeCharts.TaiTham), LowerMiddle.TaiTham());
			SafeList.PunchHolesIfNeeded(ref safeList, LowerMiddle.IsFlagSet(codeCharts, LowerMidCodeCharts.Balinese), LowerMiddle.Balinese());
			SafeList.PunchHolesIfNeeded(ref safeList, LowerMiddle.IsFlagSet(codeCharts, LowerMidCodeCharts.Sudanese), LowerMiddle.Sudanese());
			SafeList.PunchHolesIfNeeded(ref safeList, LowerMiddle.IsFlagSet(codeCharts, LowerMidCodeCharts.Lepcha), LowerMiddle.Lepcha());
			SafeList.PunchHolesIfNeeded(ref safeList, LowerMiddle.IsFlagSet(codeCharts, LowerMidCodeCharts.OlChiki), LowerMiddle.OlChiki());
			SafeList.PunchHolesIfNeeded(ref safeList, LowerMiddle.IsFlagSet(codeCharts, LowerMidCodeCharts.VedicExtensions), LowerMiddle.VedicExtensions());
			SafeList.PunchHolesIfNeeded(ref safeList, LowerMiddle.IsFlagSet(codeCharts, LowerMidCodeCharts.PhoneticExtensions), LowerMiddle.PhoneticExtensions());
			SafeList.PunchHolesIfNeeded(ref safeList, LowerMiddle.IsFlagSet(codeCharts, LowerMidCodeCharts.PhoneticExtensionsSupplement), LowerMiddle.PhoneticExtensionsSupplement());
			SafeList.PunchHolesIfNeeded(ref safeList, LowerMiddle.IsFlagSet(codeCharts, LowerMidCodeCharts.CombiningDiacriticalMarksSupplement), LowerMiddle.CombiningDiacriticalMarksSupplement());
			SafeList.PunchHolesIfNeeded(ref safeList, LowerMiddle.IsFlagSet(codeCharts, LowerMidCodeCharts.LatinExtendedAdditional), LowerMiddle.LatinExtendedAdditional());
		}
		private static void PunchCodeCharts(ref char[][] safeList, MidCodeCharts codeCharts)
		{
			SafeList.PunchHolesIfNeeded(ref safeList, Middle.IsFlagSet(codeCharts, MidCodeCharts.GreekExtended), Middle.GreekExtended());
			SafeList.PunchHolesIfNeeded(ref safeList, Middle.IsFlagSet(codeCharts, MidCodeCharts.GeneralPunctuation), Middle.GeneralPunctuation());
			SafeList.PunchHolesIfNeeded(ref safeList, Middle.IsFlagSet(codeCharts, MidCodeCharts.SuperscriptsAndSubscripts), Middle.SuperscriptsAndSubscripts());
			SafeList.PunchHolesIfNeeded(ref safeList, Middle.IsFlagSet(codeCharts, MidCodeCharts.CurrencySymbols), Middle.CurrencySymbols());
			SafeList.PunchHolesIfNeeded(ref safeList, Middle.IsFlagSet(codeCharts, MidCodeCharts.CombiningDiacriticalMarksForSymbols), Middle.CombiningDiacriticalMarksForSymbols());
			SafeList.PunchHolesIfNeeded(ref safeList, Middle.IsFlagSet(codeCharts, MidCodeCharts.LetterlikeSymbols), Middle.LetterlikeSymbols());
			SafeList.PunchHolesIfNeeded(ref safeList, Middle.IsFlagSet(codeCharts, MidCodeCharts.NumberForms), Middle.NumberForms());
			SafeList.PunchHolesIfNeeded(ref safeList, Middle.IsFlagSet(codeCharts, MidCodeCharts.Arrows), Middle.Arrows());
			SafeList.PunchHolesIfNeeded(ref safeList, Middle.IsFlagSet(codeCharts, MidCodeCharts.MathematicalOperators), Middle.MathematicalOperators());
			SafeList.PunchHolesIfNeeded(ref safeList, Middle.IsFlagSet(codeCharts, MidCodeCharts.MiscellaneousTechnical), Middle.MiscellaneousTechnical());
			SafeList.PunchHolesIfNeeded(ref safeList, Middle.IsFlagSet(codeCharts, MidCodeCharts.ControlPictures), Middle.ControlPictures());
			SafeList.PunchHolesIfNeeded(ref safeList, Middle.IsFlagSet(codeCharts, MidCodeCharts.OpticalCharacterRecognition), Middle.OpticalCharacterRecognition());
			SafeList.PunchHolesIfNeeded(ref safeList, Middle.IsFlagSet(codeCharts, MidCodeCharts.EnclosedAlphanumerics), Middle.EnclosedAlphanumerics());
			SafeList.PunchHolesIfNeeded(ref safeList, Middle.IsFlagSet(codeCharts, MidCodeCharts.BoxDrawing), Middle.BoxDrawing());
			SafeList.PunchHolesIfNeeded(ref safeList, Middle.IsFlagSet(codeCharts, MidCodeCharts.BlockElements), Middle.BlockElements());
			SafeList.PunchHolesIfNeeded(ref safeList, Middle.IsFlagSet(codeCharts, MidCodeCharts.GeometricShapes), Middle.GeometricShapes());
			SafeList.PunchHolesIfNeeded(ref safeList, Middle.IsFlagSet(codeCharts, MidCodeCharts.MiscellaneousSymbols), Middle.MiscellaneousSymbols());
			SafeList.PunchHolesIfNeeded(ref safeList, Middle.IsFlagSet(codeCharts, MidCodeCharts.Dingbats), Middle.Dingbats());
			SafeList.PunchHolesIfNeeded(ref safeList, Middle.IsFlagSet(codeCharts, MidCodeCharts.MiscellaneousMathematicalSymbolsA), Middle.MiscellaneousMathematicalSymbolsA());
			SafeList.PunchHolesIfNeeded(ref safeList, Middle.IsFlagSet(codeCharts, MidCodeCharts.SupplementalArrowsA), Middle.SupplementalArrowsA());
			SafeList.PunchHolesIfNeeded(ref safeList, Middle.IsFlagSet(codeCharts, MidCodeCharts.BraillePatterns), Middle.BraillePatterns());
			SafeList.PunchHolesIfNeeded(ref safeList, Middle.IsFlagSet(codeCharts, MidCodeCharts.SupplementalArrowsB), Middle.SupplementalArrowsB());
			SafeList.PunchHolesIfNeeded(ref safeList, Middle.IsFlagSet(codeCharts, MidCodeCharts.MiscellaneousMathematicalSymbolsB), Middle.MiscellaneousMathematicalSymbolsB());
			SafeList.PunchHolesIfNeeded(ref safeList, Middle.IsFlagSet(codeCharts, MidCodeCharts.SupplementalMathematicalOperators), Middle.SupplementalMathematicalOperators());
			SafeList.PunchHolesIfNeeded(ref safeList, Middle.IsFlagSet(codeCharts, MidCodeCharts.MiscellaneousSymbolsAndArrows), Middle.MiscellaneousSymbolsAndArrows());
			SafeList.PunchHolesIfNeeded(ref safeList, Middle.IsFlagSet(codeCharts, MidCodeCharts.Glagolitic), Middle.Glagolitic());
			SafeList.PunchHolesIfNeeded(ref safeList, Middle.IsFlagSet(codeCharts, MidCodeCharts.LatinExtendedC), Middle.LatinExtendedC());
			SafeList.PunchHolesIfNeeded(ref safeList, Middle.IsFlagSet(codeCharts, MidCodeCharts.Coptic), Middle.Coptic());
			SafeList.PunchHolesIfNeeded(ref safeList, Middle.IsFlagSet(codeCharts, MidCodeCharts.GeorgianSupplement), Middle.GeorgianSupplement());
			SafeList.PunchHolesIfNeeded(ref safeList, Middle.IsFlagSet(codeCharts, MidCodeCharts.Tifinagh), Middle.Tifinagh());
			SafeList.PunchHolesIfNeeded(ref safeList, Middle.IsFlagSet(codeCharts, MidCodeCharts.BlockElements), Middle.EthiopicExtended());
		}
		private static void PunchCodeCharts(ref char[][] safeList, UpperMidCodeCharts codeCharts)
		{
			SafeList.PunchHolesIfNeeded(ref safeList, UpperMiddle.IsFlagSet(codeCharts, UpperMidCodeCharts.CyrillicExtendedA), UpperMiddle.CyrillicExtendedA());
			SafeList.PunchHolesIfNeeded(ref safeList, UpperMiddle.IsFlagSet(codeCharts, UpperMidCodeCharts.SupplementalPunctuation), UpperMiddle.SupplementalPunctuation());
			SafeList.PunchHolesIfNeeded(ref safeList, UpperMiddle.IsFlagSet(codeCharts, UpperMidCodeCharts.CjkRadicalsSupplement), UpperMiddle.CjkRadicalsSupplement());
			SafeList.PunchHolesIfNeeded(ref safeList, UpperMiddle.IsFlagSet(codeCharts, UpperMidCodeCharts.KangxiRadicals), UpperMiddle.KangxiRadicals());
			SafeList.PunchHolesIfNeeded(ref safeList, UpperMiddle.IsFlagSet(codeCharts, UpperMidCodeCharts.IdeographicDescriptionCharacters), UpperMiddle.IdeographicDescriptionCharacters());
			SafeList.PunchHolesIfNeeded(ref safeList, UpperMiddle.IsFlagSet(codeCharts, UpperMidCodeCharts.CjkSymbolsAndPunctuation), UpperMiddle.CjkSymbolsAndPunctuation());
			SafeList.PunchHolesIfNeeded(ref safeList, UpperMiddle.IsFlagSet(codeCharts, UpperMidCodeCharts.Hiragana), UpperMiddle.Hiragana());
			SafeList.PunchHolesIfNeeded(ref safeList, UpperMiddle.IsFlagSet(codeCharts, UpperMidCodeCharts.Katakana), UpperMiddle.Katakana());
			SafeList.PunchHolesIfNeeded(ref safeList, UpperMiddle.IsFlagSet(codeCharts, UpperMidCodeCharts.Bopomofo), UpperMiddle.Bopomofo());
			SafeList.PunchHolesIfNeeded(ref safeList, UpperMiddle.IsFlagSet(codeCharts, UpperMidCodeCharts.HangulCompatibilityJamo), UpperMiddle.HangulCompatibilityJamo());
			SafeList.PunchHolesIfNeeded(ref safeList, UpperMiddle.IsFlagSet(codeCharts, UpperMidCodeCharts.Kanbun), UpperMiddle.Kanbun());
			SafeList.PunchHolesIfNeeded(ref safeList, UpperMiddle.IsFlagSet(codeCharts, UpperMidCodeCharts.BopomofoExtended), UpperMiddle.BopomofoExtended());
			SafeList.PunchHolesIfNeeded(ref safeList, UpperMiddle.IsFlagSet(codeCharts, UpperMidCodeCharts.CjkStrokes), UpperMiddle.CjkStrokes());
			SafeList.PunchHolesIfNeeded(ref safeList, UpperMiddle.IsFlagSet(codeCharts, UpperMidCodeCharts.KatakanaPhoneticExtensions), UpperMiddle.KatakanaPhoneticExtensions());
			SafeList.PunchHolesIfNeeded(ref safeList, UpperMiddle.IsFlagSet(codeCharts, UpperMidCodeCharts.EnclosedCjkLettersAndMonths), UpperMiddle.EnclosedCjkLettersAndMonths());
			SafeList.PunchHolesIfNeeded(ref safeList, UpperMiddle.IsFlagSet(codeCharts, UpperMidCodeCharts.CjkCompatibility), UpperMiddle.CjkCompatibility());
			SafeList.PunchHolesIfNeeded(ref safeList, UpperMiddle.IsFlagSet(codeCharts, UpperMidCodeCharts.CjkUnifiedIdeographsExtensionA), UpperMiddle.CjkUnifiedIdeographsExtensionA());
			SafeList.PunchHolesIfNeeded(ref safeList, UpperMiddle.IsFlagSet(codeCharts, UpperMidCodeCharts.YijingHexagramSymbols), UpperMiddle.YijingHexagramSymbols());
			SafeList.PunchHolesIfNeeded(ref safeList, UpperMiddle.IsFlagSet(codeCharts, UpperMidCodeCharts.CjkUnifiedIdeographs), UpperMiddle.CjkUnifiedIdeographs());
			SafeList.PunchHolesIfNeeded(ref safeList, UpperMiddle.IsFlagSet(codeCharts, UpperMidCodeCharts.YiSyllables), UpperMiddle.YiSyllables());
			SafeList.PunchHolesIfNeeded(ref safeList, UpperMiddle.IsFlagSet(codeCharts, UpperMidCodeCharts.YiRadicals), UpperMiddle.YiRadicals());
			SafeList.PunchHolesIfNeeded(ref safeList, UpperMiddle.IsFlagSet(codeCharts, UpperMidCodeCharts.Lisu), UpperMiddle.Lisu());
			SafeList.PunchHolesIfNeeded(ref safeList, UpperMiddle.IsFlagSet(codeCharts, UpperMidCodeCharts.Vai), UpperMiddle.Vai());
			SafeList.PunchHolesIfNeeded(ref safeList, UpperMiddle.IsFlagSet(codeCharts, UpperMidCodeCharts.CyrillicExtendedB), UpperMiddle.CyrillicExtendedB());
			SafeList.PunchHolesIfNeeded(ref safeList, UpperMiddle.IsFlagSet(codeCharts, UpperMidCodeCharts.Bamum), UpperMiddle.Bamum());
			SafeList.PunchHolesIfNeeded(ref safeList, UpperMiddle.IsFlagSet(codeCharts, UpperMidCodeCharts.ModifierToneLetters), UpperMiddle.ModifierToneLetters());
			SafeList.PunchHolesIfNeeded(ref safeList, UpperMiddle.IsFlagSet(codeCharts, UpperMidCodeCharts.LatinExtendedD), UpperMiddle.LatinExtendedD());
			SafeList.PunchHolesIfNeeded(ref safeList, UpperMiddle.IsFlagSet(codeCharts, UpperMidCodeCharts.SylotiNagri), UpperMiddle.SylotiNagri());
			SafeList.PunchHolesIfNeeded(ref safeList, UpperMiddle.IsFlagSet(codeCharts, UpperMidCodeCharts.CommonIndicNumberForms), UpperMiddle.CommonIndicNumberForms());
			SafeList.PunchHolesIfNeeded(ref safeList, UpperMiddle.IsFlagSet(codeCharts, UpperMidCodeCharts.Phagspa), UpperMiddle.Phagspa());
			SafeList.PunchHolesIfNeeded(ref safeList, UpperMiddle.IsFlagSet(codeCharts, UpperMidCodeCharts.Saurashtra), UpperMiddle.Saurashtra());
		}
		private static void PunchCodeCharts(ref char[][] safeList, UpperCodeCharts codeCharts)
		{
			SafeList.PunchHolesIfNeeded(ref safeList, Upper.IsFlagSet(codeCharts, UpperCodeCharts.DevanagariExtended), Upper.DevanagariExtended());
			SafeList.PunchHolesIfNeeded(ref safeList, Upper.IsFlagSet(codeCharts, UpperCodeCharts.KayahLi), Upper.KayahLi());
			SafeList.PunchHolesIfNeeded(ref safeList, Upper.IsFlagSet(codeCharts, UpperCodeCharts.Rejang), Upper.Rejang());
			SafeList.PunchHolesIfNeeded(ref safeList, Upper.IsFlagSet(codeCharts, UpperCodeCharts.HangulJamoExtendedA), Upper.HangulJamoExtendedA());
			SafeList.PunchHolesIfNeeded(ref safeList, Upper.IsFlagSet(codeCharts, UpperCodeCharts.Javanese), Upper.Javanese());
			SafeList.PunchHolesIfNeeded(ref safeList, Upper.IsFlagSet(codeCharts, UpperCodeCharts.Cham), Upper.Cham());
			SafeList.PunchHolesIfNeeded(ref safeList, Upper.IsFlagSet(codeCharts, UpperCodeCharts.MyanmarExtendedA), Upper.MyanmarExtendedA());
			SafeList.PunchHolesIfNeeded(ref safeList, Upper.IsFlagSet(codeCharts, UpperCodeCharts.TaiViet), Upper.TaiViet());
			SafeList.PunchHolesIfNeeded(ref safeList, Upper.IsFlagSet(codeCharts, UpperCodeCharts.MeeteiMayek), Upper.MeeteiMayek());
			SafeList.PunchHolesIfNeeded(ref safeList, Upper.IsFlagSet(codeCharts, UpperCodeCharts.HangulSyllables), Upper.HangulSyllables());
			SafeList.PunchHolesIfNeeded(ref safeList, Upper.IsFlagSet(codeCharts, UpperCodeCharts.HangulJamoExtendedB), Upper.HangulJamoExtendedB());
			SafeList.PunchHolesIfNeeded(ref safeList, Upper.IsFlagSet(codeCharts, UpperCodeCharts.CjkCompatibilityIdeographs), Upper.CjkCompatibilityIdeographs());
			SafeList.PunchHolesIfNeeded(ref safeList, Upper.IsFlagSet(codeCharts, UpperCodeCharts.AlphabeticPresentationForms), Upper.AlphabeticPresentationForms());
			SafeList.PunchHolesIfNeeded(ref safeList, Upper.IsFlagSet(codeCharts, UpperCodeCharts.ArabicPresentationFormsA), Upper.ArabicPresentationFormsA());
			SafeList.PunchHolesIfNeeded(ref safeList, Upper.IsFlagSet(codeCharts, UpperCodeCharts.VariationSelectors), Upper.VariationSelectors());
			SafeList.PunchHolesIfNeeded(ref safeList, Upper.IsFlagSet(codeCharts, UpperCodeCharts.VerticalForms), Upper.VerticalForms());
			SafeList.PunchHolesIfNeeded(ref safeList, Upper.IsFlagSet(codeCharts, UpperCodeCharts.CombiningHalfMarks), Upper.CombiningHalfMarks());
			SafeList.PunchHolesIfNeeded(ref safeList, Upper.IsFlagSet(codeCharts, UpperCodeCharts.CjkCompatibilityForms), Upper.CjkCompatibilityForms());
			SafeList.PunchHolesIfNeeded(ref safeList, Upper.IsFlagSet(codeCharts, UpperCodeCharts.SmallFormVariants), Upper.SmallFormVariants());
			SafeList.PunchHolesIfNeeded(ref safeList, Upper.IsFlagSet(codeCharts, UpperCodeCharts.ArabicPresentationFormsB), Upper.ArabicPresentationFormsB());
			SafeList.PunchHolesIfNeeded(ref safeList, Upper.IsFlagSet(codeCharts, UpperCodeCharts.HalfWidthAndFullWidthForms), Upper.HalfWidthAndFullWidthForms());
			SafeList.PunchHolesIfNeeded(ref safeList, Upper.IsFlagSet(codeCharts, UpperCodeCharts.Specials), Upper.Specials());
		}
		private static void PunchHolesIfNeeded(ref char[][] safeList, bool needed, IEnumerable whiteListedCharacters)
		{
			if (!needed)
			{
				return;
			}
			foreach (int num in whiteListedCharacters)
			{
				safeList[num] = null;
			}
		}
	}
}
