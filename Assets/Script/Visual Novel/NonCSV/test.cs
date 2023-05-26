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
        // 모든 스크립트 실행 완료 후, 다음 버튼은 비활성화 (+ 추가 함수 실행)
        if (index == last_index)
        {
            next_button.SetActive(false);
        }
    }
    public void skip()
    {
        dialog.instance.skip(index);
    }

    // 스크립트 출력 시작
    public void Story()
    {
        index = 0;
        if (dialog.instance.dialog_read(index) && !dialog.instance.running)
        {
            IEnumerator dialog_co = dialog.instance.dialog_system_start(0);
            StartCoroutine(dialog_co);
        }
    }

    // 다음 스크립트 출력
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
