
function clone(object)
    local lookup_table = {}
    local function _copy(object)
        if type(object) ~= "table" then
            return object
        elseif lookup_table[object] then
            return lookup_table[object]
        end
        local new_table = {}
        lookup_table[object] = new_table
        for key, value in pairs(object) do
            new_table[_copy(key)] = _copy(value)
        end
        return setmetatable(new_table, getmetatable(object))
    end
    return _copy(object)
end


local id = 0

--Create an class.
function class(classname, super)
    local cls
        -- inherited from Lua Object
    if super then
        cls = clone(super)
        cls.super = super
    else
        cls = {AutoInit = function() end}
    end

    cls.__clsName = classname
    cls.__index = cls
    cls.__id = 0

    function cls.New(...)
        id = id + 1

        local instance = setmetatable({}, cls)
        instance.__id = id
        instance.__instanceName = instance.__clsName .. "_" .. instance.__id
        instance.class = cls
        instance:AutoInit()
        return instance
    end

    return cls
end