using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class UIPageControl : MonoBehaviour
    {
        [SerializeField]
        private Toggle toggle_Base; // 복사 원본 페이지 인디케이터

        private List<Toggle> list_Toggles = new List<Toggle>(); // 페이지 인디케이터를 저장

        void Awake()
        {
            // 복사 원본 페이지 인디케이터는 비활성화시켜 둔다
            toggle_Base.gameObject.SetActive(false);
        }

        // 페이지 수를 설정하는 메서드
        public void SetNumberOfPages(int number)
        {
            if (list_Toggles.Count < number)
            {
                // 페이지 인디케이터 수가 지정된 페이지 수보다 적으면
                // 복사 원본 페이지 인디케이터로부터 새로운 페이지 인디케이터를 작성한다
                for (int i = list_Toggles.Count; i < number; i++)
                {
                    Toggle indicator = Instantiate(toggle_Base) as Toggle;
                    indicator.gameObject.SetActive(true);
                    indicator.transform.SetParent(toggle_Base.transform.parent);
                    indicator.transform.localScale = toggle_Base.transform.localScale;
                    indicator.isOn = false;
                    list_Toggles.Add(indicator);
                }
            }
            else if (list_Toggles.Count > number)
            {
                // 페이지 인디케이터 수가 지정된 페이지 수보다 많으면 삭제한다
                for (int i = list_Toggles.Count - 1; i >= number; i--)
                {
                    Destroy(list_Toggles[i].gameObject);
                    list_Toggles.RemoveAt(i);
                }
            }
        }

        // 현재 페이지를 설정하는 메서드
        public void SetCurrentPage(int index)
        {
            if (index >= 0 && index <= list_Toggles.Count - 1)
            {
                // 지정된 페이지에 대응하되는 페이지 인디케이터를 ON으로 지정한다
                // 토글 그룹을 설정해두었으므로 다른 인디케이터는 자동으로 OFF가 된다
                list_Toggles[index].isOn = true;
            }
        }
    }
}

