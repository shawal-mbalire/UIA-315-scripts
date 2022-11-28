using System.Collections;
using System.Collections.Generic;
using Unity Engine;

public class Footsteps : MonoBehaviour
{
    private AudioSource audioSource;

    [Header("Footsteps Sources")]
    public AudioClip[] footstepsSound;
    private AudioClip GetRandomFootStep()
    {
        return footstepsSound[Random.Range(0, footstepsSound.Length)];
    }
    private void Step()
    {
        AudioClip clip = GetRandomFootStep();
        audioSource.PlayOneShot(clip);
    }
}
