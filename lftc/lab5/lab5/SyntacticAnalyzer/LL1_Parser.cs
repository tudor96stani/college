using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace lab5.SyntacticAnalyzer
{
    public class LL1_Parser
    {
        public Grammar G { get; set; } 
        public ParseTable Table { get; set; } 
        public Stack<string> Stack { get; set; }
        public List<string> Output { get; set; }
        public List<string> InputSequence { get; set; }
        public LL1_Parser()
        {
            try
            {
                G = new Grammar("Grammar.txt");
                Table = new ParseTable(G);
                PrintFirstAndFollow(Table.Firsts,Table.Follows);
                Stack = new Stack<string>();
                Stack.Push("$");
                Stack.Push(G.StartSymbol);
                Output = new List<string>();
            }
            catch (ArgumentException ex)
            {
                var fi = Utils.First(G);
                var fol = Utils.Follow(G, fi);
                PrintFirstAndFollow(fi,fol);
                throw ex;
            }

        }

        private void PrintFirstAndFollow(Dictionary<string,HashSet<string>> Firsts,Dictionary<string,HashSet<string>> Follows)
        {
            Console.WriteLine("First(1) for each terminal:");
            foreach (var kvp in Firsts.Where(x=>G.Nonterminals.Contains(x.Key)))
            {
                Console.Write(kvp.Key + ":");
                foreach (var e in kvp.Value)
                {
                    Console.Write(e + " ");
                }
                Console.WriteLine();
            }
            Console.WriteLine("Follow(1) for each terminal:");
            foreach (var kvp in Follows)
            {
                Console.Write(kvp.Key + ":");
                foreach (var e in kvp.Value)
                {
                    Console.Write(e + " ");
                }
                Console.WriteLine();
            }
        }

        public void PrintCurrentConfiguration()
        {
            //Console.Write("("+String.Join("",InputSequence)+",");
            foreach(var stackElem in Stack)
            {
                Console.Write(stackElem+" ");
            }
            Console.Write(",");
            if (Output.Count == 0)
                Console.Write(Utils.EPSILON);
            else
            {
                foreach (var outputElem in Output)
                {
                    Console.Write(outputElem + " ");
                }
            }
            Console.Write(")|-");
        }

        public bool ParseSequence(string sequence)
        {
            InputSequence = sequence.Select(x => x.ToString()).ToList();
            InputSequence.Add("$");
            PrintCurrentConfiguration();
            bool working = true;
            while(working)
            {
                string tableEntry;
                var A = Stack.Peek();
                var a = InputSequence[0];
                if(Table.Table.TryGetValue(A[0]+","+a,out tableEntry))
                {
                    //we have a value that is either a (Rule,number) pair, pop or acc
                    if(tableEntry=="acc")
                    {
                        working = false;
                        Console.Write("ACC");
                        break;
                    }
                    else if(tableEntry=="pop")
                    {
                        Console.Write($"(pop {InputSequence[0]})");
                        InputSequence = InputSequence.Skip(1).ToList();
                        Stack.Pop();
                        working = true;

                        PrintCurrentConfiguration();
                    }
                    else
                    {
                        Console.Write("(push)");
                        var elements = tableEntry.Split(' ');
                        var alpha = elements.Take(elements.Length - 1).ToList();
                        var i = elements[elements.Length-1];
                        Stack.Pop();
                        alpha.Reverse();
                        foreach(var ch in alpha)
                            if(ch.ToString()!=Utils.EPSILON)
                                Stack.Push(ch);
                        PrintCurrentConfiguration();
                    }
                }
                else
                {
                    //error
                    working = false;
                    Console.WriteLine($"\nERROR\nUnexpected token {a}.");
                    return false;
                }
            }
            return true;
        }

        public bool Parse(List<string> PIF)
        {
            PIF.Add("$");
            PrintCurrentConfiguration();
            bool working = true;
            while (working)
            {
                string tableEntry;
                var A = Stack.Peek();
                var a = PIF[0];
                if (Table.Table.TryGetValue(A + "," + a, out tableEntry))
                {
                    //we have a value that is either a (Rule,number) pair, pop or acc
                    if (tableEntry == "acc")
                    {
                        working = false;
                        Console.Write("ACC");
                        break;
                    }
                    else if (tableEntry == "pop")
                    {
                        Console.Write($"(pop {PIF[0]})");
                        PIF = PIF.Skip(1).ToList();
                        Stack.Pop();
                        working = true;

                        PrintCurrentConfiguration();
                    }
                    else
                    {
                        Console.Write("(push)");
                        var elements = tableEntry.Split(' ');
                        var alpha = elements.Take(elements.Length - 1).ToList();
                        var i = elements[elements.Length - 1 ];
                        Stack.Pop();
                        alpha.Reverse();
                        //Array.Reverse(charArrayOfAlpha);
                        foreach (var ch in alpha)
                            if (ch != Utils.EPSILON)
                                Stack.Push(ch);
                        Output.Add(i);
                        PrintCurrentConfiguration();
                    }
                }
                else
                {
                    //error
                    working = false;
                    Console.WriteLine($"\nERROR\nUnexpected token {a}.");
                    return false;
                }
            }
            return true;
        }

        public void PrintDerivationString()
        {
            Console.WriteLine("\n"+G.StartSymbol+"(0)=>");
            var tail = new List<string>();
            foreach(var ruleNumber in Output)
            {
                var rule = G.ProductionRules.FirstOrDefault(x => x.Value == int.Parse(ruleNumber));
                //tail = rule.Key.Right;
                Console.Write(String.Join(" ",rule.Key.Right)+" "+String.Join(" ",tail)+$" ({rule.Value})=>");
                Console.WriteLine();
                tail = rule.Key.Right.Skip(1).ToList();
            }
        }
    }
}
