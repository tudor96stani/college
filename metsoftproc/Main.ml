open Ast
open Helpers
open Graph
open GraphV2
open Dataflow

(*let () = Printf.printf "\n%s\n" (printNode (toNode ast 1))*)

let () = let (nodes,edges) = createNodeList ast 1 [] [] in 
    List.iter (fun s -> Printf.printf "%s \n" (printNodes2 s)) (sortNodes (nodes));
    Printf.printf "%s\n" (printEdges (sortEdges edges));
    runner nodes edges
