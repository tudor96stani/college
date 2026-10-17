using System;
using System.Collections.Generic;

namespace lab5.SyntacticAnalyzer
{
    public class ProductionRule
    {
        public string Left { get; set; }
        public List<string> Right { get; set; }
        public ProductionRule()
        {
        }
    }
}
