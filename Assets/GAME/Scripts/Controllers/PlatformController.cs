using System.Collections.Generic;
using UnityEngine;
using System;

public class PlatformController : MonoBehaviour
{
    #region Serialized
    [SerializeField] private DynamicPlatform platformPrefab;
    [SerializeField] private DynamicPlatform firstPlatform;
    [SerializeField] private GameObject finishLine;
    #endregion
    
    #region Local
    private float prefabScaleZ;
    private float finishPosZ;
    private int maxPlatformCount;
    private int platformCount;
    private float firstPlatformPosZ;
    private List<DynamicPlatform> platformList = new List<DynamicPlatform>();
    private LevelFacade levelFacade;
    #endregion
    
    #region Property
    public DynamicPlatform lastPlatform { get; private set; }
    public DynamicPlatform currentPlatform { get; private set; }
    public AudioController AudioController;
    #endregion
    

    private void Start()
    {
        ControllerHub.Get<GameManager>().SetLevelStarted(false);
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
        if (Input.GetMouseButtonDown(0))
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
        tempFinishPos.z = maxPlatformCount * prefabScaleZ + prefabScaleZ;
        finishPos = tempFinishPos;
        finishLine.transform.position = finishPos;
    }
}