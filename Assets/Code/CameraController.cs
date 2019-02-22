using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    void Start()
    {
        
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {

            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            Debug.DrawRay(ray.origin, ray.direction * 500);

            if (Physics.Raycast(ray, out hit))
            {
                var hitNode = hit.transform.GetComponent<IGrammarTreeNodeObject>();
                if (hitNode != null)
                {
                    Debug.Log("Hit node!");
                }
            }
        }
    }
}
