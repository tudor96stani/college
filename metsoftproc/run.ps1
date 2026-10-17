#!/bin/bash
ocamlc -o out Models.ml Graph.ml Helpers.ml Ast.ml GraphV2.ml Main.ml 

if($?)
{
    Write-Output "Compiled files successfully"
    Remove-Item *.cmi
    Remove-Item *.cmo
    ocamlrun out
}
else
{
    Write-Output "Could not compile files"
}