using System;
using System.Globalization;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;
using ESRI.ArcGIS.AnalysisTools;
using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.Controls;
using ESRI.ArcGIS.esriSystem;
using ESRI.ArcGIS.Geodatabase;
using ESRI.ArcGIS.Geometry;
using ESRI.ArcGIS.Geoprocessing;
using ESRI.ArcGIS.Geoprocessor;


namespace ArcEngine
{
    public partial class EraseForm : Form
    {
        private IHookHelper _hookHelper;

        private AddLayer _addLayer;

        private IMap _map;
        
        public EraseForm(IHookHelper hookHelper, AddLayer addLayer)
        {
            InitializeComponent();

            _hookHelper = hookHelper;

            _addLayer = addLayer;

            _map = hookHelper.FocusMap;
        }
        
        private void EraseForm_Load(object sender, EventArgs e)
        {
            // 添加 输入元素、擦除元素
            
            for (int i = 0; i < _map.LayerCount; i++)
            {
                inputBox.Items.Add(_map.Layer[i].Name);
                eraseBox.Items.Add(_map.Layer[i].Name);
            }
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            SeletPath();
        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            SeletPath();
        }

        /// <summary>
        /// 设置默认输出元素
        /// </summary>
        private void SeletPath()
        {
            if (inputBox.Text != ""  && eraseBox.Text != "")
            {
                string path = System.IO.Path.GetTempPath();

                outputBox.Text = System.IO.Path.Combine(path, String.Format("{0}_{1}_erase.shp", inputBox.Text, eraseBox.Text));

            }
        }

        
        /// <summary>
        /// 选择自定义保存路径
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void 选择_Click(object sender, EventArgs e)
        {
            //set the output layer
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.CheckPathExists = true;
            saveFileDialog.Filter = "Shapefile (*.shp)|*.shp";
            saveFileDialog.OverwritePrompt = true;
            saveFileDialog.Title = "Output Layer";
            saveFileDialog.RestoreDirectory = true;
            saveFileDialog.FileName = String.Format("{0}_{1}_erase.shp", inputBox.Text, eraseBox.Text);

            DialogResult dr = saveFileDialog.ShowDialog();
            if (dr == DialogResult.OK)
                outputBox.Text = saveFileDialog.FileName;
        }

        private void 确定_Click(object sender, EventArgs e)
        {
            IFeatureLayer inputLayer = GetLayer(inputBox.Text);

            IFeatureLayer eraseLayer = GetLayer(eraseBox.Text);

            string path = outputBox.Text;
            string folder = System.IO.Path.GetDirectoryName(path);
            string filename = System.IO.Path.GetFileName(path);
            
//            IWorkspaceName workspaceName = new WorkspaceNameClass();
//            workspaceName.WorkspaceFactoryProgID = "esriDataSourcesFile.ShapefileWorkspaceFactory";
//            workspaceName.PathName = folder;
//            
//            IFeatureClassName featureClassName = new FeatureClassNameClass();
//            featureClassName.FeatureType = esriFeatureType.esriFTSimple;
//            featureClassName.ShapeFieldName = "Shape";
//            featureClassName.ShapeType = inputLayer.FeatureClass.ShapeType;
//            
//            IDatasetName datasetName = featureClassName as IDatasetName;
//            datasetName.Name = filename;
//            datasetName.WorkspaceName = workspaceName;

//            IFeatureLayer outLayer = new FeatureLayerClass();
//            IFeatureClass outClass = null;
//            outClass = EraseRun(inputLayer.FeatureClass, eraseLayer.FeatureClass, outClass);

            EraseRun(inputLayer.FeatureClass, eraseLayer.FeatureClass, outputBox.Text);
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="name">图层名</param>
        /// <returns>图层名对应图层</returns>
        private IFeatureLayer GetLayer(string name)
        {
            for (int i = 0; i < _map.LayerCount; i++)
            {
                if (_map.Layer[i].Name == name)
                {
                    return _map.Layer[i] as IFeatureLayer;
                }
            }
            
            Show("没有找到图层 ！");
            
            return null;
        }

        private void EraseRun(IFeatureClass in_features, IFeatureClass erase_features, object out_feature_class)
        {
//            IAoInitialize m_AoInitialize = new AoInitializeClass();
//            esriLicenseStatus licenseStatus = esriLicenseStatus.esriLicenseUnavailable;
//              
//            licenseStatus = m_AoInitialize.Initialize(esriLicenseProductCode.esriLicenseProductCodeEngine);

            
            Erase erase = new Erase()
            {
                in_features = in_features,
                erase_features = erase_features,
                out_feature_class = out_feature_class
            };
            
            Geoprocessor processor = new Geoprocessor();
            
            processor.OverwriteOutput = true;



            try
            {
                IGeoProcessorResult result =  processor.Execute(erase, new TrackCancelClass()) as IGeoProcessorResult;
            }
            catch (Exception e)
            {
                Show(e.ToString());
                throw;
            }

           
        }
        
        private void Show(string s)
        {
            MessageBox.Show(s);
        }

        
    }
}
