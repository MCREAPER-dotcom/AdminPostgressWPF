using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfTESTOVOE.Model
{
    public class ModelExportElements
    {
        public string elements { get; set; }
        public string currentString { get; set; }
        public bool isReade { get; set; }
        public ModelExportElements() { }
        public ModelExportElements(string Element) 
        {
            elements = Element;
            currentString = "0";
        }
        public ModelExportElements(string Element,string CurrentString)
        {
            elements = Element;
            currentString = CurrentString;
            isReade = false;
        }
        public ModelExportElements(string Element, string CurrentString,bool IsReade)
        {
            elements = Element;
            currentString = CurrentString;
            isReade = IsReade;
        }

        public object ToString()
        {
            string str = $"{elements} {currentString} {isReade.ToString()}";
            return str;
        }
    }
}
