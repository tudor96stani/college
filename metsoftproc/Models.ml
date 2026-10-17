
type exp = 
      | Int of int
      | Var of string 
      | AddInt of exp*exp
      | MulInt of exp*exp 
      | DiffInt of exp*exp
      | DivInt of exp*exp

and
stmt =
      | CompStmt of stmt*stmt
      | AssignStmt of string*exp
      | PrintStmt of exp
      | IfStmt of exp*stmt*stmt
      | WhileStmt of exp*stmt
      | IfJoin
and
prg = Prg of stmt