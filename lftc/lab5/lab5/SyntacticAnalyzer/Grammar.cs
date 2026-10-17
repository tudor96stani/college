using System;
using System.Collections.Generic;
using System.Linq;

namespace lab5.SyntacticAnalyzer
{
    public class Grammar
    {
        public HashSet<string> Nonterminals { get; set; }
        public HashSet<string> Terminals { get; set; }
        public Dictionary<ProductionRule, int> ProductionRules { get; set; }
        public string StartSymbol { get; set; }
        private int count = 1;
        public Grammar(string filename)
        {
            Terminals = new HashSet<string>();
            Nonterminals = new HashSet<string>();
            ProductionRules = new Dictionary<ProductionRule, int>();
            ReadFromFile(filename);
        }

        public void ReadFromFile(string filename)
        {
            string[] lines = System.IO.File.ReadAllLines("../../Input/"+filename);
            Nonterminals = new HashSet<string>(lines[0].Split(' '));
            Terminals = new HashSet<string>(lines[1].Split(' '));
            StartSymbol = lines[2];
            foreach(var line in lines.Skip(3))
            {
                string[] elements = line.Split('?');
                string[] right = elements[1].Split(' ');
                var produRule = new ProductionRule() { Left = elements[0], Right = right.ToList() };
                ProductionRules.Add(produRule, count++);
            }

            Console.WriteLine("Terminals:");
            foreach(var t in Terminals)
                Console.Write(t+" ");
            Console.WriteLine("\nNonterminals");
            foreach (var t in Nonterminals)
                Console.Write(t + " ");
            Console.WriteLine("\nProduction rules");
            foreach(var rule in ProductionRules)
            {
                Console.Write(rule.Key.Left+" -> "+String.Join(" ",rule.Key.Right) +" ("+ rule.Value+")");
                Console.WriteLine();
            }
        }
    }
}
