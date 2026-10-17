using System;
using System.Collections.Generic;

namespace Fa
{
    public class State
    {
        public MultiMap<State> TransitionsMultiMap;
       



       // public Dictionary<string, State> Transitions;
       


        public string Name { get; set; }
        public bool IsAccepted { get; set; }

        public State()
        {
            //Transitions = new Dictionary<string, State>();
            TransitionsMultiMap = new MultiMap<State>();
        }
    }
}
