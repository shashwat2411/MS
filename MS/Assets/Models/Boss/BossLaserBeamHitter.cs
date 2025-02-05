using System.Collections;
using UnityEngine;
using UnityEngine.Jobs;

public class BossLaserBeamHitter : MonoBehaviour
{
    public BoxCollider collider { get; private set; }

    public Vector3 originalLocalScale { get; private set; }

    private BossEnemy owner;
    // Start is called before the first frame update
    void Awake()
    {
        collider = GetComponent<BoxCollider>();

        originalLocalScale = new Vector3(1f, 1f, 1f);
        transform.localScale = new Vector3(1f, 1f, 0.01f);

        collider.enabled = false;

        owner = GetComponentInParent<BossEnemy>();
    }

    // Update is called once per frame
    //void FixedUpdate()
    //{
    //    transform.rotation = laserBeamEffect.transform.rotation;
    //}


    private void OnTriggerEnter(Collider other)
    {
        var player = other.GetComponent<PlayerManager>();
        if (player)
        {
            player.playerHP.Damage(owner.laserBeamAttackPower);
            Debug.Log("Laser hit");
        }
    }

    public IEnumerator CollisionSize(float start, float end, float duration)
    {
        float elapsed = 0f;

        collider.enabled = true;

        Vector3 newScale = originalLocalScale;
        newScale.z = start;
        transform.localScale = newScale;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;

            float z = Mathf.Lerp(start, end, elapsed / duration);
            newScale.z = z;
            transform.localScale = newScale;

            yield return null;
        }

        newScale.z = start;
        transform.localScale = newScale;

        collider.enabled = false;
    }
}