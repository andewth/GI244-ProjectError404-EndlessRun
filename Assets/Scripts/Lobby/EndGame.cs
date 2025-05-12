using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndGame : MonoBehaviour
{
    public string sceneToLoadLobby = "Lobby";
    public AudioSource audioSource;
    public AudioClip soundClip;

    public TMP_Text CoinEndUI;
    public TMP_Text TimerEndUI;
    public TMP_Text Latest5Score;  // TextMeshProUI ที่ใช้แสดงคะแนน 5 อันล่าสุด

    void Start()
    {
        GameResultList gameResults = GameResultManager.LoadResults();

        // แสดงผลข้อมูลล่าสุด
        if (gameResults.results.Count > 0)
        {
            CoinEndUI.text = gameResults.results[0].coins;
            TimerEndUI.text = gameResults.results[0].time;
        }
        else
        {
            CoinEndUI.text = "0";
            TimerEndUI.text = "00:00";
        }

        // แสดงคะแนนล่าสุด 5 อัน
        string latestScoresText = "\n";

        int count = Mathf.Min(5, gameResults.results.Count); // ตรวจสอบไม่ให้เกิน 5 รายการ
        for (int i = 0; i < count; i++)
        {
            latestScoresText += $"# {gameResults.results[i].time}       :       {gameResults.results[i].coins}\n";
        }
        
        Latest5Score.text = latestScoresText;  // ตั้งค่าข้อความให้กับ Latest5Score
    }

    public void LoadLobbyScene()
    {
        audioSource.PlayOneShot(soundClip);
        SceneManager.LoadScene(sceneToLoadLobby);
    }
}
