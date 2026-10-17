open GraphV2
open Helpers
open Models


let ins = ref []
let outs = ref []

let rec initEmptyList n = match n with
  | 0 -> []
  | k -> []::(initEmptyList (n-1))

(* Get the list of variables used in the given exp *)
let rec useExp = function 
  | AddInt (e1,e2) | MulInt (e1,e2) | DiffInt (e1,e2) | DivInt (e1,e2) -> removeDuplicates ((useExp e1)@(useExp e2))
  | Var (vn) -> [vn]
  | Int (_) -> []


(* Get the list variables used in the given stmt *)
let rec use stmt = match stmt with
  | AssignStmt (_,e) | PrintStmt (e) -> useExp e
  | WhileStmt (e,_) -> (useExp e)
  | IfStmt (e,_,_) -> useExp e
  | CompStmt (s1,s2) -> removeDuplicates ((use s1)@(use s2))
  | IfJoin -> []

(* Get the list variables defined in the given stmt *)
let rec def stmt = match stmt with
  | AssignStmt (vn,_) -> [vn]
  | WhileStmt (_,_) | IfStmt (_,_,_) | IfJoin | PrintStmt (_) -> []
  | CompStmt (s1,s2) -> removeDuplicates ((def s1)@(def s2))


(* Get the In set for given node *)
let getIn (Node(id,_)) = List.nth !ins (id-1)


(* Get the Out set for given node *)
let getOut (Node(id,_)) = List.nth !outs (id-1)


(* Get the successor nodes for given node *)
let getSucc nodes edges (Node(id,stmt)) = 
  let destinations = List.map (fun (id1,id2) -> id2) (List.filter (fun (id1,id2) -> id1 = id) edges) in
    List.filter (fun (Node(i,s)) -> member i destinations) nodes 


let diff l1 l2 = List.filter (fun x -> not (List.mem x l2)) l1

let rec replaceInList lst el idx cnt = match cnt,lst  with
  | c,h::t when c=idx -> el::replaceInList t el idx (cnt+1)
  | c,h::t -> h::replaceInList t el idx (cnt+1)
  | c,[] -> []

let update nodes edges (Node(id,stmt)) = 
  let newin = replaceInList !ins (List.sort compare (removeDuplicates ((use stmt)@(diff (getOut (Node(id,stmt))) ) (def stmt)))) (id-1) 0 
    and newout = replaceInList !outs (List.sort compare (removeDuplicates (List.flatten (List.map (fun n -> getIn n) (getSucc nodes edges (Node(id,stmt)) ))))) (id-1) 0
      in newin,newout


let updateAll nodes edges = 
  let hasChanged = ref false in
    List.iter
    (fun (Node(id,stmt)) ->
      let newin,newout = update nodes edges (Node (id,stmt)) in
        if newin <> !ins || newout <> !outs 
        then hasChanged := true;
        ins := newin;
        outs := newout
    )
    nodes;
    !hasChanged

let rec printList = function
  | [] -> ".\n"
  | h::t -> h ^ " " ^ (printList t)

let rec printListOfList = function
  | [] -> "\n"
  | h::t -> (printList h) ^ (printListOfList t)



let runner nodes edges =
  let n = List.length nodes in
  ins := initEmptyList n;
  outs := initEmptyList n;
  let changes = ref true in
    changes := updateAll nodes edges;
    while !changes do
      changes := updateAll nodes edges;
    done;
    Printf.printf "INS:\n%s" (printListOfList !ins);
    Printf.printf "OUTS:\n%s" (printListOfList !outs);


