using ESRI.ArcGIS.Geodatabase;
using ESRI.ArcGIS.Geometry;

namespace ArcEngine
{
    public class EraseClass
    {
        ///<summary>

        ///裁切框

        ///</summary>

        private IEnvelope _Envelope;

 

        public IEnvelope pEnvelope

        {

            get { return _Envelope; }

            set { _Envelope = value; }

        }

        ///<summary>

        ///被裁切图层

        ///</summary>

        private IFeatureClass _FeatureClass;

 

        public IFeatureClass pFeatureClass

        {

            get { return _FeatureClass; }

            set { _FeatureClass = value; }

        }

        public EraseClass()

        { }

        public EraseClass(IEnvelope pEnvelope, IFeatureClass pFeatureClass)

        {

            _FeatureClass = pFeatureClass;

            _Envelope = pEnvelope;

        }

        public void EraseOper()

        {

            ISpatialFilter tSF = new SpatialFilterClass();

            tSF.Geometry = _Envelope;

           

            tSF.SpatialRel = esriSpatialRelEnum.esriSpatialRelIntersects;

            //求出与裁切框相交要素

            IFeatureCursor tFeatureCursor = _FeatureClass.Search(tSF, false);

            IFeature tFeature = tFeatureCursor.NextFeature();

            while (tFeature != null)

            {

                IGeometry tGeo2 = tFeature.ShapeCopy;

                ITopologicalOperator tTope2 = tGeo2 as ITopologicalOperator;

                tTope2.Simplify();

                IGeometry tGeo = tFeature.ShapeCopy;

                ITopologicalOperator tTope = tGeo as ITopologicalOperator;

                tTope.Simplify();

                //用Envelope对要素进行裁切

                tTope.Clip(this._Envelope);

                IGeometry tGeoClip = (IGeometry)tTope;

                //用裁切出来的要素,再与其源要素进行求差处理,即得到外围要素

                IGeometry tGeoDe = tTope2.Difference(tGeoClip);

                //把外围要素赋予源要素

                tFeature.Shape = tGeoDe;

                tFeature.Store();

                tFeature = tFeatureCursor.NextFeature();

            }

            ReleaseCom(tFeatureCursor);

        }

        private void ReleaseCom(object o)

        {

            if (o != null)

            {

                while (System.Runtime.InteropServices.Marshal.ReleaseComObject(o) > 0)

                { }

            }

        }
    }
}