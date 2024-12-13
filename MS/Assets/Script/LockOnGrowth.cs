using UnityEngine;

public class LockOnGrowth : MonoBehaviour
{
    public Transform outerCircle;
    public Transform innerCircle;

    [Range(0f, 1f)]
    public float growthValue = 0f;

    private Vector3 initialInnerPosition;
    private float initialRadius;

    void Start()
    {
        SetInitialPosition(innerCircle.localPosition);
        initialRadius = innerCircle.localScale.x;
    }

    void FixedUpdate()
    {
        float currentRadius = Mathf.Lerp(initialRadius, 1f, growthValue);

        innerCircle.localScale = new Vector3(currentRadius, currentRadius, currentRadius);

        Vector3 position = Vector3.Lerp(initialInnerPosition, new Vector3(0f, 0f, 0f), growthValue);
        innerCircle.localPosition = new Vector3(position.x, innerCircle.localPosition.y, position.z);
    }

    public void SetInitialPosition(Vector3 position)
    {
        innerCircle.localPosition = new Vector3(position.x, innerCircle.localPosition.y, position.z);
        initialInnerPosition = innerCircle.localPosition;
    }

    public Vector3 GenerateRandomPosition()
    {
        float angle = Random.Range(0, Mathf.PI * 2); // Random angle

        float value = 0.9f;
        float distance = Random.Range(-value, value); // Random distance within the outer radius

        Vector3 position = new Vector3(Mathf.Cos(angle), 0f, Mathf.Sin(angle)) * distance;

        return position;
    }

    public float GetInitialRadius() { return initialRadius; }
}
