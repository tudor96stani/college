open Models
open Graph
open Helpers

let ast = CompStmt(
  AssignStmt ("a", Int(5)),
  CompStmt (
    CompStmt(
      IfStmt ( Var("a"), 
        CompStmt(
          CompStmt (
            PrintStmt(Var ("a")), 
            PrintStmt ( AddInt (Var ("a"),Int (50))) 
          ),
          PrintStmt (Var ("a"))
        ), (* IF *)
        CompStmt(PrintStmt (Int (5)),PrintStmt (Int (10))) (* ELSE *)
        ),
      PrintStmt (Int (100))),
    PrintStmt (Int (0))
  ) 
)
(*
  a = 5
  if a
    print a
    print a+50
  else
    print 5
    print 10
  print 100
  print 0
*)

(*
a=1
a=2
a=3
a=4
a=5
a=6
a=7
*) (*
let ast = 
  CompStmt 
  (
    CompStmt
    (
      CompStmt
      (
        CompStmt
        (
          AssignStmt 
          (
            "a", Int (1)
          ),
          AssignStmt 
          (
            "a", Int (2)
          )
        ),
        AssignStmt 
        (
          "a", Int (3)
        )
      ),
      CompStmt
      (
        CompStmt
        (
          AssignStmt 
          (
            "a", Int (4)
          ),
          AssignStmt 
          (
            "a", Int (5)
          )
        ),
        AssignStmt
        (
          "a", Int (6)
        )
      )
    ),
    AssignStmt
    (
      "a", Int (7)
    )
  )*)


let ast2 = CompStmt (CompStmt (PrintStmt (Int (1)), PrintStmt (Int (2))), PrintStmt (Int (3)))

let ast = CompStmt (WhileStmt (Int (0),ast2), PrintStmt (Int (4))) 
(*
(1)while(0)
(2)  print(1)
(3)  print(2)
(4)  print(3)
(5)print(4)
1->2
2->3
3->4
4->1
4->5
*)

let ast = CompStmt (
  CompStmt(
    CompStmt(AssignStmt ("a",Int (0)), AssignStmt ("b", AddInt(Var ("a"),Int (1)))),
    CompStmt(AssignStmt ("c",AddInt (Var ("a"),Var ("b"))),AssignStmt ("a",MulInt (Var ("a"),Int (2))))
  ),
  CompStmt(PrintStmt (Var ("a")),PrintStmt (Var ("c"))))

(*
let ast = CompStmt(
  CompStmt(
    AssignStmt ("a",Int (0)),
    WhileStmt ( Var ("a"),
    CompStmt(
      CompStmt(
        AssignStmt("b",AddInt (Var ("a"),Int (1))),
        AssignStmt("c",AddInt (Var ("c"),Var ("b")))
      ),
      AssignStmt("a",MulInt (Var ("b"),Int (2))))
    )
  ),
  PrintStmt (Var ("c"))
)
*)