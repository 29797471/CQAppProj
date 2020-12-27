require 'Util/EnumUtil'
require 'Util/MathUtil'
require 'Util/Proxy'
require 'Util/DelegateUtil'
require 'Util/TableUtil'
require 'Util/TableSerialize'
require 'Util/StringUtil'
require 'Util/super_class'
require "Util/LuaUtils_Print"

JSON = (loadfile "Util/JSON")()
--[[
    local lua_value = JSON:decode(raw_json_text) -- decode example
    local raw_json_text    = JSON:encode(lua_table_or_value)        -- encode example
    local pretty_json_text = JSON:encode_pretty(lua_table_or_value) -- "pretty printed" version
]]--