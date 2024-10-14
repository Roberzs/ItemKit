using UnityEngine;
using UnityEngine.EventSystems;

namespace QFramework.Example
{
    public partial class UISlot : ViewController, IBeginDragHandler, IDragHandler, IEndDragHandler, 
        IPointerEnterHandler, IPointerExitHandler
	{
        private bool _isDragging;

        public Slot Data { get; private set; }

        public UISlot InitWithSlot(Slot data)
        {
            Data = data;

            Data.Changed.Register(ReflushView);

            ReflushView();

            return this;
        }

        private void ReflushView()
        {
            if (Data != null && Data.Count != 0)
            {
                ImgIcon.Show();
                TxtName.text = Data.Item.GetName();
                if (Data.Item.IsStackable) 
                {
                    TxtCount.text = Data.Count.ToString();
                    TxtCount.Show();
                }
                else
                {
                    TxtCount.Hide();
                }
                
                ImgIcon.sprite = Data.Item.GetIcon();
            }
            else
            {
                TxtName.text = "空";
                ImgIcon.sprite = null;
                ImgIcon.Hide();
                TxtCount.text = "";
            }
        }

        private void SyncItemToMousePos()
        {
            var mousePos = Input.mousePosition;
            //var rectTransform = FindObjectOfType<UGUIInventoryExample>().transform as RectTransform;
            if (RectTransformUtility.ScreenPointToLocalPointInRectangle(transform as RectTransform, mousePos, 
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

            //var parent = FindObjectOfType<UGUIInventoryExample>().transform;
            //ImgIcon.Parent(parent);

            var canvas = ImgIcon.GetOrAddComponent<Canvas>();
            canvas.overrideSorting = true;
            canvas.sortingOrder = 100;

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

            //ImgIcon.Parent(transform);
            var canvas= ImgIcon.GetOrAddComponent<Canvas>();
            canvas.DestroySelf();
            ImgIcon.LocalPositionIdentity();

            var uiSlot = ItemKit.CurrentSlotPointerOn;
            if (uiSlot != null) 
            {
                if (uiSlot == this)
                {
                    //ImgIcon.LocalPositionIdentity();
                }
                else
                {
                    if (!uiSlot.Data.SlotGroup.CheckCondition(Data.Item))
                    {
                        return;
                    }
                    // 交换位置
                    var cacheItem = uiSlot.Data.Item;
                    var cacheCount = uiSlot.Data.Count;

                    uiSlot.Data.Item = Data.Item;
                    uiSlot.Data.Count = Data.Count;
                    Data.Item = cacheItem;
                    Data.Count = cacheCount;

                    uiSlot.Data.Changed.Trigger();
                    Data.Changed.Trigger();
                }
            }
            else
            {
                Data.Item = null;
                Data.Count = 0;

                Data.Changed.Trigger();
            }
            
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            ItemKit.CurrentSlotPointerOn = null;
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            ItemKit.CurrentSlotPointerOn = this;
        }
    }
}
