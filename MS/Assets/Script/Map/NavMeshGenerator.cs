using System.Collections;
using System.Collections.Generic;
using Unity.AI.Navigation;
using UnityEngine;
using UnityEngine.AI;

public class NavMeshGenerator : MonoBehaviour
{   
    public void GenerateNavMesh(GameObject stage)
    {
        NavMesh.RemoveAllNavMeshData();

        NavMeshSurface navMeshSurface = stage.transform.GetChild(1).gameObject.GetComponent<NavMeshSurface>();

        if (navMeshSurface == null)
        {
            Debug.LogError("NavMeshSurface is not assigned!");
            return;
        }

        // Rebuild the NavMesh
        navMeshSurface.BuildNavMesh();
    }
}
