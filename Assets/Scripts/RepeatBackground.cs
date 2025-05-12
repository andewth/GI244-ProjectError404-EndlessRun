using UnityEngine;

public class RepeatBackground : MonoBehaviour
{
    public GameObject[] backgroundPrefabs;
    public float[] activeTimes;
    public Transform spawnPoint;
    public float scrollSpeed = 2f;

    private GameObject[] backgrounds = new GameObject[2];
    private float backgroundWidth;
    private int currentIndex = 0;
    private float timer = 0f;

    void Start()
    {
        if (backgroundPrefabs.Length == 0 || activeTimes.Length == 0)
        {
            Debug.LogError("Please assign prefabs and active times.");
            return;
        }

        if (spawnPoint == null) spawnPoint = transform;

        // Spawn พื้นหลังเริ่มต้น 2 ชิ้น
        backgrounds[0] = Instantiate(backgroundPrefabs[currentIndex], spawnPoint.position, Quaternion.identity, spawnPoint);
        backgroundWidth = backgrounds[0].GetComponent<Renderer>().bounds.size.x;

        Vector3 secondPos = spawnPoint.position + new Vector3(backgroundWidth, 0, 0);
        backgrounds[1] = Instantiate(backgroundPrefabs[currentIndex], secondPos, Quaternion.identity, spawnPoint);
    }

    void Update()
    {
        // เลื่อนพื้นหลัง
        foreach (GameObject bg in backgrounds)
        {
            bg.transform.position += Vector3.left * scrollSpeed * Time.deltaTime;
        }

        // เพิ่มเวลา
        timer += Time.deltaTime;

        // ถึงเวลาเปลี่ยนพื้นหลังทั้งหมด
        if (timer >= activeTimes[currentIndex])
        {
            timer = 0f;

            // ลบพื้นหลังเก่า ถ้าพื้นหลังยังไม่ถูกทำลาย
            if (backgrounds[0] != null && backgrounds[1] != null)
            {
                Destroy(backgrounds[0]);
                Destroy(backgrounds[1]);
            }

            // สร้างพื้นหลังใหม่ 2 ชิ้น
            currentIndex = (currentIndex + 1) % backgroundPrefabs.Length;
            
            Vector3 firstPos = spawnPoint.position;
            backgrounds[0] = Instantiate(backgroundPrefabs[currentIndex], firstPos, Quaternion.identity, spawnPoint);

            backgroundWidth = backgrounds[0].GetComponentInChildren<Renderer>().bounds.size.x;

            Vector3 secondPos = firstPos + new Vector3(backgroundWidth, 0, 0);
            backgrounds[1] = Instantiate(backgroundPrefabs[currentIndex], secondPos, Quaternion.identity, spawnPoint);
        }

        // เลื่อนกลับพื้นหลังถ้าเลยจุดซ้ายสุด
        for (int i = 0; i < backgrounds.Length; i++)
        {
            if (backgrounds[i].transform.position.x < spawnPoint.position.x - backgroundWidth)
            {
                int otherIndex = (i + 1) % 2;
                backgrounds[i].transform.position = backgrounds[otherIndex].transform.position + new Vector3(backgroundWidth, 0, 0);
            }
        }
    }


}
