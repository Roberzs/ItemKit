using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace QFramework
{
    public class Slot
    {
        public IItem Item { get; set; }
        public int Count { get; set; }

        public Slot(IItem item, int count)
        {
            Item = item;
            Count = count;
        }
    }
}