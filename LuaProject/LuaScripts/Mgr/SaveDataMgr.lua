SaveDataMgr={}
function SaveDataMgr.Init()
    
    if SettingModel.instance.LuaData==nil then
        SaveDataMgr.data={}
        SaveDataMgr.SaveData()
    else
        SaveDataMgr.data=JSON:decode(SettingModel.instance.LuaData)
    end
    --printTbl(SaveDataMgr.data)

			--[[
				0：”大秦铁路”，股票名字；
1：”27.55″，今日开盘价；
2：”27.25″，昨日收盘价；
3：”26.91″，当前价格；
4：”27.55″，今日最高价；
5：”26.20″，今日最低价；
6：”26.91″，竞买价，即“买一”报价；
7：”26.92″，竞卖价，即“卖一”报价；
8：”22114263″，成交的股票数，由于股票交易以一百股为基本单位，所以在使用时，通常把该值除以一百；
9：”589824680″，成交金额，单位为“元”，为了一目了然，通常以“万元”为成交金额的单位，所以通常把该值除以一万；
10：”4695″，“买一”申请4695股，即47手；
11：”26.91″，“买一”报价；
12：”57590″，“买二”
13：”26.90″，“买二”
14：”14700″，“买三”
15：”26.89″，“买三”
16：”14300″，“买四”
17：”26.88″，“买四”
18：”15100″，“买五”
19：”26.87″，“买五”
20：”3100″，“卖一”申报3100股，即31手；
21：”26.92″，“卖一”报价
(22, 23), (24, 25), (26,27), (28, 29)分别为“卖二”至“卖四的情况”
30：”2008-01-11″，日期；
31：”15:05:32″，时间；
			]]--

    SaveDataMgr.Parse={}
	SaveDataMgr.Parse[Market.USA]=function (list)
		return 
		{
			sname			= list[1],
            db_price		= list[22],
            last_price		= list[2],
            rise_fall_per	= list[3],
            uptime			= list[4],
            rise_fall		= list[5],
            open_price		= list[6],
            high_price		= list[7],
            low_price		= list[8],
            week52_high		= list[9],
            week52_low		= list[10],
            volume			= list[11],
            day10_volume	= list[12],
            mvalue			= list[13],
            ep_share		= list[14],
            equity			= list[20],
            db_volume		= list[28],
            yesy_price		= list[27],
			market			= "美国",
		}
		--[[
		temp[0]------新浪------
temp[1]------48.98------当前价
temp[2]------ -0.57------涨幅（-0.57%）
temp[3]------2016-06-22 08:19:42------时间
temp[4]------ -0.28------涨跌
temp[5]------49.31------开盘价
temp[6]------49.83------最高价
temp[7]------48.92------最低价
temp[8]------57.01------52周最高
temp[9]------32.61------52周最低
temp[10]------280775------成交量
temp[11]------609728------10日均量
temp[12]------3443098080------市值
temp[13]------0.79------每股收益
temp[14]------62.00------市盈率
temp[15]------0.00------
temp[16]------1.15------贝塔系数
temp[17]------0.00------
temp[18]------0.00------
temp[19]------70296000------股本
temp[20]------58.00------
temp[21]------48.98------今日收盘价
temp[22]------0.00------
temp[23]------0.00------
temp[24]------------
temp[25]------Jun 21 04:00PM EDT------
temp[26]------49.26------昨日收盘价
temp[27]------0.00------
————————————————
版权声明：本文为CSDN博主「happydecai」的原创文章，遵循CC 4.0 BY-SA版权协议，转载请附上原文出处链接及本声明。
原文链接：https://blog.csdn.net/happydecai/java/article/details/100112749	
		]]--
	end

	SaveDataMgr.Parse[Market.HongKong]=function (list)
		return 
		{
			sname			= list[2],
            open_price		= list[3],
            yesy_price		= list[4],
            high_price		= list[5],
            low_price		= list[6],
            last_price		= list[7],
            rise_fall_per	= list[9],
            rise_fall		= list[8],
            volume			= list[13],
            turn_volume		= list[12],
            week52_high		= list[16],
            week52_low		= list[17],
			market			= "香港",
		}
		--[[
			temp[0]------新浪------
temp[1]------48.98------当前价
temp[2]------ -0.57------涨幅（-0.57%）
temp[3]------2016-06-22 08:19:42------时间
temp[4]------ -0.28------涨跌
temp[5]------49.31------开盘价
temp[6]------49.83------最高价
temp[7]------48.92------最低价
temp[8]------57.01------52周最高
temp[9]------32.61------52周最低
temp[10]------280775------成交量
temp[11]------609728------10日均量
temp[12]------3443098080------市值
temp[13]------0.79------每股收益
temp[14]------62.00------市盈率
temp[15]------0.00------
temp[16]------1.15------贝塔系数
temp[17]------0.00------
temp[18]------0.00------
temp[19]------70296000------股本
temp[20]------58.00------
temp[21]------48.98------今日收盘价

		]]--
		
	end

	SaveDataMgr.Parse[Market.ShangHai]=function (list)
		local tbl= 
		{
			sname			= list[1],
            open_price		= list[2],
            yesy_price		= list[3],
            high_price		= list[5],
            low_price		= list[6],
            last_price		= list[4],
            volume			= list[9],
            turn_volume		= list[10],
			market			= "上海",
		}
		tbl.rise_fall = tbl.last_price - tbl.yesy_price
		tbl.rise_fall_per = tbl.rise_fall / tbl.yesy_price * 100

		return tbl
	end

	SaveDataMgr.Parse[Market.ShenZhen]=function (list)
		local tbl= 
		{
			sname			= list[1],
            open_price		= list[2],
            yesy_price		= list[3],
            high_price		= list[5],
            low_price		= list[6],
            last_price		= list[7],
            volume			= list[9],
            turn_volume		= list[10],
			market			= "深圳",
		}
		tbl.rise_fall = tbl.last_price - tbl.yesy_price
        tbl.rise_fall_per = tbl.rise_fall / tbl.yesy_price * 100

		return tbl
	end
end

function SaveDataMgr.SaveData()
    SettingModel.instance.LuaData=JSON:encode(SaveDataMgr.data)
    --printTbl(SaveDataMgr.data)
end

function SaveDataMgr.GetFunds()
    return table.BeTable(SaveDataMgr.data,"funds")
end

function SaveDataMgr.GetShares()
    return table.BeTable(SaveDataMgr.data,"shares")
end


--获取基金数据接口
function SaveDataMgr.GetFundData(code,OnData)
	if code=="" then
		CommonWinMgr.Open(2001)
		return
    end
    FundModel.Inst:UpdateFunCode(code,function (result,str)
		if result==1 then
            local data=JSON:decode(str) 
            local item={}
			item.code=code
			item.name=data.name
			item.rise_fall_per=tonumber(data.gszzl)/100
			item.last_price=tonumber(data.gsz)
			--item.market=data.market
			OnData(item)
		elseif result ==2 then
			CommonWinMgr.FloatText("返回数据格式异常")
		elseif result ==3 then
			CommonWinMgr.FloatText("网络请求异常")
		elseif result==4 then
			CommonWinMgr.Open(2002,nil,nil,code)
			--CommonWinMgr.FloatText(content)
		end
	end)
end

--获取基金历史数据接口
function SaveDataMgr.GetFundOldData(code,OnData)
	if code=="" then
		CommonWinMgr.Open(2001)
		return
    end
	FundHistoryModel.instance:GetFundOldData(code,function (result,str)
		if result==1 then
			local dic=JSON:decode(str)
			--printTbl(dic)
			--print(dic.fS_name)
			--print(dic.fund_Rate)
			for i,data in ipairs(dic.Data_netWorthTrend) do
				data.time=data.x/1000
				data.price=data.y
				data.percent=data.equityReturn/100
			end
		
			OnData(table.reverseTable(dic.Data_netWorthTrend))
		elseif result ==2 then
			CommonWinMgr.FloatText("返回数据格式异常")
		elseif result ==3 then
			CommonWinMgr.FloatText("网络请求异常")
		elseif result==4 then
			CommonWinMgr.Open(2002,nil,nil,code)
			--CommonWinMgr.FloatText(content)
		end
	end)
end

--获取股票数据接口
function SaveDataMgr.GetSharesData(code,OnData)
	if code=="" then
		CommonWinMgr.Open(2001)
		return
    end
    SharesModel.instance:GetDataFormSina(code,function (market,result,str)
		if result==1 then
			local list=string.split(str,",")
			--print(m)
			--printTbl(list)
            local  data=SaveDataMgr.Parse[market](list)
            local item={}
			item.code=code
			item.name=data.sname
			
			item.rise_fall_per=tonumber(data.rise_fall_per)/100
			item.last_price=tonumber(data.last_price)
			item.market=ToEnumFlag(market+1)
			OnData(item)
		elseif result ==2 then
			CommonWinMgr.FloatText("返回数据格式异常")
		elseif result ==3 then
			CommonWinMgr.FloatText("网络请求异常")
		end
	end)
end

--获取沪深,港股历史数据
function SaveDataMgr.GetSharesHistory(code,OnData)
	if code=="" then
		CommonWinMgr.Open(2001)
		return
	end
	
	SharesHistoryModel.instance:RequestChineseShares(code,function (result)
		local dic=JSON:decode(result)
		--printTbl(dic)
		
		if dic.showapi_res_body.ret_code==0 then
			local list={}
			for i,data in ipairs(dic.showapi_res_body.dataList) do
				local item={}
				item.time=data.time
				item.price_open=tonumber(data.open)
				item.price_close=tonumber(data.close)
				item.percent=(item.price_close-item.price_open)/item.price_open
				item.volume=tonumber(data.volumn)
				table.insert(list,item)
			end
			OnData(list)
		elseif dic.showapi_res_body.ret_code ==-1 then
			CommonWinMgr.FloatText(dic.showapi_res_body.remark)
		end
	end)
end
SaveDataMgr.Init()