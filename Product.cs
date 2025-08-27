using PdfSharpCore.Drawing;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PDFiller
{
    internal class Product
    {
        private string id;
        private byte[] buff;
        private string name;

        public Product()
        {
            id = "";
            buff = null;
            name = "";
        }
        public Product(string id, byte[] buff, string name)
        {
            this.id = id;
            this.buff = buff;
            this.name = name;
        }

        public string Id { get => id; }
        public byte[] ImageBuffer { get => buff; }
        public string Name { get => name; }

        public Bitmap Image
        {
            get
            {
                if (buff == null)
                {
                    return null;
                }
                using (var ms = new MemoryStream(buff))
                {
                    return new Bitmap(ms);
                }
            }
        }







        /// <summary>
        /// Modifies the name of the topper, if it is in the special swaps dictionary.
        /// Otherwise, it trims the worthless words out.
        /// </summary>
        /// <param name="tId">The Product Number (PN) of the product</param>
        /// <param name="tName">The original name of said product</param>
        /// <returns>The name with </returns>
        //private string ModifyName(string tId, string tName)
        //{
        //    //Sa vad ce naiba fac cu numele in bulgara, cum le editez 
        //    if (SpecialSwaps.ContainsKey(tId))
        //    {
        //        return tName = SpecialSwaps[tId];
        //    }
        //    /*
        //     омплект украса за торта KZE Prints Пес Патрул/ Paw Patrol, Гланцова хартия, Многоцветен, 17 бр
        //     */

        //    string[] list = { "briose de ", "briose ", "tort ", };
        //    foreach (string s in list)
        //    {
        //        int index = tName.LastIndexOf(s);
        //        if (index > 0)
        //        {
        //            return tName = tName.Substring(index + s.Length).Replace(", KZE Prints, Photo Paper Glossy", "");
        //        }

        //    }

        //    //improvizatie pentru bulgaria  

        //    if (tName.StartsWith("Комплект украса за торта "))
        //    {
        //        try
        //        {
        //            tName = tName.Replace("Комплект украса за торта ", "");
        //            if (tName.Contains("/") && tName.IndexOf("/") < tName.Length)
        //            {
        //                tName = tName.Substring(tName.IndexOf("/") + 1).Trim();
        //            }

        //            if (tName.Contains(","))
        //            {
        //                tName = tName.Substring(0, tName.IndexOf(","));
        //            }
        //            return tName;
        //        }
        //        catch (Exception)
        //        {

        //            return tName;
        //        }
        //    }

        //    return tName;

        //}
    }

}
