function l = Hermite( xi,fi,fd,x )
%UNTITLED2 Summary of this function goes here
%   Detailed explanation goes here
    I = xi==x;
    if sum(I)==1
       l=fi(I);
    else
        xn = zeros(1,length(xi)*2);
        fn = zeros(1,length(xi)*2);
        fdn = zeros(1,length(xi)*2);
        j=1;
        for i=1:length(xi)
           xn(j)=xi(i);
           xn(j+1)=xi(i);
           j=j+2;
        end
        j=1;
        for i=1:length(fi)
           fn(j)=fi(i);
           fn(j+1)=fi(i);
           j=j+2;
        end
        j=1;
        for i=1:length(fi)
           fdn(j)=fd(i);
           fdn(j+1)=fd(i);
           j=j+2;
        end
        divtbl=divdifHermite(xn,fn,fdn); %The divided difference table
        divx0=divtbl(1,:); %The first line of the div diff table, corresponding to x0
        l=fi(1);
        for i=2:length(xn)
           z=x-xn(1:i-1);
           l=l+prod(z)*divx0(i+1);
        end
    end

end

