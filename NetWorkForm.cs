using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ESRI.ArcGIS.DataSourcesGDB;
using ESRI.ArcGIS.Geodatabase;

namespace ArcEngine
{
    public delegate void SetNetWorkPropety(string featureName, string networkName);

    public partial class NetWorkForm : Form
    {
        private string _path;
        private SetNetWorkPropety _setNetWorkPropety;

        public NetWorkForm(string path, SetNetWorkPropety setNetWorkPropety)
        {
            InitializeComponent();
            _path = path;
            _setNetWorkPropety = setNetWorkPropety;
        }

        private void NetWorkForm_Load(object sender, EventArgs e)
        {
            IWorkspaceFactory factory = new FileGDBWorkspaceFactoryClass();

            IWorkspace workspace = factory.OpenFromFile(_path, 0);

            IEnumDataset enumDataset = workspace.get_Datasets(esriDatasetType.esriDTAny);

            IDataset dataset = enumDataset.Next();

            while (dataset != null)
            {
                if (dataset is IFeatureDataset) //要素数据集
                {
                    this.featureDataset.Items.Add(dataset.Name);
                }

                dataset = enumDataset.Next();
            }
        }

        /// <summary>
        /// 由 要素数据集 更改 网络要素集
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void featureDataset_SelectedIndexChanged(object sender, EventArgs e)
        {
            IWorkspaceFactory factory = new FileGDBWorkspaceFactoryClass();

            IWorkspace workspace = factory.OpenFromFile(_path, 0);

            IEnumDataset enumDataset = workspace.get_Datasets(esriDatasetType.esriDTAny);

            IDataset dataset = enumDataset.Next();

            while (dataset != null)
            {
                if (dataset is IFeatureDataset) //要素数据集
                {
                    if (featureDataset.Text == dataset.Name)
                    {
                        IFeatureWorkspace featureWorkspace =
                            workspace as IFeatureWorkspace;

                        IFeatureDataset iFeatureDataset =
                            dataset as IFeatureDataset;

                        IEnumDataset iEnumDataset = iFeatureDataset.Subsets;
                        
                        IDataset iDataset = iEnumDataset.Next();

                        while (iDataset != null)
                        {
                            if (iDataset is NetworkDataset) // 网络要素列
                            {
                                this.networkDataset.Items.Add(iDataset.Name);
                            }

                            iDataset = iEnumDataset.Next();
                        }
                    }
                }

                dataset = enumDataset.Next();
            }
        }

        private void 确定_Click(object sender, EventArgs e)
        {
            _setNetWorkPropety(featureDataset.Text, networkDataset.Text);
        }

        private void 取消_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}