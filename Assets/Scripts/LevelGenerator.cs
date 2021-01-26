using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelGenerator : MonoBehaviour
{
    [SerializeField]
    private GameObject firstBg;

    private GameObject platformPrefab;
    private GameObject bgPrefab;
    private GameObject cloudPrefab;
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
        bgPrefab = Resources.Load<GameObject>("Prefabs/Background");
        cloudPrefab = Resources.Load<GameObject>("Prefabs/Cloud");

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
                nextX -= 1;
            }
            else
            {
                nextX += 1;
            }
        }
        float nextY = lastPos.y + Random.Range(3, 6);
        Vector3 nextPos = new Vector3(nextX, nextY, 0);
        Instantiate(platformPrefab, nextPos, Quaternion.identity);
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
        float nextX;
        if (Random.value > 0.5f)
        {
            nextX = 20;
        } else
        {
            nextX = -20;
        }
        float nextY = lastBgPos.y + bgHeight - 0.01f;
        Vector3 nextPos = new Vector3(nextX, nextY, 0);
        Instantiate(cloudPrefab, nextPos, Quaternion.identity);
    }
}