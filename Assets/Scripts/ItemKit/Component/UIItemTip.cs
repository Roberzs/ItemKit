using UnityEngine;
using QFramework;

// 1.请在菜单 编辑器扩展/Namespace Settings 里设置命名空间
// 2.命名空间更改后，生成代码之后，需要把逻辑代码文件（非 Designer）的命名空间手动更改
namespace QFramework.Example
{
	public partial class UIItemTip : ViewController
	{
		private static UIItemTip _instance;

        private void Awake()
        {
            _instance = this;
        }

        void Start()
		{
			// Code Here
			gameObject.Hide();
		}

		public static void Show(UISlot uiSlot)
		{
			if (uiSlot.Data != null && uiSlot.Data.Item != null)
			{
                _instance.Show();
                _instance.ImgIcon.sprite = uiSlot.Data.Item.GetIcon();
                _instance.TxtName.text = uiSlot.Data.Item.GetName();
                _instance.TxtDescription.text = uiSlot.Data.Item.GetDescription();

				var pos = uiSlot.Position2D();
				
				var posToCenter = pos - new Vector2(Screen.width, Screen.height) / 2;

				float offsetScale = 30;
				if (posToCenter.x.Abs() > posToCenter.y.Abs())
				{
					if (posToCenter.x < 0)
					{
						_instance.SelfRectTransform.pivot = new Vector2(0, .5f);
                        _instance.transform.Position2D(pos + Vector2.right * offsetScale);
					}
					else 
					{
                        _instance.SelfRectTransform.pivot = new Vector2(1, .5f);
                        _instance.transform.Position2D(pos - Vector2.right * offsetScale);
                    }
				}
				else
				{
					if (posToCenter.y < 0) 
					{
                        _instance.SelfRectTransform.pivot = new Vector2(.5f, 0f);
                        _instance.transform.Position2D(pos + Vector2.up * offsetScale);
                    }
					else
					{
                        _instance.SelfRectTransform.pivot = new Vector2(.5f, 1f);
                        _instance.transform.Position2D(pos - Vector2.up * offsetScale);
                    }
				}
            }
		}

		public static void Hide()
		{
            _instance.Hide();
		}
	}
}
