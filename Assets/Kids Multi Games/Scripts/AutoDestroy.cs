using UnityEngine;

public class AutoDestroy : MonoBehaviour
{
    [SerializeField] float TimeToDestroy = 5;
    // Start is called before the first frame update
    void Start()
    {
        Invoke(nameof(destroy), TimeToDestroy);
    }

    // Update is called once per frame
    void destroy()
    {
        Destroy(gameObject);
    }
}
