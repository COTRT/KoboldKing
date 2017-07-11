using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordScript : MonoBehaviour
{
    public float durationSeconds = 2;
    public Vector3 swordSwipePosDelta;
    public Vector3 swordSwipeRotDelta;
    public Vector3 swordSwipeScaleDelta;
    
    public bool swiping;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (!swiping&&Input.GetButton("Fire1"))
        {
            Debug.Log("Starting Sword Swipe");
            
            StartCoroutine(SwordSwipe());
            swiping = true; //since the coroutine kinda runs seperately from everything else, there's a chance that swiping will still be false despite the coroutine having been started (they call it a "race condition").  We set swiping to true here as well to avoid that.
        }
    }

    IEnumerator SwordSwipe()
    {
        Debug.Log("Swiping Sword");
        
        Vector3 startingSwipePos = transform.localPosition;
        Vector3 startingSwipeRot = transform.localRotation.eulerAngles;
        Vector3 startingSwipeScale = transform.localScale;
        Vector3 swipeDestPos = startingSwipePos + swordSwipePosDelta;
        Vector3 swipeDestRot = startingSwipeRot + swordSwipeRotDelta;
        Vector3 swipeDestScale = startingSwipeScale + swordSwipeScaleDelta;
        if (swiping)
        {
            yield break; //If we're already swiping, abort!
        }
        swiping = true;
        float swipePos = 0;
        float perSec = 180.0f / durationSeconds;
        while (swiping)
        {
            swipePos += Time.deltaTime * perSec;
            if(swipePos >= 180)
            {
                //if swipePos would be greater than 180 (complete), set swipe Pos to 180 (complete)
                swipePos = 180;
                swiping = false;
            }
            Debug.Log(swipePos);
            //Yes I DID just learn what the heck a Sine Wave is, thank you
            float sin = Mathf.Sin(swipePos*Mathf.Deg2Rad);
            transform.localPosition = Vector3.Lerp(startingSwipePos, swipeDestPos, sin);
            transform.localRotation = Quaternion.Euler(Vector3.Lerp(startingSwipeRot, swipeDestRot, sin));
            transform.localScale = Vector3.Lerp(startingSwipeScale, swipeDestScale, sin);
            yield return new WaitForEndOfFrame();
        }
    }
}
