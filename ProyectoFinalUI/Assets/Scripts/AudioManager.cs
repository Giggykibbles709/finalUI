using UnityEngine;
using System.Collections;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;
    public AudioSource musicSource;
    public AudioSource sfxSource;

    public float fadeDuration = 1.0f;

    private void Awake()
    {
        // Patron Singleton: evita que se duplique al volver al menu
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void PlayMusic(AudioClip clip)
    {
        // Si es la misma cancion, no hacemos nada
        if (musicSource.clip == clip) return;

        // Iniciamos la transicion
        StartCoroutine(FadeMusic(clip));
    }

    public void PlaySFX(AudioClip clip)
    {
        sfxSource.PlayOneShot(clip);
    }

    private IEnumerator FadeMusic(AudioClip newClip)
    {
        float startVolume = musicSource.volume;

        // 1. FADE OUT (Bajar volumen)
        if (musicSource.isPlaying)
        {
            for (float t = 0; t < fadeDuration; t += Time.deltaTime)
            {
                musicSource.volume = Mathf.Lerp(startVolume, 0, t / fadeDuration);
                yield return null; // Espera al siguiente frame
            }
            musicSource.volume = 0;
            musicSource.Stop();
        }

        // 2. CAMBIO DE CLIP
        musicSource.clip = newClip;

        if (newClip != null)
        {
            musicSource.Play();

            // 3. FADE IN (Subir volumen)
            for (float t = 0; t < fadeDuration; t += Time.deltaTime)
            {
                // Volvemos al volumen original (normalmente 1, o el que tengas en el componente)
                musicSource.volume = Mathf.Lerp(0, startVolume, t / fadeDuration);
                yield return null;
            }
            musicSource.volume = startVolume;
        }
    }
}