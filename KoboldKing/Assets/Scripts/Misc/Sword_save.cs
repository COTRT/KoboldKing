using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using Debug = UnityEngine.Debug;

public class Sword_save : MonoBehaviour
{
    public float durationSeconds = 2;
    public Vector3 swordSwipePosDelta;
    public Vector3 swordSwipeRotDelta;
    public Vector3 swordSwipeScaleDelta;
    public float Damage;
    public float Range = 10.0f;


    private bool swiping = false;
    private GameObject player;

    public bool Swiping
    {
        get
        {
            return swiping;
        }

        private set
        {
            swiping = value;
        }
    }


    // Use this for initialization
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        //if (!Swiping && Input.GetButton("Fire1"))
        if (!Swiping && Input.GetButton("Fire1"))
        {
            StartCoroutine(SwordSwipe(true));
        };
    }

    IEnumerator DelayedRaycastDamage(float amount, float delay)
    {
        yield return new WaitForSeconds(delay);
        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out hit)&& Vector3.Distance(hit.transform.position, player.transform.position) < Range)
        {
            var damageable = hit.transform.GetComponent<Damageable>();
            if (damageable != null)
            {
                damageable.DealDamage(DamageType.Sharp, amount);
            }
        }
    }

    IEnumerator SwordSwipe(bool damage)
    {
        Vector3 startingSwipePos = transform.localPosition;
        Vector3 startingSwipeRot = transform.localRotation.eulerAngles;
        Vector3 startingSwipeScale = transform.localScale;
        Vector3 swipeDestPos = startingSwipePos + swordSwipePosDelta;
        Vector3 swipeDestRot = startingSwipeRot + swordSwipeRotDelta;
        Vector3 swipeDestScale = startingSwipeScale + swordSwipeScaleDelta;
        if (Swiping)
        {
            yield break; //If we're already swiping, abort!
        }
        else
        {
            Swiping = true;
            yield return new WaitForEndOfFrame();
            if (damage)
            {
                StartCoroutine(DelayedRaycastDamage(Damage, durationSeconds / 2));
            }
            float swipePos = 0.0f;
            float perSec = 180.0f / durationSeconds;
            while (Swiping)
            {
                swipePos += Time.deltaTime * perSec;
                if (swipePos >= 180)
                {
                    //if swipePos would be greater than 180 (complete), set swipe Pos to 180 (complete)
                    swipePos = 180;
                    Swiping = false;
                }
                //Yes I DID just learn what the heck a Sine Wave is, thank you
                float sin = Mathf.Sin(swipePos * Mathf.Deg2Rad);
                transform.localPosition = Vector3.Lerp(startingSwipePos, swipeDestPos, sin);
                transform.localRotation = Quaternion.Euler(Vector3.Lerp(startingSwipeRot, swipeDestRot, sin));
                transform.localScale = Vector3.Lerp(startingSwipeScale, swipeDestScale, sin);
                if (!Swiping)
                    yield break;
                else
                    yield return new WaitForEndOfFrame();
            }
        }
    }
}
