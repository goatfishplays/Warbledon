using UnityEngine;

public class GameStateManager : MonoBehaviour
{
    public GameStateManager instance;
    public AudioManager audioManager => AudioManager.instance;


    public bool isPaused { get; private set; }
    public void PauseGame(bool pause = true)
    {
        if (pause != isPaused)
        {
            isPaused = pause;
            if (pause)
            {
                Time.timeScale = 0f;
                audioManager.PauseAllSFX();
            }
            else
            {
                Time.timeScale = 1f;
                audioManager.UnPauseAllSFX();
            }
        }
    }

    public bool isBattling { get; private set; }
    public void SetBattleState(bool battling)
    {
        isBattling = battling;
    }

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Debug.LogWarning("Two GameStateManagers detected, deleting second");
            Destroy(this);
        }
    }
}
