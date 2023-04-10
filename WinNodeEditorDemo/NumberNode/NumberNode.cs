using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using ST.Library.UI.NodeEditor;
using System.Drawing;

namespace WinNodeEditorDemo.NumberNode
{
    /// <summary>
    /// Base class for Number nodes, used to determine node style, title color, and data type color.
    /// </summary>
    public abstract class NumberNode : STNode
    {
        protected override void OnCreate() {
            base.OnCreate();
            this.TitleColor = Color.FromArgb(200, Color.CornflowerBlue);
        }
        protected override void OnOwnerChanged() {
            base.OnOwnerChanged();
            if (this.Owner != null) this.Owner.SetTypeColor(typeof(int), Color.CornflowerBlue);
        }
    }
}
