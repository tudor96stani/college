using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using Fa;
namespace Scanner
{
    class MainClass
    {
        public static Dictionary<string, int> Codes;
        public static Dictionary<string, int> SymbolTableIDs = new Dictionary<string, int>();
        public static Dictionary<string, int> SymbolTableConsts = new Dictionary<string, int>();

        public static List<int[]> PIF = new List<int[]>();
        public static int IDidx = 1;
        public static int Constidx = 1;


        //FA
        public static FA Fa_Int_Literals = new FA("fa_integers.txt","Integers");
        public static FA Fa_Float_Literals = new FA("fa_real_full.txt","Floating numbers");
        public static FA Fa_Identifiers = new FA("fa_identifiers.txt","Identifiers");

        public static void Main(string[] args)
        {
           
            string[] lines = ReadInput("input.txt");
            Console.WriteLine("Original source code of program:");
            foreach(var line in lines)
                if(!String.IsNullOrWhiteSpace(line))
                    Console.WriteLine(line);
           
            Codes = InitializeCodesDictionary();
            Console.WriteLine("Codes for the language items:");
            Codes.ForEach(x => Console.WriteLine(x.Key + " " + x.Value));

            /*
            foreach(var atom in lines)
            {
                if (Codes.ContainsKey(atom))
                        PIF.Add(new int[]{Codes[atom]});
                else if (IdentifierCheck(atom))
                    {
                        
                        if (!SymbolTableIDs.ContainsKey(atom))
                            SymbolTableIDs.Add(atom, IDidx++);

                        PIF.Add(new int[] { 1, SymbolTableIDs[atom] });
                    }
                    else if(ConstantCheck(atom))
                    {
                        if (!SymbolTableConsts.ContainsKey(atom))
                            SymbolTableConsts.Add(atom, Constidx++);
                        PIF.Add(new int[] { 2, SymbolTableConsts[atom] });
                }
                else if(String.IsNullOrWhiteSpace(atom)){
                    //just ignore the whitespaces
                }
                else{
                    Console.WriteLine("Error - token not recognized: " + atom);
                }
            }*/
            int lineNumber = 0;
            foreach(var line in lines)
            {
                lineNumber++;
                int i = 0;
                while(i<line.Length)
                {
                    int number;
                    if(int.TryParse(line[i].ToString(),out number))
                    {
                        //current input character is a number -> must be either real or int constant
                        //get the longest prefix from both the integer and the real FA
                        //compare their lengths.
                        //If equal, choose the integer 
                        //If real > int, choose real
                        var restOfLine = line.Substring(i);
                        var int_prefix = LongestPrefix(Fa_Int_Literals, restOfLine);
                        var real_prefix = LongestPrefix(Fa_Float_Literals, restOfLine);
                        if(int_prefix.Length==real_prefix.Length)
                        {
                            if (!SymbolTableConsts.ContainsKey(int_prefix))
                                SymbolTableConsts.Add(int_prefix, Constidx++);
                            PIF.Add(new int[] { 2, SymbolTableConsts[int_prefix] });
                            //skip these characters
                            i += int_prefix.Length;
                        }
                        else if (int_prefix.Length>real_prefix.Length)
                        {
                            if (!SymbolTableConsts.ContainsKey(int_prefix))
                                SymbolTableConsts.Add(int_prefix, Constidx++);
                            PIF.Add(new int[] { 2, SymbolTableConsts[int_prefix] });
                            //skip these characters
                            i += int_prefix.Length;
                        }
                        else
                        {
                            if (!SymbolTableConsts.ContainsKey(real_prefix))
                                SymbolTableConsts.Add(real_prefix, Constidx++);
                            PIF.Add(new int[] { 2, SymbolTableConsts[real_prefix] });
                            //skip these characters
                            i += real_prefix.Length;
                        }
                    }
                    else
                    {
                        //current input is not a number => either an identifier or a keyword or an operator
                        //if the character is #, check if it's followed by "include" (check in the codes for
                        //line.Substring(i,8) 
                        //if character is letter
                        //hand the line to the identifier FA
                        //get the longest prefix
                        //If the prefix matches something in the Codes table, great
                        //If not, look in the symbol table
                        //if the character is not a letter, try looking for "line[i]line[i+1]" and
                        //then "line[i]" in the codes table (might be an operator)
                        if (line[i]=='#')
                        {
                            //check for #include
                            if (line.Substring(i, 8) == "#include")
                            {
                                PIF.Add(new int[] { Codes[line.Substring(i, 8)] });
                                i += 8;
                            }
                        }
                        else if (Char.IsLetter(line[i]))
                        {
                            // the current character is a letter   => keyword or identifier
                            var restOfLine = line.Substring(i);
                            var longestPref = LongestPrefix(Fa_Identifiers, restOfLine);
                            if (Codes.ContainsKey(longestPref))
                            {
                                //the prefix was a keyword
                                PIF.Add(new int[] { Codes[longestPref] });
                            }
                            else
                            {
                                if (longestPref.Length <= 8)
                                {
                                    //the prefix is an identifier
                                    if (!SymbolTableIDs.ContainsKey(longestPref))
                                        SymbolTableIDs.Add(longestPref, IDidx++);

                                    PIF.Add(new int[] { 1, SymbolTableIDs[longestPref] });
                                }
                                else
                                {
                                    Console.WriteLine($"The identifier must be at most 8 characters long: {longestPref} on line {lineNumber}");
                                }
                            }
                            i += longestPref.Length;
                        }
                        //line.Length > i+2 must be checked in case i is line.Length - 1 or -2
                        //(might get an index out of range exception otherwise
                        else if (line.Length > i+2 && Codes.ContainsKey(line.Substring(i, 2)))
                        {
                            //maybe it's a 2 character operator (==,<<)
                            PIF.Add(new int[] { Codes[line.Substring(i, 2)] });
                            i += 2;
                        }
                        else if (Codes.ContainsKey(line[i].ToString()))
                        {
                            //maybe it's a one character operator (=,+)
                            PIF.Add(new int[] { Codes[line[i].ToString()] });
                            i += 1;
                        }
                        else if(String.IsNullOrWhiteSpace(line[i].ToString()))
                        {
                            //just skip the whitespaces
                            i++;   
                        }
                        else
                        {
                            //nothing matched
                            //unrecognized token
                            Console.WriteLine($"Unrecognized token \'{line[i]}\' on line {lineNumber}");
                            i++;
                        }
                    }
                }
            }
            Console.WriteLine("Program internal form:");
            foreach(var line in PIF)
            {
                foreach(var item in line)
                    Console.Write(item + " ");
                Console.WriteLine();
            }
            Console.WriteLine("Symbol table (identifiers):");
            foreach(var s in SymbolTableIDs)
            {
                Console.WriteLine(s.Key + " | " + s.Value);
            }
            Console.WriteLine("Symbol table (constants):");
            foreach (var s in SymbolTableConsts)
            {
                Console.WriteLine(s.Key + " | " + s.Value);
            }
            WriteOutput();
        }

        private static string[] ReadInput(string filename)
        {
            //string pattern = "('[^']*')|\t|\n| |(\\+)|(-)|(%)|(\\*)|(\\/)|(==)|(<=)|(>=)|(!=)|(\\|\\|)|(=)|(\\!)|(<<)|(>>)|(>\\?)|(<)|(>)|(&&)|(\\?)|(\\[)|(])|(\\()|(\\))|(:)|(;)|(,)|(\\.)";
            //Regex rgx = new Regex(pattern);
            using (System.IO.StreamReader file = new System.IO.StreamReader(filename))
            {
                //string[] tokens = rgx.Split(file.ReadToEnd());
                string[] tokens = file.ReadToEnd().Split(new[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);
                return tokens;
            }

        }

        static void WriteOutput()
        {
            string[] pif = PIF.Select(x => String.Join(" ",x)).ToArray();
            string[] symtblids = SymbolTableIDs.Keys.Select(x => x + " " + SymbolTableIDs[x]).ToArray();
            string[] symtblconsts = SymbolTableConsts.Keys.Select(x => x + " " + SymbolTableConsts[x]).ToArray();
            System.IO.File.WriteAllLines("output.txt", pif);
            System.IO.File.WriteAllLines("symtblids.txt", symtblids);
            System.IO.File.WriteAllLines("symtblconsts.txt", symtblconsts);
        }

        private static Dictionary<string, int> InitializeCodesDictionary()
        {
            Dictionary<string, int> result = new Dictionary<string, int>();
            using (System.IO.StreamReader file = new System.IO.StreamReader("Codes.txt"))
            {
                string line;
                while ((line = file.ReadLine()) != null)
                {
                    var tokens = line.Split('?');
                    result.Add(tokens[0], int.Parse(tokens[1]));
                }
            }
            return result;
        }
        /*

        static bool IdentifierCheck(string input)
        {
            Regex rgx = new Regex("(^[a-zA-Z][a-zA-Z0-9_]{0,7}$)");
            return rgx.IsMatch(input);
        }
        static bool ConstantCheck(string input)
        {
            
            Regex rgx = new Regex($"(^[\\+$|^\\-]?[1-9][0-9]*$|^0$|^[\\+$|^\\-]?[1 - 9][0 - 9] *[.][0 - 9][1 - 9] *$|^[\\+$|^\\-] ? 0[.][0 - 9][1 - 9] *$)");
            return rgx.IsMatch(input);
        }*/

       
        static string LongestPrefix(FA fa,string input)
        {
            var longestPrefix = fa.longestPrefix(input);
            if (!fa.accept(longestPrefix))
            {
                Console.WriteLine("Input not accepted by {0} FA: {1}", fa.Name, input);
                return "";
            }
            return longestPrefix;
            //return fa.accept(input) ? longestPrefix : "";
        }
    }
}
