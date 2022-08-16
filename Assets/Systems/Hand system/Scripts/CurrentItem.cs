using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems; 



namespace VirtualLab.HandSystem 
{

public class CurrentItem 
{
    Camera camera; 



    public CurrentItem () 
    {
        camera = Camera.main; 
    }



    //  Current items  ---------------------------------------------- 
    AbstractHandOverItem _handOverItem; 
    public AbstractHandOverItem handOverItem 
    {
                get => _handOverItem; 
        private set => _UpdateItem(ref _handOverItem, value); 
    }

    AbstractTouchItem _touchItem; 
    public AbstractTouchItem touchItem 
    {
                get => _touchItem; 
        private set => _UpdateItem(ref _touchItem, value); 
    }
    
    AbstractDragItem _dragItem; 
    public AbstractDragItem dragItem 
    {
                get => _dragItem; 
        private set => _UpdateItem(ref _dragItem, value); 
    }

    void _UpdateItem<T> (ref T currentItem, T newItem) where T: HandItem 
    {
        if (currentItem == newItem) return; 

        if (currentItem != null) onItemLost(currentItem); 
        currentItem = newItem; 
        if (currentItem != null) onItemFound(currentItem); 
    }



    //  Events  ----------------------------------------------------- 
    public delegate void EventHandler (HandItem item); 
    public event EventHandler onItemFound = delegate {}; 
    public event EventHandler onItemLost  = delegate {}; 



    //  Updating  --------------------------------------------------- 
    public void Update () 
    {
        GetItemsUnderHand(out List<HandItem> itemsUnderHand, out List<float> distances); 
        closestItems.Update(itemsUnderHand, distances); 

        handOverItem = closestItems.handOverItem; 
        touchItem    = closestItems.touchItem; 
        dragItem     = closestItems.dragItem; 

        FilterOutInactive(); 
    }

    bool CanUseItem (HandItem item) 
    {
        return item != null && item.active; 
    }



    //  Items under hand  ------------------------------------------- 
    void GetItemsUnderHand (out List<HandItem> items, out List<float> distances) 
    {
        items = new List<HandItem>(); 
        distances = new List<float>(); 

        RaycastHit [] hitInfoList = RaycastAll(); 

        foreach (RaycastHit hitInfo in hitInfoList) 
        {
            ExtractHandItems(hitInfo, items, distances); 
        }
    }

    RaycastHit [] RaycastAll () 
    {
        Ray ray = camera.ScreenPointToRay(Input.mousePosition); 
        return Physics.RaycastAll(ray, float.MaxValue); 
    }

    void ExtractHandItems (RaycastHit hitInfo, List<HandItem> items, List<float> distances) 
    {
        HandItem [] itemsOnObject = hitInfo.collider.GetComponentsInParent<HandItem>(); 

        foreach (HandItem item in itemsOnObject) 
        {
            if (CanUseItem(item)) 
            {
                items.Add(item); 
                distances.Add(hitInfo.distance); 
            }
        }
    }



    //  Closest items  ---------------------------------------------- 
    ClosestItems closestItems = new ClosestItems(); 
    class ClosestItems 
    {
        float distanceHandOver; 
        float distanceTouch; 
        float distanceDrag; 

        public AbstractHandOverItem handOverItem { get; private set; }
        public AbstractTouchItem    touchItem    { get; private set; }
        public AbstractDragItem     dragItem     { get; private set; }
        

        void Init () 
        {
            distanceHandOver = float.MaxValue; 
            distanceTouch = float.MaxValue; 
            distanceDrag = float.MaxValue; 

            handOverItem = null; 
            touchItem = null; 
            dragItem = null; 
        }

        public void Update (List<HandItem> items, List<float> distances) 
        {
            Init(); 

            for (int i = 0; i < items.Count; i++) 
            {
                UpdateItem(items[i], distances[i]); 
            }
        }

        void UpdateItem (HandItem item, float distance) 
        {
            if (item is AbstractHandOverItem && distance < distanceHandOver) 
            {
                handOverItem = (AbstractHandOverItem) item; 
                distanceHandOver = distance; 
            } 
            else if (item is AbstractTouchItem && distance < distanceTouch) 
            {
                touchItem = (AbstractTouchItem) item; 
                distanceTouch = distance; 
            }
            else if (item is AbstractDragItem && distance < distanceDrag) 
            {
                dragItem = (AbstractDragItem) item; 
                distanceDrag = distance; 
            }
        }
    }








    void Something (List<HandItem> itemsUnderHand) 
    {
        bool handOverFound = false; 
        bool touchFound = false; 
        bool dragFound = false; 

        foreach (HandItem item in itemsUnderHand) 
        {
            if (!handOverFound && item is AbstractHandOverItem) 
            {
                handOverItem = (AbstractHandOverItem) item; 
                handOverFound = true; 
            }
            else if (!touchFound && item is AbstractTouchItem) 
            {
                touchItem = (AbstractTouchItem) item; 
                touchFound = true; 
            }
            else if (!dragFound && item is AbstractDragItem) 
            {
                dragItem = (AbstractDragItem) item; 
                dragFound = true; 
            }

            if (handOverFound && touchFound && dragFound) break; 
        }

        if (!handOverFound) handOverItem = null; 
        if (!touchFound)    touchItem = null; 
        if (!dragFound)     dragItem = null; 
    }

    void FilterOutInactive () 
    {
        if (handOverItem != null && !handOverItem.active) handOverItem = null; 
        if (touchItem != null    && !touchItem.active)    touchItem = null; 
        if (dragItem != null     && !dragItem.active)     dragItem = null; 
    }

    


}

}
