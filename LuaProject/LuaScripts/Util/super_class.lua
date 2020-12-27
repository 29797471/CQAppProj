local _class={}
super_class = {}
local mt = {}

--超级类，模拟对象写法，实现类函数和继承
local superKey = {}

local reserved =
{
    __classname = true
}

local function build_super_class(classname, super)
    local class_type    = {}
    class_type.__init   = false
    class_type.new      = false
    class_type.super    = super
    class_type.__classname = classname

    local vtbl={}
    _class[class_type]=vtbl
    vtbl.__classname = classname

    setmetatable(class_type,{__newindex =
        function(t,k,v)
            vtbl[k]=v
        end
        ,
        __index =
        function(t,k)
            return vtbl[k]
        end
        ,
        __call =
        function(curtype, ...)
            local obj={}
            obj.__classname = classname
            --obj.__classtype = class_type
            --obj.__vtbl = vtbl
            --obj.__type = 'simple_object'

            setmetatable(obj,{ __index= _class[class_type] })
            do
                local create
                create = function(c,...)
                    if c.super then
                        local sp = c.super
                        create(sp,...)
                        --暂时不实现对父类的检验
                        --[[
                        if sp then
                            if obj[superKey] == nil then
                                obj[superKey] = {}
                            end
                            local supername = _class[sp].__classname
                            obj[SuperKey][supername] = _class[sp]
                        end
                        ]]--
                    end
                    if c.__init then
                        c.__init(obj,...)
                    end
                end

                create(class_type,...)
            end

            return obj
        end
    })

    if super then
        setmetatable(vtbl,{__index =
            function(t,k)
                local ret   = _class[super][k]
                vtbl[k]     = ret
                return ret
            end
        })
    end

    return class_type
end
        
function mt:__index(name)
    return function(...)
        local c = super_class(name, ...)
        strict_ignore_set = true;
        local e = getfenv()
        e[name] = c
        strict_ignore_set = false;
        return c
    end
end

function mt:__call(classname, super)
    return build_super_class(classname, super)
end

setmetatable(super_class, mt)

function isclass(class, object)
    local t = type(object)
    return t == 'table' and class.__classname == object.__classname
end
