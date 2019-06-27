using System;
using System.Runtime.InteropServices;
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
using ESRI.ArcGIS.Output;
using ESRI.ArcGIS.SystemUI;
using stdole;
using Object = System.Object;
using Path = System.IO.Path;


namespace ArcEngine
{
    public partial class MainForm : Form
    {
        public MainForm()
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

                else if (name == "擦除")
                {
                    IHookHelper hookHelper = new HookHelperClass();

                    hookHelper.Hook = axMapControl1.Object;

                    EraseForm eraseForm = new EraseForm(hookHelper, addLayer);

                    eraseForm.ShowDialog();
                }

                else if (name == "加载站点")
                {

                    ICommand pCommand;
                    pCommand = new AddNetStopsTool();
                    pCommand.OnCreate(axMapControl1.Object);
                    axMapControl1.CurrentTool = pCommand as ITool;
                    
                }
                else if (name == "加载障碍点")
                {
                    ICommand pCommand;
                    pCommand = new AddNetBarriesTool();
                    pCommand.OnCreate(axMapControl1.Object);
                    axMapControl1.CurrentTool = pCommand as ITool;
                }
                else if (name == "最短路径分析")
                {
                    ICommand pCommand;
                    pCommand = new ShortPathSolveCommand();
                    pCommand.OnCreate(axMapControl1.Object);
                    pCommand.OnClick();
                }
                else if (name == "清除分析")
                {
                    axMapControl1.CurrentTool = null;
                    try
                    {
                        IFeatureWorkspace pFWorkspace;
                        string path = System.AppDomain.CurrentDomain.SetupInformation.ApplicationBase;
                        string nameN = NetWorkAnalysClass.getPath(path) + "\\data\\HuanbaoGeodatabase.gdb";
                        //打开工作空间
                        pFWorkspace = NetWorkAnalysClass.OpenWorkspace(nameN) as IFeatureWorkspace;
                        IGraphicsContainer pGrap = axMapControl1.ActiveView as IGraphicsContainer;
                        pGrap.DeleteAllElements();//删除所添加的图片要素
                        IFeatureClass inputFClass = pFWorkspace.OpenFeatureClass("Stops");
                        //删除站点要素
                        if (inputFClass.FeatureCount(null) > 0)
                        {
                            ITable pTable = inputFClass as ITable;
                            pTable.DeleteSearchedRows(null);                 
                        }
                        IFeatureClass barriesFClass = pFWorkspace.OpenFeatureClass("Barries");//删除障碍点要素
                        if (barriesFClass.FeatureCount(null) > 0)
                        {
                            ITable pTable = barriesFClass as ITable;
                            pTable.DeleteSearchedRows(null);
                        }
                        for (int i = 0; i < axMapControl1.LayerCount; i++)//删除分析结果
                        {
                            ILayer pLayer = axMapControl1.get_Layer(i);
                            if (pLayer.Name == ShortPathSolveCommand.m_NAContext.Solver.DisplayName)
                            {
                                axMapControl1.DeleteLayer(i);
                                break;
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                    axMapControl1.Refresh();
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

            if (pElement != null)
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

            envelope.PutCoords(x, y, x + 5, y + 5);
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
            if (pElement != null)
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

            envelope.PutCoords(x, y, x + 5, y + 5);
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
            IScaleBar markerScaleBar = ((IScaleBar) mapSurround);
            markerScaleBar.LabelPosition = esriVertPosEnum.esriBelow;
            markerScaleBar.UseMapSettings();
            //QI，确定mapSurroundFrame的位置  
            IElement element = mapSurroundFrame as IElement;
            IEnvelope envelope = new EnvelopeClass();
            double x = 12;
            double y = 5;
            envelope.PutCoords(x, y, x + 1, y + 1);
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
            envelope.PutCoords(x, y, x + 5, y + 5);
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
            // 传入点击的图层

            PropertySheet propertySheet = new PropertySheet(_hitLayer as IFeatureLayer);

            propertySheet.ShowDialog();
        }

//        打开符号系统

        private void 符号系统ToolStripMenuItem_Click(Object sender, EventArgs e)
        {
        }


        // 导出地图

        private void 导出ToolStripMenuItem_Click(Object sender, EventArgs e)
        {
            SaveFileDialog mapExportDialog = new SaveFileDialog();

            mapExportDialog.Filter =
                "JPEG格式(*.jpg)|*.jpg|EPS格式(*.eps)|*.eps|EMF格式(*.emf)|*.emf|BMP格式(*.bmp)|*.bmp|PDF格式(*.pdf)|*.pdf|TIFF格式(*.tif)|*.tif|PNG格式(*.png)|*.png|SVG格式(*.svg)|*.svg|AI格式(*.ai)|*.ai|所有格式(*.*)|*.*";
            mapExportDialog.RestoreDirectory = true;

            if (mapExportDialog.ShowDialog() == DialogResult.OK)
            {
                string strLocalFileName = mapExportDialog.FileName;
                //获取文件路径，不带文件名
                string strFilePath = strLocalFileName.Substring(0, strLocalFileName.LastIndexOf("\\") + 2);
                //string strFileName = strLocalFileName.Substring(strLocalFileName.LastIndexOf("\\") + 1, strLocalFileName.LastIndexOf(".")-3);
                String strFileName = Path.GetFileNameWithoutExtension(strLocalFileName);
                string picType;
                switch (mapExportDialog.FilterIndex)
                {
                    case 1:
                        picType = "JPEG";
                        break;
                    case 2:
                        picType = "EPS";
                        break;
                    case 3:
                        picType = "EMF";
                        break;
                    case 4:
                        picType = "BMP";
                        break;
                    case 5:
                        picType = "PDF";
                        break;
                    case 6:
                        picType = "TIFF";
                        break;
                    case 7:
                        picType = "PNG";
                        break;
                    case 8:
                        picType = "SVG";
                        break;
                    case 9:
                        picType = "AI";
                        break;
                    default:
                        picType = "EMF";
                        break;
                }

                // MessageBox.Show(picType);
                ExportActiveViewParameterized(96, 1, picType, strFilePath, strFileName, false, axPageLayoutControl1);
            }
        }

        [DllImport("GDI32.dll")]
        public static extern int GetDeviceCaps(int hdc, int nIndex);

        [DllImport("User32.dll")]
        public static extern int GetDC(int hWnd);

        [DllImport("User32.dll")]
        public static extern int ReleaseDC(int hWnd, int hDC);

        //[DllImport("user32.dll", SetLastError = true)]
        //static extern bool SystemParametersInfo(uint uiAction, uint uiParam, IntPtr pvParam, uint fWinIni);
        [DllImport("user32.dll", SetLastError = true)]
        static extern bool SystemParametersInfo(uint uiAction, uint uiParam, ref int pvParam, uint fWinIni);

        const uint SpiGetfontsmoothing = 74;
        const uint SpiSetfontsmoothing = 75;
        const uint SpifUpdateinifile = 0x1;

        private void ExportActiveViewParameterized(long iOutputResolution, long lResampleRatio, string ExportType,
            string sOutputDir, string sNameRoot, Boolean bClipToGraphicsExtent, AxPageLayoutControl pageLayoutControl)
        {
            //解决文件名错误
            sNameRoot = sNameRoot.Substring(1, sNameRoot.Length - 1);
            IActiveView docActiveView = pageLayoutControl.ActiveView;
            IExport docExport;
            long iPrevOutputImageQuality;
            IOutputRasterSettings docOutputRasterSettings;
            IEnvelope PixelBoundsEnv;
            tagRECT exportRECT;
            tagRECT DisplayBounds;
            IDisplayTransformation docDisplayTransformation;
            IPageLayout docPageLayout;
            IEnvelope docMapExtEnv;
            long hdc;
            long tmpDC;
            //string sNameRoot;
            long iScreenResolution;
            bool bReenable = false;

            IEnvelope docGraphicsExtentEnv;
            IUnitConverter pUnitConvertor;
            if (GetFontSmoothing())
            {
                bReenable = true;
                DisableFontSmoothing();
                if (GetFontSmoothing())
                {
                    //font smoothing is NOT successfully disabled, error out.
                    return;
                }

                //else font smoothing was successfully disabled.
            }

            // The Export*Class() type initializes a new export class of the desired type.
            if (ExportType == "PDF")
            {
                docExport = new ExportPDFClass();
            }
            else if (ExportType == "EPS")
            {
                docExport = new ExportPSClass();
            }
            else if (ExportType == "AI")
            {
                docExport = new ExportAIClass();
            }
            else if (ExportType == "BMP")
            {
                docExport = new ExportBMPClass();
            }
            else if (ExportType == "TIFF")
            {
                docExport = new ExportTIFFClass();
            }
            else if (ExportType == "SVG")
            {
                docExport = new ExportSVGClass();
            }
            else if (ExportType == "PNG")
            {
                docExport = new ExportPNGClass();
            }
            else if (ExportType == "GIF")
            {
                docExport = new ExportGIFClass();
            }
            else if (ExportType == "EMF")
            {
                docExport = new ExportEMFClass();
            }
            else if (ExportType == "JPEG")
            {
                docExport = new ExportJPEGClass();
            }
            else
            {
                MessageBox.Show("！！不支持的格式 " + ExportType + ", 默认导出 EMF.");
                ExportType = "EMF";
                docExport = new ExportEMFClass();
            }

            //  save the previous output image quality, so that when the export is complete it will be set back.
            docOutputRasterSettings = docActiveView.ScreenDisplay.DisplayTransformation as IOutputRasterSettings;
            iPrevOutputImageQuality = docOutputRasterSettings.ResampleRatio;

            if (docExport is IExportImage)
            {
                // always set the output quality of the DISPLAY to 1 for image export formats
                SetOutputQuality(docActiveView, 1);
            }
            else
            {
                // for vector formats, assign the desired ResampleRatio to control drawing of raster layers at export time  
                SetOutputQuality(docActiveView, lResampleRatio);
            }

            //set the name root for the export
            // sNameRoot = "ExportActiveViewSampleOutput";
            //set the export filename (which is the nameroot + the appropriate file extension)
            docExport.ExportFileName =
                sOutputDir + sNameRoot + "." + docExport.Filter.Split('.')[1].Split('|')[0].Split(')')[0];


            tmpDC = GetDC(0);

            iScreenResolution = GetDeviceCaps((int) tmpDC, 88); //88 is the win32 const for Logical pixels/inch in X)

            ReleaseDC(0, (int) tmpDC);
            docExport.Resolution = iOutputResolution;

            if (docActiveView is IPageLayout)
            {
                //get the bounds of the "exportframe" of the active view.
                DisplayBounds = docActiveView.ExportFrame;
                //set up pGraphicsExtent, used if clipping to graphics extent.
                docGraphicsExtentEnv = GetGraphicsExtent(docActiveView);
            }
            else
            {
                //Get the bounds of the deviceframe for the screen.
                docDisplayTransformation = docActiveView.ScreenDisplay.DisplayTransformation;
                DisplayBounds = docDisplayTransformation.get_DeviceFrame();
            }

            PixelBoundsEnv = new Envelope() as IEnvelope;
            if (bClipToGraphicsExtent && (docActiveView is IPageLayout))
            {
                docGraphicsExtentEnv = GetGraphicsExtent(docActiveView);
                docPageLayout = docActiveView as PageLayout;
                pUnitConvertor = new UnitConverter();
                //assign the x and y values representing the clipped area to the PixelBounds envelope
                PixelBoundsEnv.XMin = 0;
                PixelBoundsEnv.YMin = 0;
                PixelBoundsEnv.XMax =
                    pUnitConvertor.ConvertUnits(docGraphicsExtentEnv.XMax, docPageLayout.Page.Units,
                        esriUnits.esriInches) * docExport.Resolution -
                    pUnitConvertor.ConvertUnits(docGraphicsExtentEnv.XMin, docPageLayout.Page.Units,
                        esriUnits.esriInches) * docExport.Resolution;
                PixelBoundsEnv.YMax =
                    pUnitConvertor.ConvertUnits(docGraphicsExtentEnv.YMax, docPageLayout.Page.Units,
                        esriUnits.esriInches) * docExport.Resolution -
                    pUnitConvertor.ConvertUnits(docGraphicsExtentEnv.YMin, docPageLayout.Page.Units,
                        esriUnits.esriInches) * docExport.Resolution;
                //'assign the x and y values representing the clipped export extent to the exportRECT
                exportRECT.bottom = (int) (PixelBoundsEnv.YMax) + 1;
                exportRECT.left = (int) (PixelBoundsEnv.XMin);
                exportRECT.top = (int) (PixelBoundsEnv.YMin);
                exportRECT.right = (int) (PixelBoundsEnv.XMax) + 1;
                //since we're clipping to graphics extent, set the visible bounds.
                docMapExtEnv = docGraphicsExtentEnv;
            }
            else
            {
                double tempratio = iOutputResolution / iScreenResolution;
                double tempbottom = DisplayBounds.bottom * tempratio;
                double tempright = DisplayBounds.right * tempratio;
                //'The values in the exportRECT tagRECT correspond to the width
                //and height to export, measured in pixels with an origin in the top left corner.
                exportRECT.bottom = (int) Math.Truncate(tempbottom);
                exportRECT.left = 0;
                exportRECT.top = 0;
                exportRECT.right = (int) Math.Truncate(tempright);

                //populate the PixelBounds envelope with the values from exportRECT.
                // We need to do this because the exporter object requires an envelope object
                // instead of a tagRECT structure.
                PixelBoundsEnv.PutCoords(exportRECT.left, exportRECT.top, exportRECT.right, exportRECT.bottom);
                //since it's a page layout or an unclipped page layout we don't need docMapExtEnv.
                docMapExtEnv = null;
            }

            // Assign the envelope object to the exporter object's PixelBounds property.  The exporter object
            // will use these dimensions when allocating memory for the export file.
            docExport.PixelBounds = PixelBoundsEnv;
            // call the StartExporting method to tell docExport you're ready to start outputting.
            hdc = docExport.StartExporting();
            // Redraw the active view, rendering it to the exporter object device context instead of the app display.
            // We pass the following values:
            //  * hDC is the device context of the exporter object.
            //  * exportRECT is the tagRECT structure that describes the dimensions of the view that will be rendered.
            // The values in exportRECT should match those held in the exporter object's PixelBounds property.
            //  * docMapExtEnv is an envelope defining the section of the original image to draw into the export object.
            docActiveView.Output((int) hdc, (int) docExport.Resolution, ref exportRECT, docMapExtEnv, null);
            //finishexporting, then cleanup.
            docExport.FinishExporting();
            docExport.Cleanup();
            MessageBox.Show(
                "成功导出地图： " + sOutputDir + sNameRoot + "." + docExport.Filter.Split('.')[1].Split('|')[0].Split(')')[0] +
                ".", "提示");
            //set the output quality back to the previous value
            SetOutputQuality(docActiveView, iPrevOutputImageQuality);
            if (bReenable)
            {
                EnableFontSmoothing();
                bReenable = false;
                if (!GetFontSmoothing())
                {
                    //error: cannot reenable font smoothing.
                    MessageBox.Show("Unable to reenable Font Smoothing", "Font Smoothing error");
                }
            }

            docMapExtEnv = null;
            PixelBoundsEnv = null;
        }

        private void SetOutputQuality(IActiveView docActiveView, long iResampleRatio)
        {
            IGraphicsContainer oiqGraphicsContainer;
            IElement oiqElement;
            IOutputRasterSettings docOutputRasterSettings;
            IMapFrame docMapFrame;
            IActiveView TmpActiveView;
            if (docActiveView is IMap)
            {
                docOutputRasterSettings = docActiveView.ScreenDisplay.DisplayTransformation as IOutputRasterSettings;
                docOutputRasterSettings.ResampleRatio = (int) iResampleRatio;
            }
            else if (docActiveView is IPageLayout)
            {
                //assign ResampleRatio for PageLayout
                docOutputRasterSettings = docActiveView.ScreenDisplay.DisplayTransformation as IOutputRasterSettings;
                docOutputRasterSettings.ResampleRatio = (int) iResampleRatio;
                //and assign ResampleRatio to the Maps in the PageLayout
                oiqGraphicsContainer = docActiveView as IGraphicsContainer;
                oiqGraphicsContainer.Reset();
                oiqElement = oiqGraphicsContainer.Next();
                while (oiqElement != null)
                {
                    if (oiqElement is IMapFrame)
                    {
                        docMapFrame = oiqElement as IMapFrame;
                        TmpActiveView = docMapFrame.Map as IActiveView;
                        docOutputRasterSettings =
                            TmpActiveView.ScreenDisplay.DisplayTransformation as IOutputRasterSettings;
                        docOutputRasterSettings.ResampleRatio = (int) iResampleRatio;
                    }

                    oiqElement = oiqGraphicsContainer.Next();
                }

                docMapFrame = null;
                oiqGraphicsContainer = null;
                TmpActiveView = null;
            }

            docOutputRasterSettings = null;
        }

        private IEnvelope GetGraphicsExtent(IActiveView docActiveView)
        {
            IEnvelope GraphicsBounds;
            IEnvelope GraphicsEnvelope;
            IGraphicsContainer oiqGraphicsContainer;
            IPageLayout docPageLayout;
            IDisplay GraphicsDisplay;
            IElement oiqElement;
            GraphicsBounds = new EnvelopeClass();
            GraphicsEnvelope = new EnvelopeClass();
            docPageLayout = docActiveView as IPageLayout;
            GraphicsDisplay = docActiveView.ScreenDisplay;
            oiqGraphicsContainer = docActiveView as IGraphicsContainer;
            oiqGraphicsContainer.Reset();
            oiqElement = oiqGraphicsContainer.Next();
            while (oiqElement != null)
            {
                oiqElement.QueryBounds(GraphicsDisplay, GraphicsEnvelope);
                GraphicsBounds.Union(GraphicsEnvelope);
                oiqElement = oiqGraphicsContainer.Next();
            }

            return GraphicsBounds;
        }

        private void DisableFontSmoothing()
        {
            bool iResult;
            int pv = 0;

            SystemParametersInfo(SpiSetfontsmoothing, 0, ref pv, SpifUpdateinifile);
        }

        private void EnableFontSmoothing()
        {
            bool iResult;
            int pv = 0;

            SystemParametersInfo(SpiSetfontsmoothing, 1, ref pv, SpifUpdateinifile);
        }

        private Boolean GetFontSmoothing()
        {
            bool iResult;
            int pv = 0;

            SystemParametersInfo(SpiGetfontsmoothing, 0, ref pv, 0);
            if (pv > 0)
            {
                //pv > 0 means font smoothing is ON.
                return true;
            }
            else
            {
                //pv == 0 means font smoothing is OFF.
                return false;
            }
        }

        private void 按属性选择ToolStripMenuItem_Click(Object sender, EventArgs e)
        {
            // https://www.cnblogs.com/dongteng/p/5925120.html
        }

        private void 按位置选择ToolStripMenuItem_Click(Object sender, EventArgs e)
        {
            // https://blog.csdn.net/jhoneyan/article/details/52473470
        }

        private void Main_DragEnter(Object sender, DragEventArgs e)
        {
            string path = ((System.Array) e.Data.GetData(DataFormats.FileDrop)).GetValue(0).ToString();
            Show(path);
        }

        /// <summary>
        /// 打开 mxd 文件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void 打开ToolStripMenuItem_Click(Object sender, EventArgs e)
        {
            IMapDocument xjMxdMapDocument = new MapDocumentClass();
            OpenFileDialog xjMxdOpenFileDialog = new OpenFileDialog();
            xjMxdOpenFileDialog.Filter = "地图文档(*.mxd)|*.mxd";

            if (xjMxdOpenFileDialog.ShowDialog() == DialogResult.OK)
            {
                string xjmxdFilePath = xjMxdOpenFileDialog.FileName;

                if (axMapControl1.CheckMxFile(xjmxdFilePath))
                {
                    axMapControl1.Map.ClearLayers();
                    axMapControl1.LoadMxFile(xjmxdFilePath);
                }
            }

            axMapControl1.ActiveView.Refresh();
        }

        /// <summary>
        ///  保存 mxd 文件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void 保存ToolStripMenuItem_Click(Object sender, EventArgs e)
        {
            IMxdContents pMxdC;
            pMxdC = axMapControl1.Map as IMxdContents;
            IMapDocument pMapDocument = new MapDocumentClass();
            pMapDocument.Open(axMapControl1.DocumentFilename, "");

            pMapDocument.ReplaceContents(pMxdC);
            pMapDocument.Save(true, true);
        }

        /// <summary>
        /// 另存为 mdx
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void 另存为ToolStripMenuItem_Click(Object sender, EventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "地图文档(*.mxd)|*.mxd";

            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                IMxdContents pMxdC;
                pMxdC = axMapControl1.Map as IMxdContents;
                IMapDocument pMapDocument = new MapDocumentClass();
                pMapDocument.Open(axMapControl1.DocumentFilename, "");

                pMapDocument.ReplaceContents(pMxdC);
                pMapDocument.SaveAs(saveFileDialog.FileName);
            }
        }

        private void 新建ToolStripMenuItem_Click(Object sender, EventArgs e)
        {
            MainForm form = new MainForm();
            form.Show();
            //Close();
        }
    }
}