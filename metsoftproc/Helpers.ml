open Graph;;
open Models;;

let rec printExp = function
  | Int (v) -> string_of_int v
  | Var (vn) -> vn
  | AddInt (e1,e2) -> (printExp e1) ^ "+" ^ (printExp e2)
  | MulInt (e1,e2) -> (printExp e1) ^ "*" ^ (printExp e2)
  | DiffInt (e1,e2) -> (printExp e1) ^ "-" ^ (printExp e2)
  | DivInt (e1,e2) -> (printExp e1) ^ "/" ^ (printExp e2)

let rec printNode node = match node with 
  | AssignNode (id,vn,e,next) -> "[" ^ (string_of_int id) ^ "] " ^ vn ^ " = " ^ (printExp e) ^ "\n" ^ (printNode next)
  | PrintNode (id,e,next) -> "[" ^ (string_of_int id) ^ "] Print(" ^ (printExp e) ^ ")\n" ^ (printNode next)
  | IfNode (id,e,tr,fl,next) -> "[" ^ (string_of_int id) ^ "]" ^ " if (" ^ (printExp e) ^ ")\n  " ^ (printNode tr) ^ " " ^ (printNode fl) ^ "\n" ^ (printNode next)
  | WhileNode (id,e,tr,next) -> "[" ^ (string_of_int id) ^ "] " ^ " while (" ^ (printExp e) ^ ") { " ^ (printNode tr) ^ " }" ^ "\n" ^ (printNode next)
  | CompNode (_,n1,n2) -> (printNode n1) ^ "\n" ^ (printNode n2)
  | Default -> ""


let rec member a lst = match lst with
  [] -> false
  | h::t -> a = h || member a t

let rec removeDuplicates lst = match lst with 
  | [] -> []
  | h::t -> if member h t then removeDuplicates t else h::removeDuplicates t