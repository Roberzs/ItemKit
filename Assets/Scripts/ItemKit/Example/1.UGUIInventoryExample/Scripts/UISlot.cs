using UnityEngine;
using QFramework;
using UnityEngine.EventSystems;
using UnityEngine.Profiling.Memory.Experimental;

namespace QFramework.Example
{
    public partial class UISlot : ViewController, IBeginDragHandler, IDragHandler, IEndDragHandler
	{
        private bool _isDragging;

        public Slot Data { get; private set; }

        public UISlot InitWithSlot(Slot data)
        {
            Data = data;

            if (data != null && data.Count != 0)
            {
                ImgIcon.Show();
                TxtName.text = data.Item.Name;
                TxtCount.text = data.Count.ToString();
            }
            else
            {
                TxtName.text = "空";
                ImgIcon.sprite = null;
                ImgIcon.Hide();
                TxtCount.text = "";
            }
            return this;
        }

        private void SyncItemToMousePos()
        {
            var mousePos = Input.mousePosition;
            var rectTransform = FindObjectOfType<UGUIInventoryExample>().transform as RectTransform;
            if (RectTransformUtility.ScreenPointToLocalPointInRectangle(rectTransform, mousePos, 
                null, out var localPos))
            {
                ImgIcon.LocalPosition2D(localPos);
            }
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            if (_isDragging || Data.Count == 0)
                return;
            _isDragging = true;

            var parent = FindObjectOfType<UGUIInventoryExample>().transform;
            ImgIcon.Parent(parent);

            SyncItemToMousePos();
        }

        public void OnDrag(PointerEventData eventData)
        {
            if (_isDragging)
            {
                SyncItemToMousePos();
            }
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            if (!_isDragging)
                return;

            _isDragging = false;

            ImgIcon.Parent(transform);

            var isThrowItem = true;

            var uiSlots = transform.parent.GetComponentsInChildren<UISlot>();
            foreach (var uiSlot in uiSlots)
            {
                var rectTransform = uiSlot.transform as RectTransform;
                if (RectTransformUtility.RectangleContainsScreenPoint(rectTransform, Input.mousePosition))
                {
                    isThrowItem = false;

                    if (uiSlot == this)
                    {
                        ImgIcon.LocalPositionIdentity();
                    }
                    else
                    {
                        // 交换位置
                        var cacheItem = uiSlot.Data.Item;
                        var cacheCount = uiSlot.Data.Count;

                        uiSlot.Data.Item = Data.Item;
                        uiSlot.Data.Count = Data.Count;
                        Data.Item = cacheItem;
                        Data.Count = cacheCount;

                        FindObjectOfType<UGUIInventoryExample>().ReflushUISlot();
                    }
                    
                    break;
                }
            }

            if (isThrowItem)
            {
                Data.Item = null;
                Data.Count = 0;
                FindObjectOfType<UGUIInventoryExample>().ReflushUISlot();
            }
            
        }

        
	}
}
