namespace MarkdownKit
{
    internal class LinkInfo
	{
		public LinkInfo(LinkDefinition def, string link_text)
		{
			this.def = def;
			this.link_text = link_text;
		}

		public LinkDefinition def;
		public string link_text;
	}

}
