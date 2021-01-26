using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelGenerator : MonoBehaviour
{
    [SerializeField]
    private GameObject firstBg;
    private GameObject platformPrefab;
    private GameObject platformWidePrefab;
    private GameObject bgPrefab;
    private GameObject cloudPrefab;
    private GameObject cloud2Prefab;
    private GameObject cloud3Prefab;
    private Vector3 lastPos;
    private Vector3 lastBgPos;
    private Vector3 firstBgPos;
    private float bgHeight;
    private float bgWidth;

    private void Awake()
    {
        SpriteRenderer bgRenderer = firstBg.transform.GetChild(0).GetComponent<SpriteRenderer>();
        bgHeight = bgRenderer.bounds.size.y;
        bgWidth = bgRenderer.bounds.size.x;
        lastBgPos = firstBg.transform.position;

        platformPrefab = Resources.Load<GameObject>("Prefabs/Platform");
        platformWidePrefab = Resources.Load<GameObject>("Prefabs/PlatformWide");
        bgPrefab = Resources.Load<GameObject>("Prefabs/Background");
        cloudPrefab = Resources.Load<GameObject>("Prefabs/Cloud");
        cloud2Prefab = Resources.Load<GameObject>("Prefabs/Cloud2");
        cloud3Prefab = Resources.Load<GameObject>("Prefabs/Cloud3");

        for (int i = 0; i < 50; i++)
        {
            generateNext();
        }
    }

    void generateNext()
    {
        generateNextPlatform();
        if (lastPos.y > lastBgPos.y)
        {
            generateNextCloud();
            generateNextBg();
        }
    }

    void generateNextPlatform()
    {
        float nextX = Random.Range(-5, 5);
        if (nextX == lastPos.x)
        {
            if (Random.value > 0.5)
            {
                nextX -= 2;
            }
            else
            {
                nextX += 2;
            }
        }
        float nextY = lastPos.y + Random.Range(3, 6);
        Vector3 nextPos = new Vector3(nextX, nextY, 0);
        if (Random.value > 0.5)
        {
            Instantiate(platformPrefab, nextPos, Quaternion.identity);
        } else
        {
            Instantiate(platformWidePrefab, nextPos, Quaternion.identity);
        }
        lastPos = nextPos;
    }

    void generateNextBg()
    {
        float nextX = lastBgPos.x;
        float nextY = lastBgPos.y + bgHeight - 0.01f;
        Vector3 nextPos = new Vector3(nextX, nextY, 0);
        Instantiate(bgPrefab, nextPos, Quaternion.identity);
        lastBgPos = nextPos;
    }

    void generateNextCloud()
    {
        float nextX = Random.Range(-30, 30);
        float nextY = lastBgPos.y;
        Vector3 nextPos = new Vector3(nextX, nextY, 0);
        float randomValue = Random.value;
        if (randomValue > 0.66f)
        {
            Instantiate(cloudPrefab, nextPos, Quaternion.identity);
        } else if (Random.value > 0.33f)
        {
            Instantiate(cloud2Prefab, nextPos, Quaternion.identity);
        } else
        {
            Instantiate(cloud3Prefab, nextPos, Quaternion.identity);
        }
    }
}