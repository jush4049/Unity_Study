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
        private Toggle toggle_Base; // ���� ���� ������ �ε�������

        private List<Toggle> list_Toggles = new List<Toggle>(); // ������ �ε������͸� ����

        void Awake()
        {
            // ���� ���� ������ �ε������ʹ� ��Ȱ��ȭ���� �д�
            toggle_Base.gameObject.SetActive(false);
        }

        // ������ ���� �����ϴ� �޼���
        public void SetNumberOfPages(int number)
        {
            if (list_Toggles.Count < number)
            {
                // ������ �ε������� ���� ������ ������ ������ ������
                // ���� ���� ������ �ε������ͷκ��� ���ο� ������ �ε������͸� �ۼ��Ѵ�
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
                // ������ �ε������� ���� ������ ������ ������ ������ �����Ѵ�
                for (int i = list_Toggles.Count - 1; i >= number; i--)
                {
                    Destroy(list_Toggles[i].gameObject);
                    list_Toggles.RemoveAt(i);
                }
            }
        }

        // ���� �������� �����ϴ� �޼���
        public void SetCurrentPage(int index)
        {
            if (index >= 0 && index <= list_Toggles.Count - 1)
            {
                // ������ �������� �����ϵǴ� ������ �ε������͸� ON���� �����Ѵ�
                // ��� �׷��� �����صξ����Ƿ� �ٸ� �ε������ʹ� �ڵ����� OFF�� �ȴ�
                list_Toggles[index].isOn = true;
            }
        }
    }
}

