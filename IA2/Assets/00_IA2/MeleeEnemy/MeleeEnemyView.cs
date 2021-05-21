using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeEnemyView : MonoBehaviour
{
    [SerializeField]
    private Animator _animator;
    void Awake()
    {
        _animator = GetComponent<Animator>();
    }

    void Update()
    {
        
    }

    void AttackAnim(bool value)
    {
        _animator.SetBool("isAttacking", value);
    }

    void DeathAnim()
    {
        _animator.SetTrigger("isDeath");
    }
}
