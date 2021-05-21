using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MedKit : MonoBehaviour
{
    [SerializeField] private float lifeRecover;
    [SerializeField] private Collider myCollider;
    [SerializeField] private MeshRenderer myRenderer;
    [SerializeField] private AudioSource myAudio;

    private void Awake()
    {
        EventManager.Subscribe("OnPlayerRespawn", MedkitRespawn);
        EventManager.Subscribe("OnPause", PauseSounds);
        EventManager.Subscribe("OnUnpause", UnPauseSounds);
    }

    public float GetLifeRecover()
    {
        return lifeRecover;
    }

    private void MedkitUsed()
    {
        myAudio.Play();
        myCollider.enabled = false;
        myRenderer.enabled = false;
    }

    private void MedkitRespawn(params object[] parameters)
    {
        myCollider.enabled = true;
        myRenderer.enabled = true;
    }

    public void PauseSounds(params object[] parameters)
    {
        myAudio.Pause();
    }
    public void UnPauseSounds(params object[] parameters)
    {
        myAudio.UnPause();
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.layer == 8)
        {
            var player = other.GetComponent<CharacterModel>();
            if(player.CurrentLife < player.MaxLife)
            {
                MedkitUsed();
            }
        }
    }
}
