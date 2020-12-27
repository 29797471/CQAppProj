--算法扩展

--数在数轴上的区间位置
--数轴从小到大排列
--每区间前闭后开
--位置从0开始,第一个区间返回1
function GetIndexOfExtent(v,rangeList)
    local k=1
    for i,d in ipairs(rangeList) do
        k=i
        if v<d then
            return i-1
        end
    end
    return k-1
end

--数在数轴上的区间位置
--数轴从小到大排列
--每区间前闭后开
--位置从0开始,第一个区间返回1
function GetIndexOfExtentX(v,rangeList)
    local k=1
    for i,d in ipairs(rangeList) do
        k=i
        if v<d then
            return i-1
        end
    end
    return k
end

---获取位运算之后的数值
--1->1 2->2 3->4 4->8 5->16
function GetBitFlag(flag)
    if flag then
        return bit.lshift(1, flag)
    end
end
