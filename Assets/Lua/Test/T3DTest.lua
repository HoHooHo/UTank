T3DTest = class("T3DTest", ViewBaseLogic)

function T3DTest:Start(  )
	ulog("T3DTest:Start -->> " .. "id = " .. self.__id .. "  object.name = " .. self._gameObject.name .. "  Instance.name = " .. self.__instanceName)
	self._luaBehaviour:AddClick("BG/Create", self.OnCreate3D)
end

function T3DTest:OnDestroy(  )
	ulog("T3DTest:Destroy -->> " .. "id = " .. self.__id .. "  Instance.name = " .. self.__instanceName)
end

function T3DTest:OnCreate3D(  )

	local cb = function ( go )
		self.t3d = go
		go.transform.localPosition = Vector3.New(0, 5, 0)
	end

	UIManager.Open("Goblin_rouge_b", cb, self, nil, "test3d")
end

function T3DTest:OnPointerDown(  )
	ulog("T3DTest:OnPointerDown")
end

function T3DTest:OnDrag( eventData )
	ulog("T3DTest:OnDrag")
	if self.t3d then
		self.t3d.transform:Rotate(self.t3d.transform.localRotation.x, self.t3d.transform.localRotation.y - eventData.delta.x, self.t3d.transform.localRotation.z)
	end

end

function T3DTest:OnPointerUp(  )
	ulog("T3DTest:OnPointerUp")
end