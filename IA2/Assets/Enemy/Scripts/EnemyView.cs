using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyView : MonoBehaviour
{
    [SerializeField] private AudioSource _myAudio, _myShoots, _mySteps;
    [SerializeField] private AudioClip _death, _shoot;
    [SerializeField] private Animator _myAnimator;

    private void Update()
    {
        if (PauseManager.isPause)
        {
            _myAudio.Pause();
            _myShoots.Pause();
            _mySteps.Pause();
        }
        else
        {
            _myAudio.UnPause();
            _myShoots.UnPause();
            _mySteps.UnPause();
        }

        if (GameTimeManager.isGameOver)
        {
            StepsStop();
        }
    }

    public void StepsPlay()
    {
        _mySteps.Play();
    }

    public void StepsStop()
    {
        _mySteps.Stop();
    }

    public void Shoot()
    {
        _myShoots.PlayOneShot(_shoot);
    }

    public void Death()
    {
        _myAudio.PlayOneShot(_death);        
    }

    public void ShootAnimation()
    {
        _myAnimator.SetBool("IsShooting", true);
        _myAnimator.SetBool("IsRunning", false);
        _myAnimator.SetBool("IsDead", false);
    }

    public void RunAnimation()
    {
        _myAnimator.SetBool("IsShooting", false);
        _myAnimator.SetBool("IsRunning", true);
        _myAnimator.SetBool("IsDead", false);
    }

    public void DeathAnimation()
    {
        _myAnimator.SetBool("IsShooting", false);
        _myAnimator.SetBool("IsRunning", false);
        _myAnimator.SetBool("IsDead", true);
    }
}
