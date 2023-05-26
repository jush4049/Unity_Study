using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using static UnityEditor.PlayerSettings;
using static UnityEngine.GraphicsBuffer;

public class DragTest : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public CanvasGroup canvasGroup = null;
    public Transform basePos;
    private Vector3 pos;

    void Start()
    {
        /*pos = Camera.main.ViewportToScreenPoint(transform.position);
        transform.position = Camera.main.ViewportToWorldPoint(pos);*/
        transform.position = basePos.position;
    }
    public void OnBeginDrag(PointerEventData eventData)
    {
        canvasGroup.blocksRaycasts = false;
        Debug.Log("드래그 전 세팅");
    }

    public void OnDrag(PointerEventData eventData)
    {
        transform.position = Input.mousePosition;
        Debug.Log("드래그 하는중!");
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        canvasGroup.blocksRaycasts = true;
        transform.position = basePos.position;
        Debug.Log("드래그 끝");
        List<GameObject> hoveredList = eventData.hovered;
        if (hoveredList.Exists(x => x.gameObject.name.Contains("DropArea")))
        {
            print("dd");
        }
    }

}
