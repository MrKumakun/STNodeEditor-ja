using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using ST.Library.UI.NodeEditor;

namespace WinNodeEditorDemo
{
    /// <summary>
    /// The built-in STNodeHub in the library is not marked with STNodeAttribute, so it cannot be displayed in the STNodeTreeView. Therefore, it needs to be extended.
    /// </summary>
    [STNode("/", "Crystal_lz", "2212233137@qq.com", "st233.com", "This is single Hub")]
    public class STNodeHubSingle : STNodeHub
    {
        public STNodeHubSingle()
            : base(true) {
            this.Title = "S_HUB";
        }
    }
    /// <summary>
    /// The built-in STNodeHub in the library is not marked with STNodeAttribute, so it cannot be displayed in the STNodeTreeView. Therefore, it needs to be extended.
    /// </summary>
    [STNode("/", "Crystal_lz", "2212233137@qq.com", "st233.com", "This multi is Hub")]
    public class STNodeHubMulti : STNodeHub
    {
        public STNodeHubMulti()
            : base(false) {
            this.Title = "M_HUB";
        }
    }
}
