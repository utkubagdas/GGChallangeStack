using UnityEngine;

public class PlayerMovementController : MonoBehaviour
{
    #region Serialized
    [SerializeField] private Animator animator;
    [SerializeField] private PlayerFacade playerFacade;
    #endregion
    
    #region Public
    public float movementSpeed;
    #endregion

    #region Local
    private bool isControlable;
    #endregion

    
    private void OnEnable()
    {
        EventManager.LevelStartEvent.AddListener(LevelStart);
        EventManager.StageStartEvent.AddListener(LevelStart);
    }

    private void OnDisable()
    {
        EventManager.LevelStartEvent.RemoveListener(LevelStart);
        EventManager.StageStartEvent.RemoveListener(LevelStart);
    }
    
    void Update()
    {
        if(isControlable)
        {
            var transform1 = transform;
            transform1.position += transform1.forward * Time.deltaTime * movementSpeed;
        }
    }

    private void LevelStart()
    {
        isControlable = true;
        playerFacade.AnimationController.SetTriggerAnimation(Consts.AnimatorKeywords.RUN);
    }

    public void SetControlable(bool controlable)
    {
        isControlable = controlable;
    }
}
