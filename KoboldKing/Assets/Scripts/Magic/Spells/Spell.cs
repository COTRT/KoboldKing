using UnityEngine;
using System.Collections;

[RequireComponent(typeof(ParticleSystem))]
public class Spell : MonoBehaviour
{
    ParticleSystem partSys;
    MeshRenderer meshRend;
    //ParticleSystem particleSystem;
    // Use this for initialization
    void Start()
    {
        partSys = GetComponent<ParticleSystem>();
        meshRend = GetComponent<MeshRenderer>();
        if (meshRend != null)
        {
            meshRend.enabled = false;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Cast(GameObject Caster)
    {
        transform.position = Caster.transform.position;
        if (meshRend != null)
        {
            meshRend.enabled = false;
        }
        
        partSys.Play();
    }
}
