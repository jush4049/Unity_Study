using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public enum DropZoneType
{
    Replaceable,
    NotReplaceable
}

public enum DropCheckType
{
    Distance,
    Overlap
}

public class UIDropzone : MonoBehaviour
{
    [SerializeField]
    private DropZoneType obj_DropZoneType = DropZoneType.Replaceable;

    [SerializeField]
    private DropCheckType obj_checkType = DropCheckType.Distance;

    [SerializeField]
    [Range(1, 10)]
    private float fCheckDistance = 1f;

    [SerializeField]
    UnityEvent obj_dropped = null;

    private RectTransform rTfm_this;

    private RectTransform rTfm_fill;

    public int Index { get; set; }

    public bool IsFill
    {
        get { return rTfm_fill != null; }
    }
    public RectTransform FillRect
    {
        get { return rTfm_fill; }
    }

    public UIDragBehaviour DragBehaviour
    {
        get
        {
            if (FillRect)
                if (FillRect.GetComponent<UIDragBehaviour>())
                    return FillRect.GetComponent<UIDragBehaviour>();
            return null;
        }
    }

    void Awake()
    {
        rTfm_this = GetComponent<RectTransform>();
    }

    public void AddDropListener(UnityAction dropped)
    {
        obj_dropped.AddListener(dropped);
    }

    public virtual bool CheckDrop(RectTransform itemRect)
    {
        if (itemRect == null || rTfm_this == null) return false;

        switch (obj_checkType)
        {
            case DropCheckType.Distance:
                {
                    float distance = Vector2.Distance(itemRect.position, rTfm_this.position);
                    if (distance < fCheckDistance)
                    {
                        if (IsFill && obj_DropZoneType == DropZoneType.NotReplaceable)
                            return false;
                        else
                            return true;
                    }
                    break;
                }
            case DropCheckType.Overlap:
                {
                    if (IsOverlaps(rTfm_this, itemRect))
                    {
                        if (IsFill && obj_DropZoneType == DropZoneType.NotReplaceable)
                            return false;
                        else
                            return true;
                    }
                    break;
                }
        }
        return false;
    }

    public void DropObject(RectTransform itemRect)
    {
        rTfm_fill = itemRect;
        if (DragBehaviour != null)
            if (obj_DropZoneType == DropZoneType.NotReplaceable)
                DragBehaviour.SetDragEnable(false);
        if (obj_dropped != null) obj_dropped.Invoke();
    }

    public void UnsetObject()
    {
        rTfm_fill = null;
    }

    protected bool IsOverlaps(RectTransform rectTrans1, RectTransform rectTrans2)
    {
        Rect rect1 = new Rect(rectTrans1.localPosition.x, rectTrans1.localPosition.y, rectTrans1.rect.width, rectTrans1.rect.height);
        Rect rect2 = new Rect(rectTrans2.localPosition.x, rectTrans2.localPosition.y, rectTrans2.rect.width, rectTrans2.rect.height);

        return rect1.Overlaps(rect2);
    }
}