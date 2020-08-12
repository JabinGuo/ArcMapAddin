using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using ESRI.ArcGIS.esriSystem;
using ESRI.ArcGIS.ArcMapUI;
using ESRI.ArcGIS.Carto;

namespace ArcMapAddin1
{
    public class ArcGISAddinErrorInspector : ESRI.ArcGIS.Desktop.AddIns.Button
    {
        

        public ArcGISAddinErrorInspector()
        {
        }

        protected override void OnClick()
        {   
            bool Enable = false;
            IEnumLayer pEnumLayer = ArcMap.Document.FocusMap.get_Layers(null, false);
            ILayer pLayer = pEnumLayer.Next();
            //string a = pLayer.Name.Substring(pLayer.Name.LastIndexOf('_') + 1);
            while (null != pLayer)
            {
                if ("TopoCheck" == pLayer.Name.Substring(pLayer.Name.LastIndexOf('_') + 1))
                {
                    Enable = true;
                    break;
                }
                pLayer = pEnumLayer.Next();
            }
            if (null == pLayer)
            {
                Enable = false;
            }
            if (!Enable)
            {
                return;
            }

            ESRI.ArcGIS.Framework.ICommandBars CommandBars = ArcMap.Application.Document.CommandBars;
            UID uid = new UIDClass();
            uid.Value = "esriEditor.StartEditingCommand";
            ESRI.ArcGIS.Framework.ICommandItem CommandItem = CommandBars.Find(uid, false, false);
            try
            {
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

            }
        }

        protected override void OnUpdate()
        {
            
            
        }
    }
}
