using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using ST.Library.UI.NodeEditor;
using System.Drawing;

namespace WinNodeEditorDemo.NumberNode
{
    /// <summary>
    /// This node provides an integer input through the Number property
    /// </summary>
    [STNode("/Number","Crystal_lz","2212233137@qq.com","st233.com","Number input node")]
    public class NumberInputNode : NumberNode
    {
        private int _Number;
        [STNodeProperty("Input","this is input number")]
        public int Number {
            get { return _Number; }
            set { 
                _Number = value;
                m_op_number.TransferData(value); //Pass data down.
                this.Invalidate();
            }
        }

        private STNodeOption m_op_number;       //Output options
        private StringFormat m_sf = new StringFormat();

        protected override void OnCreate() {
            base.OnCreate();
            this.Title = "NumberInput";
            m_op_number = new STNodeOption("", typeof(int), false);
            this.OutputOptions.Add(m_op_number);
            m_sf = new StringFormat();
            m_sf.LineAlignment = StringAlignment.Center;
            m_sf.Alignment = StringAlignment.Far;
        }
        /// <summary>
        /// When drawing option text, draw numbers because STNodeOption.Text is protected and cannot be set on STNode.
        /// Because the author does not recommend modifying options that have already been added to STNode, especially when AutoSize is set.
        /// If necessary, other methods should be used, such as redraw or add STNodeControl to display changing text information.
        /// </summary>
        /// <param name="dt">DrawingTools</param>
        /// <param name="op">The option that needs to be drawn.</param>
        protected override void OnDrawOptionText(DrawingTools dt, STNodeOption op) {
            base.OnDrawOptionText(dt, op);
            dt.Graphics.DrawString(this._Number.ToString(), this.Font, Brushes.White, op.TextRectangle, m_sf);
        }
    }
}
