using System;
using System.Collections.Generic;
using System.Linq;

namespace lab5.SyntacticAnalyzer
{
    public class ParseTable
    {
        public Dictionary<string, string> Table = new Dictionary<string, string>();
        public Dictionary<string, HashSet<string>> Firsts;
        public Dictionary<string, HashSet<string>> Follows;
        public ParseTable(Grammar g)
        {
            Firsts = Utils.First(g);
            Follows = Utils.Follow(g, Firsts);
            Init(g);
        }

        private void Init(Grammar g)
        {

            foreach (var rule in g.ProductionRules)
            {
                var firstOfAlpha = Utils.FirstOf(rule.Key.Right[0], Firsts, rule.Key);
                foreach (var f in firstOfAlpha.Where(x => !x.Contains(Utils.EPSILON)))
                {
                    Table.Add(rule.Key.Left + "," + f, String.Join(" ", rule.Key.Right) + " " + rule.Value);
                }

                if (firstOfAlpha.Contains(Utils.EPSILON))
                {
                    var followOfLHS = Follows[rule.Key.Left];
                    foreach (var fo in followOfLHS)
                    {
                        Table.Add(rule.Key.Left + "," + fo, String.Join(" ", rule.Key.Right) + " " + rule.Value);
                    }
                }
            }

            foreach (var a in g.Terminals.Where(x => !x.Contains(Utils.EPSILON)))
            {
                Table.Add(a + "," + a, "pop");
            }
            Table.Add("$,$", "acc");
        }



        public void PrintTable()
        {
            Console.WriteLine("\nPARSING TABLE:");
            foreach (var kvp in Table)
            {
                Console.WriteLine("("+kvp.Key + ")=" + kvp.Value);
            }
        }
    }
}
