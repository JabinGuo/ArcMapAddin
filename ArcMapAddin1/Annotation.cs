using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ESRI.ArcGIS.ArcMapUI;
using ESRI.ArcGIS.Geodatabase;
using ESRI.ArcGIS.DataSourcesGDB;
using ESRI.ArcGIS.DataSourcesFile;
using ESRI.ArcGIS.Geometry;
using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.Geoprocessor;
using ESRI.ArcGIS.Geoprocessing;
using ESRI.ArcGIS.DataManagementTools;
using ESRI.ArcGIS.Display;
using ESRI.ArcGIS.esriSystem;
using stdole;

namespace ArcMapAddin1
{
    class MyAnnotation
    {

#if false

        /// <summary>
        /// 标注转注记图层（shp文件不支持标注）
        /// </summary>
        /// <param name="pMap">地图</param>
        /// <param name="pLayer">图层</param>
        /// <param name="featureLinked">是否festurelinked</param>
        /// <param name="sError">错误信息</param>
        /// <returns>是否成功</returns>
        private bool LabelConvertToAnnotation(IMap pMap, ILayer pLayer, bool featureLinked, out string sError)
        {
            sError = string.Empty;
            bool bIsSucceed = true;
            string sName = string.Empty;
            try
            {
                IConvertLabelsToAnnotation pConvertLabelsToAnnotation = new ConvertLabelsToAnnotationClass();
                ITrackCancel pTrackCancel = new CancelTrackerClass();
                pMap.ReferenceScale = 100000000;
                pConvertLabelsToAnnotation.Initialize(pMap,/*
                esriAnnotationStorageType.esriMapAnnotation*/esriAnnotationStorageType.esriDatabaseAnnotation,
                    esriLabelWhichFeatures.esriAllFeatures, true, pTrackCancel, null);

                IGeoFeatureLayer pGeoFeatureLayer = pLayer as IGeoFeatureLayer;
                pGeoFeatureLayer.DisplayField = "Text_";
                pGeoFeatureLayer.DisplayAnnotation = true;



                if (pGeoFeatureLayer == null)
                {
                    sError = "图层为空";
                    return false;
                }

                IFeatureClass pFeatureClass = pGeoFeatureLayer.FeatureClass;
                if (pFeatureClass == null)
                {
                    sError = "图层要素为空";
                    return false;
                }

                IDataset pDataset = pFeatureClass as IDataset;
                //sName = GtMap.GxAEHelper.FeatureClass.GetPureName(pDataset.Name);

                sName = pLayer.Name;
                IFeatureWorkspace pFeatureWorkspace = pDataset.Workspace as IFeatureWorkspace;


                pConvertLabelsToAnnotation.AddFeatureLayer(pGeoFeatureLayer, sName + "ZJ", pFeatureWorkspace,
                    pFeatureClass.FeatureDataset, featureLinked, false, false, true, true, "");
                //// true, true, true, true
                pConvertLabelsToAnnotation.ConvertLabels();

                string sErrorInfo = pConvertLabelsToAnnotation.ErrorInfo;
                if (!string.IsNullOrEmpty(sErrorInfo))
                {
                    sError = sErrorInfo;
                    bIsSucceed = false;
                }

                IEnumLayer pEnumLayer = pConvertLabelsToAnnotation.AnnoLayers;

                pGeoFeatureLayer.DisplayAnnotation = true;
                pMap.AddLayers(pEnumLayer, true);








                IActiveView pActiveView = pMap as IActiveView;
                pActiveView.Refresh();

            }
            catch (Exception ex)
            {
                sError = ex.Message;
                bIsSucceed = false;
            }

            return bIsSucceed;
        }
#endif

        /// <summary>
        /// 标注转注记图层（shp文件不支持标注）
        /// </summary>
        /// <param name="pMap">地图</param>
        /// <param name="pLayer">图层</param>
        /// <param name="featureLinked">是否feasturelinked</param>
        /// <param name="sError">错误信息</param>
        /// <returns>是否成功</returns>
        public bool LabelConvertToAnnotation(IMap pMap, ILayer pLayer, bool featureLinked, out string sError)
        {
            sError = string.Empty;
            bool bIsSucceed = true;
            string sName = string.Empty;
            try
            {
                IConvertLabelsToAnnotation pConvertLabelsToAnnotation = new ConvertLabelsToAnnotationClass();
                ITrackCancel pTrackCancel = new CancelTrackerClass();
                IGeoFeatureLayer pGeoFeatureLayer = pLayer as IGeoFeatureLayer;
                pGeoFeatureLayer.DisplayField = "Text_";//此处指定要转的字段
                pGeoFeatureLayer.DisplayAnnotation = true;
               // pGeoFeatureLayer.AnnotationProperties
               // pMap.MapUnits = esriUnits.esriMeters;
                
                //pMap.ReferenceScale = 12000;//设置缩放比例
                //pMap.ActiveGraphicsLayer.SpatialReference.

                pConvertLabelsToAnnotation.Initialize(pMap, esriAnnotationStorageType.esriDatabaseAnnotation,
                    esriLabelWhichFeatures.esriAllFeatures, true, pTrackCancel, null);
                
                if (pGeoFeatureLayer == null)
                {
                    sError = "图层为空";
                //    pMap.MapUnits = esriUnits.esriUnknownUnits;
                    return false;
                }

                IFeatureClass pFeatureClass = pGeoFeatureLayer.FeatureClass;
                if (pFeatureClass == null)
                {
                    sError = "图层要素为空";
                //    pMap.MapUnits = esriUnits.esriUnknownUnits;
                    return false;
                }

                IDataset pDataset = pFeatureClass as IDataset;
                

                sName = pLayer.Name;
                IFeatureWorkspace pFeatureWorkspace = pDataset.Workspace as IFeatureWorkspace;
                
                
                pConvertLabelsToAnnotation.AddFeatureLayer(pGeoFeatureLayer,
                    sName + "ZJ", pFeatureWorkspace,
                    pFeatureClass.FeatureDataset,
                    featureLinked,
                    false, false, false, true, "");


                pConvertLabelsToAnnotation.ConvertLabels();//将标注转换为注释

                string sErrorInfo = pConvertLabelsToAnnotation.ErrorInfo;
                if (!string.IsNullOrEmpty(sErrorInfo))
                {
                    sError = sErrorInfo;
                    bIsSucceed = false;
                }

                IEnumLayer pEnumLayer = pConvertLabelsToAnnotation.AnnoLayers;

                pGeoFeatureLayer.DisplayAnnotation = false;
                pMap.AddLayers(pEnumLayer, false);//将注释层添加到arcmap显示

                IActiveView pActiveView = pMap as IActiveView;
                pActiveView.Refresh();
                  

            }
            catch (Exception ex)
            {

                sError = ex.Message;
                bIsSucceed = false;
            }
           // pMap.MapUnits = esriUnits.esriUnknownUnits;
            return bIsSucceed;
        }

        /// <summary>
        /// 配置指定的注记要素
        /// </summary>
        /// <param name="feature">要修改的注记要素</param>
        /// <param name="pointGeometry">注记要素坐标</param>
        /// <param name="text">注记内容</param>
        /// <param name="fontSize"></param>
        /// <param name="verticalAlignment"></param>
        /// <param name="horizontalAlignment"></param>
        public void ConfigAnnotation(IFeature feature, IGeometry pointGeometry,string text,int fontSize,esriTextVerticalAlignment verticalAlignment,esriTextHorizontalAlignment horizontalAlignment )
        {
            IFontDisp font = new StdFontClass() as IFontDisp;
            font.Name = "宋体";
            font.Bold = true;
            
            IFormattedTextSymbol formattedTextSymbol = new TextSymbolClass();
            
            formattedTextSymbol.Font = font;
            formattedTextSymbol.Size = fontSize;
            formattedTextSymbol.VerticalAlignment = verticalAlignment;
            formattedTextSymbol.HorizontalAlignment = horizontalAlignment;
            formattedTextSymbol.Angle = 0;
            formattedTextSymbol.CharacterSpacing = 10;
            formattedTextSymbol.CharacterWidth = 80;
            formattedTextSymbol.FlipAngle = 90;
            formattedTextSymbol.Leading = 0;
            formattedTextSymbol.WordSpacing = 10;
            formattedTextSymbol.Text = text;
            IColor rgb = new RgbColorClass();
            rgb.RGB = 15354;
            formattedTextSymbol.Color = rgb;


            ITextElement textElement = new TextElementClass();
            textElement.Symbol = formattedTextSymbol;
            textElement.Text = text;
            IElement element = textElement as IElement;
            
            element.Geometry = pointGeometry;
            element.Geometry.Envelope.Width = 0.06;
            
            IAnnotationFeature2 annotationFeature2 = feature as IAnnotationFeature2;
            try
            {
               // annotationFeature2.Annotation.Geometry = pointGeometry;
               
                annotationFeature2.Annotation = element;
                annotationFeature2.Status = esriAnnotationStatus.esriAnnoStatusPlaced;

                feature.Store();
            }
            catch { }
        }

        /// <summary>
        /// 配置指定要素的字段
        /// </summary>
        /// <param name="AnnotationFeatureClass"></param>
        /// <param name="AnnotationFeatureCursor"></param>
        /// <param name="AnnotationFeature"></param>
        public void ConfigFields(IFeatureClass AnnotationFeatureClass, IFeatureCursor AnnotationFeatureCursor, IFeature AnnotationFeature)
        {
            IWorkspace WorkSpace = ((IDataset)AnnotationFeatureClass).Workspace;
            IWorkspaceEdit edit = WorkSpace as IWorkspaceEdit;
            
            bool startEdit = edit.IsBeingEdited();
            if (!startEdit) { edit.StartEditing(false); }
            edit.StartEditOperation();
            try
            {
                int index = AnnotationFeature.Fields.FindField("Status");
                AnnotationFeature.set_Value(index, esriAnnotationStatus.esriAnnoStatusPlaced);
                index = AnnotationFeature.Fields.FindField("Angle");
                AnnotationFeature.set_Value(index, 90);
                index = AnnotationFeature.Fields.FindField("FontName");
                AnnotationFeature.set_Value(index, "华文彩云");

            }//先找到字段的索引值，再set_Value
            catch { }
            AnnotationFeature.Store();
            System.Runtime.InteropServices.Marshal.ReleaseComObject(AnnotationFeature);
            AnnotationFeatureCursor.Flush();
            System.Runtime.InteropServices.Marshal.ReleaseComObject(AnnotationFeatureCursor);
            edit.StopEditOperation();
            startEdit = edit.IsBeingEdited();
            if (startEdit)
            {
                edit.StopEditing(true);
            }
        }

    
    }
}
