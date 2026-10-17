open Models
open Helpers

type nodet = Node of int*stmt
type graph = (nodet list)*((int*int) list)

let rec printEdges = function
  | [] -> "\n"
  | (l,r)::t -> (string_of_int l) ^ " -> " ^ (string_of_int r) ^ "\n" ^ (printEdges t)


let rec printStmt = function
  | AssignStmt (vn,e) -> vn ^ "=" ^ (printExp e) ^ "\n"
  | PrintStmt (e) -> "Print(" ^ (printExp e) ^ ")\n"
  | IfStmt (e,s1,s2) -> "if " ^ (printExp e) ^ " then\n" ^ (printStmt s1) ^ " else\n" ^ (printStmt s2) ^ "\n"
  | WhileStmt (e,s) -> "while " ^ (printExp e) ^ " then\n" ^ (printStmt s) ^ "\n"
  | CompStmt (s1,s2) -> (printStmt s1) ^ " " ^ (printStmt s2) ^ "\n"
  | IfJoin -> "endif\n"

let printNodes2 = function
  | Node (id,stmt) -> (string_of_int id) ^ "  " ^ (printStmt stmt)

let maxOfList = function
    [] -> invalid_arg "empty list"
  | x::xs -> List.fold_left max x xs

let minOfList = function
  [] -> invalid_arg "empty list"
  | x::xs -> List.fold_left min x xs

let rec transformList = function
  | [] -> []
  | Node (id,_) :: t -> id::(transformList t) 

let getMaxId nodeList = maxOfList (transformList nodeList)

let getMinId nodeList = minOfList (transformList nodeList)

let rec createNodeList ast id edges lst = match ast with 
  | AssignStmt (vn,e) -> (Node (id,AssignStmt (vn,e))::lst),edges
  | PrintStmt (e) -> (Node (id,(PrintStmt (e)))::lst),edges
  | WhileStmt (e,s) -> 
    (
      let nWhile = [Node (id,WhileStmt (e,s))] in 
        let n1,e1 = createNodeList s (id+1) [] [] in  (*edges (Node (id,WhileStmt (e,s))::lst) in*)
          nWhile@n1, [((getMaxId n1),id)]@[id,id+1]@e1
    )
  | IfStmt (e,s1,s2) ->
    (*(
      let ifNode = (Node  (id,IfStmt (e,s1,s2))::lst) in
        let (firstNode,edges2) = createNodeList s1 (id+1) edges ifNode in
          let minId = getMinId firstNode in
            let sndNode,edges3 = createNodeList s2 ((getMaxId firstNode)+1) ((id,minId)::edges2) firstNode
              in (sndNode,edges3)

    ) *)
    (
      let nIf = [Node (id,IfStmt (e,s1,s2))] in
        let n1,e1 = createNodeList s1 (id+1) [] [] in
          let n2,e2 = createNodeList s2 ((getMaxId n1) + 1) [] [] in
            let eIf = [(id,(getMinId n1)); (id, (getMinId n2))] in
              let ifJoin = [Node (((getMaxId n2) + 1),IfJoin)] in 
                ifJoin@n2@n1@nIf, [(getMaxId n1),((getMaxId n2) + 1);(getMaxId n2),((getMaxId n2) + 1)]@eIf@e2@e1 
    )
  | CompStmt (s1,s2) -> (*createNodeList s2 (id+1) ((createNodeList s1 id lst))*)
    (*(
      Printf.printf ">>%s" (printEdges edges);
      let (firstNode,edges2) = createNodeList s1 id edges lst in
        let minId = getMinId firstNode in
          Printf.printf "Min=%d\n" minId;
          let (sndNode, edges3) = createNodeList s2 ((getMaxId firstNode)+1) edges2 firstNode in
            List.iter (fun s -> Printf.printf "%s" (printNodes2 s)) sndNode;
            let maxId = getMaxId sndNode in 
              Printf.printf "Max=%d\n\n" maxId;
              sndNode, ((minId,maxId)::edges3)
    )*)
    (
      let n1,e1 = createNodeList s1 id [] [] in
        let n2,e2 = createNodeList s2 ((getMaxId n1)+1) [] [] in
          n2@n1, e1@e2@[getMaxId n1,getMinId n2]
    )
    | IfJoin -> lst,edges (*We know we will never find this type of 
                  stmt in the AST, it's just there to help us with the CFG *)

let compareNodes n1 n2 = match n1, n2 with
  | Node (id1,_), Node (id2, _) -> compare id1 id2

let compareEdges e1 e2 = match e1,e2 with
  | (f1,_), (f2,_) -> compare f1 f2

let sortEdges edgeList = List.sort compareEdges edgeList

let sortNodes nodeList = List.sort compareNodes nodeList