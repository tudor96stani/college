%2
%a
a=[10 7 8 7;7 5 6 5;8 6 10 9;7 5 9 10];
b=[32;23;33;31];
x=a\b;
cnd=cond(a);
fprintf('Ex 2 a) x=\n');
disp(x);
fprintf('Ex 2 a) cond = %2.4f\n',cnd);
%b
b1=[32.1;22.9;33.1;30.9];
x1=a\b1;
fprintf('Ex 2 b) x1=\n');
disp(x1);
inpRelErr = norm(b-b1)/norm(b);
outRelErr = norm(x-x1)/norm(x);
dif=inpRelErr/outRelErr;
fprintf('Ex 2 b) Input relative error = %2.4f\n',inpRelErr);
fprintf('Ex 2 b) Ouput relative error = %2.4f\n',outRelErr);
fprintf('Ex 2 b) Input relative error / Ouput relative error= %2.4f\n',dif);
%c
a1=[10 7 8.1 7.2;7.08 5.04 6 5;8 5.98 9.89 9;6.99 4.99 9 9.98];
x2 = a1\b;
fprintf('Ex 2 c) x=\n');
disp(x2);
inpRelErr = norm(a-a1)/norm(a);
outRelErr = norm(x-x2)/norm(x);
dif=inpRelErr/outRelErr;
fprintf('Ex 2 c) Input relative error = %2.4f\n',inpRelErr);
fprintf('Ex 2 c) Ouput relative error = %2.4f\n',outRelErr);
fprintf('Ex 2 c) Input relative error / Ouput relative error= %2.4f\n',dif);

%3
clear
fprintf('\n');
n=10;
while n<=15
    H=zeros(10);
    for i = 1:n
       for j=1:n
          H(i,j) = 1/(i+j-1); 
       end
    end
    c = cond(H);
    fprintf('Ex 3 Cond(H) for n=%d is %2.5f\n',n,c);
    n = n+1;
end