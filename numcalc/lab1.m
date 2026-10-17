%  I
clear
a = [1 2 13;4 5 6;7 8 9];
b = [4 8 12;-1 0 5;2 3 8];
[m,n]=size(a)
t = b'
c=a*b
d=a.*b
e=a.^2
size(a)
length(a)
m = mean(a)
m1 = mean(a,2)
g=geomean(a)
s=sum(a)
s1=sum(a,2)
p=prod(a)
p1=prod(a,2)
max(a)
min(a)
diag(a)
m>2
a>b
inv(a)
det(a)
f=abs(b)
b=[16 15 24]'
x=a\b
triu(a)
tril(a)
m = [2 3 5; 7 11 13; 17 19 23]
m(2,1)
m(:,1)
m(2,:)
m(2,1:2)
m(2,2:end)
m(2:3,2:3)
eye(8)
eye(5,7)
M=magic(4)
sum(M)
sum(M,2)
sum(diag(M))
sum(diag(fliplr(M)))

% II
clear
p=[2 -5 0 8];
polyval(p,2)
p1=[1 -1 -17 21];
roots(p1)


% III
%1
f=@(x) exp(10*x.*(x-1)).*sin(12*pi*x);
x=0:0.01:1;
figure;
plot(x,f(x))
hold on
g=@(x) exp(5*x.^2-1).*sqrt(3*cos(12*pi*x));
plot(x,f(x))


%2
clear
a=2;
b=4;
x=@(t) (a+b)*cos(t) - b*cos((a/b + 1)*t);
y=@(t) (a+b)*sin(t) - b*sin((a/b + 1)*t);
t=0:0.5:10*pi;
figure;
plot(t,x(t),t,y(t))


%3
clear
f1=@(x) cos(x);
f2=@(x) sin(x);
f3=@(x) cos(2*x);
x=0:0.1:2*pi;
%plot(x,f1(x),x,f2(x),x,f3(x))

%4
x1=0:2:50;
x2=1:2:49;
y1=x1/2;
y2=3*x2+1;
y=zeros(1,51);
y(1:2:51)=y1;
y(2:2:50)=y2;
figure;
plot(0:50,y);

%5
clear
r=2;
for i = 1:5
    r=1+1/r;
end

%6

g=@(x,y) exp(-((x-1/2).^2 + (y-1/2).^2));
x=-2:.2:2;
y=-4:.4:4;
[X,Y]=meshgrid(-2:0.1:2,-4:0.1:4);
Z = g(X,Y);
mesh(X,Y,Z)