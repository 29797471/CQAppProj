function Floor(num)
	return math.floor(num + 0.000001)
end

function Ceil(num)
	return math.floor(num - 0.000001)
end

--1,2,3,4->1,2,4,8
function ToEnumFlag(flag)
	return bit.lshift(1, flag-1)
end

--|
function BitOR(a,b)--Bitwise or
    local p,c=1,0
    while a+b>0 do
        local ra,rb=a%2,b%2
        if ra+rb>0 then c=c+p end
        a,b,p=(a-ra)/2,(b-rb)/2,p*2
    end
    return c
end


--~
function BitNOT(n)
    local p,c=1,0
    while n>0 do
        local r=n%2
        if r<1 then c=c+p end
        n,p=(n-r)/2,p*2
    end
    return c
end

--&
function BitAND(a,b)--Bitwise and
    local p,c=1,0
    while a>0 and b>0 do
        local ra,rb=a%2,b%2
        if ra+rb>1 then c=c+p end
        a,b,p=(a-ra)/2,(b-rb)/2,p*2
    end
    return c
end

--^
function Xor(num1,num2)
	local tmp1 = checkNums(num1)
	local tmp2 = checkNums(num2)
	local ret = 0
	local count = 0
	repeat
		local s1 = tmp1 % 2
		local s2 = tmp2 % 2
		if s1 ~= s2 then
			ret = ret + 2^count
		end
		tmp1 = math.modf(tmp1/2)
		tmp2 = math.modf(tmp2/2)
		count = count + 1
	until(tmp1 == 0 and tmp2 == 0)
	return resultCover(ret)
end

--(n& bit) == bit
function StateCheck(n,bit)
	--return MathUtil.StateCheck(n,bit)
	return BitAND(n,bit)==bit
end

--n | bit
function StateAdd(n,bit)
	return BitOR(n,bit)
end

--n ^ bit
function StateChange(n,bit)
	return Xor(n,bit)
end

--n & (~bit)
function StateDel(n,bit)
	return BitAND(n,BitNOT(bit))
end