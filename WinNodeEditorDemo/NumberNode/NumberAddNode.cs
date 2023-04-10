using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using ST.Library.UI.NodeEditor;
using System.Drawing;

namespace WinNodeEditorDemo.NumberNode
{
    [STNode("/Number/", "Crystal_lz", "2212233137@qq.com", "www.st233.com", "This node can get two numbers add result")]
    public class NumberAddNode : NumberNode
    {
        private STNodeOption m_in_num1;
        private STNodeOption m_in_num2;
        private STNodeOption m_out_num;
        private int m_nNum1, m_nNum2;
        private StringFormat m_sf;

        protected override void OnCreate() {
            base.OnCreate();
            this.Title = "NumberAdd";
            m_sf = new StringFormat();
            m_sf.LineAlignment = StringAlignment.Center;
            m_in_num1 = new STNodeOption("", typeof(int), true);//只能有一个连线
            m_in_num2 = new STNodeOption("", typeof(int), true);//只能有一个连线
            m_out_num = new STNodeOption("", typeof(int), false);//可以多个连线
            this.InputOptions.Add(m_in_num1);
            this.InputOptions.Add(m_in_num2);
            this.OutputOptions.Add(m_out_num);
            m_in_num1.DataTransfer += new STNodeOptionEventHandler(m_in_num_DataTransfer);
            m_in_num2.DataTransfer += new STNodeOptionEventHandler(m_in_num_DataTransfer);
        }

        // Called when data is transferred to an input option
        void m_in_num_DataTransfer(object sender, STNodeOptionEventArgs e) {
            // Check if the connection is established (connection established or broken will trigger this event)
            if (e.Status == ConnectionStatus.Connected) {
                if (sender == m_in_num1) {
                    if (e.TargetOption.Data != null) m_nNum1 = (int)e.TargetOption.Data;//TargetOption为触发此事件的Option
                } else {
                    if (e.TargetOption.Data != null) m_nNum2 = (int)e.TargetOption.Data;
                }
            } else {
                if (sender == m_in_num1) m_nNum1 = 0; else m_nNum2 = 0;
            }
            // Transfer the sum of the two numbers to all output options (which will trigger DataTransfer event for all connected options)
            m_out_num.TransferData(m_nNum1 + m_nNum2); // m_out_num.Data will be automatically set
            this.Invalidate();
        }
        /// <summary>
        /// When drawing the option text, draw the numbers because STNodeOption.Text is protected and cannot be set on STNode.
        /// Because the author does not recommend modifying options that have already been added to STNode, especially when AutoSize is set.
        /// If there is a need, other methods should be used, such as redrawing or adding STNodeControl to display changing text information.
        /// </summary>
        /// <param name="dt">Drawing tool</param>
        /// <param name="op">The option that needs to be drawn</param>
        protected override void OnDrawOptionText(DrawingTools dt, STNodeOption op) {
            base.OnDrawOptionText(dt, op);
            string strText = "";
            if (op == m_in_num1) {
                m_sf.Alignment = StringAlignment.Near;
                strText = m_nNum1.ToString();
            } else if (op == m_in_num2) {
                m_sf.Alignment = StringAlignment.Near;
                strText = m_nNum2.ToString();
            } else {
                m_sf.Alignment = StringAlignment.Far;
                strText = (m_nNum1 + m_nNum2).ToString();
            }
            dt.Graphics.DrawString(strText, this.Font, Brushes.White, op.TextRectangle, m_sf);
        }
    }
}
