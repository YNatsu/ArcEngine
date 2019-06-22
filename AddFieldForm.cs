using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ESRI.ArcGIS.Geodatabase;

namespace ArcEngine
{
    public delegate bool AddField(string fieldName, esriFieldType filedType, int fieldLength);
    
    public partial class AddFieldForm : Form
    {
        private AddField _addField;
        
        public AddFieldForm(AddField addField)
        {
            InitializeComponent();

            _addField = addField;
        }

        private void 确定_Click(object sender, EventArgs e)
        {
            string fieldName = textBox1.Text;

            string fieldType = comboBox1.Text;

            int fieldSize = Convert.ToInt16(numericUpDown1.Text);

            switch (fieldType)
            {
                case "string":

                    _addField(fieldName, esriFieldType.esriFieldTypeString, fieldSize);

                    break;

                case "int":

                    _addField(fieldName, esriFieldType.esriFieldTypeInteger, fieldSize);

                    break;

                case "double":

                    _addField(fieldName, esriFieldType.esriFieldTypeDate, fieldSize);

                    break;
            }


            Close();
        }

        private void 取消_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void AddFieldForm_Load(object sender, EventArgs e)
        {

        }
    }
}
