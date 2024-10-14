using QFramework;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Qframework
{
    public class SlotGroup
    {
        private List<Slot> _slots = new List<Slot>();
        public IReadOnlyList<Slot> Slots => _slots;

        public string Key;

        public struct ItemOperateResult
        {
            public bool Succend;
            public int RemainCount;
        }

        private Func<IItem, bool> _condition = _ => true;
        
        public bool CheckCondition(IItem item)
        {
            return _condition(item);
        }

        public SlotGroup(string key) 
        {
            Key = key;
        }

        public SlotGroup CreateSlot(IItem item, int count)
        {
            _slots.Add(new Slot(this, item, count));
            return this;
        }

        public SlotGroup CreateSlotsByCount(int count)
        {
            for (int i = 0; i < count; i++)
            {
                _slots.Add(new Slot(this, null, 0));
            }
            return this;
        }

        public SlotGroup Condition(Func<IItem, bool> condition)
        {
            _condition = condition;
            return this;
        }

        public ItemOperateResult AddItem(string key, int cnt = 1)
        {
            var item = ItemKit.ItemByKeyDict[key];
            if (!item.IsStackable || !item.HasMaxStackableCount)
            {
                var slot = FindAdditiveSlotByKey(key);
                if (slot != null)
                {
                    slot.Count += cnt;
                    slot.Changed.Trigger();
                }
                else
                {
                    return new ItemOperateResult() { Succend = false, RemainCount = cnt };
                }
            }
            else 
            {
                var addCount = cnt;
                do
                {
                    var slot = FindAdditiveSlotByKey(key);
                    if (slot != null)
                    {
                        var canAddCount = item.MaxStackableCount - slot.Count;
                        if (canAddCount > addCount)
                        {
                            slot.Count += addCount;
                            slot.Changed.Trigger();
                            addCount-= addCount;
                        }
                        else
                        {
                            slot.Count += canAddCount;
                            slot.Changed.Trigger();
                            addCount -= canAddCount;
                        }
                    }
                    else
                    {
                        return new ItemOperateResult() { Succend = false, RemainCount = addCount };
                    }

                }
                while (addCount > 0);
            }
            return new ItemOperateResult() { Succend = true, RemainCount = 0 };
        }

        public bool SubItem(string key, int cnt = 1)
        {
            var slot = FindSlotByKey(key, false);
            if (slot != null)
            {
                if (slot.Count >= cnt)
                {
                    slot.Count -= cnt;
                    slot.Changed.Trigger();
                    return true;
                }
            }
            return false;
        }

        Slot FindSlotByKey(string key, bool isZeroResultsIncluded)
        {
            var solt = _slots.Find(s => s != null && s.Item != null 
                            && s.Item.GetKey() == key
                            );

            if (solt == null)
            {
                return null;
            }

            if (!isZeroResultsIncluded && solt.Count == 0)
            {
                return null;
            }
            return solt;
        }

        Slot FindEmptySlot()
        {
            return _slots.Find(s => s != null && s.Count == 0);
        }

        Slot FindAdditiveSlotByKey(string key)
        {
            var item = ItemKit.ItemByKeyDict[key];
            Slot slot = null;
            if (!item.IsStackable)
            {
                slot = FindEmptySlot();
                if (slot != null)
                {
                    slot.Item = ItemKit.ItemByKeyDict[key];
                }
            }
            else
            {
                if (!item.HasMaxStackableCount)
                {
                    slot = FindSlotByKey(key, false);
                    if (slot == null)
                    {
                        slot = FindEmptySlot();
                        if (slot != null)
                        {
                            slot.Item = ItemKit.ItemByKeyDict[key];
                        }
                    }
                }
                else
                {
                    return FindAdditiveMaxStackableCountSlotByKey(key);
                }
            }
            
            return slot;
        }

        Slot FindAdditiveMaxStackableCountSlotByKey(string key)
        {
            var item = ItemKit.ItemByKeyDict[key];
            //var slot = FindSlotByKey(key, false);
            foreach (var tmpSlot in _slots)
            {
                if (tmpSlot.Item != null && tmpSlot.Item.GetKey() == key && item.MaxStackableCount > tmpSlot.Count)
                {
                    return tmpSlot;
                }
            }
            var slot = FindEmptySlot();
            if (slot != null)
            {
                slot.Item = ItemKit.ItemByKeyDict[key];
            }
            return slot;
        }
    }
}

