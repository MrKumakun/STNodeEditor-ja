using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using ST.Library.UI.NodeEditor;
using System.Drawing;

namespace WinNodeEditorDemo.ImageNode
{
    /// <summary>
    /// Base class for image nodes used to determine node style, title color, and data type color.
    /// </summary>
    public abstract class ImageBaseNode : STNode
    {
        /// <summary>
        /// Image to be drawn as display.
        /// </summary>
        protected Image m_img_draw;
        /// <summary>
        /// Output node
        /// </summary>
        protected STNodeOption m_op_img_out;

        protected override void OnCreate() {
            base.OnCreate();
            m_op_img_out = this.OutputOptions.Add("", typeof(Image), false);
            this.AutoSize = false;          //This node requires a customized UI, so there is no need for AutoSize.
            //this.Size = new Size(320,240);
            this.Width = 160;               //Manually set the size of the node
            this.Height = 120;
            this.TitleColor = Color.FromArgb(200, Color.DarkCyan);
        }

        protected override void OnOwnerChanged() {  //Submit data type color to the editor
            base.OnOwnerChanged();
            if (this.Owner == null) return;
            this.Owner.SetTypeColor(typeof(Image), Color.DarkCyan);
        }
    }
}
