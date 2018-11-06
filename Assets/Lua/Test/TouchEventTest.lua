TouchEventTest = class("TouchEventTest", ViewBaseLogic)

function TouchEventTest:Start(  )
	ulog("TouchEventTest:Start -->> " .. "id = " .. self.__id .. "  object.name = " .. self._gameObject.name .. "  Instance.name = " .. self.__instanceName)
	self.text = self:GetComponent("Text", "BG/Text")
end

local function SetString( self, str )
	ulog(str)
	self.text.text = str
end


function TouchEventTest:OnBeginDrag( eventData )
	SetString(self, "TouchEventTest:OnBeginDrag")
end

function TouchEventTest:OnCancel( eventData )
	SetString(self, "TouchEventTest:OnCancel")
end

function TouchEventTest:OnDeselect( eventData )
	SetString(self, "TouchEventTest:OnDeselect")
end

function TouchEventTest:OnDrag( eventData )
	SetString(self, "TouchEventTest:OnDrag")
end

function TouchEventTest:OnDrop( eventData )
	SetString(self, "TouchEventTest:OnDrop")
end

function TouchEventTest:OnEndDrag( eventData )
	SetString(self, "TouchEventTest:OnEndDrag")
end

function TouchEventTest:OnInitializePotentialDrag( eventData )
	SetString(self, "TouchEventTest:OnInitializePotentialDrag")
end

function TouchEventTest:OnMove( eventData )
	SetString(self, "TouchEventTest:OnMove")
end

function TouchEventTest:OnPointerClick( eventData )
	SetString(self, "TouchEventTest:OnPointerClick")
end

function TouchEventTest:OnPointerDown( eventData )
	SetString(self, "TouchEventTest:OnPointerDown")
end

function TouchEventTest:OnPointerEnter( eventData )
	SetString(self, "TouchEventTest:OnPointerEnter")
end

function TouchEventTest:OnPointerExit( eventData )
	SetString(self, "TouchEventTest:OnPointerExit")
end

function TouchEventTest:OnPointerUp( eventData )
	SetString(self, "TouchEventTest:OnPointerUp")
end

function TouchEventTest:OnScroll( eventData )
	SetString(self, "TouchEventTest:OnScroll")
end

function TouchEventTest:OnSelect( eventData )
	SetString(self, "TouchEventTest:OnSelect")
end

function TouchEventTest:OnSubmit( eventData )
	SetString(self, "TouchEventTest:OnSubmit")
end

function TouchEventTest:OnUpdateSelected( eventData )
	SetString(self, "TouchEventTest:OnUpdateSelected")
end