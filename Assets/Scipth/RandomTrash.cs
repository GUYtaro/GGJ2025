using System.Collections.Generic;
using UnityEngine;

public class RandomTrash : MonoBehaviour
{
    public GameObject[] possibleObjects; // Array เก็บ Object ที่สามารถเลือกได้
    public int numberOfObjects = 20;    // จำนวนวัตถุที่ต้องการสร้าง
    public Collider spawnArea;          // Collider สำหรับกำหนดพื้นที่สุ่ม

    private List<GameObject> spawnedObjects = new List<GameObject>(); // เก็บ Object ที่ถูกสร้างทั้งหมด

    void Start()
    {
        // ตรวจสอบว่ามี Collider หรือไม่
        if (spawnArea == null)
        {
            Debug.LogError("Please assign a Collider to define the spawn area!");
            return;
        }

        // สร้างวัตถุแบบสุ่ม
        for (int i = 0; i < numberOfObjects; i++)
        {
            SpawnRandomObject();
        }
    }

    void SpawnRandomObject()
    {
        // เลือก Object แบบสุ่มจาก Array
        int randomIndex = Random.Range(0, possibleObjects.Length);
        GameObject selectedObject = possibleObjects[randomIndex];

        // สุ่มตำแหน่งภายใน Collider
        Vector3 randomPosition = GetRandomPositionInCollider();

        // สุ่ม Rotation แบบ 3 แกน (x, y, z)
        Quaternion randomRotation = Quaternion.Euler(
            Random.Range(0f, 360f), // หมุนในแกน X
            Random.Range(0f, 360f), // หมุนในแกน Y
            Random.Range(0f, 360f)  // หมุนในแกน Z
        );

        // สร้าง Object พร้อมตำแหน่งและการหมุนแบบสุ่ม และเก็บไว้ใน List
        GameObject spawnedObject = Instantiate(selectedObject, randomPosition, randomRotation);
        spawnedObjects.Add(spawnedObject);
    }

    Vector3 GetRandomPositionInCollider()
    {
        // หา Boundaries ของ Collider
        Bounds bounds = spawnArea.bounds;

        // สุ่มตำแหน่งใน Boundaries
        Vector3 randomPosition = new Vector3(
            Random.Range(bounds.min.x, bounds.max.x),
            Random.Range(bounds.min.y, bounds.max.y),
            Random.Range(bounds.min.z, bounds.max.z)
        );

        // ตรวจสอบว่าอยู่ใน Collider จริงๆ หรือไม่
        if (spawnArea is MeshCollider meshCollider && !meshCollider.convex)
        {
            if (!Physics.Raycast(randomPosition + Vector3.up * 10, Vector3.down, out RaycastHit hit, Mathf.Infinity) || hit.collider != spawnArea)
            {
                return GetRandomPositionInCollider();
            }
        }

        return randomPosition;
    }

    private void OnDrawGizmos()
    {
        if (spawnArea != null)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireCube(spawnArea.bounds.center, spawnArea.bounds.size);
        }
    }
}
