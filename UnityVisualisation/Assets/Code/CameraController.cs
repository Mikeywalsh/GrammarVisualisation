using TreeVisualisation.Core;
using TreeVisualisation.Implementations.Grammar;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    private const float PADDING = 20f;
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
                    Debug.Log($"X:{hitNode.Node.Position.X}    Y:{hitNode.Node.Position.Y}");
                }
            }
        }
    }

    public void PositionCamera(Rect treeBounds)
    {
        var fov = GetComponent<Camera>().fieldOfView;
        var viewWidth = Mathf.Min(treeBounds.width, 250);

        var newPosition = new Vector3((treeBounds.xMax - treeBounds.xMin) / 2,
                                      -(treeBounds.yMax - treeBounds.yMin) / 2,
                                      ((viewWidth * Mathf.Sin(fov / 2)) / 2) - PADDING);

        transform.position = newPosition;
    }
}
