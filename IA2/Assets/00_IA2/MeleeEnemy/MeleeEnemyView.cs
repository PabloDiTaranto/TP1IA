﻿//IA2-P1

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeEnemyView : MonoBehaviour
{
    private Animator _animator;
    private AudioSource _audioSource;
    [SerializeField] private AudioClip[] _clips;
    private void Awake()
    {
        _animator = GetComponent<Animator>();
        _audioSource = GetComponent<AudioSource>();
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


    public void DeathAnim(bool value)
    {
        _animator.SetBool("isDead",value);
    }
}
