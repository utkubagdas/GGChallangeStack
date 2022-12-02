using UnityEngine;

public class DynamicPlatform : MonoBehaviour
{
    #region Local
    private PlatformController platformController;
    private Vector3 pos1 = new Vector3(-3, 0, 0);
    private Vector3 pos2 = new Vector3(3, 0, 0);
    private LevelFacade levelFacade;
    private bool canSpawn = true;
    private bool firstPlatform;
    #endregion
    
    #region Public
    public MeshRenderer MeshRenderer;
    #endregion
    
    #region Property
    public bool canMove { get; private set; }
    #endregion
    
    private void Start()
    {
        levelFacade = ControllerHub.Get<LevelController>().LevelFacade;
        platformController = levelFacade.PlatformController;

        if (platformController.FirstPlatformCheck(this))
        {
            canMove = false;
            firstPlatform = true;
        }
        else
        {
            canMove = true;
            transform.localScale = platformController.lastPlatform.transform.localScale;
        }
    }

    private void Update()
    {
        if (canMove)
        {
            var tempPos = transform.position;
            tempPos.x = Mathf.Lerp(pos1.x, pos2.x, Mathf.PingPong(Time.time, platformController.PlatformSpeed));
            transform.position = tempPos;
        }
    }

    private void SlicePlatform(float diff, float direction)
    {
        var myTransform = transform;
        
        float newXSize = platformController.lastPlatform.transform.localScale.x - Mathf.Abs(diff);
        float fallingBlockSize = myTransform.localScale.x - newXSize;
        
        float newXPosition = platformController.lastPlatform.transform.position.x + (diff / 2);
        myTransform.localScale = new Vector3(newXSize, transform.localScale.y, myTransform.localScale.z);
        myTransform.position = new Vector3(newXPosition, myTransform.position.y, myTransform.position.z);

        float cubeEdge = myTransform.position.x + (newXSize/2f * direction);
        float fallingBlockXPosition = cubeEdge + fallingBlockSize / 2f * direction;

        SpawnSlicedObj(fallingBlockXPosition, fallingBlockSize);

    }

    private void SpawnSlicedObj(float fallingBlockXPosition, float fallingBlockSize)
    {
        var cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
        var myTransform = transform;
        cube.GetComponent<Renderer>().sharedMaterial = GetComponent<Renderer>().sharedMaterial;
        cube.transform.localScale = new Vector3(fallingBlockSize, myTransform.localScale.y, myTransform.localScale.z);
        cube.transform.position = new Vector3(fallingBlockXPosition, myTransform.position.y, myTransform.position.z);

        cube.AddComponent<Rigidbody>();
        Destroy(cube.gameObject, 1f);
    }
    internal void Stop()
    {
        if (firstPlatform) return;

        canMove = false;
        
        float diff = transform.position.x - platformController.lastPlatform.transform.position.x;
        
        
        if(Mathf.Abs(diff) >= platformController.lastPlatform.transform.localScale.x)
        {
            canSpawn = false;
            EventManager.LevelFailEvent.Invoke();
            return;
        }
        
        platformController.AudioController.PlayAudio(diff);

        float direction = diff > 0 ? 1f : -1f;
        SlicePlatform(diff, direction);
        
    }

    public bool CanSpawn()
    {
        return canSpawn;
    }
}
