using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.Geodatabase;
using ESRI.ArcGIS.DataSourcesGDB;


namespace ArcMapAddin1
{
    public class ArcGISAddinRender : ESRI.ArcGIS.Desktop.AddIns.Button
    {
        public ArcGISAddinRender()
        {
        }
        private static void DirectRender(ILayer pLayer)
        {
                        
            IGeoFeatureLayer MyGeoFeatureLayer = pLayer as IGeoFeatureLayer;
            IFeatureCursor MyFeatureCursor = MyGeoFeatureLayer.Search(null, false);
            IFeature MyFeature = MyFeatureCursor.NextFeature();
            

            Render renderer = new Render();

            IUniqueValueRenderer pUniqueValueR = new UniqueValueRendererClass();
            pUniqueValueR.FieldCount = 1;//单值渲染
            string FieldName = MyFeature.Fields.get_Field(0).Name;
            pUniqueValueR.set_Field(0, FieldName);//渲染字段

            while (null != MyFeature)
            {
                //MyFeature.Fields.FindField("OBJECTID");
                string value = Convert.ToString((int)MyFeature.get_Value(0));
                pUniqueValueR.AddValue(value, FieldName, renderer.GetSymbol(MyFeature));
                MyFeature = MyFeatureCursor.NextFeature();
            }


            //pUniqueValueR.AddValue((Convert.ToString(tem)), "FID", pSymbol);


            MyGeoFeatureLayer.Renderer = pUniqueValueR as IFeatureRenderer;
        }
        protected override void OnClick()
        {
            //ISelection sel = ArcMap.Document.FocusMap.FeatureSelection;
            //sel.Clear();

            IEnumLayer EnumLayer = ArcMap.Document.FocusMap.get_Layers(null, false);
            ILayer pLayer = EnumLayer.Next();
            
            while (null != pLayer)
            {
                if ("Point" == pLayer.Name.Substring(pLayer.Name.LastIndexOf('_') + 1) || "Line" == pLayer.Name.Substring(pLayer.Name.LastIndexOf('_') + 1) )
                {
                    DirectRender(pLayer);
                    //pLayer = EnumLayer.Next();
                    //continue;
                }
                
                pLayer = EnumLayer.Next();
            }
            IActiveView pActiveView = ArcMap.Document.FocusMap as IActiveView;
            pActiveView.Refresh();
        }

        protected override void OnUpdate()
        {
        }
    }
}
