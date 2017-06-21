using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Microsoft.Exchange.Data.TextConverters
{
    public class HtmlToHtml
    {
        public bool FilterHtml { get; internal set; }
        public bool NormalizeHtml { get; internal set; }
        public bool OutputHtmlFragment { get; internal set; }

        internal void Convert(TextReader sourceReader, TextWriter destinationWriter)
        {
            throw new NotImplementedException();
        }

        internal void Convert(TextReader sourceReader, Stream destinationStream)
        {
            throw new NotImplementedException();
        }
    }
}
