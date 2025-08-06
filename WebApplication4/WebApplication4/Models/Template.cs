using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TradePlace
{
    public class Template
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string TableName { get; set; }
        public string Type { get; set; }
        public string DataType { get; set; }
        public string Length { get; set; }
        public string LengthDecimal { get; set; }
        public string Required { get; set; }
        public string Order { get; set; }
        public string Extractor { get; set; }
    }
}