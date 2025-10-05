-- Backpack items definition in Lua (XLua)
-- Return a table with an array field `items`.
-- Each item: { itemId=string, displayName=string, iconAddress=string, maxStack=number, usedAction=function() ... end }

return {
    items = {
        {
            itemId = "potion",
            displayName = "小型生命药水",
            iconAddress = "Icons/potion.png",
            maxStack = 99,
            usedAction = function()
                CS.LikeLoL04.EventSystem.EventBus.Emit("AddHp", { "10" })
            end
        },
        {
            itemId = "sword",
            displayName = "铁剑",
            iconAddress = "Icons/sword.png",
            maxStack = 1,
            usedAction = function()
                CS.LikeLoL04.EventSystem.EventBus.Emit("AddAttack", { "10" })
            end
        },
        {
            itemId = "controlGuards",
            displayName = "控制守卫",
            iconAddress = "Icons/controlGuards.png",
            maxStack = 99
        },
        {
            itemId = "DoranBlade",
            displayName = "多兰之刃",
            iconAddress = "Icons/DoranBlade.png",
            maxStack = 99,
            usedAction = function()
                CS.LikeLoL04.EventSystem.EventBus.Emit("AddAttack", { "10" })
            end
        }
    }
}
