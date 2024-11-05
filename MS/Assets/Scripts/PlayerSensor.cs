using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum SENSORTYPE
{
    INTERACT = 5

}
public class PlayerSensor : MonoBehaviour
{



    [Header("プレーヤーができる距離")]
    public SENSORTYPE checkDistance;

    [Header("角度は制限され、プレーヤーの正面と障害物の法線との間の角度は、この値よりも大きく、インタラクティブにすることはできません。")]
    public float interactAngle = 45f;



    Vector3 hitNormal;   //障害物の法線

    // Start is called before the first frame update
    void Start()
    {
        
    }

    public bool SensorCheck(Transform playerTransform,Vector3 inputDirection, SENSORTYPE type)
    {
        Vector3 offset = new Vector3(0.0f, 0.5f, 0.0f);
        Debug.DrawRay(playerTransform.position + offset, playerTransform.forward);
        if (Physics.Raycast(playerTransform.position + offset,playerTransform.forward, out RaycastHit obsHit, (float)type))
        {
            Debug.Log(obsHit.collider.name);
            if (!obsHit.collider.CompareTag("Interactable"))
            {
                Debug.Log("tag false");
                return false;
            }

            hitNormal = obsHit.normal;
            if(Vector3.Angle(-hitNormal, playerTransform.forward) > interactAngle || Vector3.Angle(hitNormal, inputDirection) > interactAngle)
            {
                Debug.Log("角度不正");
                return false;
            }
            return true;
        }
        return false;
    }
}
