using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [SerializeField]
    private AudioSource audioSource1;

    [SerializeField]
    private AudioSource audioSource2;

    [SerializeField]
    private float mixTimeS = 2f;
    public float MixTimeS
    {
        get
        {
            return mixTimeS;
        }
        set
        {
            mixTimeS = value;
        }
    }

    [SerializeField]
    private AudioSource sfxSource;

    [SerializeField]
    private Delayer delayer;

    [SerializeField]
    private string backgroundVolumePrefsKey;
    public string BackgroundVolumePrefsKey
    {
        get
        {
            return backgroundVolumePrefsKey;
        }
    }

    [SerializeField]
    private string effectsVolumePrefsKey;
    public string EffectsVolumePrefsKey
    {
        get
        {
            return effectsVolumePrefsKey;
        }
    }

    public const float MAX_VOLUME = 1f;

    private AudioSource currentAudioSource;
    private AudioSource otherAudioSource;
    private bool isChangingBgSound;
    private float currentMaxVolume;
    private float otherMaxVolume;
    private float currentVolume;
    private float otherVolume;
    private float elapsedTimeS;

    private AudioClip currentBackgroundSound;
    public AudioClip CurrentBackgroundSound
    {
        get
        {
            return currentBackgroundSound;
        }
    }

    private void Awake()
    {
        UpdateVolume();
    }

    public void PlayBackgroundSound(AudioClip audioClip)
    {
        if (currentAudioSource != null
            && currentAudioSource.clip == audioClip)
        {
            return;
        }

        currentBackgroundSound = audioClip;

        if (currentAudioSource == null
            || currentAudioSource == audioSource2)
        {
            currentAudioSource = audioSource1;
            otherAudioSource = audioSource2;
        }
        else
        {
            currentAudioSource = audioSource2;
            otherAudioSource = audioSource1;
        }

        currentMaxVolume
            = PlayerPrefs.GetFloat(
                backgroundVolumePrefsKey,
                MAX_VOLUME / 2f
            );
        otherMaxVolume = otherAudioSource.volume;

        isChangingBgSound = true;
        elapsedTimeS = 0;

        currentAudioSource.clip = audioClip;
        currentAudioSource.volume = 0;
        currentAudioSource.Play();
        if (otherAudioSource.isPlaying)
        {
            delayer.AddDelay(mixTimeS, (delay) =>
            {
                otherAudioSource.Stop();
            });
        }
    }

    public void PlayFX(AudioClip audioClip)
    {
        if (audioClip == null)
        {
            return;
        }
        sfxSource.PlayOneShot(audioClip);
    }

    private void Update()
    {
        if (isChangingBgSound)
        {
            if (elapsedTimeS >= mixTimeS)
            {
                isChangingBgSound = false;
                currentVolume = currentMaxVolume;
                otherVolume = 0;
            }
            else
            {
                currentVolume = MathUtils.NormalizeValue(
                    elapsedTimeS,
                    0,
                    currentMaxVolume,
                    0,
                    mixTimeS
                );

                otherVolume = MathUtils.NormalizeValue(
                    elapsedTimeS,
                    otherMaxVolume,
                    0,
                    0,
                    mixTimeS
                );

                elapsedTimeS += Time.deltaTime;
            }

            currentAudioSource.volume = currentVolume;
            otherAudioSource.volume = otherVolume;
        }
    }

    public void UpdateVolume()
    {
        UpdateBackgroundVolume();
        UpdateSFXVolume();
    }

    private void UpdateBackgroundVolume()
    {
        var volume = PlayerPrefs.GetFloat(
            backgroundVolumePrefsKey,
            MAX_VOLUME / 2f
        );
        currentMaxVolume = volume;
        audioSource1.volume = volume;
        audioSource2.volume = volume;
    }

    private void UpdateSFXVolume()
    {
        var volume = PlayerPrefs.GetFloat(
            effectsVolumePrefsKey,
            MAX_VOLUME / 2f
        );
        sfxSource.volume = volume;
    }
}
