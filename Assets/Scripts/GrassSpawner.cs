using UnityEngine;

public class GrassSpawner : MonoBehaviour
{
    [Header("Settings")]
    public Color sampledColor;          // Input: current sampled color (set from your other script)
    GrassDetector grassDetector;
    [Range(0f, 1f)] public float greenThreshold = 0.25f; // Min green intensity normalized (0 to 1)
    public int grassCount = 10;         // Number of grass objects to spawn
    public float spawnRadius = 3f;      // Radius around center point to spawn grass
    public GameObject grassPrefab;      // Prefab of grass to spawn

    [Header("Floor Detection")]
    public LayerMask floorLayerMask;    // Layer mask to detect floor only

    void Start()
    {
        grassDetector = GetComponent<GrassDetector>();
    }

    void LateUpdate()
    {
        sampledColor = grassDetector.currentColor;
        if (IsGreenEnough(sampledColor))
        {
            SpawnGrassOnFloor();
        }
    }

    bool IsGreenEnough(Color color)
    {
        // Simple green check: green channel must be higher than red and blue by threshold percentage
        // You can customize this logic for your needs
        float green = color.g;
        float red = color.r;
        float blue = color.b;

        // Check if green is significantly dominant
        bool greenDominant = (green - Mathf.Max(red, blue)) > greenThreshold;
        return greenDominant;
    }

    void SpawnGrassOnFloor()
    {
        Vector3 cameraPos = Camera.main.transform.position;
        Vector3 cameraForward = Camera.main.transform.forward;

        // Cast a ray downwards from a point in front of the camera to find the floor
        Vector3 rayOrigin = cameraPos + cameraForward * 1f; // 1 meter in front of camera
        Ray ray = new Ray(rayOrigin, Vector3.down);
        if (Physics.Raycast(ray, out RaycastHit hit, 10f, floorLayerMask))
        {
            Vector3 floorPoint = hit.point;

            // Spawn grassCount objects randomly within spawnRadius on the floor plane
            for (int i = 0; i < grassCount; i++)
            {
                Vector2 randomCircle = Random.insideUnitCircle * spawnRadius;
                Vector3 spawnPos = floorPoint + new Vector3(randomCircle.x, 0f, randomCircle.y);

                // Raycast down from above to find exact floor height at spawnPos
                Ray downRay = new Ray(spawnPos + Vector3.up * 2f, Vector3.down);
                if (Physics.Raycast(downRay, out RaycastHit floorHit, 5f, floorLayerMask))
                {
                    spawnPos.y = floorHit.point.y+0.1f;
                    Instantiate(grassPrefab, spawnPos, Quaternion.identity);
                }
            }
        }
    }


}
