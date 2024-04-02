using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using ST.Library.UI.NodeEditor;
using System.Drawing;


namespace WinNodeEditorDemo.Blender
{

    [STNode("/Blender", "DebugST", "2212233137@qq.com", "st233.com", "此节点仅演示UI自定义以及控件,并不包含功能.")]

    public class ColorNode : STNode
    {
        private STNodeOption m_op_out_color;
        private STNodeColorButton m_ctrl_btn;

        private Color _Color = Color.LightGray;//默认的DescriptorType不支持颜色的显示 需要扩展
        [STNodeProperty("_Color", "This is color", DescriptorType = typeof(WinNodeEditorDemo.DescriptorForColor))]
        public Color myColor
        {
            get { return _Color; }
            set { _Color = value; m_ctrl_btn.BackColor = value; }
        }

        protected override void OnCreate()
        {
            base.OnCreate();
            this.Title = "ColorNode";
            m_op_out_color = this.OutputOptions.Add("Color", typeof(Color), false);

            m_ctrl_btn = new STNodeColorButton();
            m_ctrl_btn.Text = "";
            m_ctrl_btn.BackColor = this._Color;
            m_ctrl_btn.DisplayRectangle = new Rectangle(1, 1, 16, 16);
            m_ctrl_btn.ValueChanged += (s, e) => { this._Color = m_ctrl_btn.BackColor; m_op_out_color.TransferData(this._Color); };
            this.Controls.Add(m_ctrl_btn);
        }

        protected override void OnOwnerChanged()
        {
            base.OnOwnerChanged();
            if (this.Owner == null) return;
            this.Owner.SetTypeColor(typeof(float), Color.Gray);
            this.Owner.SetTypeColor(typeof(Color), Color.Yellow);
        }
    }
}
