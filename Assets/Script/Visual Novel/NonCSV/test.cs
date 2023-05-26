using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class test : MonoBehaviour
{
    public GameObject next_button;
    public int index;
    public int last_index;
    void Start()
    {
        Story();
    }

    void Update()
    {
        // ��� ��ũ��Ʈ ���� �Ϸ� ��, ���� ��ư�� ��Ȱ��ȭ (+ �߰� �Լ� ����)
        if (index == last_index)
        {
            next_button.SetActive(false);
        }
    }
    public void skip()
    {
        dialog.instance.skip(index);
    }

    // ��ũ��Ʈ ��� ����
    public void Story()
    {
        index = 0;
        if (dialog.instance.dialog_read(index) && !dialog.instance.running)
        {
            IEnumerator dialog_co = dialog.instance.dialog_system_start(0);
            StartCoroutine(dialog_co);
        }
    }

    // ���� ��ũ��Ʈ ���
    public void NextStory()
    {
        next_button.SetActive(false);
        index++;
        if (dialog.instance.dialog_read(index) && !dialog.instance.running)
        {
            IEnumerator dialog_co = dialog.instance.dialog_system_start(index);
            StartCoroutine(dialog_co);
        }
    }
}
