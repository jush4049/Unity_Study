using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEditor.VersionControl;

public class Clipboard : MonoBehaviour
{
    public TMP_Text characterText;
    public TMP_Text scriptText;

    [SerializeField]
    private int num = 0;
    //private string message;
    //private float speed = 1.0f;

    public int endNum;
    public string NextScene;

    void Update()
    {
        List<Dictionary<string, object>> Story = CSVReader.Read("Script_Test");
        for (int i = 0; i < Story.Count; i++)
        {
            characterText.text = Story[num]["Character"].ToString();
            scriptText.text = Story[num]["Script"].ToString();
        }
        if (Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.Space))
        {
            //message = Story[num]["Script"].ToString();
            num++;
            //StartCoroutine(Typing(scriptText, message, speed));
            if (num == endNum)
            {
                SceneManager.LoadScene(NextScene);
            }
        }
    }
    /*IEnumerator Typing(TMP_Text scriptText, string message, float speed)
    {
        for (int i = 0; i < message.Length; i++)
        {
            scriptText.text = message.Substring(0, i + 1);
            yield return new WaitForSeconds(speed);
        }
    }*/
}
