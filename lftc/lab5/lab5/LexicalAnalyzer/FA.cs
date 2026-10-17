using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;


namespace lab5.LexicalAnalysis
{
    public class FA
    {
        public HashSet<string> Q;
        public HashSet<string> E;
        public Dictionary<string, string> d;
        public string q0;
        public HashSet<string> F;
        public string Name { get; set; }
        public State StartState;
        public HashSet<State> States;
        public HashSet<State> FinalStates;


        public FA()
        {
            Q = new HashSet<string>();      //set of states
            E = new HashSet<string>();      //alphabet
            d = new Dictionary<string, string>();      //transition function
            F = new HashSet<string>();      //set of final states
            FinalStates = new HashSet<State>();
            States = new HashSet<State>();
        }

        public FA(string filename,string name): this()
        {
            this.Name = name;
            readFromFile(filename);
        }

       


        public void readFromFile(string file)
        {
            StreamReader r = new StreamReader(file);
            string line;
            foreach (var q in r.ReadLine().Split(' '))
            {
                Q.Add(q);
                States.Add(new State() { Name = q });
            }
            foreach (var e in r.ReadLine().Split(' '))
                E.Add(e);
            q0 = r.ReadLine();   //initial state
            this.StartState = this.States.FirstOrDefault(x => x.Name == q0);
            foreach (var f in r.ReadLine().Split(' '))
            {
                F.Add(f);
                this.FinalStates.Add(this.States.FirstOrDefault(x => x.Name == f));
            }
            while ((line = r.ReadLine()) != null)
            {
                var elements = line.Split(',');//the line is qi,a,qj
                var start = elements[0];//qi
                var a = elements[1];//a
                var end = elements[2];//qj
                //d.Add(line.Split('=')[0], line.Substring(line.Length-2));

                //the Transitions dictionary for a state has the form:
                // key = qi,a
                // value = qj
                //this.States.FirstOrDefault(x => x.Name == start).Transitions.Add(start+","+a, this.States.FirstOrDefault(x=>x.Name==end));
                if(this.States.FirstOrDefault(x => x.Name == start).TransitionsMultiMap.Keys.Contains(start + "," + a)){
                    
                    Console.WriteLine("WARNING: Non deterministic finite automaton!");
                }
                this.States.FirstOrDefault(x => x.Name == start).TransitionsMultiMap.Add(start + "," + a, this.States.FirstOrDefault(x => x.Name == end));
            }
            r.Close();
        }

        public void print()
        {
            Console.WriteLine("G = (Q, E, d, q0, F)");
            Console.WriteLine();
            Console.WriteLine("Set of states : ");
            /*
            foreach (var q in Q)
            {
                Console.Write(q + " ");

            }*/

            foreach(var q in this.States){
                Console.WriteLine(q.Name);
                foreach(var tr in q.TransitionsMultiMap.Keys){
                    //Console.WriteLine($"d({q.Name},{tr.Key.Split(',')[0]})={tr.Value.Name}");
                    //Console.WriteLine($"d({q.Name},{tr.Split(',')[0]})={q.TransitionsMultiMap[tr][0].Name}");
                    foreach(var res in q.TransitionsMultiMap[tr]){
                        Console.WriteLine($"d({q.Name},{tr.Split(',')[1]})={res.Name}");
                    }
                }
            }
            Console.WriteLine();
            Console.Write("Alphabet : ");
            foreach (var e in E)
                Console.Write(e + " ");
            Console.WriteLine();
            Console.WriteLine("Transition function : ");
            //foreach (var i in d)
            //  Console.WriteLine(i.Key + '=' + i.Value);
            foreach(var q in States){
                foreach (var tr in q.TransitionsMultiMap.Keys)
                {
                    //Console.WriteLine($"d({q.Name},{tr.Key.Split(',')[0]})={tr.Value.Name}");
                    //Console.WriteLine($"d({q.Name},{tr.Split(',')[0]})={q.TransitionsMultiMap[tr][0].Name}");
                    foreach (var res in q.TransitionsMultiMap[tr])
                    {
                        Console.WriteLine($"d({q.Name},{tr.Split(',')[1]})={res.Name}");
                    }
                }
            }
            Console.WriteLine("Initial state : " + this.StartState.Name);
            Console.Write("Final states : ");
            foreach (var f in F)
                Console.Write(f + " ");
        }

        public void printQ()
        {
            /*Console.Write("Set of states : ");
            foreach (var q in Q)
                Console.Write(q + " ");*/
            foreach (var q in this.States)
            {
                Console.WriteLine(q.Name);
                foreach (var tr in q.TransitionsMultiMap.Keys)
                {
                    //Console.WriteLine($"d({q.Name},{tr.Key.Split(',')[0]})={tr.Value.Name}");
                    //Console.WriteLine($"d({q.Name},{tr.Split(',')[0]})={q.TransitionsMultiMap[tr][0].Name}");
                    foreach (var res in q.TransitionsMultiMap[tr])
                    {
                        Console.WriteLine($"d({q.Name},{tr.Split(',')[1]})={res.Name}");
                    }
                }
            }
        }

        public void printE()
        {
            Console.Write("Alphabet : ");
            foreach (var e in E)
                Console.Write(e + " ");
        }

        public void printD()
        {
            /*Console.WriteLine("Transition function : ");
            foreach (var i in d)
                Console.WriteLine(i.Key + '=' + i.Value);*/
            foreach (var q in States)
            {
                foreach (var tr in q.TransitionsMultiMap.Keys)
                {
                    //Console.WriteLine($"d({q.Name},{tr.Key.Split(',')[0]})={tr.Value.Name}");
                    //Console.WriteLine($"d({q.Name},{tr.Split(',')[0]})={q.TransitionsMultiMap[tr][0].Name}");
                    foreach (var res in q.TransitionsMultiMap[tr])
                    {
                        Console.WriteLine($"d({q.Name},{tr.Split(',')[1]})={res.Name}");
                    }
                }
            }
        }

        public void printF()
        {
            Console.Write("Final states : ");
            foreach (var f in F)
                Console.Write(f + " ");
        }

        public bool accept(string seq)
        {
            var currentState = this.q0;
            var currState = this.StartState;
            var transitions = this.d;
            var finalStates = this.F;
            string prefix = "";
            int i = 0;


            //if (seq.Length != 1 && seq[0] is '0')
            //  return false;
            //if (seq[0] is '-' && seq[1] is '0')
            //  return false;
            while (i != seq.Length)
            {

                //var rule2 = "d(" + currentState + "," + seq[i] + ")";


                /*
                if (!transitions.ContainsKey(rule2))
                {
                    Console.WriteLine("longest prefix: " + prefix);
                    return false;
                }*/
                var rule3 = currState.Name + "," + seq[i];
                //Console.WriteLine(rule3);
                if (!currState.TransitionsMultiMap.Keys.Contains(rule3))
                {
                    //Console.WriteLine("longest prefix: " + prefix);
                    return false;
                }

                //currentState = transitions[rule2];
                //currState = currState.Transitions[rule3];
                if (currState.TransitionsMultiMap[rule3].Count > 1)
                {
                    /*
                    var rand = new Random();
                    int next = rand.Next(0, currState.TransitionsMultiMap[rule3].Count - 1);
                    currState = currState.TransitionsMultiMap[rule3][next];*/
                    Console.WriteLine("Error! Non deterministic FA!");
                    return false;
                }
                else
                {
                    currState = currState.TransitionsMultiMap[rule3][0];
                }
                prefix += seq[i];
                i++;

                //Console.WriteLine(prefix);

            }


            if (!finalStates.Contains(currState.Name))
            {
                //Console.WriteLine("longest prefix: " + prefix);
                return false;
            }
            //Console.WriteLine("longest prefix: " + prefix);
            return true;
        }

        public string longestPrefix( string seq)
        {
            var currentState = this.q0;
            var currState = this.StartState;
            var transitions = this.d;
            var finalStates = this.F;
            string prefix = "";
            int i = 0;
            while (i != seq.Length)
            {
                var rule3 = currState.Name + "," + seq[i];
                if (!currState.TransitionsMultiMap.Keys.Contains(rule3))
                {
                    return prefix;
                }
                if (currState.TransitionsMultiMap[rule3].Count > 1)
                {
                    /*
                    var rand = new Random();
                    int next = rand.Next(0, currState.TransitionsMultiMap[rule3].Count - 1);
                    currState = currState.TransitionsMultiMap[rule3][next];*/
                    return "Error: non deterministic FA!";
                }
                else
                {
                    currState = currState.TransitionsMultiMap[rule3][0];
                }
                prefix += seq[i];
                i++;
            }
            return prefix;
        }
    }
}
