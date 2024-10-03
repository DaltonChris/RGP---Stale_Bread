using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SfxManager : MonoBehaviour
{
    private static SfxManager instance;
    public static SfxManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<SfxManager>();
                if (instance == null)
                {
                    instance = new GameObject("Spawned SFX manager", typeof(SfxManager)).GetComponent<SfxManager>();
                }
            }
            return instance;
        }
        private set
        {
            instance = value;
        }
    }

    public GameObject sfxSound;

    private float SfxVolume = 0.4f;
    private AudioSource sfxSource;


    private void Awake()
    {
        sfxSound = GameObject.FindWithTag("SFX");
        sfxSource = sfxSound.GetComponent<AudioSource>();

    }
    private void Update()
    {
        sfxSource.volume = SfxVolume;
    }
    public void VoulmeUpdater(float volume)
    {
        SfxVolume = volume;
    }

    public void PlaySfx(AudioClip clip)
    {
        sfxSource.PlayOneShot(clip);
    }

}
