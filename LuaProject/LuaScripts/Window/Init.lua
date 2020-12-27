require 'Window/Base/LuaWin'

--创建lua窗口table
function CreateWindow(winLuaFilePath)
    
    require(winLuaFilePath)
    local nIndex = string.refind(winLuaFilePath, "/", 0)
    local winName=string.sub(winLuaFilePath,nIndex+1)
    local winTbl=_G[winName]()
    return winTbl
end
