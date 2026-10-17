flex l4.l
bison -d l4.y > /dev/null 2>&1
gcc -w -o l4 l4.tab.c lex.yy.c
echo -e ''
DIRECTORY=.
for i in $DIRECTORY/lab1_*.cpp; do
    echo -e 'Result for '.$i.':';
    ./l4 $i
    echo -e '\n';
done
