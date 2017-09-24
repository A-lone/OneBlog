using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Newtonsoft.Json.Linq;
using OneBlog.Configuration;

namespace OneBlog.UEditor
{

    /// <summary>
    /// Config 的摘要说明
    /// </summary>
    public class ConfigHandler : IHandler
    {
        private IOptions<EditorSettings> _editorSettings;

        public ConfigHandler(IOptions<EditorSettings> editorSettings)
        {
            _editorSettings = editorSettings;
        }

        public object Process()
        {
            return _editorSettings.Value;
        }
    }
}
