using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using Lab2;


namespace lab2p1
{
    class FA
    {
        public HashSet<string> Q;
        public HashSet<string> E;
        public Dictionary<string, string> d;
        public string q0;
        public HashSet<string> F;

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

        public void generateFA(int nrStates)
        {
            HashSet<string> Q = new HashSet<string>();
            HashSet<string> E = new HashSet<string>();
            Dictionary<string, string> d= new Dictionary<string, string>();
            //string q0="q0";
            HashSet<string> F = new HashSet<string>();

            for (int i = 0; i < 10; i++)
                E.Add(i.ToString());
            E.Add("-");
            for (int i = 0; i <= nrStates; i++)
            {
                var state = "q" + i;
                Q.Add(state);
            }
            for (int i = 1; i <= nrStates; i++)
            {
                var state = "q" + i;
                F.Add(state);
            }
            int x = 0, j=1;
            while(x!=nrStates && j <= nrStates)
            {
                
                    for (int k = 0; k < 10; k++)
                    {
                        var key = "d(q" + x + ',' + k + ")";
                        var value =  "=q" + j;
                        d.Add(key,value);
                    }
                    
                
                j++;
                x++;
            }
            
            d.Add("d(q0,-)","=q1");
            StringBuilder sb = new StringBuilder();
            foreach (var q in Q)
            {
                sb.Append(q);
                sb.Append(" ");
            }
            sb.Append("\n");
            foreach (var e in E)
            {
                sb.Append(e);
                sb.Append(" ");
            }
            sb.Append("\n");
            sb.Append("q0");
            sb.Append("\n");
            foreach (var f in F)
            {
                sb.Append(f);
                sb.Append(" ");
            }
            
            foreach (var q in d)
            {
                var str = q.Key + q.Value;
                sb.Append("\n");
                sb.Append(str);
                
            }
            
            string[] lines = {sb.ToString()};
            File.WriteAllLines("fa3.txt",lines);

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
                        Console.WriteLine($"d({q.Name},{tr.Split(',')[0]})={res.Name}");
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
    }
}
