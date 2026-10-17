using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace lab5.SyntacticAnalyzer
{
    public static class Utils
    {
        public static string EPSILON = "ε";

        public static HashSet<string> PlusCircle(List<string> l1, List<string> l2)
        {

            if (l1.Count > 0 && !l1.Contains(EPSILON))
            {
                return new HashSet<string>(l1.Distinct().ToList());
            }
            var l1Set = l1.Where(x => x != EPSILON).Select(x => x[0].ToString()).Distinct().ToList();
            var l2Set = l2.Where(x => x != EPSILON).Select(x => x[0].ToString()).Distinct().ToList();
            if (l1.Contains(EPSILON) && l2.Contains(EPSILON))
                l1Set.Add(EPSILON);
            return new HashSet<string>(l1Set.Union(l2Set));
        }


        //compute the First table
        //The key of the dictionary is the NonTerminal
        //The value is the set of terminals
        public static Dictionary<string, HashSet<string>> First(Grammar g)
        {

            var fi = new Dictionary<string, HashSet<string>>();


            foreach (var a in g.Terminals)
            {
                fi.Add(a + 0.ToString(), new HashSet<string>() { a });
            }
            foreach (var A in g.Nonterminals)
            {
                bool added = false;
                if (g.ProductionRules.Any(x => x.Key.Left.Equals(A) && g.Terminals.Contains(x.Key.Right[0])))
                {
                    //we have the case A->aX
                    //F0(A)={a}
                    var rule = g.ProductionRules.FirstOrDefault(x => x.Key.Left.Equals(A) && g.Terminals.Contains(x.Key.Right[0]));
                    fi.Add(A + 0.ToString(), new HashSet<string>() { rule.Key.Right[0] });
                    added = true;
                }
                if (g.ProductionRules.Any(x => x.Key.Left.Equals(A) && x.Key.Right.Contains(EPSILON)))
                {
                    if (fi.Keys.Contains(A + 0.ToString()))
                    {
                        fi[A + 0.ToString()].Add(EPSILON);
                    }
                    else
                    {
                        fi.Add(A + 0.ToString(), new HashSet<string>() { EPSILON });
                    }
                    added = true;
                }
                if (!added)
                {
                    fi.Add(A + 0.ToString(), new HashSet<string>());
                }
            }
            int i = 0; bool changed = false; int tries = 2;
            do
            {
                changed = false;
                i++;
                var tempFi = new Dictionary<string, HashSet<string>>();
                foreach (var A in g.Nonterminals)
                {
                    var rules = g.ProductionRules.Where(x => x.Key.Left == A);
                    foreach (var rule in rules)
                    {

                        var firstOfEachTermInRule = new List<HashSet<string>>();
                        var rightRuleWhere = rule.Key.Right;
                        foreach (var rightElem in rightRuleWhere)
                        {
                            if (g.Nonterminals.Contains(rightElem))
                            {
                                HashSet<string> outV;
                                int iCopy = i - 1;
                                while (!fi.TryGetValue(rightElem + iCopy.ToString(), out outV))
                                {
                                    iCopy--;
                                }
                                if (!firstOfEachTermInRule.Any(x => x.SetEquals(outV)))
                                {
                                    firstOfEachTermInRule.Add(outV);
                                }
                            }
                            else
                            {
                                HashSet<string> outV;
                                int iCopy = i - 1;
                                while (!fi.TryGetValue(A + iCopy.ToString(), out outV))
                                {
                                    iCopy--;
                                }

                                firstOfEachTermInRule.Add(new HashSet<string>() { rightElem });
                            }
                        }
                        if (firstOfEachTermInRule.Count > 0)
                        {
                            var sum = firstOfEachTermInRule[0];
                            for (int t = 1; t < firstOfEachTermInRule.Count(); t++)
                            {
                                sum = PlusCircle(sum.ToList(), firstOfEachTermInRule[t].ToList());
                            }

                            HashSet<string> outV;
                            int iCopy = i;
                            while (!fi.TryGetValue(A + iCopy.ToString(), out outV))
                            {
                                iCopy--;
                            }
                            if (sum.Count != 0)
                            {
                                if (!outV.SetEquals(sum))
                                {
                                    if (fi.ContainsKey(A + i.ToString()))
                                    {
                                        fi[A + i.ToString()] = new HashSet<string>(sum.Union(fi[A + i.ToString()]));

                                        changed = true;
                                    }
                                    else
                                    {
                                        HashSet<string> outV2;
                                        int iCopy2 = i - 1;
                                        while (!fi.TryGetValue(A + iCopy2.ToString(), out outV2))
                                        {
                                            iCopy2--;
                                        }
                                        if (!outV2.SetEquals(sum))
                                        {
                                            //Verify if sum is a subset of outV2 and if their difference is not {EPSILON}
                                            if (sum.Any(x => !outV2.Contains(x)) && !sum.Except(outV2).All(x => x == EPSILON))
                                            {
                                                fi.Add(A + i.ToString(), new HashSet<string>(sum.Union(outV2)));
                                                changed = true;
                                            }
                                        }
                                    }
                                }
                            }

                        }

                    }

                }
                if (!changed)
                    tries--;
            } while (changed || tries >= 0);
            var result = new Dictionary<string, HashSet<string>>();
            foreach (var nont in g.Nonterminals)
            {
                HashSet<string> outV2;
                int iCopy2 = i - 1;
                while (!fi.TryGetValue(nont + iCopy2.ToString(), out outV2))
                {
                    iCopy2--;
                }
                result.Add(nont, new HashSet<string>(fi[nont + iCopy2.ToString()]));
            }

            foreach (var term in g.Terminals)
            {
                HashSet<string> outV2;
                int iCopy2 = i - 1;
                while (!fi.TryGetValue(term + iCopy2.ToString(), out outV2) && iCopy2 > 0)
                {
                    iCopy2--;
                }
                result.Add(term, outV2);
            }
            return result;
        }
        //Determine the first set for sym, where sym is the first element in the RHS of the rule
        public static HashSet<string> FirstOf(string sym, Dictionary<string, HashSet<string>> firsts, ProductionRule rule)
        {

            var first = firsts[sym];
            if (first.Count() == 0 || first.Any(x => x == EPSILON))
            {
                foreach (var rightElem in rule.Right.Where(x => x != sym))
                {
                    first = new HashSet<string>(first.Union(firsts[rightElem]));
                    if (first.Count() == 0 || firsts[rightElem].Any(x => x == EPSILON))
                        continue;
                    else
                    {
                        first.Remove(EPSILON);
                        break;
                    }
                }
            }
            return first;
        }
        //Determine the first set for sym, where sym is followed in the RHS of the production rule by tail
        public static HashSet<string> FirstOfWithoutRule(string sym, Dictionary<string, HashSet<string>> firsts, List<string> tail)
        {
            var first = firsts[sym];
            //if there is an epsilon in the First set for the sym,
            //take the elements in the tail one by one, until one that doesn't contain epsilon is found.
            //when one that does not contain epsilon is found 
            //remove epsilon from the set that is going to be returned and stop processing the tail
            //if no such symbol is found, epsilon will remain in the tail

            if (first.Count() == 0 || first.Any(x => x == EPSILON))
            {
                for (int i = 0; i < tail.Count(); i++)
                {
                    first = new HashSet<string>(first.Union(firsts[tail[i]]));
                    if (first.Count() == 0 || firsts[tail[i]].Any(x => x == EPSILON))
                    {
                        continue;
                    }
                    else
                    {
                        first.Remove(EPSILON);
                        break;
                    }
                }
            }
            return first;
        }

        //Compute the follow sets for all the non terminals in the grammar
        public static Dictionary<string, HashSet<string>> Follow(Grammar g, Dictionary<string, HashSet<string>> Firsts)
        {
            Dictionary<string, HashSet<string>> res = new Dictionary<string, HashSet<string>>();
            res.Add(g.StartSymbol, new HashSet<string>() { "$" });
            foreach (var A in g.Nonterminals.Where(x => x != g.StartSymbol))
                res.Add(A, new HashSet<string>());
            bool changed = false;
            int tries = 2;
            do
            {
                changed = false;
                foreach (var A in g.Nonterminals)
                {
                    var rules = g.ProductionRules.Where(x => x.Key.Right.Contains(A));
                    foreach (var rule in rules)
                    {
                        //X -> alpha B beta
                        var listOfIndexes = rule.Key.Right.Select((x, i) => new { x, i })
                            .Where(x => x.x == A)
                            .Select(x => x.i);
                        foreach (var i in listOfIndexes)
                        {
                            if (i < rule.Key.Right.Count() - 1)
                            {
                                var firstOfBeta = new HashSet<string>();
                                var beta = rule.Key.Right.Skip(i + 1).ToList();
                                firstOfBeta = FirstOfWithoutRule(beta[0], Firsts, beta.Skip(1).ToList());

                                //Follow(A) = Follow(A) U First(beta)
                                var reunion = new HashSet<string>(res[rule.Key.Right[i]].Union(firstOfBeta.Where(x => x != EPSILON)));
                                if (!res[rule.Key.Right[i]].SetEquals(reunion))
                                {
                                    changed = true;
                                    res[rule.Key.Right[i]] = reunion;
                                }
                                //if ε is in First(beta), Follow(A) = Follow(A) U Follow(X)
                                if (firstOfBeta.Contains(EPSILON))
                                {
                                    var reunion2 = new HashSet<string>(res[rule.Key.Right[i]].Union(res[rule.Key.Left]));

                                    if (!res[rule.Key.Right[i]].SetEquals(reunion2))
                                    {
                                        changed = true;
                                        res[rule.Key.Right[i]] = reunion2;
                                    }
                                }
                            }
                            else
                            {
                                //this is the case where X -> alpha B (B is the last symbol in the RHS of the rule)
                                var reunion2 = new HashSet<string>(res[rule.Key.Right[i]].Union(res[rule.Key.Left]));
                                if (!res[rule.Key.Right[i]].SetEquals(reunion2))
                                {
                                    changed = true;
                                    res[rule.Key.Right[i]] = reunion2;
                                }
                            }
                        }
                    }
                }
                if (!changed)
                    tries--;
            } while (changed || tries >= 0);
            return res;
        }
    }
}
