%{
	#include <stdlib.h>
	#include <string.h>
	#include <math.h>

	typedef struct{
		int key;
		int value;
	}PIF;

	typedef struct{
		char key[10];
	}ST;

	int pifLen=0;
	int idx=0;
	int len=0;
	int iCode=1;
	int cCode=2;
	PIF program[500];
	ST symtbl[50];
    ST stid[50];
    int stidlen=0;

	void addPIF(int key, int value){
		program[pifLen].key=key;
		program[pifLen].value=value;
		pifLen++;
	}

	void printPIF(){
		printf("PIF\n");
		for(int t=0;t<pifLen;t++){
			if(program[t].key != iCode && program[t].key != cCode){
				printf(" %d   0 ",program[t].key);
				printf("\n");
			}
			else{
				printf(" %d   %d ",program[t].key,program[t].value);
				printf("\n");
			}
		}
	}

	void addConst(char* token){
		int found=0;
		int i=0;
   		while(i<len){
			if(strcmp(symtbl[i].key,token)==0){
            	found =1;
            	addPIF(cCode,i);
                printf("Found %s\n",token);
            	
        	}
        	i++;
		}
    	if (found == 0){
       		strcpy(symtbl[len].key,token);
        	addPIF(cCode,len);
            printf("Did not find, adding %s\n",token);
        	len++;
    	}
	}

	void addId(char* token){
		int found=0;
		int i=0;
		while(i<stidlen){
			if(strcmp(stid[i].key,token)==0){
            	found =1;
            	addPIF(iCode,i);
            	
        	}
        	i++;
		}
		if (found == 0){
        	strcpy(stid[stidlen].key,token);
        	addPIF(iCode,stidlen);
        	stidlen++;
    	}
	}

	void printST(){
		printf("ST Const\n");
		for(int i=0;i<len;i++){
			printf("%s   %d", symtbl[i].key, i);
			printf("\n");
		}
		printf("\n");
        printf("ST ID\n");
        for(int i = 0; i< stidlen; i++){
            printf("%s  %d",stid[i].key,i);
            printf("\n");
        }
        printf("\n");
	}

	void printAll(){
		printPIF();
		printST();
	}
%}

%option noyywrap
id  [a-zA-Z][a-zA-Z0-9_]*
digit [0-9]
no [1-9][0-9]*
errI [0-9_]*[a-z]
errC [0]*[1-9]*

%%
{digit}							addConst(yytext);
{no}                                addConst(yytext);
{digit}+"."{digit}{no}          addConst(yytext);	




\id 			addPIF(1,0);
\const 			addPIF(2,0);
int 			addPIF(3,0);
double			addPIF(4,0);	
if				addPIF(5,0);
else			addPIF(6,0);
for				addPIF(7,0);
return			addPIF(8,0);
cin             addPIF(9,0);
cout			addPIF(10,0);
\+				addPIF(11,0);
\-				addPIF(12,0);
\*				addPIF(13,0);
\/				addPIF(14,0);
\%              addPIF(15,0);
#include        addPIF(16,0);
using           addPIF(17,0);
namespace       addPIF(18,0);
\;				addPIF(19,0);
\,				addPIF(20,0);
\=				addPIF(21,0);
\==				addPIF(22,0);
\<				addPIF(23,0);
\<=				addPIF(24,0);
\>				addPIF(25,0);
\>=				addPIF(26,0);
\!=				addPIF(27,0);
\[				addPIF(28,0);
\]				addPIF(29,0);
\(				addPIF(30,0);
\)				addPIF(31,0);
\{				addPIF(32,0);
\}				addPIF(33,0);
\<<             addPIF(34,0);
\>>             addPIF(35,0);
\&&             addPIF(36,0);
\|\|             addPIF(37,0);
\.              addPIF(38,0);
{id}			addId(yytext);
{errI}      		{ printf("Error [%s]\n",yytext); }
{errC}      		{ printf("Error [%s]\n",yytext); }
[ \t\n\r]+		
.                    { printf("Error [%s]\n",yytext); } 

%%
int main(argc, argv)
int argc;
char **argv;
{
	++argv, --argc;
	if ( argc > 0 )
    	yyin = fopen( argv[0], "r" );
    else
     	yyin = stdin;
    yylex();
    printAll();
}
