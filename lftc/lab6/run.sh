#!/bin/bash
flex a.l
bison -d a.y
gcc  -w -o a a.tab.c lex.yy.c
