using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class MainManager : MonoBehaviour
{
    public int i;
    public void EpisodeOn()
    {
        SceneManager.LoadScene("Story" + i);
    }
}
