
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;
    public Transform audioPlayerHolder;

    [Header("Mixers")]
    public AudioMixer mixer;

    [Header("Music")]
    [SerializeField] private GameObject musicPlayerPrefab;
    public int maxMusicPlayers = 5;
    private const float FADE_SPEED_MULT = 0.001f;
    public AudioPlayer curMusicPlayer;
    public Coroutine co_curMusicPlayer;
    // * Now some people may say a set for 5 elements is obsessive, I'm here to tell them that they are silly, see in the case we end up with 5000 music players we will have flexible code!!!
    public Dictionary<AudioPlayer, Coroutine> dyingMusicPlayers = new Dictionary<AudioPlayer, Coroutine>();
    public HashSet<AudioPlayer> freeMusicPlayers = new HashSet<AudioPlayer>();
    // public Queue<AudioPlayer> 




    [Header("SFX")]
    // TODO: If you want to raise SFX players into the thousands(why would you have thousands of sounds playing) we should probs disable the free when not in use because might incur some costs at that point
    // also most of this code goes untested because test driven dev would take 15 trillion years
    public int maxSFXPlayers = 100;
    [SerializeField] private GameObject SFXPlayerPrefab;
    private HashSet<AudioPlayer> freeSFXPlayers = new HashSet<AudioPlayer>();
    private HashSet<AudioPlayer> activeSFXPlayers = new HashSet<AudioPlayer>();
    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Debug.LogWarning("Two Audio Managers detected, deleting second");
            Destroy(this);
        }
    }



    #region Mixer
    public void SetMasterVolume(float level)
    {
        mixer.SetFloat("MasterVolume", Mathf.Log10(level) * 20f);
    }
    public void SetSFXVolume(float level)
    {
        mixer.SetFloat("SFXVolume", Mathf.Log10(level) * 20f);
    }
    public void SetMusicVolume(float level)
    {
        mixer.SetFloat("MusicVolume", Mathf.Log10(level) * 20f);
    }
    #endregion Mixer

    #region SFX

    /// <summary>
    /// If can get/make a free sfx player will return the free sfx player
    /// </summary>
    /// <returns></returns>
    private AudioPlayer RequestFreeSFXPlayer()
    {
        AudioPlayer player = null;

        // get a free audio player or create if below cap
        if (freeSFXPlayers.Count == 0 && activeSFXPlayers.Count < maxSFXPlayers)
        {
            GameObject newPlayer = Instantiate(SFXPlayerPrefab, transform);
            player = newPlayer.GetComponent<AudioPlayer>();
        }
        else
        {
            foreach (AudioPlayer ap in freeSFXPlayers)
            {
                player = ap;
                freeSFXPlayers.Remove(ap);
                break;
            }
        }

        return player;
    }

    /// <summary>
    /// Play a sound effect that will be track an object
    /// </summary>
    /// <param name="clip">The sound effect to play</param> 
    /// <param name="tracker">The object to track</param>
    /// <param name="volume">Volume of sound effect</param>
    /// <param name="pitchMin">Lower Bound on pitch randomization</param>
    /// <param name="pitchMax">Upper Bound on pitch randomization</param>
    /// <param name="loop">Note: pitch will rerandomize within bounds every time loop happens</param>
    public void PlaySFXAtTracker(AudioClip clip, Transform tracker, float volume = 1f, float pitchMin = 0.9f, float pitchMax = 1.1f, bool loop = false)
    {
        // float pitch = Random.Range(pitchMin, pitchMax);
        AudioPlayer player = RequestFreeSFXPlayer();
        if (player == null)
        {
            Debug.LogWarning("Audio clip failed to play, likely because too many audio sources currently playing");
            return;
        }

        // Set things for audio source
        activeSFXPlayers.Add(player);

        // Play audio
        player.PlayAudioAtTracker(clip, tracker, volume, pitchMin, pitchMax, loop);
    }

    /// <summary>
    /// Play a sound effect that will stay at a point
    /// </summary>
    /// <param name="clip">The sound effect to play</param> 
    /// <param name="tracker">The object to track</param>
    /// <param name="volume">Volume of sound effect</param>
    /// <param name="pitchMin">Lower Bound on pitch randomization</param>
    /// <param name="pitchMax">Upper Bound on pitch randomization</param>
    /// <param name="loop">Note: pitch will rerandomize within bounds every time loop happens</param>
    public void PlaySFXAtPoint(AudioClip clip, Vector3 pos, Quaternion rot, float volume = 1f, float pitchMin = 0.9f, float pitchMax = 1.1f, bool loop = false)
    {
        // float pitch = Random.Range(pitchMin, pitchMax);
        AudioPlayer player = RequestFreeSFXPlayer();
        if (player == null)
        {
            Debug.LogWarning("Audio clip failed to play, likely because too many audio sources currently playing");
            return;
        }

        // Set things for audio source 
        activeSFXPlayers.Add(player);

        // Play audio
        player.PlayAudioAtPoint(clip, pos, rot, volume, pitchMin, pitchMax, loop);
    }

    /// <summary>
    /// Play a sound effect at the player 
    /// </summary>
    /// <param name="clip">The sound effect to play</param> 
    /// <param name="tracker">The object to track</param>
    /// <param name="volume">Volume of sound effect</param>
    /// <param name="pitchMin">Lower Bound on pitch randomization</param>
    /// <param name="pitchMax">Upper Bound on pitch randomization</param>
    /// <param name="loop">Note: pitch will rerandomize within bounds every time loop happens</param>
    public void PlaySFX(AudioClip clip, float volume = 1f, float pitchMin = 0.9f, float pitchMax = 1.1f, bool loop = false)
    {
        // float pitch = Random.Range(pitchMin, pitchMax);

        AudioPlayer player = RequestFreeSFXPlayer();
        if (player == null)
        {
            Debug.LogWarning("Audio clip failed to play, likely because too many audio sources currently playing");
            return;
        }


        // Set things for audio source  
        activeSFXPlayers.Add(player);

        player.transform.parent = audioPlayerHolder;
        player.transform.localPosition = Vector3.zero;
        player.transform.localRotation = Quaternion.identity;

        // Play audio
        player.PlayAudio(clip, volume, pitchMin, pitchMax, loop);
    }

    /// <summary>
    /// Use when destroying an SFX Player or making immune to sound pool limits
    /// </summary>
    /// <param name="player"></param>
    public void RemoveSFXPlayer(AudioPlayer player)
    {
        freeSFXPlayers.Remove(player);
        activeSFXPlayers.Remove(player);
    }

    /// <summary>
    /// Use to return an SFX player
    /// </summary>
    /// <param name="player"></param>
    public void FreeSFXPlayer(AudioPlayer player)
    {
        freeSFXPlayers.Add(player);
        activeSFXPlayers.Remove(player);
        player.tracker = null;
    }


    public void PauseAllSFX()
    {
        foreach (AudioPlayer player in activeSFXPlayers)
        {
            player.Pause();
        }
    }
    public void UnPauseAllSFX()
    {
        foreach (AudioPlayer player in activeSFXPlayers)
        {
            player.UnPause();
        }
    }

    #endregion SFX

    #region Music
    /// <summary>
    /// Will return a free music player
    /// </summary>
    /// <returns></returns>
    private AudioPlayer GetFreeMusicPlayer()
    {
        AudioPlayer player = null;

        // get a free audio player or create if below cap
        if (freeMusicPlayers.Count == 0)
        {
            if (dyingMusicPlayers.Count < maxMusicPlayers)
            {
                GameObject newPlayer = Instantiate(musicPlayerPrefab, audioPlayerHolder);
                player = newPlayer.GetComponent<AudioPlayer>();
            }
            else
            {
                float minVol = -1;
                foreach (AudioPlayer ap in dyingMusicPlayers.Keys)
                {
                    if (ap.audioSource.volume < minVol)
                    {
                        player = ap;
                        minVol = ap.audioSource.volume;
                    }
                }
                StopCoroutine(dyingMusicPlayers[player]);
            }
        }
        else
        {
            foreach (AudioPlayer ap in freeMusicPlayers)
            {
                player = ap;
                freeMusicPlayers.Remove(ap);
                break;
            }
        }

        return player;
    }

    /// <summary>
    /// Will crossfade music into the scene
    /// </summary>
    /// <param name="clip">New music to play</param>
    /// <param name="speed">Rate at which the music should fade in and currently playing should fade out</param>
    /// <param name="volume"></param>
    /// <param name="loop"></param>
    public void FadeInMusic(AudioClip clip, float speed = 0.5f, float volume = 1, bool loop = true)
    {
        if (co_curMusicPlayer != null)
        {
            StopCoroutine(co_curMusicPlayer);
        }

        if (curMusicPlayer != null)
        {
            dyingMusicPlayers.Add(curMusicPlayer, StartCoroutine(FadeOutCurrentMusic(speed)));
        }
        curMusicPlayer = GetFreeMusicPlayer();

        if (curMusicPlayer == null)
        {
            Debug.LogWarning("Failed to get a music player");
            return;
        }


        // Play audio 
        curMusicPlayer.PlayAudio(clip, 0, loop: loop);
        co_curMusicPlayer = StartCoroutine(FadeInCurrentMusic(volume, speed));
    }

    private IEnumerator FadeOutCurrentMusic(float speed = 0.5f)
    {
        speed *= FADE_SPEED_MULT;
        AudioPlayer player = curMusicPlayer;
        while (player.audioSource.volume > 0)
        {
            player.audioSource.volume -= speed;
            yield return null;
        }
        dyingMusicPlayers.Remove(player);
        freeMusicPlayers.Add(player);
        player.audioSource.Stop();
    }
    private IEnumerator FadeInCurrentMusic(float volume = 1, float speed = 0.5f)
    {
        speed *= FADE_SPEED_MULT;
        if (volume > 1)
        {
            volume = 1;
        }
        AudioPlayer player = curMusicPlayer;
        while (player.audioSource.volume < volume)
        {
            player.audioSource.volume += speed;
            yield return null;
        }
        player.audioSource.volume = volume;
        co_curMusicPlayer = null;
    }

    #endregion Music
}
