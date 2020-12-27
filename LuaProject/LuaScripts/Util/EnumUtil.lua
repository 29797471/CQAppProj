--定义一个枚举 索引从startIndex开始,默认从1开始
function CreatEnumTable(tbl,startIndex)
    local enumtbl = {name={}}
    if startIndex==nil then
        startIndex=1
    end
    for i, v in ipairs(tbl) do
        local index =  startIndex + i -1
        enumtbl[v] = index
        enumtbl.name[index]=v
    end
    return enumtbl
end