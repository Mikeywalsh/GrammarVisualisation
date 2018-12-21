using UnityEngine;

sealed public class LineConnection : MonoBehaviour
{
    private LineRenderer line;
    private GameObject Target;

    private void Awake()
    {
        line = gameObject.AddComponent<LineRenderer>();
    }

    private void Update()
    {
        Vector3[] linePositions = new Vector3[2];
        linePositions[0] = transform.position;
        linePositions[1] = Target?.transform.position ?? transform.position;

        line.SetPositions(linePositions);
    }

    public void SetConnections(GameObject target)
    {
        Target = target;
    }
}
