open Models

type tNode = AssignNode of int*string*exp*tNode
            | PrintNode of int*exp*tNode
            | IfNode of int*exp*tNode*tNode*tNode
            | WhileNode of int*exp*tNode*tNode
            | CompNode of int*tNode*tNode
            | Default

let rec setNext node next = match node with
  | AssignNode (id,vn,e,_) -> AssignNode (id,vn,e,next)
  | PrintNode (id,e,_) -> PrintNode (id,e,next)
  | IfNode (id,e,tr,fl,_) -> IfNode(id,e,tr,fl,next)
  | WhileNode (id,e,s,_) -> WhileNode(id,e,s,next)
  | CompNode (id,fst,_) -> CompNode (id,fst,next)
  | Default -> Default

let rec getNodeId = function 
  | AssignNode (id,_,_,_) -> id
  | PrintNode (id,_,_) -> id
  | IfNode (id,_,_,_,_) -> id
  | WhileNode (id,_,_,_) -> id
  | CompNode (id,_,_) -> id
  | Default -> -1

let rec getLargestId = function
  | IfNode (id,e,tr,fl,_) -> 
    (
      let id1 = (getLargestId tr) and id2 = (getLargestId fl) in
        max id1 id2
    )
  | AssignNode (id,_,_,n) | PrintNode (id,_,n) -> max id (getLargestId n)
  | WhileNode (_,_,s,n) -> max (getLargestId s) (getLargestId n)
  | CompNode (_,fst,snd) ->
    (
      let id1 = (getLargestId fst) and id2 = (getLargestId snd) in
        max id1 id2 
    )
  | Default -> -1

let rec toNode ast currentIdx = match ast with
  | IfStmt (e,s1,s2) -> 
    (
      let s1n = toNode s1 (currentIdx + 1) in
        let s1nId = getLargestId s1n in
          let s2n = toNode s2 (s1nId + 1) in 
            IfNode (currentIdx,e,s1n,s2n,Default)
    )
  | AssignStmt (vn,e) -> AssignNode (currentIdx,vn,e,Default)
  | PrintStmt (e) -> PrintNode (currentIdx,e,Default)
  | WhileStmt (e,s) -> WhileNode (currentIdx,e,(toNode s (currentIdx + 1)),Default)
  | CompStmt (s1,s2) -> 
    (
      let s1n = toNode s1 (currentIdx) in
        let s1mId = getLargestId s1n in
          let s2n = toNode s2 (s1mId + 1) in 
            CompNode (currentIdx,(setNext s1n s2n),Default)
    )
  | IfJoin -> Default
 
