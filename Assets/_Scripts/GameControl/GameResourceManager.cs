using UnityEngine;

public class GameResourceManager : MonoBehaviour
{
    public static GameResourceManager instance { get; private set; }
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Debug.LogWarning("Two 'GameResourceManager's detected, deleting second");
            Destroy(gameObject);
        }
    }


    // [Header("Cards")]
    // public Sprite[] cardNumbers;
    // public Color playableColor = Color.white;
    // public Color unplayableColor = Color.red;
}
