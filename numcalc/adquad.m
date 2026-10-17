function I = adquad( a,b,f,er )
    I1=Simpson(a,b,f);
    I2=Simpson(a,(a+b)/2,f)+Simpson((a+b)/2,b,f); 
    if abs(I1-I2)<15*er
        I=I2;
        return
    else
        I=adquad(a,(a+b)/2,f,er/2)+adquad((a+b)/2,b,f,er/2);
    end
end

