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
        //private StringFormat m_sf;

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
        //当有数据传入时
        void m_in_num_DataTransfer(object sender, STNodeOptionEventArgs e) {
            //判断连线是否是连接状态(建立连线 断开连线 都会触发该事件)
            if (e.Status == ConnectionStatus.Connected) {
                if (sender == m_in_num1) {
                    if (e.TargetOption.Data != null) m_nNum1 = (int)e.TargetOption.Data;//TargetOption为触发此事件的Option
                } else {
                    if (e.TargetOption.Data != null) m_nNum2 = (int)e.TargetOption.Data;
                }
            } else {
                if (sender == m_in_num1) m_nNum1 = 0; else m_nNum2 = 0;
            }
            //向输出选项上的所有连线传输数据 输出选项上的所有连线都会触发 DataTransfer 事件
            m_out_num.TransferData(m_nNum1 + m_nNum2); //m_out_num.Data 将被自动设置
            this.Invalidate();
        }
        /// <summary>
        /// 当绘制选项文本时候 将数字绘制 因为STNodeOption.Text被protected修饰 STNode无法进行设置
        /// 因为作者并不建议对已经添加在STNode上的选项进行修改 尤其是在AutoSize被设置的情况下
        /// 若有需求 应当采用其他方式 比如:重绘 或者添加STNodeControl来显示变化的文本信息
        /// </summary>
        /// <param name="dt">绘制工具</param>
        /// <param name="op">需要绘制的选项</param>
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


    [STNode("/Number/", "Crystal_lz", "2212233137@qq.com", "www.st233.com", "This node can get two numbers add result")]
    public class NumberAddNewNode : NumberNode
    {
        private STNodeOption m_in_num1;
        private STNodeOption m_in_num2;
        private STNodeOption m_out_num;
        private long m_nNum1, m_nNum2;

        protected override void OnCreate()
        {
            base.OnCreate();
            this.Title = "NumberAddNew";

            m_in_num1 = this.InputOptions.Add("0", typeof(long), true);
            m_in_num2 = this.InputOptions.Add("0", typeof(long), true);
            m_out_num = this.OutputOptions.Add("0", typeof(long), false);

            m_in_num1.DataTransfer += new STNodeOptionEventHandler(m_in_num_DataTransfer);
            m_in_num2.DataTransfer += new STNodeOptionEventHandler(m_in_num_DataTransfer);
        }

        private long EpochTime(DateTime targetTime)
        {
            var epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            targetTime = targetTime.ToUniversalTime();
            TimeSpan elapsedTime = targetTime - epoch;
            return (long)elapsedTime.TotalSeconds;
        }

        //当有数据传入时
        void m_in_num_DataTransfer(object sender, STNodeOptionEventArgs e)
        {
            //判断连线是否是连接状态(建立连线 断开连线 都会触发该事件)
            if (e.Status == ConnectionStatus.Connected)
            {
                if (e.TargetOption.Data != null)
                {
                    if (sender == m_in_num1)
                    {
                        if (e.TargetOption.Data is DateTime)
                        {
                            m_nNum1 = EpochTime((DateTime)e.TargetOption.Data);
                        }
                        else
                        {
                            m_nNum1 = (long)e.TargetOption.Data;//TargetOption为触发此事件的Option
                        }
                    }
                    else
                    {
                        if (e.TargetOption.Data is DateTime)
                        {
                            m_nNum2 = EpochTime((DateTime)e.TargetOption.Data);
                        }
                        else
                        {
                            m_nNum2 = (long)e.TargetOption.Data;//TargetOption为触发此事件的Option
                        }
                    }
                }

                this.LetGetOptions = true;
                var olutnodenums = this.GetInputOptions();
                foreach (var olutnodenum in olutnodenums)
                {
                    var opt_cnt = olutnodenum.ConnectionCount;
                    var cnd_opt = olutnodenum.GetConnectedOption();

                }

            }
            else
            {
                if (sender == m_in_num1) m_nNum1 = 0; else m_nNum2 = 0;
            }

            var sum = m_nNum1 + m_nNum2;

            //向输出选项上的所有连线传输数据 输出选项上的所有连线都会触发 DataTransfer 事件
            m_out_num.TransferData(sum); //m_out_num.Data 将被自动设置

            this.SetOptionText(m_in_num1, m_nNum1.ToString());
            this.SetOptionText(m_in_num2, m_nNum2.ToString());
            this.SetOptionText(m_out_num, sum.ToString());

            this.Invalidate();
        }

    }

}
