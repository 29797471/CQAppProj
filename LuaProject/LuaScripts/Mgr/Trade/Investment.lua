--理财操盘类
--包含买入卖出操作,盈亏查询
super_class.Investment()

function Investment:Init(inMoney)

    --初始入金
    self.inMoney=inMoney
    
    --持有资金
    self.holdMoney=inMoney

    --交易资金
    self.buySellMoney=0

    --当前持有
    self.currentCount=0

    --上一次涨跌切换时的价格.
    self.conUpDownPrice=0

    self.print=false

    self.priceList={}
end

--更新时刻现价
function Investment:UpdatePrice(time,price)
    if #self.priceList~=0 then
        local lastPrice=self:CurrentPrice()
        if price>lastPrice and self.conUpDownPrice>=lastPrice then
            self.conUpDownPrice=lastPrice
        elseif price<lastPrice and self.conUpDownPrice<=lastPrice then
            self.conUpDownPrice=lastPrice
        end
    end
    table.insert(self.priceList,{time=time,price=price})
    --print(TextMgr.GetText(19,nil,{time}).." 更新现价"..self:CurrentPrice().." 累计连续涨跌:"..self:ConUpDownK())
end

--累计连续涨跌率
function Investment:ConUpDownK()
    return self:CurrentPrice()/self.conUpDownPrice-1
end

function Investment:CurrentPrice()
    return self.priceList[#self.priceList].price
end

function Investment:CurrentTime()
    return TextMgr.GetText(19,nil,{self.priceList[#self.priceList].time})
end

--成本,均价
function Investment:CostPrice()
    if self.currentCount==0 then
        return 0
    end
    return self.buySellMoney / self.currentCount
end

--浮动盈亏率
function Investment:EarnPercent()
    if self.currentCount==0 then
        return 0
    end
    if self.currentCount<0 then
        return -(self:CurrentPrice() / self:CostPrice()-1 )
    end
    return self:CurrentPrice() / self:CostPrice()-1 
end

--加减仓/建仓(count 负数为减仓)
function Investment:ScaleInOut(count)
    count=math.floor(count)
    local deltaMoney=self:CurrentPrice() * count
    if self.holdMoney < deltaMoney then
        if self.print then
            print(self:CurrentTime().." 加仓失败 当前需要:"..deltaMoney.." 实际剩余:"..self.holdMoney)
        end
        return false
    end
    
    self.holdMoney = self.holdMoney - deltaMoney
    self.buySellMoney = self.buySellMoney + deltaMoney
    self.currentCount = self.currentCount + count
    if self.print then
        if count>=0 then
            print(self:CurrentTime().." 以"..self:CurrentPrice().."加仓"..count)
        else
            print(self:CurrentTime().." 以"..self:CurrentPrice().."减仓"..-count)
        end
    end
    if self.currentCount==0 then
        self.buySellMoney=0
    end
    return true
end

--最大可买数量
function Investment:MaxInCount()
    return math.floor(self.holdMoney / self:CurrentPrice())
end

function Investment:GetData()
    
    local p=self:TotalWinK()
    if self.print then
        if self.currentCount==0 then
            --print(self:CurrentTime().." 当前未持仓 剩余资金:"..self.holdMoney.."  总收益:"..p.."%")
        else
            --print(self:CurrentTime().." 当前持仓"..self.currentCount.." 均价"..self:CostPrice().." 盈亏"..self:EarnPercent()*100.."%".." 总资产收益:"..p.."%")
        end
    end
    local tbl={
        holdCount=self.currentCount,
        holdMoney=self.currentCount*self:CurrentPrice(),
        costPrice=self:CostPrice(),
        earnK=self:EarnPercent(),
        totalK=self:TotalWinK(),
    }
    return tbl
end

--总收益率
function Investment:TotalWinK()
    --当前总资产收益
    local totalValue=self.holdMoney+self:CurrentPrice() * self.currentCount-self.inMoney
    return totalValue/self.inMoney
end

--历史数据数量
function Investment:DataCount()
    return #self.priceList
end

--统计number个历史数据,计算均价
function Investment:GetAverage(number)
    local total=0
    for i=#self.priceList,#self.priceList-number+1,-1 do
        total=total+self.priceList[i].price
    end
    return total/number
end





