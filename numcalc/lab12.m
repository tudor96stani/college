%1
clear
x0=pi/4;
eps = 10^(-4);
N=100;
f=@(x) x-cos(x);
fd=@(x) 1+sin(x);
r = newton(f,fd,x0,N,eps);
fprintf('Ex 1 x = %2.5f, f(x)=%2.4f\n',r,f(r));


%2
clear
fprintf('\n');
x1=1;
x2=2;
f=@(x) x^3-x^2-1;
N=100;
eps=10^-4;
r = secant(f,x1,x2,N,eps);
fprintf('Ex 2 x = %2.5f, f(x)=%2.4f\n',r,f(r));

%3
clear
fprintf('\n');
N=100;
eps = 10^(-4);
a=1;
b=2;
f=@(x) (x-2)^2-log(x);
r = bisect(f,a,b,N,eps);
fprintf('Ex 3 x = %2.5f, f(x)=%2.6f\n',r,f(r));

%4
clear
fprintf('\n');
f=@(x) x-cos(x);
N=100;
a=0.5; b0=pi/4;
x0=pi/4;
eps=10^(-4);
r = falsePos(f,a,b0,N,eps);
fprintf('Ex 4 x = %2.5f, f(x)=%2.4f\n',r,f(r));


%Functions
function res = newton(f,fd,x0,N,err )
    it = 0;
    while it < N
        x1 = x0 - f(x0)/fd(x0);
        it = it +1;
        if abs(x1-x0)<err
           break
        end
        x0=x1;
    end
    res = x0;
end

function res = secant(f,x0,x1,N,err)
    it = 0;
    while (it<N && (abs(x1-x0)/abs(x1)>err))
       x2 = x1 - f(x1)*((x1-x0)/(f(x1)-f(x0)));
       it = it + 1;
       x0=x1;
       x1=x2;
    end
    res = x1;
end

function res = bisect(f,a,b,N,err)
    it = 0;
    while(it < N && (abs(a-b)/abs(a)) > err)
       c = (a + b) / 2;
       p = f(c)*f(b);
       if(p < 0)
           a = c;
       else
           b = c;
       end
       it = it + 1;
    end
    res = (a+b)/2;
end

function res = falsePos(f,a,b,N,err)
    it = 0;
    while(it < N && (abs(a-b)/abs(a)) > err)
       c = (a*f(b)-b*f(a))/(f(b)-f(a));
       p = f(a)*f(c);
       if p < 0
          b=c; 
       else
           a=c;
       end
       it = it + 1;
    end
    res = c;
end