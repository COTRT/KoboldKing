using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine;

public class Resume : MonoBehaviour
{
    private GameObject Canvas;
    private GameObject resume;

    void OnMouseDown()
    {
        // this object was clicked - do something
        Canvas = GameObject.FindGameObjectWithTag("Canvas");
        Canvas.gameObject.SetActive(false);
    }
    //void Update()
    //{
    //    if (Input.GetKeyDown(KeyCode.Mouse0)) {
    //        RaycastHit hit;
    //        var ray = Camera.main.ScreenPointToRay(Input.mousePosition);

    //        if (Physics.Raycast(ray, hit)) {
    //            if (hit.transform.name == "Resume") {
    //                Canvas = GameObject.FindGameObjectWithTag("Canvas");
    //                Canvas.gameObject.SetActive(false);
    //            } } } }
    //void OnMouseDown()
    //    {
    //        resume = GameObject.FindGameObjectWithTag("Resume");
    //        Canvas = GameObject.FindGameObjectWithTag("Canvas");
    //        Canvas.gameObject.SetActive(false);
    //        Time.timeScale = 1;
    //    }
     //   if (resume.Input.GetKeyDown(KeyCode.Mouse0))
       //     {
            //resume = GameObject.FindGameObjectWithTag("Resume");
            //Canvas = GameObject.FindGameObjectWithTag("Canvas");
            //Canvas.gameObject.SetActive(false);
            //Time.timeScale = 1;
        //}
    //}
}