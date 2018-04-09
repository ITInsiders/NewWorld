using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;

namespace NW.PL
{
    public static class SVG
    {
        public static string getCodeSVG(string url)
        {
            string code;
            using (FileStream fstream = new FileStream(url, FileMode.Open))
            {
                byte[] array = new byte[fstream.Length];
                fstream.Read(array, 0, array.Length);
                code = System.Text.Encoding.Default.GetString(array);
            }
            return code;
        }
    }
}