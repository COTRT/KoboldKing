using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicTrigger : MonoBehaviour {
    GameObject EventManager;
    [SerializeField]
  public AudioClip Music;
    public void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            EventManager = GameObject.FindGameObjectWithTag("Event Manager");
            AudioSource audioSource = EventManager.GetComponent<AudioSource>();
           // AudioClip audioClip = audioSource.GetComponent<AudioClip>();
            //audioClip = Music;
            audioSource.clip = Music;
            audioSource.Play();
        }
    }
    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
