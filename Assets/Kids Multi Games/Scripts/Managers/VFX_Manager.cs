using UnityEngine;

public class VFX_Manager : MonoBehaviour
{
    [SerializeField] GameObject ConfittiPrefab;
    [SerializeField] GameObject BalloonPopPrefab;
    // Start is called before the first frame update
    public static VFX_Manager Instance;
    void Awake()
    {
        if (Instance != null)
            Destroy(Instance);
        Instance = this;
    }

    public float ConfittiAtRandomPosition(int NbrToSpawn = 1, float DistanceFromCameraOnZ = 0)
    {
        while (NbrToSpawn > 0)
        {
            Vector3 RandomPosition = new Vector3(Random.Range(0f + Screen.width * 2.5f, Screen.width - Screen.width * 2.5f),
                                                 Random.Range(0f + Screen.height * 2.5f, Screen.height - Screen.height * 2.5f),
                                                 DistanceFromCameraOnZ);

            Instantiate(ConfittiPrefab, Camera.main.ScreenToWorldPoint(new Vector3(Screen.width / 2, Screen.height / 2, 0)), Quaternion.identity);
            NbrToSpawn--;
        }

        return ConfittiPrefab.GetComponent<ScreenCelebration>().CelebratingTime;
    }

    public void BalloonPop(Vector3 Position)
    {
        if (Position == null)
        {
            Debug.LogWarning("No position Provided for the Balloon Pop Effect.", this);
            Position = new Vector3(0, 0, 3);
        }

        Instantiate(BalloonPopPrefab, Position, Quaternion.identity);
    }

}
