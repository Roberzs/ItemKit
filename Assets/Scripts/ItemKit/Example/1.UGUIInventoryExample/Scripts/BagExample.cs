using UnityEngine;
using QFramework;

// 1.请在菜单 编辑器扩展/Namespace Settings 里设置命名空间
// 2.命名空间更改后，生成代码之后，需要把逻辑代码文件（非 Designer）的命名空间手动更改
namespace QFramework.Example
{
	public partial class BagExample : ViewController
	{
		void Start()
		{
			// Code Here

			// Init data
			for (int i = 0; i < 20; i++) 
			{
				ItemKit.BagSlots.Add(new Slot(null, 0));
			}

			UISlot.Hide();
            ReflushUISlot();

        }

        public void ReflushUISlot()
        {
            UISlotRoot.DestroyChildren();

            foreach (var slot in ItemKit.BagSlots)
            {
                UISlot.InstantiateWithParent(UISlotRoot)
                    .InitWithSlot(slot)
                    .Show();
            }
        }
    }
}
