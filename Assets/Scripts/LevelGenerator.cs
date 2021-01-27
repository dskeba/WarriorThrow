
using UnityEngine;

public class LevelGenerator : MonoBehaviour
{
    private GameObject backgroundColorPrefab;
    private GameObject platformPrefab;
    private GameObject platformSpikesPrefab;
    private GameObject platformSpikesStarPrefab;
    private GameObject platformWidePrefab;
    private GameObject platformWideSpikesPrefab;
    private GameObject bgPrefab;
    private GameObject cloudPrefab;
    private GameObject cloud2Prefab;
    private GameObject cloud3Prefab;
    private Vector3 lastPlatformPos = new Vector3(0, 0, 0);
    private Vector3 lastBgPos = new Vector3(0, 0, 0);
    private float bgHeight;
    [SerializeField]
    private int totalPlatforms;

    private void Awake()
    {
        LoadPrefabs();
        CacheBgSize();
        GenerateBackgroundColor();
        GenerateFirstBg();
        GenerateFirstPlatform();
        for (int i = 0; i < totalPlatforms; i++)
        {
            GenerateNext(i == totalPlatforms - 1);
        }
    }

    private void LoadPrefabs()
    {
        backgroundColorPrefab = Resources.Load<GameObject>("Prefabs/BackgroundColor");
        platformPrefab = Resources.Load<GameObject>("Prefabs/Platform");
        platformSpikesPrefab = Resources.Load<GameObject>("Prefabs/PlatformSpikes");
        platformSpikesStarPrefab = Resources.Load<GameObject>("Prefabs/PlatformSpikesStar");
        platformWidePrefab = Resources.Load<GameObject>("Prefabs/PlatformWide");
        platformWideSpikesPrefab = Resources.Load<GameObject>("Prefabs/PlatformWideSpikes");
        bgPrefab = Resources.Load<GameObject>("Prefabs/Background");
        cloudPrefab = Resources.Load<GameObject>("Prefabs/Cloud");
        cloud2Prefab = Resources.Load<GameObject>("Prefabs/Cloud2");
        cloud3Prefab = Resources.Load<GameObject>("Prefabs/Cloud3");
    }

    private void CacheBgSize()
    {
        SpriteRenderer bgRenderer = bgPrefab.transform.GetChild(0).GetComponent<SpriteRenderer>();
        bgHeight = bgRenderer.bounds.size.y;
    }

    private void GenerateFirstPlatform()
    {
        Vector3 firstPlatformPos = new Vector3(0, 0, 0);
        Instantiate(platformWidePrefab, firstPlatformPos, Quaternion.identity);
        lastPlatformPos = firstPlatformPos;
    }

    private void GenerateFirstBg()
    {
        Vector3 firstBgPos = new Vector3(0, 0, 0);
        Instantiate(bgPrefab, firstBgPos, Quaternion.identity);
        lastBgPos = firstBgPos;
    }

    private void GenerateBackgroundColor()
    {
        Instantiate(backgroundColorPrefab, new Vector3(0, 0, 0), Quaternion.identity);
    }

    private void GenerateNext(bool lastPlatform)
    {
        GenerateNextPlatform(lastPlatform);
        if (lastPlatformPos.y >= lastBgPos.y)
        {
            GenerateNextCloud();
            GenerateNextBg();
        }
    }

    private void GenerateNextPlatform(bool lastPlatform)
    {
        float nextX = Random.Range(-5, 5);
        if (nextX == lastPlatformPos.x)
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
        float nextY = lastPlatformPos.y + Random.Range(3, 6);
        Vector3 nextPlatformPos = new Vector3(nextX, nextY, 0);
        if (lastPlatform)
        {
            Instantiate(platformSpikesStarPrefab, nextPlatformPos, Quaternion.identity);
            return;
        }
        if (Random.value > 0.5)
        {
            InstantiatePlatform(nextPlatformPos);
        } else
        {
            InstantiateWidePlatform(nextPlatformPos);
        }
        lastPlatformPos = nextPlatformPos;
    }

    private void InstantiatePlatform(Vector3 nextPlatformPos)
    {
        if (Random.value > 0.5)
        {
            Instantiate(platformPrefab, nextPlatformPos, Quaternion.identity);
        }
        else
        {
            Instantiate(platformSpikesPrefab, nextPlatformPos, Quaternion.identity);
        }
    }

    private void InstantiateWidePlatform(Vector3 nextPlatformPos)
    {
        if (Random.value > 0.5)
        {
            Instantiate(platformWidePrefab, nextPlatformPos, Quaternion.identity);
        }
        else
        {
            Instantiate(platformWideSpikesPrefab, nextPlatformPos, Quaternion.identity);
        }
    }

    private void GenerateNextBg()
    {
        float nextX = lastBgPos.x;
        float nextY = lastBgPos.y + bgHeight - 0.01f;
        Vector3 nextPos = new Vector3(nextX, nextY, 0);
        Instantiate(bgPrefab, nextPos, Quaternion.identity);
        lastBgPos = nextPos;
    }

    private void GenerateNextCloud()
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