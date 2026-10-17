%1
x=[0 3 5 8 13];
f=[0 225 383 623 993];
fd=[75 77 80 74 72];
Hermite(x,f,fd,10)

%2
x=[1 2];
f=[0 0.6931];
fd=[1 0.5];
Hermite(x,f,fd,3/2)


%3
clear
xplot=-5:0.01:5;
fplot=sin(2*xplot);
figure
plot(xplot,fplot);
hold on
grid on
nodes = linspace(-5,5,15);
fnodes = sin(2*nodes);
fdnodes = 2*cos(2*nodes);
l=zeros(size(xplot));
for i=1:length(xplot)
    l(i)=Hermite(nodes,fnodes,fdnodes,xplot(i));
end
plot(xplot,l);
plot(nodes,fnodes,'*');
legend('function','interpolation','nodes')
