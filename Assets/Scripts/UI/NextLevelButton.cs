using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class NextLevelButton : MonoBehaviour
{

    [SerializeField] private string nextLevelName;

    public void NextLevel()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(nextLevelName);
    }
}
