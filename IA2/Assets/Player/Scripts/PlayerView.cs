using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerView : MonoBehaviour
{
    [SerializeField] private AudioSource _myAudio, _myShootsSound, _mySteps;
    [SerializeField] private AudioClip hurt, death, shoot, reload;

    private void Awake()
    {
        EventManager.Subscribe("OnPause", PauseSounds);
        EventManager.Subscribe("OnUnpause", UnPauseSounds);
        EventManager.Subscribe("OnGameOver", StepsStop);
    }
    public void Hurt()
    {
        if (!_myAudio.isPlaying)
            _myAudio.PlayOneShot(hurt);
    }

    public void Death()
    {
        _myAudio.PlayOneShot(death);
    }

    public void StepsPlay()
    {
        _mySteps.Play();
    }

    public void StepsStop(params object[] parameters)
    {
        _mySteps.Stop();
    }

    public void ShootPlay()
    {
        _myShootsSound.PlayOneShot(shoot);
    }

    public void ReloadPlay()
    {
        _myShootsSound.PlayOneShot(reload);
    }

    public void PauseSounds(params object[] parameters)
    {
        _myShootsSound.Pause();
        _myAudio.Pause();
        _mySteps.Pause();
    }
    public void UnPauseSounds(params object[] parameters)
    {
        _myShootsSound.UnPause();
        _myAudio.UnPause();
        _mySteps.UnPause();
    }
}
