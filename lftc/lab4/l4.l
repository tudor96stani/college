%{
	#include <stdlib.h>
	#include <string.h>
	#include <math.h>
	#include "l4.tab.h"

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
	int iCode=0;
	int cCode=1;
	PIF program[500];
	ST symtbl[50];
	ST stid[50];
    int stidlen=0;
	int line=1;

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
            	
        	}
        	i++;
		}
    	if (found == 0){
       		strcpy(symtbl[len].key,token);
        	addPIF(cCode,len);
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
		printf("ST const\n");
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
{digit}							{addConst(yytext); return CONST; }
{no}                                {addConst(yytext);return CONST; }
{digit}+"."{digit}{no}         {addConst(yytext);	return CONST; }




\id 			{addPIF(1,0);return ID;}
\const 			{addPIF(2,0);return CONST;}
int 			{addPIF(3,0);return INT;}
double			{addPIF(4,0);return DOUBLE;}
if				{addPIF(5,0);return IF;}
else			{addPIF(6,0);return ELSE;}
for				{addPIF(7,0);return FOR;}
return			{addPIF(8,0);return RETURN;}
cin             {addPIF(9,0);return CIN;}
cout			{addPIF(10,0);return COUT;}
\+				{addPIF(11,0);return '+';}
\-				{addPIF(12,0);return '-';}
\*				{addPIF(13,0);return '*';}
\/				{addPIF(14,0);return '/';}
\%              {addPIF(15,0);return '%';}
\#include       {addPIF(16,0);return INCLUDE;}
using           {addPIF(17,0);return USING;}
namespace       {addPIF(18,0);return NAMESPACE;}
\;				{addPIF(19,0);return ';';}
\,				{addPIF(20,0);return ',';}
\=				{addPIF(21,0);return '=';}
\==				{addPIF(22,0);return EQ;}
\<				{addPIF(23,0);return '<';}
\<=				{addPIF(24,0);return LTE;}
\>				{addPIF(25,0);return '>';}
\>=				{addPIF(26,0);return GTE;}
\!=				{addPIF(27,0);return NEQ;}
\[				{addPIF(28,0);return '[';}
\]				{addPIF(29,0);return ']';}
\(				{addPIF(30,0);return '(';}
\)				{addPIF(31,0);return ')';}
\{				{addPIF(32,0);return '{';}
\}				{addPIF(33,0);return '}';}
\<<             {addPIF(34,0);return LL;}
\>>             {addPIF(35,0);return GG;}
\&&             {addPIF(36,0);return AA;}
\|\|            {addPIF(37,0);return OO;}
\.              {addPIF(38,0);return '.';}
\+\+			{addPIF(39,0);return PP;}
\-\-			{addPIF(40,0);return MM;}
{id}			{addId(yytext);return ID;}
{errI}      		{ printf("Error [%s]\n",yytext); }
{errC}      		{ printf("Error [%s]\n",yytext); }
[ \t]+
[\n\r]			++line;
.                    { printf("Error [%s]\n",yytext); } 

