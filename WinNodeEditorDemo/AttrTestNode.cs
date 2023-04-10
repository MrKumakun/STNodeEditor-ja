using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using ST.Library.UI.NodeEditor;
using System.Drawing;
using System.Windows.Forms;

namespace WinNodeEditorDemo
{
    // STNodeAttribute is used to add metadata to the node class
    [STNode("/", "Crystal_lz", "2212233137@qq.com", "www.st233.com", "Description for this node\r\nThis class is\r\nSTNodeAttribute\r\nSTNodePropertyAttribute\r\nDemo class")]
    public class AttrTestNode : STNode
    {
        // We need to specify a descriptor for Color property 
        // since the default property editor doesn't support Color data type
        [STNodeProperty("Color", "Color information", DescriptorType = typeof(DescriptorForColor))]
        public Color Color { get; set; }

        [STNodeProperty("Integer Array", "Test for integer array")]
        public int[] IntArr { get; set; }

        [STNodeProperty("Boolean", "Test for boolean type")]
        public bool Bool { get; set; }

        [STNodeProperty("String", "Test for string type")]
        public string String { get; set; }

        [STNodeProperty("Integer", "Test for integer type")]
        public int Int { get; set; }

        [STNodeProperty("Float", "Test for float type")]
        public float Float { get; set; }

        [STNodeProperty("Enum Value", "Test for enum type -> FormBorderStyle")]
        public FormBorderStyle STYLE { get; set; }

        public AttrTestNode() {
            this.String = "string";
            IntArr = new int[] { 10, 20 };
            base.InputOptions.Add("string", typeof(string), false);
            base.OutputOptions.Add("string", typeof(string), false);
            this.Title = "AttrTestNode";
            this.TitleColor = Color.FromArgb(200, Color.Goldenrod);
        }
        /// <summary>
        /// This is a magic function. If a static method named ShowHelpInfo(string) 
        /// exists in the class and this class is marked with STNodeAttribute, 
        /// this method will be used as the help function in the property editor.
        /// </summary>
        /// <param name="strFileName">The file path of the module where this class is located.</param>
        public static void ShowHelpInfo(string strFileName) {
            MessageBox.Show("this is -> ShowHelpInfo(string);\r\n" + strFileName);
        }

        protected override void OnOwnerChanged() {
            base.OnOwnerChanged();
            if (this.Owner == null) return;
            this.Owner.SetTypeColor(typeof(string), Color.Goldenrod);
        }
    }

    // Since the default property editor doesn't support Color data type, 
    // we need to create a custom descriptor for it.
    public class DescriptorForColor : STNodePropertyDescriptor
    {
        private Rectangle m_rect; // This area is used to draw the color preview on the property window.

        // Called when the property window is initialized.
        protected override void OnSetItemLocation()
        {
            base.OnSetItemLocation();
            Rectangle rect = base.RectangleR;
            m_rect = new Rectangle(rect.Right - 25, rect.Top + 5, 19, 12);
        }

        // Convert the property value to a string to display on the property window.
        protected override string GetStringFromValue()
        {
            Color clr = (Color)this.GetValue(null);
            return clr.A + "," + clr.R + "," + clr.G + "," + clr.B;
        }

        // This code converts the string input from the property window into a Color property.
        // It is called when the user confirms their input in the property window.
        protected override object GetValueFromString(string strText) {
            string[] strClr = strText.Split(',');
            return Color.FromArgb(
                int.Parse(strClr[0]),   //A
                int.Parse(strClr[1]),   //R
                int.Parse(strClr[2]),   //G
                int.Parse(strClr[3]));  //B
        }
        // This method is called when the value area of the property window needs to be drawn.
        protected override void OnDrawValueRectangle(DrawingTools dt) {
            base.OnDrawValueRectangle(dt);                    // Draw using default behavior first, and then draw the color preview.
            dt.SolidBrush.Color = (Color)this.GetValue(null);
            dt.Graphics.FillRectangle(dt.SolidBrush, m_rect); // Fill with the color.
            dt.Graphics.DrawRectangle(Pens.Black, m_rect);    // Draw the border.
        }

        protected override void OnMouseClick(MouseEventArgs e) {
            // If the user clicks on the color preview area, a system color dialog is displayed.
            if (m_rect.Contains(e.Location)) {
                ColorDialog cd = new ColorDialog();
                if (cd.ShowDialog() != DialogResult.OK) return;
                this.SetValue(cd.Color, null);
                this.Invalidate();
                return;
            }
            // Otherwise, the default behavior is used to display a string input box.
            base.OnMouseClick(e);
        }
    }
}
