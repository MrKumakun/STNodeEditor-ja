using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ST.Library.UI.NodeEditor;
using System.Drawing;
using System.Threading;

namespace WinNodeEditorDemo
{

    [STNode("/Clock", "DebugST", "2212233137@qq.com", "st233.com", "此节点仅演示UI自定义以及控件,并不包含功能.")]

    public class ClockNode : STNode
    {
        private Thread m_thread;
        private STNodeOption m_op_out_time;

        protected override void OnCreate()
        {
            base.OnCreate();
            this.Title = "ClockNode";
            m_op_out_time = this.OutputOptions.Add("Time", typeof(DateTime), false);
        }
        //当被添加或者移除
        protected override void OnOwnerChanged()
        {
            base.OnOwnerChanged();
            if (this.Owner == null)
            {   //如果是被移除 停止线程
                if (m_thread != null) m_thread.Abort();
                return;
            }
            this.Owner.SetTypeColor(typeof(DateTime), Color.DarkCyan);
            m_thread = new Thread(() => {
                while (true)
                {
                    Thread.Sleep(1000);
                    //STNodeOption.TransferData(object)会自动设置STNodeOption.Data
                    //然后自动向所有连接的选项进行数据传递
                    //m_op_out_time.TransferData(DateTime.Now);
                    //如果你需要一些耗时操作STNode同样提供了Begin/Invoke()操作
                    this.BeginInvoke(new MethodInvoker(() => {
                        m_op_out_time.TransferData(DateTime.Now);
                    }));
                }
            })
            { IsBackground = true };
            m_thread.Start();
        }
    }


    [STNode("/Clock", "DebugST", "2212233137@qq.com", "st233.com", "此节点仅演示UI自定义以及控件,并不包含功能.")]
    public class ShowClockNode : STNode
    {
        private STNodeOption m_op_time_in;
        protected override void OnCreate()
        {
            base.OnCreate();
            this.Title = "ShowTime";
            //采用 "single-connection" 模式
            m_op_time_in = this.InputOptions.Add("--", typeof(DateTime), true);
            //当有数据时会自动触发此事件
            m_op_time_in.DataTransfer += new STNodeOptionEventHandler(op_DataTransfer);
        }

        void op_DataTransfer(object sender, STNodeOptionEventArgs e)
        {
            //当连接的建立与断开都会触发此事件 所以需要判断连接状态
            if (e.Status != ConnectionStatus.Connected || e.TargetOption.Data == null)
            {
                //当 STNode.AutoSize=true 并不建议使用STNode.SetOptionText
                //因为当文本发生改变时候会重新计算布局 正确的做法是自定义一个如Lable控件
                //作为时间的显示 当然这里为了演示方式采用此方案
                this.SetOptionText(m_op_time_in, "--");
            }
            else if ((e.TargetOption.Data) is DateTime)
            {
                this.SetOptionText(m_op_time_in, ((DateTime)e.TargetOption.Data).ToString());
            }
            else
            {
                var epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
                var tm = (double)((long)e.TargetOption.Data);
                this.SetOptionText(m_op_time_in, epoch.AddSeconds(tm).ToString());

            }
        }

    }

}
