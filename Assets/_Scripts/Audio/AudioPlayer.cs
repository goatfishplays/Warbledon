using System.Collections;
using UnityEngine;

public class AudioPlayer : MonoBehaviour
{
    public AudioSource audioSource;
    public float pitchMin = 1f;
    public float pitchMax = 1f;
    private float pitch = 1f;
    public Transform tracker;
    public Coroutine co_audioPlaying = null;

    private void Update()
    {
        if (tracker != null)
        {
            transform.SetPositionAndRotation(tracker.position, tracker.rotation);
        }
    }

    #region Pitch
    public float GetPitch()
    {
        return pitch;
    }

    public void SetPitch(float value)
    {
        pitch = value;
    }

    public void RandomizePitch()
    {
        pitch = Random.Range(pitchMin, pitchMax);
        audioSource.pitch = pitch;
    }

    public void RandomizePitch(float pitchMin, float pitchMax)
    {
        this.pitchMin = pitchMin;
        this.pitchMax = pitchMax;
        RandomizePitch();
    }
    #endregion Pitch


    #region Playing
    public void PlayAudioAtTracker(AudioClip clip, Transform tracker, float volume = 1f, float pitchMin = 1f, float pitchMax = 1f, bool loop = false)
    {
        this.tracker = tracker;
        transform.SetPositionAndRotation(tracker.position, tracker.rotation);
        PlayAudio(clip, volume, pitchMin, pitchMax, loop);
    }

    public void PlayAudioAtPoint(AudioClip clip, Vector3 pos, Quaternion rot, float volume = 1f, float pitchMin = 1f, float pitchMax = 1f, bool loop = false)
    {
        transform.position = pos;
        transform.rotation = rot;
        PlayAudio(clip, volume, pitchMin, pitchMax, loop);
    }

    public void PlayAudio(AudioClip clip, float volume = 1f, float pitchMin = 1f, float pitchMax = 1f, bool loop = false)
    {

        audioSource.clip = clip;
        audioSource.volume = volume;
        RandomizePitch(pitchMin, pitchMax);
        audioSource.loop = loop;

        if (co_audioPlaying != null)
        {
            StopCoroutine(co_audioPlaying);
        }

        audioSource.Play();
        // if (!loop)
        // {
        co_audioPlaying = StartCoroutine(Playing(clip.length));
        // }
    }

    private IEnumerator Playing(float time)
    {
        // yield return new WaitForSeconds(time + .1f);
        do
        {
            yield return new WaitForSeconds(time);
            RandomizePitch();
        } while (audioSource.loop);
        // reset and return 
        FreeSelf();
    }

    #endregion Playing

    public void FreeSelf()
    {
        audioSource.Stop();
        AudioManager.instance.FreeSFXPlayer(this);
        tracker = null;
        transform.parent = AudioManager.instance.transform;
    }

    public void Pause()
    {
        if (co_audioPlaying != null)
        {
            StopCoroutine(co_audioPlaying);
        }
        audioSource.Pause();
    }

    public void UnPause()
    {
        audioSource.UnPause();
        // if (!audioSource.loop)
        // {
        co_audioPlaying = StartCoroutine(Playing(audioSource.clip.length - audioSource.time));
        // }
    }



    private void OnDestroy()
    {
        // prevent mem leak 
        AudioManager.instance.RemoveSFXPlayer(this);
    }


}
