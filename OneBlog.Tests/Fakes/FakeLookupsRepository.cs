using OneBlog.Core.Data.Contracts;
using OneBlog.Core.Data.Models;

namespace OneBlog.Tests.Fakes
{
    class FakeLookupsRepository : ILookupsRepository
    {
        public Lookups GetLookups()
        {
            return new Lookups();
        }

        public void SaveEditorOptions(EditorOptions options)
        {

        }
    }
}
