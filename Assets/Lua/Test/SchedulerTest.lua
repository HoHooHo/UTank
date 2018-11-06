SchedulerTest = class("SchedulerTest", ViewBaseLogic)

function SchedulerTest:Start(  )
	ulog("SchedulerTest:Start -->> " .. "id = " .. self.__id .. "  object.name = " .. self._gameObject.name .. "  Instance.name = " .. self.__instanceName)
	self._luaBehaviour:AddClick("BG/scheduleOnce", self.TestScheduleOnce)
	self._luaBehaviour:AddClick("BG/schedule", self.TestSchedule)
	self._luaBehaviour:AddClick("BG/unschedule", self.TestUnschedule)
	self._luaBehaviour:AddClick("BG/unscheduleByTarget", self.TestUnscheduleByTarget)
	self._luaBehaviour:AddClick("BG/unscheduleAll", self.TestUnscheduleAll)
	self._luaBehaviour:AddClick("BG/hide", self.Hide)
end

local o = 0
local s = 0

local function OnO( data )
	entryIdO = nil
	o = o + 1
	ulog(data .. "  =======  " .. o)
end

local function OnS( data )
	s = s + 1
	ulog(data .. "*******  " .. s)
end

local entryIdO = nil
local entryIdS = nil

function SchedulerTest:TestScheduleOnce(  )
	ulog("*** SchedulerTest:ScheduleOnce ***")
	-- entryIdO = Scheduler.ScheduleOnce(OnO, 3, "TestO", self._gameObject)
	entryIdO = self:ScheduleOnce(OnO, 3, "TestO")
end

function SchedulerTest:TestSchedule(  )
	ulog("*** SchedulerTest:Schedule ***")
	-- entryIdS = Scheduler.Schedule(OnS, 1, "TestS", self._gameObject)
	entryIdS = self:Schedule(OnS, 1, "TestS")
end

function SchedulerTest:TestUnschedule(  )
	ulog("*** SchedulerTest:Unschedule ***")

	if entryIdO then
		-- Scheduler.Unschedule(entryIdO, self._gameObject)
		self:Unschedule(entryIdO)
		entryIdO = nil
	end


	if entryIdS then
		-- Scheduler.Unschedule(entryIdS, self._gameObject)
		self:Unschedule(entryIdS)
		entryIdS = nil
	end
end

function SchedulerTest:TestUnscheduleByTarget(  )
	ulog("*** SchedulerTest:UnscheduleByTarget ***")
	-- Scheduler.UnscheduleByTarget(self._gameObject)
	self:ClearSchedule()
end

function SchedulerTest:TestUnscheduleAll(  )
	ulog("*** SchedulerTest:UnscheduleAll ***")

	Scheduler.UnscheduleAll()
end

function SchedulerTest:Hide(  )
	ulog("*** SchedulerTest:hide ***")

	self:SetActive(false)

	self:ScheduleOnce(function (  )
		self:SetActive(true)
	end, 5 )
end