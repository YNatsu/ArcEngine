using System.Windows.Forms;
using ESRI.ArcGIS.DataSourcesFile;
using ESRI.ArcGIS.Geodatabase;
using ESRI.ArcGIS.DataSourcesGDB;
using ESRI.ArcGIS.NetworkAnalyst;

namespace ArcEngine
{
    public class NetWorkAnalysClass
    {
        // 有很大问题 是个大坑
        
        
        public static string getPath(string path) 
        {
            int t;                                   
            for (t = 0; t < path.Length; t++)
            {
                
                if (path.Substring(t, "ArcEngine".Length) == "ArcEngine")
                {
                    break;
                }
            }
            string name = path.Substring(0, t - 1) + @"\ArcEngine";
            return name;         
        }
        
        //打开工作区间
        public static IWorkspace OpenWorkspace(string strGDBName)
        {
            IWorkspaceFactory workspaceFactory;
            workspaceFactory = new FileGDBWorkspaceFactoryClass();
            return workspaceFactory.OpenFromFile(strGDBName, 0);
        }
        
        //加载参与分析的点要素
        public static void LoadNANetworkLocations(string strNAClassName, IFeatureClass inputFC, INAContext m_NAContext, double snapTolerance)
        {
            ITable b1 = inputFC as ITable;
            int i1 = b1.RowCount(null);
            INAClass naClass;
            INamedSet classes;
            classes = m_NAContext.NAClasses;
            naClass = classes.get_ItemByName(strNAClassName) as INAClass;
            ITable b2 = naClass as ITable;
            int i2 = b2.RowCount(null);
            naClass.DeleteAllRows();
            ITable b3 = naClass as ITable;
            int i3 = b2.RowCount(null);
            INAClassLoader classLoader = new NAClassLoader();
            classLoader.Locator = m_NAContext.Locator;
            if (snapTolerance > 0) classLoader.Locator.SnapTolerance = snapTolerance;//设置容差
            classLoader.NAClass = naClass;           
            //设置字段映射
            INAClassFieldMap fieldMap = null;
            fieldMap = new NAClassFieldMap();
            fieldMap.set_MappedField("FID", "FID");
            classLoader.FieldMap = fieldMap;
            int rowsIn = 0;
            int rowLocated = 0;
            IFeatureCursor featureCursor = inputFC.Search(null, true);
            classLoader.Load((ICursor)featureCursor, null, ref rowsIn, ref rowLocated);
            INAClass na=classLoader.NAClass;
            ITable b5 = na as ITable;
            int i5 = b2.RowCount(null);
            ITable b4 = inputFC as ITable;
            int i4 = b1.RowCount(null);
            ((INAContextEdit)m_NAContext).ContextChanged();
        }
        //创建网络分析上下文
        public static INAContext CreatePathSolverContext(INetworkDataset networkDataset)
        {
            IDENetworkDataset deNDS = GetPathDENetworkDataset(networkDataset);
            INASolver naSolver;
            naSolver = new NARouteSolver();
            INAContextEdit contextEdit = naSolver.CreateContext(deNDS, naSolver.Name) as INAContextEdit;
            contextEdit.Bind(networkDataset, new GPMessagesClass());
            return contextEdit as INAContext;
        }
        public static IDENetworkDataset GetPathDENetworkDataset(INetworkDataset networkDataset)
        {
            IDatasetComponent dsComponent;
            dsComponent = networkDataset as IDatasetComponent;
            return dsComponent.DataElement as IDENetworkDataset;
        }
        //打开网络数据集
        public static INetworkDataset OpenPathNetworkDataset(IWorkspace networkDatasetWorkspace, string networkDatasetName, string featureDatasetName)
        {
            if (networkDatasetWorkspace == null || networkDatasetName == "" || featureDatasetName == null)
            {
                return null;
            }
            IDatasetContainer3 datasetContainer3 = null;
            IFeatureWorkspace featureWorkspace = networkDatasetWorkspace as IFeatureWorkspace;
            IFeatureDataset featureDataset;
            featureDataset = featureWorkspace.OpenFeatureDataset(featureDatasetName);
            IFeatureDatasetExtensionContainer featureDatasetExtensionContainer = featureDataset as IFeatureDatasetExtensionContainer;
            IFeatureDatasetExtension featureDatasetExtension = featureDatasetExtensionContainer.FindExtension(esriDatasetType.esriDTNetworkDataset);
            datasetContainer3 = featureDatasetExtension as IDatasetContainer3;
            if (datasetContainer3 == null)
                return null;
            IDataset dataset = datasetContainer3.get_DatasetByName(esriDatasetType.esriDTNetworkDataset, networkDatasetName);
            return dataset as INetworkDataset;
        }       
    }
}