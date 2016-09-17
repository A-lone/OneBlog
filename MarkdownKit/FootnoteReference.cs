namespace MarkdownKit
{
    class FootnoteReference
	{
		public FootnoteReference(int index, string id)
		{
			this.index = index;
			this.id = id;
		}
		public int index;
		public string id;
	}
}
