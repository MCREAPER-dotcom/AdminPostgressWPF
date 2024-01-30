using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace WpfTESTOVOE.Model
{
    public class ModelImportElements
    {
        public string file { get; set; }
        public string currentString { get; set; }
        public string filePath { get; set; }
        public bool isReade { get; set; }

        public ModelImportElements() { }
        public ModelImportElements(string File,string CurrentString,string FileString) 
        { 
            file = File;
            currentString = CurrentString;
            filePath = FileString;
            isReade = false;
        }
        public ModelImportElements(string File, string CurrentString, string FileString,bool IsReade)
        {
            file = File;
            currentString = CurrentString;
            filePath = FileString;
            isReade = IsReade;
        }

        public object ToString()
        {
            string str = $"{file}{currentString} {filePath} {isReade.ToString()}";
            return str;
        }
    }
}
