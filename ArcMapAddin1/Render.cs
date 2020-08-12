using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.Display;
using ESRI.ArcGIS.Geodatabase;
using ESRI.ArcGIS.Geometry;

namespace ArcMapAddin1
{
    class Render
    {
        public ISymbol GetSymbol(IFeature pFeature)
        {
            ISymbol pSymbol = null;
            IColor pColor = new RgbColorClass();
            string str_color = pFeature.get_Value(pFeature.Fields.FindField("颜色")) as string;
            if (" " == str_color)
            {
                str_color = "000000";
            }
            pColor.RGB = Convert.ToInt32(RGBconvert(str_color),16);
            
            switch (pFeature.Shape.GeometryType)
            {
                case esriGeometryType.esriGeometryPoint:
                    pSymbol = GetPointSymbol(pColor);
                    break;
                case esriGeometryType.esriGeometryPolyline:
                    string str_width = pFeature.get_Value(pFeature.Fields.FindField("线宽")) as string;
                    double width= 0 ;
                    char[] char_width = str_width.ToCharArray();
                    if (1 < char_width.Count())
                    {
                        char[] widthValue = new char[char_width.Count() - 1];
                        Array.ConstrainedCopy(char_width, 0, widthValue, 0, char_width.Count() - 1);

                        width = Convert.ToDouble(new string(widthValue));
                    }
                    else
                    {
                        width = 0.25;
                    }
                    string str_LineStyle = pFeature.get_Value(pFeature.Fields.FindField("线型")) as string;
                    if (" " == str_LineStyle)
                    {
                        pSymbol = GetLineSymbol(pColor, esriSimpleLineStyle.esriSLSSolid,width);
                    }
                    else
                    {
                        esriSimpleLineStyle LineStyle = esriSimpleLineStyle.esriSLSSolid;
                        try{
                            LineStyle = GetLineStyle(str_LineStyle);
                        }
                        catch{}
                        pSymbol = GetLineSymbol(pColor, LineStyle, width);
                    }
                    
                    break;
                default:
                    pSymbol = GetFillSymbol(pColor);
                    break;

            }
            return pSymbol;
        }

        public esriSimpleLineStyle GetLineStyle(string str_LineStyle)
        {
            string[] Splitstr_LineStyle = str_LineStyle.Split(' ');
            if (2 < Splitstr_LineStyle.Count())
            {
                if (0 == ((Splitstr_LineStyle.Count() / 2 ) % 2))
                {
                    return esriSimpleLineStyle.esriSLSDashDot;
                }
                else
                {
                    return esriSimpleLineStyle.esriSLSDashDotDot;
                }
            }
            else
            {
                if (isDash(Splitstr_LineStyle[0], Splitstr_LineStyle[1]))
                {
                    return esriSimpleLineStyle.esriSLSDash;
                }

                else
                {
                    return esriSimpleLineStyle.esriSLSDot;
                }
            }
        }

        public bool isDash(string first, string second)
        {
            
            //first.ToList;
            //List<char> a = first.ToCharArray().ToList<char>();
            char[] char_first = first.ToCharArray();
            char[] char_second = second.ToCharArray();
            for (int i = (char_first.Count() - 1); i >= 0; i--)
            {
                if ('0' <= char_first[i] && char_first[i] <= '9')
                {
                    first = first.Remove(i + 1);
                    break;
                }
            }

            for (int i = (char_second.Count() - 1); i >= 0; i--)
            {
                if ('0' <= char_second[i] && char_second[i] <= '9')
                {
                    second = second.Remove(i + 1);
                    break;
                }
            }

            return Convert.ToInt32(first) > Convert.ToInt32(second);
        }

        public string RGBconvert(string str_RGB)
        {
            char[] char_RGB = str_RGB.ToCharArray(0,6);
            char tem;
            tem = char_RGB[0];
            char_RGB[0] = char_RGB[4];
            char_RGB[4] = tem;
            tem = char_RGB[1];
            char_RGB[1] = char_RGB[5];
            char_RGB[5] = tem;
            string str = new string(char_RGB);
            
            return str;
        }

        public ISymbol GetPointSymbol(IColor pColor)
        {
            ISymbol pSymbol;
            //ISimpleFillSymbol pSymbolFillSymbol = new SimpleFillSymbolClass();
            ISimpleMarkerSymbol pSymbolMarkerSymbol = new SimpleMarkerSymbolClass();
            pSymbolMarkerSymbol.Color = pColor;
            pSymbolMarkerSymbol.Size = 3;
            pSymbol = pSymbolMarkerSymbol as ISymbol;
            return pSymbol;
        }

        public ISymbol GetFillSymbol(IColor pColor)
        {
            ISymbol pSymbol;
            ISimpleFillSymbol pSymbolFillSymbol = new SimpleFillSymbolClass();
            pSymbolFillSymbol.Color = pColor;
            
            pSymbol = pSymbolFillSymbol as ISymbol;
            return pSymbol;
        }

        public ISymbol GetLineSymbol(IColor pColor, esriSimpleLineStyle LineStyle,double width)
        {
            ISymbol pSymbol;
            ISimpleLineSymbol pSymbolLineSymbol = new SimpleLineSymbolClass();
            pSymbolLineSymbol.Color = pColor;
            pSymbolLineSymbol.Width = width * 1;
            pSymbolLineSymbol.Style = LineStyle;
            
            pSymbol = pSymbolLineSymbol as ISymbol;
            return pSymbol;
        }


    }
}
