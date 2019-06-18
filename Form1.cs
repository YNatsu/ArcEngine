using System;
using System.Windows.Forms;
using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.Controls;
using ESRI.ArcGIS.DataSourcesFile;
using ESRI.ArcGIS.Display;
using ESRI.ArcGIS.esriSystem;
using ESRI.ArcGIS.Geodatabase;
using ESRI.ArcGIS.AnalysisTools;
using ESRI.ArcGIS.Geometry;
using ESRI.ArcGIS.Geoprocessor;
using ESRI.ArcGIS.GeoprocessingUI;
using stdole;
using Object = System.Object;
using Path = System.IO.Path;


namespace ArcEngine
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();

//            窗口最大化显示
            WindowState = FormWindowState.Maximized;
        }

//        axMapControl1 选中 Map 对象

        private IMap _hitMap;

//        axMapControl1 选中 Layer 对象

        private ILayer _hitLayer;

        private void Form1_Load(object sender, EventArgs e)
        {
//            设置 axMapControl1 地图显示名称

            axMapControl1.Map.Name = "图层";

//            treeView1 展开

            treeView1.ExpandAll();

//            初始化

            _hitMap = null;

            _hitLayer = null;
            

            
        }

        private void axMapControl1_OnMouseDown(object sender,
            IMapControlEvents2_OnMouseDownEvent e)
        {
//            鼠标中键平移图层
            if (e.button == 4)
            {
                axMapControl1.Pan();
            }
        }


//        axTOCControl1 中判断鼠标点中的项目并弹出右键菜单

        private void axTOCControl1_OnMouseDown(object sender, ITOCControlEvents_OnMouseDownEvent e)
        {
            if (e.button == 2)
            {
                esriTOCControlItem item = esriTOCControlItem.esriTOCControlItemNone;

                IBasicMap map = null;

                ILayer layer = null;

                Object other = null;

                Object index = null;

                axTOCControl1.HitTest(
                    e.x, e.y, ref item, ref map, ref layer, ref other, ref index
                );

//                如果点中了一个 Map 项目

                if (item == esriTOCControlItem.esriTOCControlItemMap)
                {
                    mapTOCMenuStrip.Show(axTOCControl1, e.x, e.y);

                    _hitMap = map as IMap;
                }

                else if (item == esriTOCControlItem.esriTOCControlItemLayer)
                {
                    layerTOCMenuStrip.Show(axTOCControl1, e.x, e.y);

                    _hitLayer = layer;
                }
            }
        }
        
        
        private void 添加数据ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();

            openFileDialog.Filter = @"shp files (*.shp)|*.shp";

            openFileDialog.FilterIndex = 1;

            openFileDialog.RestoreDirectory = true;

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                addLayer(openFileDialog.FileName);
            }
        }

        private void 移除ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            axMapControl1.Map.DeleteLayer(_hitLayer);
        }

//        同步 数据视图、布局视图

        private void axMapControl1_OnExtentUpdated(Object sender, IMapControlEvents2_OnExtentUpdatedEvent e)
        {
            IObjectCopy pobjectcopy = new ObjectCopyClass();
            object from = pobjectcopy.Copy(axMapControl1.Map);
            object to = axPageLayoutControl1.ActiveView.FocusMap;
            pobjectcopy.Overwrite(from, ref to);

            axPageLayoutControl1.ActiveView.Refresh();
        }

//        双击 axTOCControl1 显示符号系统

        private void axTOCControl1_OnDoubleClick(Object sender, ITOCControlEvents_OnDoubleClickEvent e)
        {
            esriTOCControlItem toccItem = esriTOCControlItem.esriTOCControlItemNone;

            ILayer iLayer = null;

            IBasicMap iBasicMap = null;

            object unk = null;

            object data = null;

            if (e.button == 1)
            {
                axTOCControl1.HitTest(e.x, e.y, ref toccItem, ref iBasicMap, ref iLayer, ref unk,
                    ref data);

                new System.Drawing.Point(e.x, e.y);

                if (toccItem == esriTOCControlItem.esriTOCControlItemLegendClass)
                {
                    ILegendClass pLC = new LegendClassClass();

                    ILegendGroup pLG = new LegendGroupClass();

                    if (unk is ILegendGroup)
                    {
                        pLG = (ILegendGroup) unk;
                    }

                    pLC = pLG.get_Class((int) data);

                    ISymbol pSym;

                    pSym = pLC.Symbol;

                    ESRI.ArcGIS.DisplayUI.ISymbolSelector pSS = new
                        ESRI.ArcGIS.DisplayUI.SymbolSelectorClass();


                    bool bOK;

                    pSS.AddSymbol(pSym);

                    bOK = pSS.SelectSymbol(0);

                    if (bOK)
                    {
                        pLC.Symbol = pSS.GetSymbolAt(0);
                    }

                    axMapControl1.ActiveView.Refresh();

                    axTOCControl1.Refresh();
                }
            }
        }



//        工具箱
        
        private void treeView1_MouseDown(Object sender, MouseEventArgs e)
        {
            if ((sender as TreeView) != null)
            {
                treeView1.SelectedNode = treeView1.GetNodeAt(e.X, e.Y);


                string name = treeView1.SelectedNode.Name;

                if (name == "缓冲区")
                {
                    IHookHelper hookHelper = new HookHelperClass();
                    
                    hookHelper.Hook = axMapControl1.Object;
                    
                    BufferAnalysisForm bufferAnalysisForm = new BufferAnalysisForm(hookHelper, addLayer);
                    
                    bufferAnalysisForm.ShowDialog();
                    
                    // http://www.itboth.com/d/MFRRFf/textbox-buffer-arcengine-string-layer
                }
            }
        }
        
//        利用 文件路径 为 axMapControl1 添加图层
        
        private void addLayer(string path)
        {
            // GetDirectoryName 方法可以解析出目录名

            string directoryName = Path.GetDirectoryName(path);

            // GetFileNameWithoutExtension 方法可以解析出

            // 不包含扩展名的文件名称（不包含路径名）
                
            string fileNameWithoutExtension = Path.GetFileNameWithoutExtension(path);
            
                
            IWorkspaceFactory shapefileWorkspaceFactory = new ShapefileWorkspaceFactory();

            // 调用 OpenFromFile 通过目录名方法打开 Shapefile 数据库

            IWorkspace workspace = shapefileWorkspaceFactory.OpenFromFile(directoryName, 0);

            if (workspace != null)
            {
                // ESRI.ArcGIS.Geodatabase.IEnumDataset

                IEnumDataset enumDataset =
                    workspace.get_Datasets(esriDatasetType.esriDTFeatureClass);

                enumDataset.Reset();

                IDataset shpDataset = enumDataset.Next();

                while (shpDataset != null)
                {
                    if (shpDataset.Name == fileNameWithoutExtension)
                    {
                        // ESRI.ArcGIS.Carto.IFeatureLayer

                        IFeatureLayer newLayer = new FeatureLayerClass();

                        newLayer.FeatureClass = shpDataset as IFeatureClass;

                        newLayer.Name = fileNameWithoutExtension;

                        axMapControl1.Map.AddLayer(newLayer);

                        break;
                    }

                    shpDataset = enumDataset.Next();
                }
            }
        }

        private void Show(string s)
        {
            MessageBox.Show(s);
        }

        private void 图例ToolStripMenuItem_Click(Object sender, EventArgs e)
        {
            AddLegend(axPageLayoutControl1);
        }
        
//        图例
        
        private void AddLegend(AxPageLayoutControl axPageLayout)  
        {  
            //删除已经存在的图例
            
            IElement pElement = axPageLayout.FindElementByName("Legends");
            
            if (pElement!=null)
            {
                axPageLayout.ActiveView.GraphicsContainer.DeleteElement(pElement);  
                
            }
            
            IActiveView pActiveView = axPageLayout.PageLayout as IActiveView;  
            IGraphicsContainer container = axPageLayout.PageLayout as IGraphicsContainer;  
            // 获得MapFrame  
            IMapFrame mapFrame = container.FindFrame(pActiveView.FocusMap) as IMapFrame;  
            //根据MapSurround的uid，创建相应的MapSurroundFrame和MapSurround  
            UID uid = new UIDClass();  
            uid.Value = "esriCarto.Legend";  
            IMapSurroundFrame mapSurroundFrame = mapFrame.CreateSurroundFrame(uid, null);  
            //设置图例的Title  
            ILegend2 legend = mapSurroundFrame.MapSurround as ILegend2;  
            legend.Title = "地图图例";  
            ILegendFormat format = new LegendFormatClass();  
            ITextSymbol symbol = new TextSymbolClass();  
            symbol.Size = 4;  
            format.TitleSymbol = symbol;  
            legend.Format = format;  
            //QI，确定mapSurroundFrame的位置  
            IElement element = mapSurroundFrame as IElement;  
            IEnvelope envelope = new EnvelopeClass(); 
            
            double x = 3;
            double y = 5;
            
            envelope.PutCoords(x, y, x+5, y+5); 
            element.Geometry = envelope;  
            //使用IGraphicsContainer接口添加显示  
            container.AddElement(element, 0);  
            pActiveView.Refresh();  
            
            
        }  
        
        

        private void 指北针ToolStripMenuItem_Click(Object sender, EventArgs e)
        {

            AddNorthArrow(axPageLayoutControl1);
        }
        
//        指北针

        public void AddNorthArrow(AxPageLayoutControl axPageLayout)
        {
            
            //删除已经存在的指北针
            
            IElement pElement = axPageLayout.FindElementByName("NorthArrows");
            if (pElement!=null)
            {
                axPageLayout.ActiveView.GraphicsContainer.DeleteElement(pElement);  
            }

            
            IGraphicsContainer container = axPageLayout.PageLayout as IGraphicsContainer;   
            IActiveView activeView = axPageLayout.PageLayout as IActiveView;   
            // 获得MapFrame  
            IFrameElement frameElement = container.FindFrame(activeView.FocusMap);  
            IMapFrame mapFrame = frameElement as IMapFrame;  
            //根据MapSurround的uid，创建相应的MapSurroundFrame和MapSurround  
            UID uid = new UIDClass();  
            uid.Value = "esriCarto.MarkerNorthArrow";  
            IMapSurroundFrame mapSurroundFrame = mapFrame.CreateSurroundFrame(uid, null);  
            //设置MapSurroundFrame中指北针的点符号  
            IMapSurround mapSurround = mapSurroundFrame.MapSurround;  
            IMarkerNorthArrow markerNorthArrow = mapSurround as IMarkerNorthArrow;   
            IMarkerSymbol markerSymbol = markerNorthArrow.MarkerSymbol;  
            
            markerSymbol.Size = 48;  
            markerNorthArrow.MarkerSymbol = markerSymbol;  
            
            //QI，确定mapSurroundFrame的位置  
            IElement element = mapSurroundFrame as IElement;  
            IEnvelope envelope = new EnvelopeClass();  
            
            double x = 15;
            double y = 22;
            
            envelope.PutCoords(x, y, x+5, y+5);  
            element.Geometry = envelope;  
            //使用IGraphicsContainer接口添加显示  
            container.AddElement(element, 0);  
            activeView.Refresh();  
            
        }


        private void 比例尺ToolStripMenuItem_Click(Object sender, EventArgs e)
        {
            AddScalebar(axPageLayoutControl1);
        }
        
//        比例尺
        
        public void AddScalebar(AxPageLayoutControl axPageLayout)  
        {  
            //删除已经存在的比例尺
            
            IElement pelement = axPageLayout.FindElementByName("AlternatingScaleBar");
            
           
            
            if (pelement != null)
            {
                axPageLayout.ActiveView.GraphicsContainer.DeleteElement(pelement);  
            }
            
            IGraphicsContainer container = axPageLayout.PageLayout as IGraphicsContainer;   
            IActiveView activeView = axPageLayout.PageLayout as IActiveView;  
            // 获得MapFrame  
            IFrameElement frameElement = container.FindFrame(activeView.FocusMap);  
            IMapFrame mapFrame = frameElement as IMapFrame;  
            //根据MapSurround的uid，创建相应的MapSurroundFrame和MapSurround  
            UID uid = new UIDClass();  
            uid.Value = "esriCarto.AlternatingScaleBar";  
            IMapSurroundFrame mapSurroundFrame = mapFrame.CreateSurroundFrame(uid, null);  
            //设置MapSurroundFrame中比例尺的样式  
            IMapSurround mapSurround = mapSurroundFrame.MapSurround;  
            IScaleBar markerScaleBar = ((IScaleBar)mapSurround);  
            markerScaleBar.LabelPosition = esriVertPosEnum.esriBelow;
            markerScaleBar.UseMapSettings();  
            //QI，确定mapSurroundFrame的位置  
            IElement element = mapSurroundFrame as IElement;  
            IEnvelope envelope = new EnvelopeClass();  
            double x = 12;
            double y = 5;
            envelope.PutCoords(x, y, x+1, y+1);  
            element.Geometry = envelope;  
            //使用IGraphicsContainer接口添加显示  
            container.AddElement(element, 0);  
            activeView.Refresh();  
        }  

        private void 标题ToolStripMenuItem_Click(Object sender, EventArgs e)
        {
            TitleForm titleForm = new TitleForm(AddTitle);

            titleForm.ShowDialog();
        }
        
//        标题
        
        private void AddTitle(String title)
        {
            IGraphicsContainer graphicsContainer = axPageLayoutControl1.PageLayout as IGraphicsContainer;
            IEnvelope envelope = new EnvelopeClass();
            double x = 8;
            double y = 22;
            envelope.PutCoords(x, y, x+5, y+5);
            IRgbColor pColor = new RgbColorClass()
            {
                Red = 0,
                Blue = 0,
                Green = 0
            };
            
            IFontDisp pFont = new StdFont()
            {
                Name = "宋体",
                Bold = true
            } as IFontDisp;
            
            ITextSymbol pTextSymbol = new TextSymbolClass()
            {
                Color = pColor,
                Font = pFont,
                Size = 25
            };
            
            ITextElement pTextElement = new TextElementClass()
            {
                Symbol = pTextSymbol,
                ScaleText = true,
                Text = title
            };
            
            IElement element = pTextElement as IElement;
            element.Geometry = envelope;
            graphicsContainer.AddElement(element, 0);
            axPageLayoutControl1.Refresh();


        }


        private void axPageLayoutControl1_OnMouseDown(Object sender, IPageLayoutControlEvents_OnMouseDownEvent e)
        {
//           中键平移布局视图
            if (e.button == 4)
            {
                axPageLayoutControl1.Pan();
            }
        }
        
//        打开属性表
        
        private void 属性ToolStripMenuItem_Click(Object sender, EventArgs e)
        {
            var command=  new LayerPropertiesCmd();
            command.OnCreate(_hitLayer);
            command.OnClick();
        }

//        打开符号系统
        
        private void 符号系统ToolStripMenuItem_Click(Object sender, EventArgs e)
        {

        }
        
        

    }
}