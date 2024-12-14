using System.Collections;
using UnityEngine;

public class LockOnGrowth : MonoBehaviour
{
    public Transform outerCircle;
    public Transform innerCircle;

    private Material outerCircleMaterial;
    private Material innerCircleMaterial;

    [Range(0f, 1f)]
    public float growthValue = 0f;

    private Vector3 initialInnerPosition;
    private float initialRadius;

    void Start()
    {
        SetInitialPosition(innerCircle.localPosition);
        initialRadius = innerCircle.localScale.x;

        innerCircleMaterial = Instantiate(innerCircle.GetComponent<MeshRenderer>().material);
        outerCircleMaterial = Instantiate(outerCircle.GetComponent<MeshRenderer>().material);

        innerCircle.GetComponent<MeshRenderer>().material = innerCircleMaterial;
        outerCircle.GetComponent<MeshRenderer>().material = outerCircleMaterial;
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

        innerCircle.localScale = new Vector3(initialRadius, initialRadius, initialRadius);
    }

    public Vector3 GenerateRandomPosition()
    {
        float angle = Random.Range(0, Mathf.PI * 2); // Random angle

        float value = 0.9f;
        float distance = Random.Range(-value, value); // Random distance within the outer radius

        Vector3 position = new Vector3(Mathf.Cos(angle), 0f, Mathf.Sin(angle)) * distance;

        return position;
    }

    public IEnumerator Activate(Material material, float waitTime = 0.2f)
    {
        float time = 0f;
        float max = material.GetFloat("_MaxDissolve");

        while(time < waitTime)
        {
            float x = material.GetFloat("_Dissolve");
            float y = Mathf.Lerp(x, max, time / waitTime);
            time += Time.deltaTime;

            yield return null;
        }

        material.SetFloat("_Dissolve", max);
        yield return null;
    }

    public IEnumerator Deactivate(Material material, float waitTime = 0.2f)
    {
        float time = 0f;
        float min = material.GetFloat("_MinDissolve");

        while (time < waitTime)
        {
            float x = material.GetFloat("_Dissolve");
            float y = Mathf.Lerp(x, min, time / waitTime);
            time += Time.deltaTime;

            yield return null;
        }

        material.SetFloat("_Dissolve", min);
        yield return null;
    }

    public float GetInitialRadius() { return initialRadius; }
    public Material GetInnerMaterial() { return innerCircleMaterial; }
    public Material GetOuterMaterial() { return outerCircleMaterial; }
}
