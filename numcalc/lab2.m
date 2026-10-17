%1
%version 1 - functions
l1=@(x) x;
l2=@(x) 3/2*x.^2-1/2;
l3=@(x) 5/2*x.^3-3/2*x;
l4=@(x) 35/8*x.^4 - 15/4*x.^2+3/8;
x=0:0.01:1;

subplot(2,2,1);
plot(x,l1(x));
subplot(2,2,2);
plot(x,l2(x));
subplot(2,2,3);
plot(x,l3(x));
subplot(2,2,4);
plot(x,l4(x));

%version 2 - polynomials
clear
figure
x=0:0.01:1;
l1 = [1 0];
l2 = [3/2 0 -1/2];
l3 = [5/2 0 -3/2 0];
l4 = [35/8 0 -15/4 0 3/8];
l1val = polyval(l1,x);
l2val = polyval(l2,x);
l3val = polyval(l3,x);
l4val = polyval(l4,x);
subplot(2,2,1);
plot(x,l1val);
subplot(2,2,2);
plot(x,l2val);
subplot(2,2,3);
plot(x,l3val);
subplot(2,2,4);
plot(x,l4val);


%P2
%a)
clear
Tn=@(n,t) cos(n*acos(t));
t=-1:0.01:1;
T1=Tn(1,t);
T2=Tn(2,t);
T3=Tn(3,t);
figure
plot(t,T1,t,T2,t,T3)

%b)
clear
n=6;
x=-1:0.01:1;
t0=ones(size(x));
t1=x;
figure
plot(x,t0,x,t1);
hold on
for i = 2:n
    tnp1 = 2*x.*t1 - t0;
    plot(x,tnp1);
    t0 = t1;
    t1=tnp1;
end


%3
clear
x0=0;
x=-1:0.01:3;
figure
hold on
p0 = ones(size(x));
plot(x,p0);
for k=1:6
    p0=p0 + x.^k/factorial(k);
    plot(x,p0);
end

%4
clear
f=@(x) sqrt(5*x.^2+1);
h=0.25;

i=0:6;
t=zeros(7);
a= (1 + h*(0:6))';
n=7;
t(:,1) = f(a);
for i = 2:n
    t(1:n-i+1,i) = diff(t(1:n-i+2,i-1));
end
t=[a t]

%5
clear
x =[2;4;6;8];
f =[4;8;14;16];
t=zeros(4);
t(:,1)=f;
n=4;
for i = 2:n
   t(1:n-i+1,i) = diff(t(1:n-i+2,i-1))./(x(i:n)-x(1:n-i+1));
end
t=[x t]