BuySellMgr = {}

--true:完成一次操盘(可以做空) 
--false 不满足操盘条件
function BuySellMgr.DoOnce(inv,calcData)
    if inv:DataCount()<10 then
        return
    end
    --先看是否有持仓
    if inv.currentCount==0 then
        --从低位进仓
        if  inv:ConUpDownK()<-calcData.conUpDownK then
            
            --计算购买力
            local buyTotalCount=inv:MaxInCount()
            inv:ScaleInOut(inv:MaxInCount()*calcData.assetK)
            return true
        elseif calcData.short then
            --从高位做空
            if inv:ConUpDownK()>calcData.conUpDownK then
                inv:ScaleInOut(-inv:MaxInCount()*calcData.assetK)
                return true
            end
        end
        
    elseif inv:EarnPercent()<-calcData.lostK then--当亏损超过-2%时，通过加仓调整亏损为-1%以上
        --inv:Print()
        local costP=inv:CostPrice()
        local currP=inv:CurrentPrice()
        --调整的目标均价
        local targetPrice = inv:CurrentPrice() / (1-calcData.newLostK )
        local num2=inv.currentCount*(targetPrice-costP)/(currP-targetPrice)
        if num2>inv:MaxInCount() then
            num2=inv:MaxInCount()
            if inv.print then
                printRed("钱不够调仓了")
            end
        end
        if num2~=0 then
            inv:ScaleInOut(num2)
            return true
        end
    elseif inv:EarnPercent()>calcData.winK then
        inv:ScaleInOut(-inv.currentCount)
        return true
    end
    return false
end

--模拟投资计算  
--list:历史数据 
--startIndex:起始数据索引
--calcData:策略参数
--recordUnsold:统计为成交记录
function BuySellMgr.Calc(list,startIndex,calcData,recordUnsold)
    local result={list={}}
    result.code=calcData.code
    result.name=calcData.name
    local xx=Investment()
    xx:Init(1000000)
	for i=startIndex,1,-1 do
		local item=list[i]
		xx:UpdatePrice(item.time,item.price)
        
        local bl=BuySellMgr.DoOnce(xx,calcData)
        
        if bl or recordUnsold then
            local data=xx:GetData()
            data.price=item.price
            data.time=item.time
            data.percent=item.percent
            table.insert(result.list,data)
        end
    end
    calcData.totalWinK=xx:TotalWinK()
    calcData.baseWinK=list[1].price/list[startIndex].price-1
    return result
end