using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace QFramework
{
    public class Slot
    {
        public Item Item { get; set; }
        public int Count { get; set; }

        public Slot(Item item, int count)
        {
            Item = item;
            Count = count;
        }
    }
}