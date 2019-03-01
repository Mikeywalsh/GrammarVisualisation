using UnityEngine;

public sealed class LineConnection : MonoBehaviour
{
    private static Material LineMaterial;

    private LineRenderer line;
    private GameObject Target;

    private static readonly Color LineColor = Color.green;

    private void Awake()
    {
        if (LineMaterial == null)
        {
            LineMaterial = new Material(Shader.Find("Mobile/Particles/Additive"));
        }

        line = gameObject.AddComponent<LineRenderer>();
        line.material = LineMaterial;
        line.startColor = LineColor;
        line.endColor = LineColor;
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
