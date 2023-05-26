using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
[System.Serializable]
public class dialog_info
{
    public string name;
    [TextArea(3, 5)]
    public string content;
    public bool check_read;
}

[System.Serializable]
public class Dialog_cycle
{
    public string cycle_name;
    public List<dialog_info> info = new List<dialog_info>();
    public int cycle_index;
    public bool check_cycle_read;
}


public class dialog : MonoBehaviour
{
    [SerializeField]
    public static dialog instance = null;
    public List<Dialog_cycle> dialog_cycles = new List<Dialog_cycle>(); // ��ȭ ���� �׷�
    public Queue<string> text_seq = new Queue<string>();                // ��ȭ �������� ������ ť�� �����Ѵ�.(������ ���� �Ǵ��ϱ� ����)
    public string name_;                                                // �ӽ÷� ������ ��ȭ ������ �̸�
    public string text_;                                                // �ӽ÷� ������ ��ȭ ������ ����

    public TMP_Text nameing;                                            // ��ȭ ���� ������Ʈ�� �ִ� ���� ǥ���� ������Ʈ
    public TMP_Text DialogT;                                            // ��ȭ ���� ���� ������Ʈ
    public GameObject dialog_obj;                                       // ��ȭ ���� ������Ʈ
    public GameObject next_button;                                      // ���� ��ȭ ��¿� ��ư
    IEnumerator seq_;
    IEnumerator skip_seq;

    public float delay;
    public bool running = false;
    public int last_index;

    void Awake()    // �̱��� �������� ��� �������� ���� �����ϰ� �Ѵ�.
    {
        next_button.SetActive(false);
        dialog_obj.SetActive(false);
        if (instance == null)
            instance = this;

        else if (instance != this)
            Destroy(gameObject);

        DontDestroyOnLoad(gameObject);
    }

    public void skip(int index)
    {
        StopAllCoroutines();                          // ��� �ڷ�ƾ ��Ȱ��ȭ
        dialog_obj.SetActive(false);                  // ���̾�α� UI ��Ȱ��ȭ
        dialog_cycles[index].check_cycle_read = true; // ���� ���̾�α� ���� ó��
        running = false;                              // ���� ���� �ƴ�

        // SceneManager.LoadScene("Main"); // �����Ӱ� ����
    }
    public IEnumerator dialog_system_start(int index)               // ���̾�α� ��� ����
    {
        nameing = dialog_obj.GetComponent<parameter>().name_text;   // ���̾�α� ������Ʈ���� �� ���� �޾ƿ���
        DialogT = dialog_obj.GetComponent<parameter>().content;

        running = true; // ���� ����
        foreach (dialog_info dialog_temp in dialog_cycles[index].info)  // ��ȭ ������ ť�� �����ϱ� ���� �ִ´�.
        {
            text_seq.Enqueue(dialog_temp.content);                      // Enqueue > ť�� ������ �߰�
        }

        dialog_obj.gameObject.SetActive(true);
        for (int i = 0; i < dialog_cycles[index].info.Count; i++)       // ��ȭ ������ ������� ���
        {

            nameing.text = dialog_cycles[index].info[i].name;

            text_ = text_seq.Dequeue();                                  // ��ȭ ������ pop

            seq_ = seq_sentence(index, i);                               // ��ȭ ���� ��� �ڷ�ƾ
            StartCoroutine(seq_);                                        // �ڷ�ƾ ����


            yield return new WaitUntil(() =>                            // �۾��� �Ϸ�� ������ ��ٸ�
            {
                if (dialog_cycles[index].info[i].check_read)            // ���� ��ȭ�� �о����� �ƴ���
                {
                    return true;                                        // �о��ٸ� ����
                }
                else
                {
                    return false;                                       // ���� �ʾҴٸ� �ٽ� �˻�
                }
            });
        }
        dialog_cycles[index].check_cycle_read = true;                   // �ش� ��ȭ �׷� ����
        running = false;
    }

    public void DisplayNext(int index, int number)                      // ���� �������� �Ѿ��
    {
        if (text_seq.Count == 0)                                        // ���� ������ ���ٸ�
        {
            if (index == last_index)
            {
                SceneManager.LoadScene("Main");                         // ��� ���̾�α׸� ������ ����
            }
            dialog_obj.gameObject.SetActive(false);                     // ���̾�α� ����
            next_button.SetActive(true);
        }
        StopCoroutine(seq_);                                            // �������� �ڷ�ƾ ����

        dialog_cycles[index].info[number].check_read = true;            // ���� ���� �������� ǥ��
    }

    public IEnumerator seq_sentence(int index, int number)              // ���� �ؽ�Ʈ �ѱ��� �� ���� ���
    {
        skip_seq = touch_wait(seq_, index, number);                     // ��ġ ��ŵ�� ���� ��ġ ��� �ڷ�ƾ �Ҵ�
        StartCoroutine(skip_seq);                                       // ��ġ ��� �ڷ�ƾ ����
        DialogT.text = "";                                              // ��ȭ ���� �ʱ�ȭ
        foreach (char letter in text_.ToCharArray())                    // ��ȭ ���� �ѱ��� �� �̾Ƴ���
        {
            DialogT.text += letter;                                     // �ѱ��ھ� ���
            yield return new WaitForSeconds(delay);                     // ��� ������
        }
        StopCoroutine(skip_seq);                                        // ���� ����� ������ ��ġ ��� �ڷ�ƾ ����
        IEnumerator next = next_touch(index, number);                   // ��ư �̿ܿ� �κ��� ��ġ�ص� �Ѿ�� �ڷ�ƾ ����
        StartCoroutine(next);
    }

    public IEnumerator touch_wait(IEnumerator seq, int index, int number)                             // ��ġ ��� �ڷ�ƾ
    {
        yield return new WaitForSeconds(0.3f);
        yield return new WaitUntil(() => Input.GetMouseButton(0) || Input.GetKeyDown(KeyCode.Space)); // ��Ŭ�� �Ǵ� �����̽� �� ��ư���� ����
        StopCoroutine(seq);                                                                           // ��ȭ ���� �ڷ�ƾ ����
        DialogT.text = text_;                                                                         // ��ŵ �� ��� ���� �ѹ��� ���
        IEnumerator next = next_touch(index, number);                                                 // ��ȭ ���� �ڷ�ƾ�� ���� �Ǿ� ���� �������� ���� �ڷ�ƾ�� ����
        StartCoroutine(next);
    }

    public IEnumerator next_touch(int index, int number)                                              // ���� �������� �Ѿ�� �ڷ�ƾ
    {
        StopCoroutine(seq_);
        StopCoroutine(skip_seq);
        yield return new WaitForSeconds(0.3f);
        yield return new WaitUntil(() => Input.GetMouseButton(0) || Input.GetKeyDown(KeyCode.Space)); // ��Ŭ�� �Ǵ� �����̽� �� ��ư���� ����
        DisplayNext(index, number);
    }

    public bool dialog_read(int check_index)          // �ش� index�� ��ũ��Ʈ �κ��� �о����� Ȯ���ϴ� �Լ�
    {
        if (!dialog_cycles[check_index].check_cycle_read)
        {
            return true;
        }

        return false;
    }
}
