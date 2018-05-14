using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace XML_Exporter.SubWindow
{
    public partial class LoadingScreen : Form
    {
        public LoadingScreen()
        {
            InitializeComponent();
        }

        private void SplashScreen_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        public void ShowCenterParent(IWin32Window own_window)
        {
            Form parent_form = own_window as Form;

            Point parent_point_to_screen = parent_form.PointToScreen(Point.Empty);

            int x = Convert.ToInt32((parent_form.Bounds.Width / 2) - (this.ClientSize.Width / 2)) + parent_point_to_screen.X;
            int y = Convert.ToInt32((parent_form.Bounds.Height / 2) - (this.ClientSize.Height / 2)) + parent_point_to_screen.Y;

            this.Location = new Point(x, y);
            this.Show();
        }
    }
}
