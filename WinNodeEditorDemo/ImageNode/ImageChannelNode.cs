using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using ST.Library.UI.NodeEditor;
using System.Drawing;
using System.Drawing.Imaging;

namespace WinNodeEditorDemo.ImageNode
{
    [STNode("/Image")]
    public class ImageChannelNode : ImageBaseNode
    {
        private STNodeOption m_op_img_in;   //Input Node
        private STNodeOption m_op_img_r;    //Red Output
        private STNodeOption m_op_img_g;    //Green Output
        private STNodeOption m_op_img_b;    //Blue Output

        protected override void OnCreate() {
            base.OnCreate();
            this.Title = "ImageChannel";

            m_op_img_in = this.InputOptions.Add("", typeof(Image), true);
            m_op_img_r = this.OutputOptions.Add("R", typeof(Image), false);
            m_op_img_g = this.OutputOptions.Add("G", typeof(Image), false);
            m_op_img_b = this.OutputOptions.Add("B", typeof(Image), false);
            //When there is data input to the input node
            m_op_img_in.DataTransfer += new STNodeOptionEventHandler(m_op_img_in_DataTransfer);
        }

        void m_op_img_in_DataTransfer(object sender, STNodeOptionEventArgs e) {
            //If the current state is not connected or the received data is empty
            if (e.Status != ConnectionStatus.Connected || e.TargetOption.Data == null) {
                m_op_img_out.TransferData(null);    //Forward empty data to all output nodes
                m_op_img_r.TransferData(null);
                m_op_img_g.TransferData(null);
                m_op_img_b.TransferData(null);
                m_img_draw = null;                  //Set the image to be displayed for drawing to null
            } else {
                Bitmap bmp = (Bitmap)e.TargetOption.Data;           //Otherwise, calculate the RGB image of the picture
                Bitmap bmp_r = new Bitmap(bmp.Width, bmp.Height);
                Bitmap bmp_g = new Bitmap(bmp.Width, bmp.Height);
                Bitmap bmp_b = new Bitmap(bmp.Width, bmp.Height);
                BitmapData bmpData = bmp.LockBits(new Rectangle(Point.Empty, bmp.Size), ImageLockMode.ReadOnly, PixelFormat.Format32bppArgb);
                BitmapData bmpData_r = bmp_r.LockBits(new Rectangle(Point.Empty, bmp.Size), ImageLockMode.WriteOnly, PixelFormat.Format32bppArgb);
                BitmapData bmpData_g = bmp_g.LockBits(new Rectangle(Point.Empty, bmp.Size), ImageLockMode.WriteOnly, PixelFormat.Format32bppArgb);
                BitmapData bmpData_b = bmp_b.LockBits(new Rectangle(Point.Empty, bmp.Size), ImageLockMode.WriteOnly, PixelFormat.Format32bppArgb);
                byte[] byColor = new byte[bmpData.Height * bmpData.Stride];
                byte[] byColor_r = new byte[byColor.Length];
                byte[] byColor_g = new byte[byColor.Length];
                byte[] byColor_b = new byte[byColor.Length];
                System.Runtime.InteropServices.Marshal.Copy(bmpData.Scan0, byColor, 0, byColor.Length);
                for (int y = 0; y < bmpData.Height; y++) {
                    int ny = y * bmpData.Stride;
                    for (int x = 0; x < bmpData.Width; x++) {
                        int nx = x << 2;
                        byColor_b[ny + nx] = byColor[ny + nx];
                        byColor_g[ny + nx + 1] = byColor[ny + nx + 1];
                        byColor_r[ny + nx + 2] = byColor[ny + nx + 2];
                        byColor_r[ny + nx + 3] = byColor_g[ny + nx + 3] = byColor_b[ny + nx + 3] = byColor[ny + nx + 3];
                    }
                }
                bmp.UnlockBits(bmpData);
                System.Runtime.InteropServices.Marshal.Copy(byColor_r, 0, bmpData_r.Scan0, byColor_r.Length);
                System.Runtime.InteropServices.Marshal.Copy(byColor_g, 0, bmpData_g.Scan0, byColor_g.Length);
                System.Runtime.InteropServices.Marshal.Copy(byColor_b, 0, bmpData_b.Scan0, byColor_b.Length);
                bmp_r.UnlockBits(bmpData_r);
                bmp_g.UnlockBits(bmpData_g);
                bmp_b.UnlockBits(bmpData_b);
                m_op_img_out.TransferData(bmp); //The "out" option outputs the original image
                m_op_img_r.TransferData(bmp_r); //R option outputs R image
                m_op_img_g.TransferData(bmp_g);
                m_op_img_b.TransferData(bmp_b);
                m_img_draw = bmp;               //Image to be displayed needs to be drawn
            }
        }

        protected override void OnDrawBody(DrawingTools dt) {
            base.OnDrawBody(dt);
            Graphics g = dt.Graphics;
            Rectangle rect = new Rectangle(this.Left + 10, this.Top + 30, 120, 80);
            g.FillRectangle(Brushes.Gray, rect);
            if (m_img_draw != null) g.DrawImage(m_img_draw, rect);
        }
    }
}
