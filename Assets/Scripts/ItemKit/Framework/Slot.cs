using Qframework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace QFramework
{
    public class Slot
    {
        public SlotGroup SlotGroup { get; set; }
        public IItem Item { get; set; }
        public int Count { get; set; }

        public EasyEvent Changed = new EasyEvent();

        public Slot(SlotGroup slotGroup, IItem item, int count)
        {
            SlotGroup = slotGroup;
            Item = item;
            Count = count;
        }
    }
}