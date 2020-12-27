Language = {}

function Language:GetTextData()
    local data={}
    local Miss="Miss" 
    local Convert=TextMgr.Convert
    
    data[1]=function (args) return [=[确认]=] end
    
    data[2]=function (args) return [=[取消]=] end
    
    data[3]=function (args) return [=[设置]=] end
    
    data[4]=function (args) return [=[重新请求]=] end
    
    data[5]=function (args) return [=[退出]=] end
    
    data[6]=function (args) return [=[更新]=] end
    
    data[7]=function (args) return [=[返回]=] end
    
    data[8]=function (args) return [=[关闭]=] end
    
    data[10]=function (args) return [=[]=]..Convert.text(args,1)..[=[]=] end
    
    data[11]=function (args) return [=[]=]..Convert.percent(args,1)..[=[]=] end
    
    data[12]=function (args) return [=[]=]..Convert.num(args,1)..[=[]=] end
    
    data[13]=function (args) return [=[]=]..Convert.id(args,1)..[=[]=] end
    
    data[14]=function (args) return [=[]=]..Convert.float(args,1)..[=[]=] end
    
    data[15]=function (args) return [=[]=]..Convert.time(args,1)..[=[]=] end
    
    data[16]=function (args) return [=[]=]..Convert.period(args,1)..[=[]=] end
    
    data[18]=function (args) return [=[]=]..Convert.text(args,1)..[=[ 
]=]..Convert.text(args,2)..[=[]=] end
    
    data[19]=function (args) return [=[]=]..Convert.time_ymd(args,1)..[=[]=] end
    
    data[20]=function (args) return [=[]=]..Convert.percent_color(args,1)..[=[]=] end
    
    data[21]=function (args) return [=[]=]..Convert.keep_number(args,1)..[=[]=] end
    
    data[22]=function (args) return [=[]=]..Convert.net_delay(args,1)..[=[]=] end
    
    data[100]=function (args) return [=[请确认代码正确后添加]=] end
    
    data[101]=function (args) return [=[没有这个代码<color=#ff0000>]=]..Convert.text(args,1)..[=[</color>]=] end
    
    data[102]=function (args) return [=[<color=#{ff0000}{00ff00}>]=]..Convert.percent(args,1)..[=[</color>]=] end
    
    data[103]=function (args) return [=[最新版本客户端]=]..Convert.text(args,1)..[=[,是否前往下载?]=] end
    
    data[104]=function (args) return [=[最新版本资源]=]..Convert.text(args,1)..[=[,更新文件]=]..Convert.num(args,2)..[=[个大小]=]..Convert.num(args,3)..[=[,是否下载(当前是移动网络)?]=] end
    
    data[105]=function (args) return [=[请确认网络正常后重新打开程序]=] end
    
    data[106]=function (args) return [=[]=]..Convert.text(args,1)..[=[ 
app版本:]=]..Convert.text(args,2)..[=[ 
资源版本:]=]..Convert.text(args,3)..[=[ ]=] end
    
    data[107]=function (args) return [=[Res v]=]..Convert.text(args,1)..[=[]=] end
    
    data[108]=function (args) return [=[资源更新完毕.]=] end
    
    data[109]=function (args) return [=[请求失败.]=] end
    
    data[110]=function (args) return [=[无法连接热更新服务器,继续游戏?]=] end
    
    data[111]=function (args) return [=[检测到有资源更新,确认后返回加载界面更新.]=] end
    
    data[112]=function (args) return [=[和服务器断开连接]=] end
    
    data[1001]=function (args) return [=[{股票}{基金}{名称}]=] end
    
    data[1002]=function (args) return [=[涨跌比率]=] end
    
    data[1003]=function (args) return [=[当前价]=] end
    
    data[1004]=function (args) return [=[上市]=] end
    
    data[1005]=function (args) return [=[定时刷新]=] end
    
    data[1006]=function (args) return [=[{股票}{基金}{其它}]=] end
    
    data[10000]=function (args) return [=[用户名不能为空]=] end
    
    data[10001]=function (args) return [=[用户不存在]=] end
    
    data[10002]=function (args) return [=[密码不能为空]=] end
    
    data[10003]=function (args) return [=[密码错误]=] end
    
    data[10004]=function (args) return [=[该账户未启用]=] end
    
    data[10005]=function (args) return [=[该用户不属于品牌商端]=] end
    
    data[10006]=function (args) return [=[登录成功]=] end
    
    data[10007]=function (args) return [=[退出成功]=] end
    
    data[10008]=function (args) return [=[退出失败]=] end
    
    data[10009]=function (args) return [=[登录异常]=] end
    
    data[10010]=function (args) return [=[登录失败]=] end
    
    data[10011]=function (args) return [=[用户不存在]=] end
    
    data[10012]=function (args) return [=[设备异常，将写入用户操作异常]=] end
    
    data[10013]=function (args) return [=[用户验证失败，请重新登录]=] end
    
    data[10014]=function (args) return [=[用户登录超时，请重新登录]=] end
    
    data[10015]=function (args) return [=[登录设备异常]=] end
    
    data[10016]=function (args) return [=[创建订单编号失败]=] end
    
    data[10017]=function (args) return [=[订单定制信息写入失败]=] end
    
    data[10018]=function (args) return [=[创建订单成功]=] end
    
    data[10019]=function (args) return [=[创建订单失败]=] end
    
    data[10020]=function (args) return [=[保存数据成功]=] end
    
    data[10021]=function (args) return [=[保存数据失败]=] end
    
    data[10022]=function (args) return [=[获取套装成功]=] end
    
    data[10023]=function (args) return [=[获取套装失败]=] end
    
    data[10024]=function (args) return [=[没有权限登陆移动端]=] end
    
    data[10025]=function (args) return [=[获取套装列表成功]=] end
    
    data[10026]=function (args) return [=[获取套装列表失败]=] end
    
    data[10027]=function (args) return [=[定制信息不能为空]=] end
    
    data[10028]=function (args) return [=[请确认必填信息]=] end
    
    data[10029]=function (args) return [=[转成图片失败]=] end
    
    data[10030]=function (args) return [=[服务器地址获成功]=] end
    
    data[10031]=function (args) return [=[服务器地址获失败]=] end
    
    data[10050]=function (args) return [=[左脚:]=]..Convert.num(args,1)..[=[(克)]=] end
    
    data[10051]=function (args) return [=[右脚:]=]..Convert.num(args,1)..[=[(克)]=] end
    
    data[10052]=function (args) return [=[左脚平均:]=]..Convert.num(args,1)..[=[(克)]=] end
    
    data[10053]=function (args) return [=[右脚平均:]=]..Convert.num(args,1)..[=[(克)]=] end
    
    data[10055]=function (args) return [=[{美国}{上海}{深圳}{香港}]=] end
    
    data[10101]=function (args) return [=[建仓策略:连续涨跌率超过]=]..Convert.percent(args,1)..[=[后按]=]..Convert.percent(args,2)..[=[资产建仓.]=] end
    
    data[10102]=function (args) return [=[加减仓策略:当亏损率超过]=]..Convert.percent(args,1)..[=[时仓位调整使亏损率降低到]=]..Convert.percent(args,2)..[=[以下.]=] end
    
    data[10103]=function (args) return [=[清仓策略:当盈利超过]=]..Convert.percent(args,1)..[=[后清仓.]=] end
    
    data[10104]=function (args) return [=[起投时间:]=]..Convert.time_ymd(args,1)..[=[]=] end
    
    data[10105]=function (args) return [=[总资产收益:]=]..Convert.percent_color(args,1)..[=[ 
原始价格增幅:]=]..Convert.percent_color(args,2)..[=[]=] end
    
    return data
end

return Language:GetTextData()