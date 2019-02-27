using System.Collections;
using System.Collections.Generic;
using GrammarTree.Core;
using GrammarTree.Implementations.Grammar;
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
                var hitNode = hit.transform.GetComponent<ITreeNodeObject<GrammarData>>();
                if (hitNode != null)
                {
                    Debug.Log("Hit node!");
                }
            }
        }
    }
}
