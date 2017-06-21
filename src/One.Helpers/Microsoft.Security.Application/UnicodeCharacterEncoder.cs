using System;
using System.Text;
using System.Threading;
namespace Microsoft.Security.Application
{
	public static class UnicodeCharacterEncoder
	{
		private delegate bool MethodSpecificEncoder(char input, out char[] output);
		private static readonly char[] UnicodeSpace = "&#32;".ToCharArray();
		private static readonly char[] UnicodeApostrophe = "&#39;".ToCharArray();
		private static readonly char[] XmlApostrophe = "&apos;".ToCharArray();
		private static LowerCodeCharts currentLowerCodeChartSettings = LowerCodeCharts.Default;
		private static LowerMidCodeCharts currentLowerMidCodeChartSettings = LowerMidCodeCharts.None;
		private static MidCodeCharts currentMidCodeChartSettings = MidCodeCharts.None;
		private static UpperMidCodeCharts currentUpperMidCodeChartSettings = UpperMidCodeCharts.None;
		private static UpperCodeCharts currentUpperCodeChartSettings = UpperCodeCharts.None;
		private static char[][] characterValues;
		private static Lazy<char[][]> namedEntitiesLazy = new Lazy<char[][]>(new Func<char[][]>(UnicodeCharacterEncoder.InitialiseNamedEntityList));
		private static readonly ReaderWriterLockSlim SyncLock = new ReaderWriterLockSlim();
		private static void AcquireReadLock()
		{
			UnicodeCharacterEncoder.SyncLock.EnterReadLock();
		}
		private static void ReleaseReadLock()
		{
			UnicodeCharacterEncoder.SyncLock.ExitReadLock();
		}
		private static void AcquireWriteLock()
		{
			UnicodeCharacterEncoder.SyncLock.EnterWriteLock();
		}
		private static void ReleaseWriteLock()
		{
			UnicodeCharacterEncoder.SyncLock.ExitWriteLock();
		}
		public static void MarkAsSafe(LowerCodeCharts lowerCodeCharts, LowerMidCodeCharts lowerMidCodeCharts, MidCodeCharts midCodeCharts, UpperMidCodeCharts upperMidCodeCharts, UpperCodeCharts upperCodeCharts)
		{
			if (lowerCodeCharts == UnicodeCharacterEncoder.currentLowerCodeChartSettings && lowerMidCodeCharts == UnicodeCharacterEncoder.currentLowerMidCodeChartSettings && midCodeCharts == UnicodeCharacterEncoder.currentMidCodeChartSettings && upperMidCodeCharts == UnicodeCharacterEncoder.currentUpperMidCodeChartSettings && upperCodeCharts == UnicodeCharacterEncoder.currentUpperCodeChartSettings)
			{
				return;
			}
			UnicodeCharacterEncoder.AcquireWriteLock();
			try
			{
				UnicodeCharacterEncoder.characterValues = SafeList.Generate(65536, new SafeList.GenerateSafeValue(SafeList.HashThenValueGenerator));
				SafeList.PunchUnicodeThrough(ref UnicodeCharacterEncoder.characterValues, lowerCodeCharts, lowerMidCodeCharts, midCodeCharts, upperMidCodeCharts, upperCodeCharts);
				UnicodeCharacterEncoder.ApplyHtmlSpecificValues();
				UnicodeCharacterEncoder.currentLowerCodeChartSettings = lowerCodeCharts;
				UnicodeCharacterEncoder.currentLowerMidCodeChartSettings = lowerMidCodeCharts;
				UnicodeCharacterEncoder.currentMidCodeChartSettings = midCodeCharts;
				UnicodeCharacterEncoder.currentUpperMidCodeChartSettings = upperMidCodeCharts;
				UnicodeCharacterEncoder.currentUpperCodeChartSettings = upperCodeCharts;
			}
			finally
			{
				UnicodeCharacterEncoder.ReleaseWriteLock();
			}
		}
		internal static string XmlEncode(string input)
		{
			return UnicodeCharacterEncoder.HtmlEncode(input, false, new UnicodeCharacterEncoder.MethodSpecificEncoder(UnicodeCharacterEncoder.XmlTweak));
		}
		internal static string XmlAttributeEncode(string input)
		{
			return UnicodeCharacterEncoder.HtmlEncode(input, false, new UnicodeCharacterEncoder.MethodSpecificEncoder(UnicodeCharacterEncoder.XmlAttributeTweak));
		}
		internal static string HtmlAttributeEncode(string input)
		{
			return UnicodeCharacterEncoder.HtmlEncode(input, false, new UnicodeCharacterEncoder.MethodSpecificEncoder(UnicodeCharacterEncoder.HtmlAttributeTweak));
		}
		internal static string HtmlEncode(string input, bool useNamedEntities)
		{
			return UnicodeCharacterEncoder.HtmlEncode(input, useNamedEntities, null);
		}
		private static void ApplyHtmlSpecificValues()
		{
			UnicodeCharacterEncoder.characterValues[60] = "lt".ToCharArray();
			UnicodeCharacterEncoder.characterValues[62] = "gt".ToCharArray();
			UnicodeCharacterEncoder.characterValues[38] = "amp".ToCharArray();
			UnicodeCharacterEncoder.characterValues[34] = "quot".ToCharArray();
			UnicodeCharacterEncoder.characterValues[39] = "#39".ToCharArray();
		}
		private static bool HtmlAttributeTweak(char input, out char[] output)
		{
			if (input == '\'')
			{
				output = UnicodeCharacterEncoder.UnicodeApostrophe;
				return true;
			}
			if (input == ' ')
			{
				output = UnicodeCharacterEncoder.UnicodeSpace;
				return true;
			}
			output = null;
			return false;
		}
		private static bool XmlTweak(char input, out char[] output)
		{
			if (input == '\'')
			{
				output = UnicodeCharacterEncoder.XmlApostrophe;
				return true;
			}
			output = null;
			return false;
		}
		private static bool XmlAttributeTweak(char input, out char[] output)
		{
			if (input == '\'')
			{
				output = UnicodeCharacterEncoder.XmlApostrophe;
				return true;
			}
			if (input == ' ')
			{
				output = UnicodeCharacterEncoder.UnicodeSpace;
				return true;
			}
			output = null;
			return false;
		}
		private static string HtmlEncode(string input, bool useNamedEntities, UnicodeCharacterEncoder.MethodSpecificEncoder encoderTweak)
		{
			if (string.IsNullOrEmpty(input))
			{
				return input;
			}
			if (UnicodeCharacterEncoder.characterValues == null)
			{
				UnicodeCharacterEncoder.InitialiseSafeList();
			}
			char[][] array = null;
			if (useNamedEntities)
			{
				array = UnicodeCharacterEncoder.namedEntitiesLazy.Value;
			}
			StringBuilder outputStringBuilder = EncoderUtil.GetOutputStringBuilder(input.Length, 10);
			UnicodeCharacterEncoder.AcquireReadLock();
			try
			{
				Utf16StringReader utf16StringReader = new Utf16StringReader(input);
				while (true)
				{
					int num = utf16StringReader.ReadNextScalarValue();
					if (num < 0)
					{
						break;
					}
					if (num > 65535)
					{
						char[] value = SafeList.HashThenValueGenerator(num);
						outputStringBuilder.Append('&');
						outputStringBuilder.Append(value);
						outputStringBuilder.Append(';');
					}
					else
					{
						char c = (char)num;
						char[] value2;
						if (encoderTweak != null && encoderTweak(c, out value2))
						{
							outputStringBuilder.Append(value2);
						}
						else
						{
							if (useNamedEntities && array[num] != null)
							{
								char[] value3 = array[num];
								outputStringBuilder.Append('&');
								outputStringBuilder.Append(value3);
								outputStringBuilder.Append(';');
							}
							else
							{
								if (UnicodeCharacterEncoder.characterValues[num] != null)
								{
									char[] value4 = UnicodeCharacterEncoder.characterValues[num];
									outputStringBuilder.Append('&');
									outputStringBuilder.Append(value4);
									outputStringBuilder.Append(';');
								}
								else
								{
									outputStringBuilder.Append(c);
								}
							}
						}
					}
				}
			}
			finally
			{
				UnicodeCharacterEncoder.ReleaseReadLock();
			}
			return outputStringBuilder.ToString();
		}
		private static void InitialiseSafeList()
		{
			UnicodeCharacterEncoder.AcquireWriteLock();
			try
			{
				if (UnicodeCharacterEncoder.characterValues == null)
				{
					UnicodeCharacterEncoder.characterValues = SafeList.Generate(65535, new SafeList.GenerateSafeValue(SafeList.HashThenValueGenerator));
					SafeList.PunchUnicodeThrough(ref UnicodeCharacterEncoder.characterValues, UnicodeCharacterEncoder.currentLowerCodeChartSettings, UnicodeCharacterEncoder.currentLowerMidCodeChartSettings, UnicodeCharacterEncoder.currentMidCodeChartSettings, UnicodeCharacterEncoder.currentUpperMidCodeChartSettings, UnicodeCharacterEncoder.currentUpperCodeChartSettings);
					UnicodeCharacterEncoder.ApplyHtmlSpecificValues();
				}
			}
			finally
			{
				UnicodeCharacterEncoder.ReleaseWriteLock();
			}
		}
		private static char[][] InitialiseNamedEntityList()
		{
			char[][] array = new char[65536][];
			array[160] = "nbsp".ToCharArray();
			array[161] = "iexcl".ToCharArray();
			array[162] = "cent".ToCharArray();
			array[163] = "pound".ToCharArray();
			array[164] = "curren".ToCharArray();
			array[165] = "yen".ToCharArray();
			array[166] = "brvbar".ToCharArray();
			array[167] = "sect".ToCharArray();
			array[168] = "uml".ToCharArray();
			array[169] = "copy".ToCharArray();
			array[170] = "ordf".ToCharArray();
			array[171] = "laquo".ToCharArray();
			array[172] = "not".ToCharArray();
			array[173] = "shy".ToCharArray();
			array[174] = "reg".ToCharArray();
			array[175] = "macr".ToCharArray();
			array[176] = "deg".ToCharArray();
			array[177] = "plusmn".ToCharArray();
			array[178] = "sup2".ToCharArray();
			array[179] = "sup3".ToCharArray();
			array[180] = "acute".ToCharArray();
			array[181] = "micro".ToCharArray();
			array[182] = "para".ToCharArray();
			array[183] = "middot".ToCharArray();
			array[184] = "cedil".ToCharArray();
			array[185] = "sup1".ToCharArray();
			array[186] = "ordm".ToCharArray();
			array[187] = "raquo".ToCharArray();
			array[188] = "frac14".ToCharArray();
			array[189] = "frac12".ToCharArray();
			array[190] = "frac34".ToCharArray();
			array[191] = "iquest".ToCharArray();
			array[192] = "Agrave".ToCharArray();
			array[193] = "Aacute".ToCharArray();
			array[194] = "Acirc".ToCharArray();
			array[195] = "Atilde".ToCharArray();
			array[196] = "Auml".ToCharArray();
			array[197] = "Aring".ToCharArray();
			array[198] = "AElig".ToCharArray();
			array[199] = "Ccedil".ToCharArray();
			array[200] = "Egrave".ToCharArray();
			array[201] = "Eacute".ToCharArray();
			array[202] = "Ecirc".ToCharArray();
			array[203] = "Euml".ToCharArray();
			array[204] = "Igrave".ToCharArray();
			array[205] = "Iacute".ToCharArray();
			array[206] = "Icirc".ToCharArray();
			array[207] = "Iuml".ToCharArray();
			array[208] = "ETH".ToCharArray();
			array[209] = "Ntilde".ToCharArray();
			array[210] = "Ograve".ToCharArray();
			array[211] = "Oacute".ToCharArray();
			array[212] = "Ocirc".ToCharArray();
			array[213] = "Otilde".ToCharArray();
			array[214] = "Ouml".ToCharArray();
			array[215] = "times".ToCharArray();
			array[216] = "Oslash".ToCharArray();
			array[217] = "Ugrave".ToCharArray();
			array[218] = "Uacute".ToCharArray();
			array[219] = "Ucirc".ToCharArray();
			array[220] = "Uuml".ToCharArray();
			array[221] = "Yacute".ToCharArray();
			array[222] = "THORN".ToCharArray();
			array[223] = "szlig".ToCharArray();
			array[224] = "agrave".ToCharArray();
			array[225] = "aacute".ToCharArray();
			array[226] = "acirc".ToCharArray();
			array[227] = "atilde".ToCharArray();
			array[228] = "auml".ToCharArray();
			array[229] = "aring".ToCharArray();
			array[230] = "aelig".ToCharArray();
			array[231] = "ccedil".ToCharArray();
			array[232] = "egrave".ToCharArray();
			array[233] = "eacute".ToCharArray();
			array[234] = "ecirc".ToCharArray();
			array[235] = "euml".ToCharArray();
			array[236] = "igrave".ToCharArray();
			array[237] = "iacute".ToCharArray();
			array[238] = "icirc".ToCharArray();
			array[239] = "iuml".ToCharArray();
			array[240] = "eth".ToCharArray();
			array[241] = "ntilde".ToCharArray();
			array[242] = "ograve".ToCharArray();
			array[243] = "oacute".ToCharArray();
			array[244] = "ocirc".ToCharArray();
			array[245] = "otilde".ToCharArray();
			array[246] = "ouml".ToCharArray();
			array[247] = "divide".ToCharArray();
			array[248] = "oslash".ToCharArray();
			array[249] = "ugrave".ToCharArray();
			array[250] = "uacute".ToCharArray();
			array[251] = "ucirc".ToCharArray();
			array[252] = "uuml".ToCharArray();
			array[253] = "yacute".ToCharArray();
			array[254] = "thorn".ToCharArray();
			array[255] = "yuml".ToCharArray();
			array[338] = "OElig".ToCharArray();
			array[339] = "oelig".ToCharArray();
			array[352] = "Scaron".ToCharArray();
			array[353] = "scaron".ToCharArray();
			array[376] = "Yuml".ToCharArray();
			array[402] = "fnof".ToCharArray();
			array[710] = "circ".ToCharArray();
			array[732] = "tilde".ToCharArray();
			array[913] = "Alpha".ToCharArray();
			array[914] = "Beta".ToCharArray();
			array[915] = "Gamma".ToCharArray();
			array[916] = "Delta".ToCharArray();
			array[917] = "Epsilon".ToCharArray();
			array[918] = "Zeta".ToCharArray();
			array[919] = "Eta".ToCharArray();
			array[920] = "Theta".ToCharArray();
			array[921] = "Iota".ToCharArray();
			array[922] = "Kappa".ToCharArray();
			array[923] = "Lambda".ToCharArray();
			array[924] = "Mu".ToCharArray();
			array[925] = "Nu".ToCharArray();
			array[926] = "Xi".ToCharArray();
			array[927] = "Omicron".ToCharArray();
			array[928] = "Pi".ToCharArray();
			array[929] = "Rho".ToCharArray();
			array[931] = "Sigma".ToCharArray();
			array[932] = "Tau".ToCharArray();
			array[933] = "Upsilon".ToCharArray();
			array[934] = "Phi".ToCharArray();
			array[935] = "Chi".ToCharArray();
			array[936] = "Psi".ToCharArray();
			array[937] = "Omega".ToCharArray();
			array[945] = "alpha".ToCharArray();
			array[946] = "beta".ToCharArray();
			array[947] = "gamma".ToCharArray();
			array[948] = "delta".ToCharArray();
			array[949] = "epsilon".ToCharArray();
			array[950] = "zeta".ToCharArray();
			array[951] = "eta".ToCharArray();
			array[952] = "theta".ToCharArray();
			array[953] = "iota".ToCharArray();
			array[954] = "kappa".ToCharArray();
			array[955] = "lambda".ToCharArray();
			array[956] = "mu".ToCharArray();
			array[957] = "nu".ToCharArray();
			array[958] = "xi".ToCharArray();
			array[959] = "omicron".ToCharArray();
			array[960] = "pi".ToCharArray();
			array[961] = "rho".ToCharArray();
			array[962] = "sigmaf".ToCharArray();
			array[963] = "sigma".ToCharArray();
			array[964] = "tau".ToCharArray();
			array[965] = "upsilon".ToCharArray();
			array[966] = "phi".ToCharArray();
			array[967] = "chi".ToCharArray();
			array[968] = "psi".ToCharArray();
			array[969] = "omega".ToCharArray();
			array[977] = "thetasym".ToCharArray();
			array[978] = "upsih".ToCharArray();
			array[982] = "piv".ToCharArray();
			array[8194] = "ensp".ToCharArray();
			array[8195] = "emsp".ToCharArray();
			array[8201] = "thinsp".ToCharArray();
			array[8204] = "zwnj".ToCharArray();
			array[8205] = "zwj".ToCharArray();
			array[8206] = "lrm".ToCharArray();
			array[8207] = "rlm".ToCharArray();
			array[8211] = "ndash".ToCharArray();
			array[8212] = "mdash".ToCharArray();
			array[8216] = "lsquo".ToCharArray();
			array[8217] = "rsquo".ToCharArray();
			array[8218] = "sbquo".ToCharArray();
			array[8220] = "ldquo".ToCharArray();
			array[8221] = "rdquo".ToCharArray();
			array[8222] = "bdquo".ToCharArray();
			array[8224] = "dagger".ToCharArray();
			array[8225] = "Dagger".ToCharArray();
			array[8226] = "bull".ToCharArray();
			array[8230] = "hellip".ToCharArray();
			array[8240] = "permil".ToCharArray();
			array[8242] = "prime".ToCharArray();
			array[8243] = "Prime".ToCharArray();
			array[8249] = "lsaquo".ToCharArray();
			array[8250] = "rsaquo".ToCharArray();
			array[8254] = "oline".ToCharArray();
			array[8260] = "frasl".ToCharArray();
			array[8364] = "euro".ToCharArray();
			array[8465] = "image".ToCharArray();
			array[8472] = "weierp".ToCharArray();
			array[8476] = "real".ToCharArray();
			array[8482] = "trade".ToCharArray();
			array[8501] = "alefsym".ToCharArray();
			array[8592] = "larr".ToCharArray();
			array[8593] = "uarr".ToCharArray();
			array[8594] = "rarr".ToCharArray();
			array[8595] = "darr".ToCharArray();
			array[8596] = "harr".ToCharArray();
			array[8629] = "crarr".ToCharArray();
			array[8656] = "lArr".ToCharArray();
			array[8657] = "uArr".ToCharArray();
			array[8658] = "rArr".ToCharArray();
			array[8659] = "dArr".ToCharArray();
			array[8660] = "hArr".ToCharArray();
			array[8704] = "forall".ToCharArray();
			array[8706] = "part".ToCharArray();
			array[8707] = "exist".ToCharArray();
			array[8709] = "empty".ToCharArray();
			array[8711] = "nabla".ToCharArray();
			array[8712] = "isin".ToCharArray();
			array[8713] = "notin".ToCharArray();
			array[8715] = "ni".ToCharArray();
			array[8719] = "prod".ToCharArray();
			array[8721] = "sum".ToCharArray();
			array[8722] = "minus".ToCharArray();
			array[8727] = "lowast".ToCharArray();
			array[8730] = "radic".ToCharArray();
			array[8733] = "prop".ToCharArray();
			array[8734] = "infin".ToCharArray();
			array[8736] = "ang".ToCharArray();
			array[8743] = "and".ToCharArray();
			array[8744] = "or".ToCharArray();
			array[8745] = "cap".ToCharArray();
			array[8746] = "cup".ToCharArray();
			array[8747] = "int".ToCharArray();
			array[8756] = "there4".ToCharArray();
			array[8764] = "sim".ToCharArray();
			array[8773] = "cong".ToCharArray();
			array[8776] = "asymp".ToCharArray();
			array[8800] = "ne".ToCharArray();
			array[8801] = "equiv".ToCharArray();
			array[8804] = "le".ToCharArray();
			array[8805] = "ge".ToCharArray();
			array[8834] = "sub".ToCharArray();
			array[8835] = "sup".ToCharArray();
			array[8836] = "nsub".ToCharArray();
			array[8838] = "sube".ToCharArray();
			array[8839] = "supe".ToCharArray();
			array[8853] = "oplus".ToCharArray();
			array[8855] = "otimes".ToCharArray();
			array[8869] = "perp".ToCharArray();
			array[8901] = "sdot".ToCharArray();
			array[8968] = "lceil".ToCharArray();
			array[8969] = "rceil".ToCharArray();
			array[8970] = "lfloor".ToCharArray();
			array[8971] = "rfloor".ToCharArray();
			array[9001] = "lang".ToCharArray();
			array[9002] = "rang".ToCharArray();
			array[9674] = "loz".ToCharArray();
			array[9824] = "spades".ToCharArray();
			array[9827] = "clubs".ToCharArray();
			array[9829] = "hearts".ToCharArray();
			array[9830] = "diams".ToCharArray();
			return array;
		}
	}
}
