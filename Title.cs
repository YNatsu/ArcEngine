using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ArcEngine
{
    public delegate void AddTitle(String title);
    
    public partial class TitleForm : Form
    {

        private AddTitle _addTitle;
        
        public TitleForm(AddTitle addTitle)
        {
            InitializeComponent();

            _addTitle = addTitle;
        }

        private void 确定_Click(object sender, EventArgs e)
        {
            if (titleBox.Text != null)
            {
                _addTitle(titleBox.Text);
                
                Close();
            }
        }

        private void 取消_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
