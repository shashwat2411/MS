using UnityEngine;
using static EnemyBase;

public class TutorialTextManager : MonoBehaviour
{
    private bool currentScreenIs1 = true;
    public float dissolveDuration = 0.75f;

    public EnemyMaterial screen1;
    public EnemyMaterial screen2;

    public Texture[] array1;
    public Texture[] array2;

    private int index1 = -1;
    private int index2 = -1;

    public float backZ = 0.061f;
    public float frontZ = 0.078f;

    private int _MainTex = Shader.PropertyToID("_MainTex");
    void Start()
    {
        index1 = -1;
        index2 = -1;

        screen1.InstantiateMaterial();
        screen2.InstantiateMaterial();

        IncrementIndex1();
        IncrementIndex2();
    }

    private void IncrementIndex1()
    {
        if (index1 < (array1.Length - 1)) { index1++; }
        else { index1 = array1.Length - 1; }

        screen1.material.SetTexture(_MainTex, array1[index1]);
    }
    private void IncrementIndex2()
    {
        if (index2 < (array2.Length - 1)) { index2++; }
        else { index2 = array2.Length - 1; }

        screen2.material.SetTexture(_MainTex, array2[index2]);
    }

    public void ChangeScreen()
    {
        if (currentScreenIs1 == false)
        {
            currentScreenIs1 = true;

            SetLocalPosition(screen1.renderer.transform, backZ);
            SetLocalPosition(screen2.renderer.transform, frontZ);

            screen1.SetDissolveToMax();
            screen2.SetDissolveToMax();

            StartCoroutine(screen2.DissolveOut(dissolveDuration));
            Invoke("IncrementIndex2", dissolveDuration);
        }
        else
        {
            currentScreenIs1 = false;

            SetLocalPosition(screen1.renderer.transform, frontZ);
            SetLocalPosition(screen2.renderer.transform, backZ);

            screen1.SetDissolveToMax();
            screen2.SetDissolveToMax();

            StartCoroutine(screen1.DissolveOut(dissolveDuration));
            Invoke("IncrementIndex1", dissolveDuration);
        }
    }
    private void SetLocalPosition(Transform point, float position)
    {
        Vector3 localPosition = point.localPosition;
        localPosition.z = position;
        point.localPosition = localPosition;
    }
}
