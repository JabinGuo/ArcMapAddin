using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using EngineWindowsApplication1;
using System.Data.OleDb;
using System.Threading;
using ESRI.ArcGIS.ArcMapUI;
using ESRI.ArcGIS.Geodatabase;
using ESRI.ArcGIS.DataSourcesGDB;
using ESRI.ArcGIS.DataSourcesFile;
using ESRI.ArcGIS.Geometry;
using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.Geoprocessor;
using ESRI.ArcGIS.Geoprocessing;
using ESRI.ArcGIS.DataManagementTools;
using System.Windows.Forms;
using ESRI.ArcGIS.Display;
using ESRI.ArcGIS.SystemUI;

using ESRI.ArcGIS.ConversionTools;
using ESRI.ArcGIS.esriSystem;
using stdole;

using ESRI.ArcGIS.Controls;

namespace ArcMapAddin1
{
    public class ArcGISAddin1 : ESRI.ArcGIS.Desktop.AddIns.Button
    {
        public ArcGISAddin1()
        {            
        }
                
        protected override void OnClick()
        {
#if true
            
#if true
            if (0 >= ArcMap.Document.FocusMap.LayerCount)
            {
                MessageBox.Show("未加入图层");
                return;
            }
            IEnumLayer EnumLayer = ArcMap.Document.FocusMap.get_Layers(null, false);
            ILayer tem_Layer = EnumLayer.Next();
           // string d = tem_Layer.Name;
            IDataLayer DataLayer;
            IDatasetName tem_DatasetName;
            string tem_Name;
            DataLayer = tem_Layer as IDataLayer;
            tem_DatasetName = DataLayer.DataSourceName as IDatasetName;
            tem_Name = tem_DatasetName.WorkspaceName.PathName;
            string tem_SubstringRef = tem_Name.Substring(tem_Name.LastIndexOf('.') + 1);

            while (null != tem_Layer)
            {
                DataLayer = tem_Layer as IDataLayer;
                tem_DatasetName = DataLayer.DataSourceName as IDatasetName;
                tem_Name = tem_DatasetName.WorkspaceName.PathName;
                string tem_Substring = tem_Name.Substring(tem_Name.LastIndexOf('.') + 1);
                if (tem_Substring != tem_SubstringRef)
                {
                    MessageBox.Show("图层来自不同的dwg","错误");
                    return;
                }

                tem_Layer = EnumLayer.Next();
            }
            if ("mdb" == tem_SubstringRef)
            {
                MessageBox.Show("图层来自个人地理数据库，如需重新生成，请清空图层后加入shp文件","提示");
                return;
            }

            EnumLayer.Reset();
            tem_Layer = EnumLayer.Next();
            string DatasetName = tem_Layer.Name.Substring(0, tem_Layer.Name.LastIndexOf('_'));
            //string DatasetName = tem_SubstringRef.Substring(tem_SubstringRef.LastIndexOf('\\') + 1);

            OpenFileDialog OFD = new OpenFileDialog();
            OFD.Multiselect = true;
            OFD.Title = "请选择要加入的个人地理数据库";
            OFD.Filter = "(*.mdb)|*.mdb";
            OFD.InitialDirectory = "C:\\";
            OFD.Multiselect = false;
            string MDB_FullName = null;
            if (OFD.ShowDialog() == DialogResult.OK)
            {
                MDB_FullName = OFD.FileName;
            }
            if (null == MDB_FullName)
            {
                return;
            }
            string MDB_Path = MDB_FullName.Substring(0, MDB_FullName.LastIndexOf('\\') + 1);
            string MDB_Name = MDB_FullName.Substring(MDB_Path.Length, MDB_FullName.LastIndexOf('.') - MDB_Path.Length);

            

            EnumLayer = ArcMap.Document.FocusMap.get_Layers(null, false);
            tem_Layer = EnumLayer.Next();
            
            //string a = tem_Layer.Name.Substring(DatasetName.Length + 1);
            string[] shps = new string[ArcMap.Document.FocusMap.LayerCount];
            int i = 0;
            bool isPointExist = false;
            bool isLineExist = false;
            bool isPolygonExist = false;
            bool isAnnotationExist = false;
            
            while (null != tem_Layer)
            {
                shps[i] = tem_Name + "\\" + tem_Layer.Name + ".shp";
                switch (tem_Layer.Name.Substring(DatasetName.Length + 1))
                {
                    case "Point":
                        isPointExist = true;
                        break;
                    case "Line":
                        isLineExist = true;
                        break;
                    case "Polygon":
                        isPolygonExist = true;
                        break;
                    case "Annotation":
                        isAnnotationExist = true;
                        break;
                }
                tem_Layer = EnumLayer.Next();
                i++;
            }

            if (!isPointExist)
            {
                DialogResult Result = MessageBox.Show("未加入点图层，是否继续", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Information);
                if (Result == DialogResult.No)
                {
                    return;
                }
            }
            if (!isLineExist)
            {
                DialogResult Result = MessageBox.Show("未加入线图层，是否继续", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Information);
                if (Result == DialogResult.No)
                {
                    return;
                }
            }
            if (!isPolygonExist)
            {
                DialogResult Result = MessageBox.Show("未加入面图层，是否继续", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Information);
                if (Result == DialogResult.No)
                {
                    return;
                }
            }
            if (!isAnnotationExist)
            {
                DialogResult Result = MessageBox.Show("未加入注记层，是否继续", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Information);
                if (Result == DialogResult.No)
                {
                    return;
                }
            }

#endif

            
#if false
            string MDB_FullName = "C:\\Users\\JabinGuo\\Desktop\\dwg\\test.mdb";
            string DatasetName = "db";
            string MDB_Path = "C:\\Users\\JabinGuo\\Desktop\\dwg\\";
            string MDB_Name = "test";
            
            
            OpenFileDialog OFD = new OpenFileDialog();
            OFD.Multiselect = true;
            OFD.Title = "请选择文件";
            OFD.Filter = "shp files(*.shp)|*.shp|所有文件(*.*)|*.*";
            OFD.InitialDirectory = "C:\\";
            IEnumLayer EnumLayer = ArcMap.Document.FocusMap.get_Layers(null, false);
            //AxMapControl mc = new AxMapControl();
            // ICommand command = new ESRI

            string[] shps = null;
            if(OFD.ShowDialog() == DialogResult.OK)
            {
                shps = OFD.FileNames;
                
            }
            
            if (null == shps || shps.Length < 1)
            {
                return;//如果没有选择文件，退出函数
            }
            
            
#endif
            ProgressForm progressForm = new ProgressForm();
            progressForm.Show();
            
            C_MDBBuilder buil = new C_MDBBuilder();

            //***********************************************
            IWorkspaceFactory iwsf = new AccessWorkspaceFactoryClass();
            IFeatureWorkspace ifws = null;
            

            if (File.Exists(MDB_FullName))
            {
                ifws = (IFeatureWorkspace)iwsf.OpenFromFile(MDB_FullName, 0);
                
                try
                {
                    ifws.OpenFeatureDataset(DatasetName).Delete();
                }
                catch { }
            }
            //如果mdb存在，那么dataset也有可能存在，尝试删除dataset
            ArcMap.Document.FocusMap.ClearLayers();
            foreach (string shpName in shps)
            {
                bool isFeatureClassExists = buil.MDB_Builder(shpName, MDB_Path, MDB_Name, DatasetName);//将shp导入mdb文件
                progressForm.AddProgress(20 / (shps.Length));
            }
            
            //**********************************开 始 渲 染**************************************
            
            EnumLayer = ArcMap.Document.FocusMap.get_Layers(null, false);
            ILayer pLayer = EnumLayer.Next();

            while (null != pLayer)
            {
                if ((DatasetName + "_Point") == pLayer.Name ||(DatasetName + "_Line") == pLayer.Name)
                {
                    DirectRender(pLayer);//渲染的方法
                }
                pLayer = EnumLayer.Next();
            }

            progressForm.AddProgress(10);
            //*********************************渲 染 结 束*****************************************
            
            //*********************************开 始 标 注 转 注 记**********************************
            //IEnumLayer EnumLayer = ArcMap.Document.FocusMap.get_Layers(null, false);
            //ILayer pLayer = EnumLayer.Next();
            EnumLayer = ArcMap.Document.FocusMap.get_Layers(null, false);
            pLayer = EnumLayer.Next();

            while (((DatasetName + "_Annotation") != pLayer.Name) && (null != pLayer) && ((DatasetName + "_AnnotationZJ") != pLayer.Name))
            {
                pLayer = EnumLayer.Next();
            }
            if (null == pLayer)
            {
                progressForm.Close();
                return;
            }
            if (((DatasetName + "_AnnotationZJ") == pLayer.Name))
            {
                progressForm.Close();
                MessageBox.Show("注记层已存在");
                return;
            }
            MyAnnotation myAnnotation = new MyAnnotation();

            string errstring = null;
            myAnnotation.LabelConvertToAnnotation(ArcMap.Document.FocusMap, pLayer, false, out errstring);//生成的注记层名字为“标注层名称+ZJ”

            progressForm.AddProgress(20);
            
            IGeoFeatureLayer single_useGeoFeatureLayer = pLayer as IGeoFeatureLayer;
            single_useGeoFeatureLayer.DisplayAnnotation = false;//隐藏标注

            ifws = (IFeatureWorkspace)iwsf.OpenFromFile(MDB_FullName, 0);
            //IFeatureDataset MyfeatureDataset = ifws.OpenFeatureDataset(DatasetName);
            IFeatureClassContainer MyFeatureClassContainer = ifws.OpenFeatureDataset(DatasetName) as IFeatureClassContainer;
            IEnumFeatureClass enumFeatureClass = MyFeatureClassContainer.Classes;
            IFeatureClass MyAnnotationFeatureClass = enumFeatureClass.Next();
            while (null != MyAnnotationFeatureClass &&((DatasetName + "_AnnotationZJ") != MyAnnotationFeatureClass.AliasName))
            {
                MyAnnotationFeatureClass = enumFeatureClass.Next();
            }
            //获取生成的注记层
            if (null == MyAnnotationFeatureClass)
            {
                MessageBox.Show("未找到注记层，请检查是否生成注记层", "警告");
                return;
            }

            progressForm.AddProgress(10);
            //IFeatureClass MyAnnotationFeatureClass = ifws.OpenFeatureClass("AnnotationZJ");//在mdb空间下打开FeatureClass，所以不同dataset下不能有重名FeatureClass
            //IFeatureCursor AnnotationFeatureCursor = MyAnnotationFeatureClass.Search(null, false);
            //IFeature MyAnnotationFeature = AnnotationFeatureCursor.NextFeature();
            enumFeatureClass = MyFeatureClassContainer.Classes;
            IFeatureClass LabelFeatureClass = enumFeatureClass.Next();
            while (null != LabelFeatureClass &&((DatasetName + "_Annotation") != LabelFeatureClass.AliasName))
            {
                LabelFeatureClass = enumFeatureClass.Next();
            }//获取原注记层
            if (null == LabelFeatureClass)
            {
                MessageBox.Show("未找到原注记层，可能造成生成的注记层要素缺失", "警告");
                return;
            }
            
            IFeature MyAnnotationFeature;
            IFeatureCursor AnnotationFeatureCursor = MyAnnotationFeatureClass.Search(null, false);
            MyAnnotationFeature = AnnotationFeatureCursor.NextFeature();
           
            while (null != MyAnnotationFeature)
            {
                int LabelFeatureID = (int)MyAnnotationFeature.get_Value(MyAnnotationFeature.Fields.FindField("FeatureID"));
                IFeature LabelFeature = LabelFeatureClass.GetFeature(LabelFeatureID);
                string st = LabelFeature.get_Value(LabelFeature.Fields.FindField("Text_")) as string;
                myAnnotation.ConfigAnnotation(MyAnnotationFeature, LabelFeature.Shape, st, 8, esriTextVerticalAlignment.esriTVABottom, esriTextHorizontalAlignment.esriTHALeft);
                MyAnnotationFeature = AnnotationFeatureCursor.NextFeature();
            }//逐个要素修改生成的注记
            progressForm.AddProgress(20);
            EnumLayer = ArcMap.Document.FocusMap.get_Layers(null, false);
            pLayer = EnumLayer.Next();
            while(null != pLayer)
            {
                if((DatasetName + "_Annotation") == pLayer.Name)
                {
                    ArcMap.Document.FocusMap.DeleteLayer(pLayer);
                }
                pLayer = EnumLayer.Next();
            }//移除原注记层
            progressForm.AddProgress(10);
            progressForm.Close();

            //******************************标 注 转 注 记 结 束********************************************

            //Thread.Sleep(1000);

#endif
#region 标注转注记
#if false
            List<string> shps = new List<string>();
            shps.Add("C:\\Users\\JabinGuo\\Desktop\\注记\\Annotation.shp");
            C_MDBBuilder buil = new C_MDBBuilder();
            bool isFeatureClassExists = buil.MDB_Builder(shps[0], "C:\\Users\\JabinGuo\\Desktop\\dwg\\", "test", "db");//将shp导入mdb文件

            IWorkspaceFactory iwsf = new AccessWorkspaceFactoryClass();
            
            IFeatureWorkspace ifws =(IFeatureWorkspace) iwsf.OpenFromFile("C:\\Users\\JabinGuo\\Desktop\\dwg\\test.mdb", 0);

            IEnumLayer EnumLayer = ArcMap.Document.FocusMap.get_Layers(null, false);
            ILayer pLayer = EnumLayer.Next();
            while ("Annotation" != pLayer.Name)
            {
                pLayer = EnumLayer.Next();
            }
            //ArcMap.Document.FocusMap.get_Layer(0);
            
            //IFeatureLayer pFeatureLayer = pLayer as IFeatureLayer;
            //IGeoFeatureLayer pGeoFeatureLayer = pFeatureLayer as IGeoFeatureLayer;
            MyAnnotation myAnnotation = new MyAnnotation();

            string errstring = null;
            myAnnotation.LabelConvertToAnnotation(ArcMap.Document.FocusMap, pLayer, true, out errstring);//生成的注记层名字为“标注层名称+ZJ”

            IGeoFeatureLayer single_useGeoFeatureLayer = pLayer as IGeoFeatureLayer;
            single_useGeoFeatureLayer.DisplayAnnotation = false;//隐藏标注
            
            IFeatureClass MyAnnotationFeatureClass = ifws.OpenFeatureClass("AnnotationZJ");
            //IFeatureCursor AnnotationFeatureCursor = MyAnnotationFeatureClass.Search(null, false);
            //IFeature MyAnnotationFeature = AnnotationFeatureCursor.NextFeature();

            IFeatureClass LabelFeatureClass = ifws.OpenFeatureClass("Annotation");
            
            IFeature MyAnnotationFeature = MyAnnotationFeatureClass.GetFeature(10);//选定要修改的要素
            
            int LabelFeatureID =(int)MyAnnotationFeature.get_Value(MyAnnotationFeature.Fields.FindField("FeatureID"));
            IFeature LabelFeature = LabelFeatureClass.GetFeature(LabelFeatureID);
            
            myAnnotation.ConfigAnnotation(MyAnnotationFeature, LabelFeature.Shape, "kkk", 12, esriTextVerticalAlignment.esriTVACenter, esriTextHorizontalAlignment.esriTHACenter);

            //myAnnotation.ConfigFields(MyAnnotationFeatureClass, AnnotationFeatureCursor, MyAnnotationFeature);//保留此函数，可以用于修改要素字段
#endif
#endregion

#region 拓扑检查
    #if false
            IFeatureDataset topocheckDataset = ifws.OpenFeatureDataset("db");
            TopologyChecker topocheck = new TopologyChecker(topocheckDataset);
            topocheck.PUB_TopoBuild("addinTopo");
            topocheck.PUB_AddFeatureClass(null);
            List<IFeatureClass> allFeatureClass = topocheck.PUB_GetAllFeatureClass();

            IFeatureClass topocheckFeatureClass = null;
            foreach (IFeatureClass tem in allFeatureClass)
            {
                if ("" == tem.AliasName)
                {
                    topocheckFeatureClass = tem;
                    break;
                }
            }

            topocheck.PUB_AddRuleToTopology(TopologyChecker.TopoErroType.线要素必须不相交, topocheckFeatureClass);
    #endif
#endregion

#region 渲染
#if false
            List<string> shps = new List<string>();
            shps.Add("C:\\Users\\JabinGuo\\Desktop\\渲染\\Polygon.shp");
            C_MDBBuilder buil = new C_MDBBuilder();
            bool isFeatureClassExists = buil.MDB_Builder(shps[0], "C:\\Users\\JabinGuo\\Desktop\\dwg\\", "test", "db");//将shp导入mdb文件

            IWorkspaceFactory iwsf = new AccessWorkspaceFactoryClass();

            IFeatureWorkspace ifws = (IFeatureWorkspace)iwsf.OpenFromFile("C:\\Users\\JabinGuo\\Desktop\\dwg\\test.mdb", 0);

            
            DirectRender(ArcMap.Document.FocusMap.get_Layer(0));
#endif
            #endregion
            


#region 拓扑检查
#if false
            
            List<string> shps = new List<string>();
            shps.Add("C:\\Users\\JabinGuo\\Desktop\\shp\\2010.6.20-500_t3\\Line.shp");
                
            C_MDBBuilder buil = new C_MDBBuilder(shps,"C:\\Users\\JabinGuo\\Desktop\\dwg\\","test","db");
            
            AccessWorkspaceFactory MyWorkSpaceFactory = new AccessWorkspaceFactoryClass();
            IFeatureWorkspace a = (IFeatureWorkspace)MyWorkSpaceFactory.OpenFromFile("C:\\Users\\JabinGuo\\Desktop\\dwg\\test.mdb", 0);
            IFeatureDataset k = a.OpenFeatureDataset("db");
            TopologyChecker topocheck = new TopologyChecker(k);
            
            topocheck.PUB_TopoBuild("addinTopo");
            topocheck.PUB_AddFeatureClass(null);
            List<IFeatureClass> test = topocheck.PUB_GetAllFeatureClass();
            
            IFeatureClass ifc = test[0];
            topocheck.PUB_AddRuleToTopology(TopologyChecker.TopoErroType.线要素必须不相交, test[0]);
#endif
#endregion
           
        }
        
        
    

private static void DirectRender(ILayer pLayer )
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
        /// <summary>
        /// 创建mdb文件
        /// </summary>
        class C_MDBBuilder
    {
        Geoprocessor GP_Tool = new Geoprocessor();//GP运行工具
        string S_Status; //当前状态
        System.Diagnostics.Stopwatch WATCH_StopWatch = new System.Diagnostics.Stopwatch();//计时器
        IGeoProcessorResult GP_Progress;//GP状态
        TimeSpan TS_TimeSpan;//时间间隔
        string S_MDBFile;//MDB路径
        string S_LDBFile;//LDB的路径
        IFeatureDataset FDS_Featuredataset = null;//生成的数据集
        Thread TH_TimeSpan;//线程
        // List<string> LI_AllShapePath = new List<string>();//要导入MDB的所有Shp文件的地址
        
            
        /// <summary>
        /// 构造MDB
        /// </summary>
        /// <param name="IN_AllShapePath">所有要导入shape的地址</param>
        /// <param name="IN_MDBPath">要构建的MDB的地址</param>
        /// <param name="IN_MDBName">要构建的MDB的名称</param>
        public bool MDB_Builder(string IN_AllShapePath, string IN_MDBPath, string IN_MDBName,string IN_DatasetName)
        {
            // LI_AllShapePath = IN_AllShapePath;
            S_MDBFile = IN_MDBPath + IN_MDBName + ".mdb";//MDB路径
            S_LDBFile = IN_MDBPath + IN_MDBName + ".ldb";//LDB路径
            IWorkspaceFactory Temp_WorkFactory = new AccessWorkspaceFactory();

            /*
            if (File.Exists(S_MDBFile))//清理MDB和LDB文件
            {
                try
                {
                    File.Delete(S_MDBFile);
                }
                catch (Exception e) { MessageBox.Show(e.Message.ToString()); }
                if (File.Exists(S_LDBFile))
                    File.Delete(S_LDBFile);
            }
            Temp_WorkFactory.Create(IN_MDBPath, IN_MDBName, null, 0);//创建一个MDB
            */

            if (File.Exists(S_LDBFile))
            {
                try
                {
                    File.Delete(S_LDBFile);

                }
                catch { }
            }
            //  if (!File.Exists(S_MDBFile))
            {
                try
                {
                    Temp_WorkFactory.Create(IN_MDBPath, IN_MDBName, null, 0);//创建一个MDB
                }
                catch { }
            }

            PRV_CreatFeatureDataset(IN_DatasetName, IN_AllShapePath);//创建要素数据集，以第一个shp文件为空间参考


            bool isExists = PRV_AddFeatureClass(IN_AllShapePath);//将每一个shp文件添加进去           

            return isExists;
        }

        //注册要素类
            /// <summary>
            /// 返回true说明FeatureClass存在，返回false说明不存在，重新创建
            /// </summary>
            /// <param name="IN_ShapePath"></param>
            /// <returns></returns>
        private bool PRV_AddFeatureClass(string IN_ShapePath)
        {
            string Temp_Direction = System.IO.Path.GetDirectoryName(IN_ShapePath);//该Shp文件的目录
            string Temp_Name = System.IO.Path.GetFileNameWithoutExtension(IN_ShapePath);//该Shp文件的名称
            IWorkspaceFactory Temp_ShapeWorkFactory = new ShapefileWorkspaceFactory();
            IFeatureWorkspace Temp_ShapeWorkspace = Temp_ShapeWorkFactory.OpenFromFile(Temp_Direction,0) as IFeatureWorkspace;
            IWorkspaceFactory Temp_AccessWorkFactory = new AccessWorkspaceFactory();
            IFeatureWorkspace Temp_Workspace = Temp_AccessWorkFactory.OpenFromFile(S_MDBFile, 0) as IFeatureWorkspace;

            IFeatureClassContainer tem_FeatureClassContainer = (IFeatureClassContainer)FDS_Featuredataset;
            IEnumFeatureClass pEnumFeatureClass = (IEnumFeatureClass)tem_FeatureClassContainer.Classes;
            IFeatureClass tem_FeatureClass = pEnumFeatureClass.Next();
            while (null != tem_FeatureClass)
            {
                if (Temp_Name == tem_FeatureClass.AliasName) 
                {// return true;
                }
                tem_FeatureClass = pEnumFeatureClass.Next();
            }

            IFeatureClass Temp_FeatureClass = Temp_ShapeWorkspace.OpenFeatureClass(Temp_Name);
            FeatureClassToFeatureClass Temp_FCToFC = new FeatureClassToFeatureClass(IN_ShapePath, S_MDBFile + "\\" + FDS_Featuredataset.Name, Temp_Name);//将Shp文件导入要素数据集
            
            GP_Progress = GP_Tool.ExecuteAsync(Temp_FCToFC);
            TH_TimeSpan = new Thread(PRV_GetStatus);//开辟线程计时
            TH_TimeSpan.Start();
            TH_TimeSpan.Join();
            
            return false;
           // IFeatureClassContainer ss = (FDS_Featuredataset.Workspace as IFeatureWorkspace).OpenFeatureDataset(FDS_Featuredataset.Name) as IFeatureClassContainer;
          //  Console.WriteLine("完成");
        }
        //创建要素数据集
        private void PRV_CreatFeatureDataset(string IN_FeatureDataSetName,string IN_ShapePath)
        {
            string Temp_Direction = System.IO.Path.GetDirectoryName(IN_ShapePath);//Shp文件的目录
            string Getname = System.IO.Path.GetFileNameWithoutExtension(IN_ShapePath);//Shp文件的文件名
            IWorkspaceFactory Temp_MDBWorkFactory = new AccessWorkspaceFactory();
            IFeatureWorkspace Temp_MDBWorkspace = Temp_MDBWorkFactory.OpenFromFile(S_MDBFile, 0) as IFeatureWorkspace;
            IWorkspaceFactory Temp_ShapeWorkFactory = new ShapefileWorkspaceFactory();
            IFeatureWorkspace Temp_ShapeWorkspace = Temp_ShapeWorkFactory.OpenFromFile(Temp_Direction, 0) as IFeatureWorkspace;
            IFeatureClass Temp_ShapeFeatureClass = Temp_ShapeWorkspace.OpenFeatureClass(Getname);//获取shp文件
            ISpatialReference SP_SpatialRefer = (Temp_ShapeFeatureClass as IGeoDataset).SpatialReference;//获取空间投影

            try
            {
                FDS_Featuredataset = Temp_MDBWorkspace.OpenFeatureDataset(IN_FeatureDataSetName);
            }
            catch { }
            /*
            if (null != FDS_Featuredataset)
            {
                FDS_Featuredataset.Delete();
            }
            */
            /*
            if (null != FDS_Featuredataset)
            {
               
                return; 
            }
            */
            try
            {
                Temp_MDBWorkspace.CreateFeatureDataset(IN_FeatureDataSetName, SP_SpatialRefer);//在MDB中创建要素数据集
                FDS_Featuredataset = Temp_MDBWorkspace.OpenFeatureDataset(IN_FeatureDataSetName);//获取返回的要素数据集
            }catch{}
            
        
        }
        //GP工具监督器，获取状态
        private void PRV_GetStatus()
        {
            WATCH_StopWatch.Start(); //  开始监视代码运行时间
            while (GP_Progress.Status != ESRI.ArcGIS.esriSystem.esriJobStatus.esriJobSucceeded && GP_Progress.Status != ESRI.ArcGIS.esriSystem.esriJobStatus.esriJobFailed)
            {
                TS_TimeSpan = WATCH_StopWatch.Elapsed;
                S_Status = "状态：" + GP_Progress.Status.ToString() + "已经运行：" + ((int)(TS_TimeSpan.TotalSeconds)).ToString() + "秒";
                Thread.Sleep(1000);
                Console.WriteLine(S_Status);//控制台输出状态
            }
            WATCH_StopWatch.Stop();//停止计时器
            WATCH_StopWatch.Reset();//重置计时器
            TH_TimeSpan.Abort();//线程自杀
        }
 
    }

       
        protected override void OnUpdate()
        {
            
        }
    }
}
