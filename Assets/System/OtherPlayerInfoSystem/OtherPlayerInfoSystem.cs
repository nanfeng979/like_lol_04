using UnityEngine;

public class OtherPlayerInfoSystem : MonoBehaviour
{
    public static OtherPlayerInfoSystem instance;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }

    }

}
