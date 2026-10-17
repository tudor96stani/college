using System;
using System.Linq;
using lab2p1;

namespace Lab2
{
    class MainClass
    {
        public static void Main(string[] args)
        {
            FA fa = new FA();
            fa.readFromFile("fa_identifiers.txt");
            printMenu();
            Console.WriteLine("your option: ");
            var opt = Console.ReadLine();
            while (opt != "0")
            {
                switch (opt)
                {
                    case "1":
                        fa.print();
                        break;
                    case "2":
                        fa.printQ();
                        break;
                    case "3":
                        fa.printE();
                        break;
                    case "4":
                        fa.printD();
                        break;
                    case "5":
                        fa.printF();
                        break;
                    case "6":
                        Console.Write("give sequence:  ");
                        var seq = Console.ReadLine();
                        if (accept(fa, seq))
                            Console.WriteLine("Accepted sequence");
                        else
                        {
                            Console.WriteLine("Not accepted sequence");
                        }
                        break;
                    case "7":
                        Console.Write("give sequence:  ");
                        var seq2 = Console.ReadLine();
                        Console.WriteLine(longestPrefix(fa, seq2));
                        break;
                }
                Console.WriteLine();
                Console.WriteLine("Option: ");
                opt = Console.ReadLine();
            }


            Console.ReadKey();
        }

        static void printMenu()
        {
            Console.WriteLine("1 - the FA");
            Console.WriteLine("2 - the set of states");
            Console.WriteLine("3 - the alphabet");
            Console.WriteLine("4 - the transition function");
            Console.WriteLine("5 - the set of final states");
            Console.WriteLine("6 - verify if sequence is accepted");
            Console.WriteLine("7 - find the longest prefix accepted");
            Console.WriteLine("0 - exit");
        }



        static bool accept(FA fa, string seq)
        {
            var currentState = fa.q0;
            var currState = fa.StartState;
            var transitions = fa.d;
            var finalStates = fa.F;
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
                if(!currState.TransitionsMultiMap.Keys.Contains(rule3)){
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

        static string longestPrefix(FA fa,string seq){
            var currentState = fa.q0;
            var currState = fa.StartState;
            var transitions = fa.d;
            var finalStates = fa.F;
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
