using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ST.Library.UI.NodeEditor;
using System.Drawing;
using System.Windows.Forms;
using System.Reflection;

namespace WinNodeEditorDemo.ImageNode
{

    [STNode("Image", "Crystal_lz", "2212233137@qq.com", "st233.com", "Image Node")]
    public class ImageInputNode : ImageBaseNode
    {
        private string _FileName;//The default DescriptorType doesn't support selecting a file path, so an extension is needed
        [STNodeProperty("InputImage", "Click to select a image", DescriptorType = typeof(OpenFileDescriptor))]
        public string FileName {
            get { return _FileName; }
            set {
                Image img = null;                       //When the file name is set, load the image and output it to the output node
                if (!string.IsNullOrEmpty(value)) {
                    img = Image.FromFile(value);
                }
                if (m_img_draw != null) m_img_draw.Dispose();
                m_img_draw = img;
                _FileName = value;
                m_op_img_out.TransferData(m_img_draw, true);
                this.Invalidate();
            }
        }

        protected override void OnCreate() {
            base.OnCreate();
            this.Title = "ImageInput";
        }

        protected override void OnDrawBody(DrawingTools dt) {
            base.OnDrawBody(dt);
            Graphics g = dt.Graphics;
            Rectangle rect = new Rectangle(this.Left + 10, this.Top + 30, 140, 80);
            g.FillRectangle(Brushes.Gray, rect);
            if (m_img_draw != null) g.DrawImage(m_img_draw, rect);
        }
    }
    /// <summary>
    /// Extend the default Descriptor to support file path selection
    /// </summary>
    public class OpenFileDescriptor : STNodePropertyDescriptor
    {
        private Rectangle m_rect_open;  //Area to draw the "Open" button is required
        private StringFormat m_sf;

        public OpenFileDescriptor() {
            m_sf = new StringFormat();
            m_sf.Alignment = StringAlignment.Center;
            m_sf.LineAlignment = StringAlignment.Center;
        }

        protected override void OnSetItemLocation() {   //When determining the area where this property needs to be displayed on the STNodePropertyGrid
            base.OnSetItemLocation();                   //Calculate the area to be drawn for the "open" button
            m_rect_open = new Rectangle(
                this.RectangleR.Right - 20,
                this.RectangleR.Top,
                20, 
                this.RectangleR.Height);
        }

        protected override void OnMouseClick(System.Windows.Forms.MouseEventArgs e) {
            if (m_rect_open.Contains(e.Location)) {     //Clicking on the "Open" area will bring up a file selection dialog
                OpenFileDialog ofd = new OpenFileDialog();
                ofd.Filter = "*.jpg|*.jpg|*.png|*.png";
                if (ofd.ShowDialog() != DialogResult.OK) return;
                this.SetValue(ofd.FileName);
            } else base.OnMouseClick(e);                //Otherwise, the default handling is to pop up a text input box
        }

        protected override void OnDrawValueRectangle(DrawingTools dt) {
            base.OnDrawValueRectangle(dt);              //When drawing the property area of this property in STNodePropertyGrid, draw the "open" button on it.
            dt.Graphics.FillRectangle(Brushes.Gray, m_rect_open);
            dt.Graphics.DrawString("+", this.Control.Font, Brushes.White, m_rect_open, m_sf);
        }
    }
}
