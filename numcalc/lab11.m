%1
clear
A = [3 -1 0 0 0 0;
    -1 3 -1 0 0 0;
    0 -1 3 -1 0 0;
    0 0 -1 3 -1 0;
    0 0 0 -1 3 -1;
    0 0 0 0 -1 3];
eps = 10^(-3);
b = [2;1;1;1;1;2];
d= diag(diag(A));
l= tril(A)-d;
u=triu(A)-d;
%Jacobi
k=1;
xold = zeros(length(b),1);
xk = inv(d)*(-(l+u)*xold+b);
while norm(xk-xold)>eps
    temp = xk;
    xk = inv(d)*(-(l+u)*temp+b);
    xold = temp;
    k = k + 1;
end
fprintf('Ex 1 Jacobi method solution:\n');
disp(xk)
fprintf('for k = %d\n',k);
%Gauss Seidel
k=1;
xold=zeros(length(b),1);
xk= inv(d+l)*(-u*xold+b);
while norm(xk-xold)>eps
   temp = xk;
   xk = inv(d+l)*(-u*temp+b);
   xold=temp;
   k=k+1;
end
fprintf('Ex 1 Gauss-Seidel method solution:\n');
disp(xk)
fprintf('for k = %d\n',k);

%Relaxation
omega = 0.95;
k=1;
xold=zeros(length(b),1);
xk = inv(d+omega*l)*(((1-omega)*d - omega*u)*xold+omega*b);
while norm(xk-xold)>eps
   temp = xk;
   xk = inv(d+omega*l)*(((1-omega)*d - omega*u)*temp+omega*b);
   xold=temp;
   k=k+1;
end
fprintf('Ex 1 Relaxation method solution:\n');
disp(xk)
fprintf('for k = %d and omega=%d\n',k,omega);


%2
clear
fprintf('\n');
A = [3 1 1;-2 4 0;-1 2 -6];
b=[12;2;-5];
eps = 10^(-5);
d= diag(diag(A));
l= tril(A)-d;
u=triu(A)-d;

%Jacobi
k=1;
xold = zeros(length(b),1);
xk = inv(d)*(-(l+u)*xold+b);
while norm(xk-xold)>eps
    temp = xk;
    xk = inv(d)*(-(l+u)*temp+b);
    xold = temp;
    k = k + 1;
end
fprintf('Ex 2 Jacobi method solution:\n');
disp(xk)
fprintf('for k = %d\n',k);

%Gauss Seidel
k=1;
xold=zeros(length(b),1);
xk= inv(d+l)*(-u*xold+b);
while norm(xk-xold)>eps
   temp = xk;
   xk = inv(d+l)*(-u*temp+b);
   xold=temp;
   k=k+1;
end
fprintf('Ex 1 Gauss-Seidel method solution:\n');
disp(xk)
fprintf('for k = %d\n',k);

%Relaxation
omega = 0.95;
k=1;
xold=zeros(length(b),1);
xk = inv(d+omega*l)*(((1-omega)*d - omega*u)*xold+omega*b);
while norm(xk-xold)>eps
   temp = xk;
   xk = inv(d+omega*l)*(((1-omega)*d - omega*u)*temp+omega*b);
   xold=temp;
   k=k+1;
end
fprintf('Ex 1 Relaxation method solution:\n');
disp(xk)
fprintf('for k = %d and omega=%d\n',k,omega);