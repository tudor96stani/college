%{
    #include <string.h>
    #include <stdio.h>
    #include <stdlib.h>

    extern int yylex();
    extern int yyparse();
    extern FILE *yyin;
    extern int line;
    extern void printAll();
    void yyerror(const char *s);
%}

%token ID
%token CONST
%token INT
%token DOUBLE
%token IF
%token ELSE
%token FOR
%token INCLUDE
%token USING
%token NAMESPACE
%token CIN
%token COUT
%token RETURN
%token EQ
%token LTE
%token GTE
%token NEQ
%token LL
%token GG
%token AA
%token OO
%token PP
%token MM
%%
program: antet '{' stmtlist RETURN CONST ';' '}';
antet: INCLUDE '<' ID '>' USING NAMESPACE ID ';' INT ID '('')';

stmtlist: stmt | stmt stmtlist;
stmt: declarationstmt | assignstmt | iostmt | forstmt | compstmt | condstmt;
compstmt: '{' stmtlist '}';

declarationstmt: INT ID extra_decl ';'| DOUBLE ID extra_decl ';';
extra_decl: | ',' ID | ',' ID extra_decl;

assignstmt: ID '=' expression ';';
expression: term | expression operator expression | '(' expression operator expression ')';
term: ID | CONST;
operator: '+' | '-' | '*' | '/' | '%';

iostmt: CIN GG ID ';'| COUT LL ID ';';

forstmt: FOR '(' assignstmt boolexpr ';' increment_decrement ')' '{' stmtlist '}';

boolexpr: boolexpr booloperator boolexpr | condition;

condition: expression relation expression;

relation: '<' | LTE | '>' | GTE | EQ | NEQ;

booloperator: AA | OO;

increment_decrement: ID PP| ID MM | PP ID | MM  ID;

condstmt: IF '(' boolexpr ')' '{' stmtlist '}';

%%
int main(int argc, char *argv[]) {
    ++argv, --argc; 
    if (argc > 0) 
        yyin = fopen(argv[0], "r"); 
    else 
        yyin = stdin; 
    while (!feof(yyin)) {
        yyparse();
    }
    printf("The file is syntactically correct!\n");
    return 0;
}

void yyerror(const char *s) {
    printf("Error: %s at line -> %d ! \n", s, line);
    exit(1);
}
