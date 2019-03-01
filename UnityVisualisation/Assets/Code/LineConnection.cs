using UnityEngine;

public sealed class LineConnection : MonoBehaviour
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

        // Put lines behind nodes
        linePositions[0] += new Vector3(0, 0, 1);
        linePositions[1] += new Vector3(0, 0, 1);

        line.SetPositions(linePositions);
    }

    public void SetConnections(GameObject target)
    {
        Target = target;
    }
}
