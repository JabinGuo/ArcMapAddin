using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using ESRI.ArcGIS.Geodatabase;
using EngineWindowsApplication1;
using ESRI.ArcGIS.DataSourcesGDB;
using ESRI.ArcGIS.esriSystem;
using ESRI.ArcGIS.Carto;
using System.Windows.Forms;
using ESRI.ArcGIS.DataSourcesRaster;

namespace ArcMapAddin1
{
    public class ArcGISAddin_Topology : ESRI.ArcGIS.Desktop.AddIns.Button
    {
        public ArcGISAddin_Topology()
        {
        }

        protected override void OnClick()
        {
            IEnumLayer EnumLayer = ArcMap.Document.FocusMap.get_Layers(null, false);
            ILayer pLayer = EnumLayer.Next();
            ProgressForm progressForm = new ProgressForm();
            progressForm.Show();
            

            while (null != pLayer)
            {
                //string b = pLayer.Name.Substring(pLayer.Name.LastIndexOf('_') + 1);
                if ("Point" == pLayer.Name.Substring(pLayer.Name.LastIndexOf('_') + 1)
                    || "Line" == pLayer.Name.Substring(pLayer.Name.LastIndexOf('_') + 1)
                    || "Polygon" == pLayer.Name.Substring(pLayer.Name.LastIndexOf('_') + 1)
                    )
                {
                    break;
                
                }
                pLayer = EnumLayer.Next();
            }//加入的图层中至少有点线面中的一个，点线面图层中有DWG字段，表明了shp的来源

            progressForm.AddProgress(10);
            IFeatureLayer pFeatureLayer = pLayer as IFeatureLayer;
            IFeature pfeature = pFeatureLayer.Search(null, false).NextFeature();

            IDataLayer DataLayer = pLayer as IDataLayer;
            IDatasetName tem_DatasetName = DataLayer.DataSourceName as IDatasetName;
            
            string PathName = tem_DatasetName.WorkspaceName.PathName;

            string DatasetName = pfeature.get_Value(pfeature.Fields.FindField("DWG")) as string;//获取生成shp的DWG文件名称
            string MDB_FullName = PathName;
            
            //*******************************************开 始 拓 扑 检 查*****************************************************
            IWorkspaceFactory iwsf = new AccessWorkspaceFactoryClass();
            IFeatureWorkspace ifws = (IFeatureWorkspace)iwsf.OpenFromFile(MDB_FullName, 0);
            IFeatureDataset topocheckDataset = ifws.OpenFeatureDataset(DatasetName);
            
            IFeatureClassContainer MyFeatureClassContainer = topocheckDataset as IFeatureClassContainer;
            
           
#if true
            TopologyChecker topocheck = new TopologyChecker(topocheckDataset);
            topocheck.PUB_TopoBuild(DatasetName + "_TopoCheck");//创建拓扑层
            progressForm.AddProgress(20);
            
            List<IFeatureClass> allFeatureClass_tem = topocheck.PUB_GetAllFeatureClass();
            //List<IFeatureClass> TopoClass;
            List<IFeatureClass> allFeatureClass = new List<IFeatureClass>();

            IFeatureClass topocheckFeatureClass_Point = null;
            IFeatureClass topocheckFeatureClass_Line = null;
            IFeatureClass topocheckFeatureClass_Polygon = null;
            
            foreach (IFeatureClass tem in allFeatureClass_tem)
            {
                switch (tem.AliasName.Substring(tem.AliasName.LastIndexOf('_') + 1))
                {
                    case "Point":
                        allFeatureClass.Add(tem);
                        topocheckFeatureClass_Point = tem;
                        break;
                    case "Line":
                        allFeatureClass.Add(tem);
                        topocheckFeatureClass_Line = tem;
                        break;
                    case "Polygon":
                        allFeatureClass.Add(tem);
                        topocheckFeatureClass_Polygon = tem;
                        break;
                    default:
                        
                        break;
                }
            }//只对点线面图层进行拓扑检查
            
            topocheck.PUB_AddFeatureClass(allFeatureClass);
            progressForm.AddProgress(10);
            
            List<string> ErrorInfo = null;
            topocheck.PUB_AddRuleToTopology(TopologyChecker.TopoErroType.线要素不能自相交, topocheckFeatureClass_Line,out ErrorInfo);
            progressForm.AddProgress(10);
            topocheck.PUB_AddRuleToTopology(TopologyChecker.TopoErroType.线要素之间不能相交, topocheckFeatureClass_Line,out ErrorInfo);
            progressForm.AddProgress(10);
            topocheck.PUB_AddRuleToTopology(TopologyChecker.TopoErroType.点要素之间不相交, topocheckFeatureClass_Point,out ErrorInfo);
            progressForm.AddProgress(10);
            topocheck.PUB_AddRuleToTopology(TopologyChecker.TopoErroType.面要素间无重叠, topocheckFeatureClass_Polygon,out ErrorInfo);
            progressForm.AddProgress(10);
            System.IO.File.WriteAllLines(MDB_FullName.Substring(0,MDB_FullName.LastIndexOf('\\')) + "\\拓扑错误报告.txt", ErrorInfo.ToArray());
            //生成检查报告
            progressForm.AddProgress(10);

            //----------------------------------------------打 开 错 误 报 告 器------------------------------------------------------
            EnumLayer = ArcMap.Document.FocusMap.get_Layers(null, false);

            ArcMap.Document.FocusMap.ClearLayers();
            ArcMap.Document.FocusMap.AddLayers(EnumLayer, true);
            pLayer = EnumLayer.Next();

            while (null != pLayer)
            {
                if ((DatasetName + "_Polygon") == pLayer.Name)
                {
                    pLayer = EnumLayer.Next();
                    continue;
                }
                DirectRender(pLayer);
                pLayer = EnumLayer.Next();
            }

            ESRI.ArcGIS.Framework.ICommandBars CommandBars = ArcMap.Application.Document.CommandBars;
            UID uid = new UIDClass();
            uid.Value = "esriEditor.StartEditingCommand";
            //bool isEditable = true;
            try
            {
                ESRI.ArcGIS.Framework.ICommandItem CommandItem = CommandBars.Find(uid, false, false);
                if (CommandItem != null)
                {
                    CommandItem.Execute();
                }

                uid.Value = "esriEditorExt.ErrorWindowCommand";
                CommandItem = CommandBars.Find(uid, false, false);
                if (CommandItem != null)
                {
                    CommandItem.Execute();
                }
            }
            catch 
            {
                
                //isEditable = false;//如果初次打开出现不可编辑的错误，下面将尝试重新添加图层并打开错误编辑器
                
            }
            /*
            if (!isEditable)
            { 
                try
                {
                    ESRI.ArcGIS.Framework.ICommandItem CommandItem = CommandBars.Find(uid, false, false);
                    if (CommandItem != null)
                    {
                        CommandItem.Execute();
                    }

                    uid.Value = "esriEditorExt.ErrorWindowCommand";
                    CommandItem = CommandBars.Find(uid, false, false);
                    if (CommandItem != null)
                    {
                        CommandItem.Execute();
                    }
                
                }catch{}
            
             
            }*/
            //------------------------------------------完成打开错误报告器---------------------------------------------
            progressForm.Close();
#endif
        }
        private static void DirectRender(ILayer pLayer)
        {
            IGeoFeatureLayer MyGeoFeatureLayer = pLayer as IGeoFeatureLayer;
            IFeatureCursor MyFeatureCursor = MyGeoFeatureLayer.Search(null, false);
            IFeature MyFeature = MyFeatureCursor.NextFeature();

            Render renderer = new Render();

            IUniqueValueRenderer pUniqueValueR = new UniqueValueRendererClass();
            pUniqueValueR.FieldCount = 1;//单值渲染
            // int index = 0;
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
        protected override void OnUpdate()
        {
        }
    }
}
