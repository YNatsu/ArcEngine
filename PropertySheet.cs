using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.Geodatabase;

namespace ArcEngine
{
    public partial class PropertySheet : Form
    {
        // 点击的图层
        
        private IFeatureLayer _layer;

        

        public PropertySheet(IFeatureLayer layer)
        {
            InitializeComponent();

            _layer = layer;
        }
        
        private void PropertySheet_Load(object sender, EventArgs e)
        {
            ShowFeatureLayerAttrib(_layer );
            
            ShowFeatures(_layer, null, false);

            _index = 0;
        }
        
        public void ShowFeatureLayerAttrib(IFeatureLayer layerIFeatureLayer)
        {
            if (layerIFeatureLayer != null)
            {
                
                
                DataTable  featureTableDataTable = new DataTable();
                
                // 矢量要素图层包含的要素类
                
                IFeatureClass fcLayerIFeatureClass = layerIFeatureLayer.FeatureClass;
                
                // 要素类中包含的属性字段总数

                int nFieldCount = fcLayerIFeatureClass.Fields.FieldCount;

                for (int i = 0; i < nFieldCount; i++)
                {
                    DataColumn fieldDataColumn = new DataColumn {ColumnName = fcLayerIFeatureClass.Fields.Field[i].Name};
                    
                    switch (fcLayerIFeatureClass.Fields.Field[i].Type)
                    {
                          case  esriFieldType.esriFieldTypeOID:
                              
                              fieldDataColumn.DataType = Type.GetType("System.Int32");
                              
                              break;
                              
                          
                          case esriFieldType.esriFieldTypeGeometry:

                              fieldDataColumn.DataType = Type.GetType("System.String");
                              
                              break;
                          
                          case esriFieldType.esriFieldTypeInteger:
                              
                              fieldDataColumn.DataType = Type.GetType("System.Int32");
                              
                              break;
                          
                          case esriFieldType.esriFieldTypeSingle:
                              
                              fieldDataColumn.DataType = Type.GetType("System.Int32");
                              
                              break;
                          
                          case esriFieldType.esriFieldTypeSmallInteger:
                              
                              fieldDataColumn.DataType = Type.GetType("System.Int32");
                              
                              break;
                          
                          case esriFieldType.esriFieldTypeString:
                              
                              fieldDataColumn.DataType = Type.GetType("System.String");
                              
                              break;
                          
                          case esriFieldType.esriFieldTypeDouble:
                              
                              fieldDataColumn.DataType = Type.GetType("System.Double");
                              
                              break;
                    }

                    featureTableDataTable.Columns.Add(fieldDataColumn);
                }

                dataGridView1.DataSource = featureTableDataTable;
                
            }

        }
        
        private void ShowFeatures(IFeatureLayer featureLayerIFeatureLayer, IQueryFilter conditionIQueryFilter, bool drawshape)
        {
            
            if (featureLayerIFeatureLayer != null)
            {
                // 矢量要素图层关联的要素类
                
                // IFeatureClass => IFeatureLayer.FeatureClass
                
                IFeatureClass featureClassIFeatureClass = featureLayerIFeatureLayer.FeatureClass;
                
                // IFeatureCursor 变量用于获取查询结果
                
                // IFeatureCursor => IFeatureClass.Search(...)
                
                IFeatureCursor cursorIFeatureCursor;

                try
                {
                    cursorIFeatureCursor = featureClassIFeatureClass.Search(conditionIQueryFilter, false);
                }
                catch (Exception)
                {
                    cursorIFeatureCursor = null;
                }

                if (cursorIFeatureCursor != null)
                {
                  
                    
                    // IFeatureSelection =>  IFeatureLayer
                    
                    IFeatureSelection featureSelectionIFeatureSelection = featureLayerIFeatureLayer as IFeatureSelection;

                    if (featureSelectionIFeatureSelection != null)
                    {
                        featureSelectionIFeatureSelection.Clear();

                        DataTable featureTableDataTable = (DataTable) dataGridView1.DataSource;

                        featureTableDataTable.Rows.Clear();

                        IFeature featureIFeature = cursorIFeatureCursor.NextFeature();

                        while (featureIFeature != null)
                        {
                            if (drawshape)
                            {
                                featureSelectionIFeatureSelection.Add(featureIFeature);
                            }

                            DataRow featureRecDataRow = featureTableDataTable.NewRow();

                            int nFieldCount = featureIFeature.Fields.FieldCount;
                            
                            // 依次读取矢量要素的每一个字段值
                            
                            for (int i = 0; i < nFieldCount; i++)
                            {
                                switch (featureIFeature.Fields.Field[i].Type)
                                {
                                    case esriFieldType.esriFieldTypeOID:

                                        featureRecDataRow[featureIFeature.Fields.Field[i].Name] = Convert.ToInt32(featureIFeature.Value[i]);
                                        
                                        break;
                                    
                                    case esriFieldType.esriFieldTypeGeometry:

                                        featureRecDataRow[featureIFeature.Fields.Field[i].Name] = 
                                            
                                            featureIFeature.Shape.GeometryType.ToString();
                                                                                        
                                        break;
                                    
                                    case esriFieldType.esriFieldTypeInteger:
                                        
                                        featureRecDataRow[featureIFeature.Fields.Field[i].Name] = Convert.ToInt32(featureIFeature.Value[i]);
                                        
                                        break;
                                    
                                    case esriFieldType.esriFieldTypeString:

                                        featureRecDataRow[featureIFeature.Fields.Field[i].Name] =

                                            featureIFeature.Value[i].ToString();
                                        
                                        break;
                                    
                                    case esriFieldType.esriFieldTypeDouble:

                                        featureRecDataRow[featureIFeature.Fields.Field[i].Name] =

                                            Convert.ToDouble(featureIFeature.Value[i]);
                                        
                                        break;
                                }     
                            }

                            featureTableDataTable.Rows.Add(featureRecDataRow);

                            featureIFeature = cursorIFeatureCursor.NextFeature();
                        }
                        
                        
                            
                           
                    }
                }
            }
        }
        
        
        // 添加字段
        
        private void 添加字段ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AddFieldForm addFieldForm = new AddFieldForm(AddField);

            addFieldForm.ShowDialog();
            
            ShowFeatures(_layer, null, false);
        }
        
        

        public bool AddField(string fieldName, esriFieldType filedType, int fieldLength)
        {
            //var featureLayer = axMapControl1.get_Layer(0) as IFeatureLayer;
            
            try
            {
                if (_layer != null)
                {
                    IFields pFields = _layer.FeatureClass.Fields;
                }

                IFieldEdit pFieldEdit = new FieldClass();
                
                pFieldEdit.Name_2 = fieldName.Length > 5 ? fieldName.Substring(0, 5) : fieldName;
                
                pFieldEdit.Type_2 = filedType;
                
                pFieldEdit.Editable_2 = true;
                
                pFieldEdit.AliasName_2 = fieldName;
                
                pFieldEdit.Length_2 = fieldLength;
                
                ITable pTable = (ITable)_layer;

                if (pTable != null) pTable.AddField(pFieldEdit);

                ShowFeatureLayerAttrib(_layer);
                
                return true;
             
            }
 
 
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
                return false;
            }
            
           
        }
        
       
        // 删除字段
        
        private void 删除字段ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string fieldName = dataGridView1.Columns[_index].Name;


            if (_layer != null)
            {

                var featureClass = _layer.FeatureClass;

                int n = featureClass.FindField(fieldName);

                if (n != -1)
                {
                    ISchemaLock schemaLock = featureClass as ISchemaLock;

                    if (schemaLock != null)
                    {
                        //schemaLock.ChangeSchemaLock(esriSchemaLock.esriExclusiveSchemaLock);

                        IField field = featureClass.Fields.Field[n];

                        featureClass.DeleteField(field);

                        //schemaLock.ChangeSchemaLock(esriSchemaLock.esriSharedSchemaLock);
                    }
                }
                
                
            }

            //ShowFeatureLayerAttrib(_layer);
            
            ShowFeatures(_layer, null, false);

        }
        
        
        // 获取列索引
        
        private int _index;
        
        private void dataGridView1_CellMouseUp(object sender, DataGridViewCellMouseEventArgs e)
        {
            _index = e.ColumnIndex;
        }
        

        
    }
}
