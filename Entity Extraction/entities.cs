using System;
using System.Collections.Generic;

namespace Extracted.Entities
{
    public class Disambiguated
    {
        public List<object> subType { get; set; }
        public string name { get; set; }
        public string website { get; set; }
        public string dbpedia { get; set; }
        public string freebase { get; set; }
        public string opencyc { get; set; }
        public string yago { get; set; }
        public string crunchbase { get; set; }
    }

    public class Quotation
    {
        public string quotation { get; set; }
    }

    public class Entity
    {
        public string type { get; set; }
        public string relevance { get; set; }
        public string count { get; set; }
        public string text { get; set; }
        public Disambiguated disambiguated { get; set; }
        public List<Quotation> quotations { get; set; }
    }

    public class RootObject
    {
        public string status { get; set; }
        public string usage { get; set; }
        public string url { get; set; }
        public string language { get; set; }
        public List<Entity> entities { get; set; }
    }
}