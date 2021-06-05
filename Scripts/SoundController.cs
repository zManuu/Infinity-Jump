using UnityEngine;

public class SoundController : MonoBehaviour
{

    public AudioSource coinPickup, powerUp, death, levelComplete;
    
    public void Play(AudioSource audio)
    {
        audio.PlayOneShot(audio.clip);
    }

}
