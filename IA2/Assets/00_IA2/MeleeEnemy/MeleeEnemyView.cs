using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeEnemyView : MonoBehaviour
{
    [SerializeField] private Animator _animator;
    [SerializeField] private AudioSource _audioSource;
    [SerializeField] private AudioClip[] _clips;
    private void Awake()
    {
        _animator = GetComponent<Animator>();
    }

    private void Update()
    {
        if (PauseManager.isPause)
        {
            if (_audioSource.isPlaying)
                _audioSource.Pause();
        }
        else
        {
            if(!_audioSource.isPlaying)
            _audioSource.UnPause();
        }
    }
    public void OneShotSoundClip(int indexClip)
    {
        _audioSource.PlayOneShot(_clips[indexClip]);
    }

    public void AttackAnim(bool value)
    {
        _animator.SetBool("isAttacking", value);
    }


    public void DeathAnim()
    {
        _animator.SetTrigger("isDeath");
    }
}
