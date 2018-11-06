NestedPrefabsTest = class("NestedPrefabsTest", ViewBaseLogic)

function NestedPrefabsTest:Start(  )
	ulog("NestedPrefabsTest:Start -->> " .. "id = " .. self.__id .. "  object.name = " .. self._gameObject.name .. "  Instance.name = " .. self.__instanceName)
	

	-- 获取 挂在了 PrefabInPrefab 组件的节点
	-- local sub1 = self._gameObject.Find("Sub1")
	-- local sub2 = self._gameObject.Find("Sub2")

	-- ulog(sub1.name)
	-- ulog(sub2.name)


	-- -- 获取 PrefabInPrefab组件
	-- local comPrefabInPrefab1 = sub1:GetComponent("PrefabInPrefab")
	-- local comPrefabInPrefab2 = sub2:GetComponent("PrefabInPrefab")

	-- -- 通过 PrefabInPrefab组件的变量 获取子预设体
	-- local childPrefab1 = comPrefabInPrefab1.Child
	-- local childPrefab2 = comPrefabInPrefab2.Child

	
	-- -- 获取子预设体的LuaBehaviour组件
	-- local lb1 = childPrefab1:GetComponent("LuaBehaviour")
	-- local lb2 = childPrefab2:GetComponent("LuaBehaviour")

	-- -- 获取子预设体的lua对象
	-- local lua1 = lb1.Lua
	-- local lua2 = lb2.Lua

	local childPrefab1, lua1 = self:GetChildPrefab("Sub1")
	local childPrefab2, lua2 = self:GetChildPrefab("Sub2")

	lua1:SetName("111111111111111")
	lua2:SetName("2222222222222222222")
end
