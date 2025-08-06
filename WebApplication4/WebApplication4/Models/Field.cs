using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TradePlace
{
    [Serializable]
    public class Field
    {
        public string IdField { get; set; }
        public string FieldName { get; set; }
        public string TableName { get; set; }
        public string Type { get; set; }
        public string DataType { get; set; }
        public string Length { get; set; }
        public string Dec { get; set; }
        public bool Required { get; set; }
        public string Order { get; set; }
        public string Extractor { get; set; }
    }
}