using UnityEngine;

public class BattleAudioManager : MonoBehaviour
{
    private AudioSource audioSourceSFX;
    private AudioSource audioSourceMusic;

    [Header("M�sicas")]
    public AudioClip battleMusic;
    public AudioClip winMusic;
    public AudioClip loseMusic;

    [Header("Efeitos Sonoros (SFX)")]
    public AudioClip damageSound;
    public AudioClip insufficientSP;

    void Awake()
    {
        AudioSource[] sources = GetComponents<AudioSource>();
        if (sources.Length < 2)
        {
            audioSourceMusic = gameObject.AddComponent<AudioSource>();
            audioSourceSFX = gameObject.AddComponent<AudioSource>();
        }
        else
        {
            audioSourceMusic = sources[0];
            audioSourceSFX = sources[1];
        }

        audioSourceSFX.playOnAwake = false;
        audioSourceSFX.loop = false;
        audioSourceMusic.playOnAwake = false;
        audioSourceMusic.loop = true;
    }

    public void PlaySFX(AudioClip clipToPlay)
    {
        if (clipToPlay != null)
        {
            audioSourceSFX.PlayOneShot(clipToPlay);
        }
        else
        {
            Debug.LogWarning("PlaySFX foi chamado, mas o AudioClip era nulo.");
        }
    }

    public void PlayBattleMusic()
    {
        if (battleMusic != null)
        {
            audioSourceMusic.clip = battleMusic;
            audioSourceMusic.Play();
        }
    }

    public void PlayWinMusic()
    {
        if (winMusic != null)
        {
            audioSourceMusic.Stop();
            audioSourceMusic.loop = false;
            audioSourceMusic.PlayOneShot(winMusic);
        }
    }

    public void PlayLoseMusic()
    {
        if (loseMusic != null)
        {
            audioSourceMusic.Stop();
            audioSourceMusic.loop = false;
            audioSourceMusic.PlayOneShot(loseMusic);
        }
    }

    public void PlayDamageSFX()
    {
        PlaySFX(damageSound);
    }

    public void PlayInsufficientSPSFX()
    {
        PlaySFX(insufficientSP);
    }
}