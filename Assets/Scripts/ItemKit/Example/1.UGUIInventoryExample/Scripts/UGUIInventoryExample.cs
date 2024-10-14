using UnityEngine;
using QFramework;
using Qframework.Example;

// 1.请在菜单 编辑器扩展/Namespace Settings 里设置命名空间
// 2.命名空间更改后，生成代码之后，需要把逻辑代码文件（非 Designer）的命名空间手动更改
namespace QFramework.Example
{
	public partial class UGUIInventoryExample : ViewController
	{
        const string InventoryName = "物品栏";
        const string BagName = "背包";
        const string EquipName = "装备";

        private void Awake()
        {
            // Initdata
            ItemKit.LoadItemDatabase("ExampleItemDatabase");

            ItemKit.CreateSlotGroup(InventoryName)
                .CreateSlot(ItemKit.ItemByKeyDict[Items.Item_Iron], 1)
                .CreateSlot(ItemKit.ItemByKeyDict[Items.Item_GreenSword], 1)
                .CreateSlot(null, 0);

            ItemKit.CreateSlotGroup(BagName)
                .CreateSlotsByCount(20);

            ItemKit.CreateSlotGroup(EquipName)
                .CreateSlotsByCount(1)
                .Condition(Item=>Item.GetBoolean("IsWeapon"));

            var weaponSlot = ItemKit.GetSlotGroupByKey(EquipName).Slots[0];
            weaponSlot.Changed.Register(() =>
            {
                Debug.Log("武器变更");
            });

        }

        void Start()
		{
            // Code Here
            UIInventory.Hide();

			// 按钮
			foreach (var item in ItemKit.Items)
			{
                var key = item.GetKey();
				UIInventory.InstantiateWithParent(UIInventoryRoot)
					.Self(self =>
					{
                        self.TxtName.text = item.GetName();
                        self.BtnAdd.onClick.AddListener(() =>
                        {
                            var res = ItemKit.GetSlotGroupByKey(InventoryName).AddItem(key, 10);
                            if (!res.Succend)
                            {
                                Debug.LogWarning("背包满了 有" + res.RemainCount + "个物品为添加成功");
                            }
                            //ReflushUISlot();
                        });
                        self.BtnSub.onClick.AddListener(() =>
                        {
                            ItemKit.GetSlotGroupByKey(InventoryName).SubItem(key, 1);
                            //ReflushUISlot();
                        });

                    }).Show();
			}

            //ReflushUISlot();

        }
	}
}
