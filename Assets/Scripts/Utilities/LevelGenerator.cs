
using System.Collections.Generic;
using UnityEngine;

public class LevelGenerator : MonoBehaviour
{
    private int totalPlatforms;
    private float widePlatformRatio;
    private float spikedPlatformRatio;
    private float icePlatformRatio;
    private GameObject backgroundColorPrefab;
    private Dictionary<string, GameObject> platformPrefabs;
    private Dictionary<LevelItem, GameObject> itemPrefabs;
    private List<GameObject> instantiatedGameObjects;
    private GameObject bgPrefab;
    private GameObject cloudPrefab;
    private GameObject cloud2Prefab;
    private GameObject cloud3Prefab;
    private Vector3 lastPlatformPos = new Vector3(0, 0, 0);
    private Vector3 lastBgPos = new Vector3(0, 0, 0);
    private float bgHeight;
    private LevelItem _levelItem;

    public float TotalHeight;

    private void Awake()
    {
        LoadPrefabs();
        CacheBgSize();
        GenerateBackgroundColor();
    }

    public void GenerateLevel(bool destroyLevel, LevelItem item, int platforms, float wideRatio, float spikedRatio, float iceRatio)
    {
        if (destroyLevel)
        {
            DestoryLevel();
        }
        _levelItem = item;
        totalPlatforms = platforms;
        widePlatformRatio = wideRatio;
        spikedPlatformRatio = spikedRatio;
        icePlatformRatio = iceRatio;
        GenerateFirstBg();
        GenerateFirstPlatform();
        for (int i = 0; i < totalPlatforms; i++)
        {
            GenerateNext(i == totalPlatforms - 1);
        }
    }

    private void DestoryLevel()
    {
        foreach(GameObject gameObject in instantiatedGameObjects)
        {
            Destroy(gameObject);
        }
    }

    private void LoadPrefabs()
    {
        instantiatedGameObjects = new List<GameObject>();

        backgroundColorPrefab = Resources.Load<GameObject>("Prefabs/BackgroundColor");

        platformPrefabs = new Dictionary<string, GameObject>();
        platformPrefabs.Add("Prefabs/Platform", Resources.Load<GameObject>("Prefabs/Platform"));
        platformPrefabs.Add("Prefabs/PlatformWide", Resources.Load<GameObject>("Prefabs/PlatformWide"));
        platformPrefabs.Add("Prefabs/PlatformWideSpiked", Resources.Load<GameObject>("Prefabs/PlatformWideSpiked"));
        platformPrefabs.Add("Prefabs/PlatformWideIce", Resources.Load<GameObject>("Prefabs/PlatformWideIce"));
        platformPrefabs.Add("Prefabs/PlatformWideSpikedIce", Resources.Load<GameObject>("Prefabs/PlatformWideSpikedIce"));
        platformPrefabs.Add("Prefabs/PlatformSpiked", Resources.Load<GameObject>("Prefabs/PlatformSpiked"));
        platformPrefabs.Add("Prefabs/PlatformSpikedIce", Resources.Load<GameObject>("Prefabs/PlatformSpikedIce"));
        platformPrefabs.Add("Prefabs/PlatformIce", Resources.Load<GameObject>("Prefabs/PlatformIce"));

        itemPrefabs = new Dictionary<LevelItem, GameObject>();
        itemPrefabs.Add(LevelItem.BOOTS_OF_FRICTION, Resources.Load<GameObject>("Prefabs/BootsOfFriction"));
        itemPrefabs.Add(LevelItem.HELMET_OF_FARSIGHT, Resources.Load<GameObject>("Prefabs/HelmetOfFarsight"));

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
        instantiatedGameObjects.Add(Instantiate(platformPrefabs["Prefabs/PlatformWide"], firstPlatformPos, Quaternion.identity));
        lastPlatformPos = firstPlatformPos;
    }

    private void GenerateFirstBg()
    {
        Vector3 firstBgPos = new Vector3(0, 0, 0);
        instantiatedGameObjects.Add(Instantiate(bgPrefab, firstBgPos, Quaternion.identity));
        lastBgPos = firstBgPos;
    }

    private void GenerateBackgroundColor()
    {
        Instantiate(backgroundColorPrefab, new Vector3(0, 0, 0), Quaternion.identity);
    }

    private void GenerateNext(bool lastPlatform)
    {
        instantiatedGameObjects.Add(GenerateNextPlatform(lastPlatform));
        if (lastPlatformPos.y >= lastBgPos.y)
        {
            instantiatedGameObjects.Add(GenerateNextCloud());
            instantiatedGameObjects.Add(GenerateNextBg());
        }
    }

    private GameObject GenerateNextPlatform(bool lastPlatform)
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
            GameObject platform = Instantiate(platformPrefabs["Prefabs/Platform"], nextPlatformPos, Quaternion.identity);
            Vector3 itemPos = nextPlatformPos + new Vector3(0f, 2f, 0f);
            Instantiate(itemPrefabs[_levelItem], itemPos, Quaternion.identity);
            TotalHeight = itemPos.y;
            return platform;
        }
        lastPlatformPos = nextPlatformPos;
        return InstantiatePlatform(nextPlatformPos);
    }

    private GameObject InstantiatePlatform(Vector3 nextPlatformPos)
    {
        string platformPrefab = "Prefabs/Platform";
        if (Random.value <= widePlatformRatio)
        {
            platformPrefab += "Wide";
        }
        if (Random.value <= spikedPlatformRatio)
        {
            platformPrefab += "Spiked";
        }
        if (Random.value <= icePlatformRatio)
        {
            platformPrefab += "Ice";
        }
        return Instantiate(platformPrefabs[platformPrefab], nextPlatformPos, Quaternion.identity);
    }

    private GameObject GenerateNextBg()
    {
        float nextX = lastBgPos.x;
        float nextY = lastBgPos.y + bgHeight;
        Vector3 nextPos = new Vector3(nextX, nextY, 0);
        lastBgPos = nextPos;
        return Instantiate(bgPrefab, nextPos, Quaternion.identity);
    }

    private GameObject GenerateNextCloud()
    {
        float nextX = Random.Range(-30, 30);
        float nextY = lastBgPos.y;
        Vector3 nextPos = new Vector3(nextX, nextY, 0);
        float randomValue = Random.value;
        if (randomValue > 0.66f)
        {
            return Instantiate(cloudPrefab, nextPos, Quaternion.identity);
        } 
        else if (Random.value > 0.33f)
        {
            return Instantiate(cloud2Prefab, nextPos, Quaternion.identity);
        } 
        else
        {
            return Instantiate(cloud3Prefab, nextPos, Quaternion.identity);
        }
    }
}