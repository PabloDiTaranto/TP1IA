using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyGOAPView : MonoBehaviour
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
            if (!_audioSource.isPlaying)
                _audioSource.UnPause();
        }
    }
    public void OneShotSoundClip(int indexClip)
    {
        _audioSource.PlayOneShot(_clips[indexClip]);
    }

    public void ShootAnim(bool value)
    {
        _animator.SetBool("isShooting", value);
    }


    public void MeleeHitAnim(bool value)
    {
        _animator.SetBool("isHit", value);
    }

    public void GrabWeaponAnim(bool value)
    {
        _animator.SetBool("hasWeapon", value);
    }

    public void DeathAnim(bool value)
    {
        _animator.SetBool("isDead", value);
    }

    public void HealAnim(bool value)
    {
        _animator.SetBool("isHealing", value);
    }

    public void SetSpeed(Vector3 dir)
    {
        _animator.SetFloat("Speed",dir.normalized.magnitude);
    }
}
