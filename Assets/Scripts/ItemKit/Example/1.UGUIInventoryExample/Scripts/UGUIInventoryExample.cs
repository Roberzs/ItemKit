using UnityEngine;
using QFramework;

// 1.请在菜单 编辑器扩展/Namespace Settings 里设置命名空间
// 2.命名空间更改后，生成代码之后，需要把逻辑代码文件（非 Designer）的命名空间手动更改
namespace QFramework.Example
{
	public partial class UGUIInventoryExample : ViewController
	{
		void Start()
		{
			// Code Here

			UISlot.Hide();
			UIInventory.Hide();

			// 按钮
			foreach (var item in ItemKit.Items)
			{
                var key = item.Key;
				UIInventory.InstantiateWithParent(UIInventoryRoot)
					.Self(self =>
					{
                        self.TxtName.text = item.Name;
                        self.BtnAdd.onClick.AddListener(() =>
                        {
                            ItemKit.AddItem(key, 1);
                            ReflushUISlot();
                        });
                        self.BtnSub.onClick.AddListener(() =>
                        {
                            ItemKit.SubItem(key, 1);
                            ReflushUISlot();
                        });

                    }).Show();
			}

            ReflushUISlot();

        }

		public void ReflushUISlot()
		{
			UISlotRoot.DestroyChildren();

            foreach (var slot in ItemKit.Slots)
            {
                UISlot.InstantiateWithParent(UISlotRoot)
                    .InitWithSlot(slot)
                    .Show();
            }
        }
	}
}
