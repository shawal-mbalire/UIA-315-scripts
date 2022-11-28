using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class ScoreManager : MonoBehaviour
{
    [Header("Score Manager")]
    public int kills;
    public int enemykills;
    public Text playerKillCounter;
    public Text enemykillCounter;
    public Text Maintext;



    IEnumerator WinOrLose()
    {
        playerkillCounter.text = "" + kills;
        enemykillCounter.text = "" + enemyKills;
        if (kills >= 10)
        {
            Maintext.text = "Blue Team Victory";
            PlayerPrefs.SetInt("kills", kills);
            Time.timeScale = 0f;
            yield return new WaitForSeconds(5f);
            SceneManager.LoadScene("TDMRoom");
        }
        else if (enemyKills >= 10)
        {
            Maintext.text = "Red Team Victory";
            PlayerPrefs.SetInt("enemyKills", enemykills);
            Time.timeScale = 0f;
            yield return new WaitForSeconds(5f);
            SceneManager.LoadScene("TDMRoom");
        }
    }
}
