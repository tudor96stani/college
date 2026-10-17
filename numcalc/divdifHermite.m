function T = divdifHermite( xn,fn,fdn )
    n=length(xn);
    T=zeros(length(xn));
    T(:,1)=fn;
    for i=1:n-1
       if mod(i,2)~=0
           T(i,2) = fdn(i);
       else
           T(i,2) = (T(i+1,1)-T(i,1))/(xn(i+1)-xn(i));
       end
    end
    for i = 3:n
        T(1:n-i+1,i) = diff(T(1:n-i+2,i-1))./(xn(i:n)-xn(1:n-i+1))';
    end
    T = [xn' T];
end

