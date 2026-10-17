function x = gauss( n,a,b )
    for p=1:n-1
       [~,q] = max(abs(a(p:n,p)));
       q = q + p - 1;
       if a(q,p)==0
          fprintf('Error 1\n');
          return
       end
       a([p q],:)=a([q p],:);
       b([p q])=b([q p]);
       for i=p+1:n
           difference = a(i,p)/a(p,p);
           a(i,:) = a(i,:) - a(p,:).*difference;
           b(i) = b(i) - b(p).*difference;
       end
       if a(n,n) == 0
          fprintf('Error 2\n');
          return
       end
    end
    x = zeros(n,1);
    x(n)=b(n)/a(n,n);
    for i=n-1:-1:1
        prod = a(i,i+1:n) * x(i+1:n);
        x(i) = (b(i)-prod) / a(i,i);
    end
end

