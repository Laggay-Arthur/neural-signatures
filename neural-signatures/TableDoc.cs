using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace neural_signatures
{
    class TableDoc
    {
        /*string name_document;
        DateTime? date_document;
        DateTime? date_validity;*/

        public TableDoc(string Name_document,string Person, string Date_document, string Date_validity)
        {
            name_document = Name_document;
            date_document = Date_document;
            date_validity = Date_validity;
            person = Person;
            
        }
        public string name_document { get; set; }
        public string person { get; set; }
        public string date_document { get; set; }
        public string date_validity { get; set; }
       
    }
}
