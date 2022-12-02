using System.Collections.Generic;
using UnityEngine;
using System;
using Random = UnityEngine.Random;

public class PlatformController : MonoBehaviour
{
    #region Serialized
    [SerializeField] private DynamicPlatform platformPrefab;
    [SerializeField] private DynamicPlatform firstPlatform;
    [SerializeField] private GameObject finishLine;
    [SerializeField] private BoxCollider fallCollider;
    #endregion
    
    #region Local
    private float prefabScaleZ;
    private float finishPosZ;
    private int maxPlatformCount;
    private int platformCount;
    private float firstPlatformPosZ;
    private List<DynamicPlatform> platformList = new List<DynamicPlatform>();
    private LevelFacade levelFacade;
    private DynamicPlatform tempFp;
    private GameManager gameManager;
    #endregion
    
    #region Property
    public DynamicPlatform lastPlatform { get; private set; }
    public DynamicPlatform currentPlatform { get; private set; }
    private float platformSpeed = 1.0f;
    public float PlatformSpeed => platformSpeed;
    #endregion
    
    #region Public
    public AudioController AudioController;
    #endregion

    private void OnEnable()
    {
        EventManager.LevelRedesignEvent.AddListener(RedesignLevel);
    }

    private void OnDisable()
    {
        EventManager.LevelRedesignEvent.RemoveListener(RedesignLevel);
    }

    private void Start()
    {
        gameManager = ControllerHub.Get<GameManager>();
        gameManager.SetLevelStarted(false);
        levelFacade = ControllerHub.Get<LevelController>().LevelFacade;
        firstPlatformPosZ = firstPlatform.transform.position.z;
        prefabScaleZ = platformPrefab.transform.localScale.z;
        
        AlignFinishLineToCubeSize(); //so that the last cube fits exactly where the finish line is placed.
        
        currentPlatform = firstPlatform;
        lastPlatform = firstPlatform;

        SpawnPlatform();
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0) && !gameManager.levelFinished && currentPlatform.canMove)
        {
            currentPlatform.Stop();
            lastPlatform = currentPlatform;
            if (!lastPlatform.CanSpawn()) return;
            
            SpawnPlatform();
        }
    }

    public void SpawnPlatform()
    {
        if(platformCount >= maxPlatformCount)
        {
            return;
        }
        platformCount++;
        var cube = Instantiate(platformPrefab, levelFacade.transform, true);
        cube.MeshRenderer.material.color = Random.ColorHSV(0f, 1f, 1f, 1f, 0.5f, 1f);
        platformList.Add(cube);

        if (lastPlatform != null)
        {
            var lpTransform = lastPlatform.transform;
            var lpPosition = lpTransform.position;
            cube.transform.position = new Vector3(lpPosition.x, lpPosition.y,
                lpPosition.z + lastPlatform.GetComponent<Renderer>().bounds.size.z);
        }
        else
        {
            lastPlatform = firstPlatform;
            var transform1 = firstPlatform.transform;
            var position = transform1.position;
            cube.transform.position = new Vector3(position.x, position.y,
                position.z + firstPlatform.GetComponent<Renderer>().bounds.size.z);
        }

        currentPlatform = cube;

    }

    public bool FirstPlatformCheck(DynamicPlatform platform)
    {
        if (platform == firstPlatform)
        {
            return true;
        }
        
        return false;
    }

    private void AlignFinishLineToCubeSize()
    {
        var finishPos = finishLine.transform.position;
        finishPosZ = finishPos.z;

        maxPlatformCount = Convert.ToInt32(Mathf.Abs((firstPlatformPosZ - finishPosZ) / prefabScaleZ));
        var tempFinishPos = finishPos;
        tempFinishPos.z = maxPlatformCount * prefabScaleZ + prefabScaleZ - firstPlatform.transform.position.z;
        finishPos = tempFinishPos;
        finishLine.transform.position = finishPos;
    }

    public void RedesignLevel()
    {
        for (int i = 0; i < platformList.Count; i++)
        {
            Destroy(platformList[i]);
        }
        platformList.Clear();
        
        tempFp = Instantiate(firstPlatform, levelFacade.transform);
        tempFp.transform.position = new Vector3(firstPlatform.transform.position.x, firstPlatform.transform.position.y,
            finishLine.transform.position.z + finishLine.GetComponent<MeshRenderer>().bounds.size.z);
        firstPlatform = tempFp;
        lastPlatform = null;
        platformCount = 0;
        AlignNewFinishLine();
        SpawnPlatform();
    }

    private void AlignNewFinishLine()
    {
        var oldFinish = finishLine;
        var newFinish = Instantiate(finishLine, levelFacade.transform);
        finishLine = newFinish;
        var finishPos = finishLine.transform.position;
        finishPosZ = finishPos.z;
        
        var tempFinishPos = finishPos;
        tempFinishPos.z = oldFinish.transform.position.z + maxPlatformCount * prefabScaleZ + prefabScaleZ + oldFinish.GetComponent<MeshRenderer>().bounds.size.z / 1.5f;
        finishPos = tempFinishPos;
        finishLine.transform.position = finishPos;

        var tempColSize = fallCollider.size;
        tempColSize.z += Mathf.Abs(firstPlatformPosZ - finishPosZ) + firstPlatform.MeshRenderer.bounds.size.z * 2.5f;
        fallCollider.size = tempColSize;
    }
}