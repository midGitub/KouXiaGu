using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace XGame
{


    public static class XmlHelper
    {


        public static T LoadEnum<T>(XAttribute xAttribute, T defaultValue)
           where T : struct
        {
            try
            {
                return xAttribute == null ?
                    defaultValue :
                    (T)Enum.Parse(typeof(T), (string)xAttribute);
            }
            catch
            {
                return defaultValue;
            }
        }

        public static int Load(XAttribute xAttribute, int defaultValue)
        {
            return xAttribute == null ? defaultValue : (int)xAttribute;
        }

        public static bool Load(XAttribute xAttribute, bool defaultValue)
        {
            return xAttribute == null ? defaultValue : (bool)xAttribute;
        }

        public static byte Load(XAttribute xAttribute, byte defaultValue)
        {
            return xAttribute == null ? defaultValue : Convert.ToByte((string)xAttribute);
        }

        public static IntVector2 Load(XAttribute xAttributeX, XAttribute xAttributeY, IntVector2 intVector2)
        {
            intVector2.x = xAttributeX == null ? intVector2.x : (short)xAttributeX;
            intVector2.y = xAttributeY == null ? intVector2.y : (short)xAttributeY;
            return intVector2;
        }


    }

}
