using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.Events;

[RequireComponent(typeof(Image))]
[RequireComponent(typeof(CanvasGroup))]
public class UIDragBehaviour : MonoBehaviour, /*IPointerDownHandler,*/ IBeginDragHandler, IEndDragHandler, IDragHandler/*, IDropHandler*/
{
    [System.Serializable]
    public class DropEvent : UnityEvent<UIDropzone> { }

    protected RectTransform rTfm_this;
    protected Vector2 v2_originPos;
    protected Transform tfm_originParent;

    [SerializeField]
    protected UIDropzone[] arr_successZones = null, arr_failZones = null;
    protected UIDropzone obj_curDropZone = null;

    [SerializeField]
    protected UnityEvent obj_beginDrag = null;
    [SerializeField]
    protected DropEvent obj_success = null, obj_fail = null;

    [SerializeField]
    [Range(0.1f, 1f)]
    protected float fMoveReturnTime = 0.4f;

    [SerializeField]
    [Range(0.1f, 1f)]
    protected float fMoveMagnetTime = 0.2f;

    protected bool DragPossible { get { return GetComponent<CanvasGroup>().blocksRaycasts; } }

    public int Index { get; set; }

    [SerializeField]
    protected LeanTweenType obj_MotionType = LeanTweenType.easeInOutSine;
    protected virtual void Awake()
    {
        rTfm_this = GetComponent<RectTransform>();
        v2_originPos = rTfm_this.anchoredPosition;
        tfm_originParent = rTfm_this.parent;

        if (!GetComponent<CanvasGroup>())
            gameObject.AddComponent<CanvasGroup>();
    }

    public void SetListener(UnityAction onBeginDrag, UnityAction<UIDropzone> onSuccess, UnityAction<UIDropzone> onFail)
    {
        obj_beginDrag = new UnityEvent();
        obj_beginDrag.AddListener(onBeginDrag);
        obj_success = new DropEvent();
        obj_success.AddListener(onSuccess);
        obj_fail = new DropEvent();
        obj_fail.AddListener(onFail);
    }

    public void AddSuccessDropZone(UIDropzone zone)
    {
        UIDropzone[] dropzones = new UIDropzone[arr_successZones.Length + 1];
        arr_successZones.CopyTo(dropzones, 0);
        dropzones[arr_successZones.Length] = zone;
        arr_successZones = dropzones;
    }

    public virtual void OnBeginDrag(PointerEventData eventData)
    {
        if (obj_beginDrag != null) obj_beginDrag.Invoke();
        
        SetDragEnable(false);

        transform.SetAsLastSibling();

        if (obj_curDropZone != null)
        {
            obj_curDropZone.UnsetObject();
            obj_curDropZone = null;
        }

        UpdateDragPosition(eventData);
    }

    protected virtual void UpdateDragPosition(PointerEventData data)
    {
        if (Input.GetMouseButton(0))
        {
            Vector3 globalMousePos;
            if (RectTransformUtility.ScreenPointToWorldPointInRectangle(rTfm_this, data.position, data.pressEventCamera, out globalMousePos))
            {
                rTfm_this.position = globalMousePos;
                rTfm_this.rotation = rTfm_this.rotation;
            }
        }
    }

    public virtual void OnDrag(PointerEventData eventData)
    {
        UpdateDragPosition(eventData);
    }

    public virtual void OnEndDrag(PointerEventData eventData)
    {
        for (int i = 0; i < arr_successZones.Length; i++)
        {
            if (arr_successZones[i].CheckDrop(rTfm_this))
            {
                if (arr_successZones[i].IsFill)
                {
                    arr_successZones[i].DragBehaviour.MoveToOrinPosition();
                }

                LeanTween.move(rTfm_this.gameObject, arr_successZones[i].transform.position, fMoveMagnetTime).setOnComplete(() =>
                {
                    SetDragEnable(true);
                    obj_curDropZone = arr_successZones[i];

                    arr_successZones[i].DropObject(rTfm_this);

                    if (obj_success != null) obj_success.Invoke(arr_successZones[i]);
                }).setEase(obj_MotionType);

                return;
            }
        }

        MoveToOrinPosition();

        UIDropzone fail = null;
        for (int i = 0; i < arr_failZones.Length; i++)
        {
            if (arr_failZones[i].CheckDrop(rTfm_this))
            {
                fail = arr_failZones[i];
                break;
            }
        }
        if (obj_fail != null)
        {
            if (fail != null)
            {
                fail.CheckDrop(rTfm_this);
            }

            obj_fail.Invoke(fail);
        }
    }

    public virtual void MoveToOrinPosition(bool isMotion = true)
    {
        if (obj_curDropZone != null)
        {
            obj_curDropZone.UnsetObject();
            obj_curDropZone = null;
        }

        rTfm_this.SetParent(tfm_originParent, true);

        if (isMotion)
        {
            LeanTween.move(rTfm_this, v2_originPos, fMoveReturnTime).setEase(obj_MotionType).setOnComplete(OnReturn);
        }
        else
        {
            rTfm_this.anchoredPosition = v2_originPos;
            OnReturn();
        }
    }
    
    protected virtual void OnReturn()
    {
        SetDragEnable(true);
    }

    public UIDropzone GetSuccessDropZone(int index)
    {
        if (arr_successZones.Length > index) return arr_successZones[index];
        return null;
    }

    public void SetDragEnable(bool enable)
    {
        GetComponent<CanvasGroup>().blocksRaycasts = enable;
    }
}
