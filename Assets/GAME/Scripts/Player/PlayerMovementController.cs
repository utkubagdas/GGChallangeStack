using UnityEngine;

public class PlayerMovementController : MonoBehaviour
{
    #region Serialized
    [SerializeField] private Animator animator;
    #endregion
    
    #region Public
    public float movementSpeed;
    #endregion

    #region Local
    private bool isControlable;
    private static readonly int Run = Animator.StringToHash("Run");
    #endregion

    private void OnEnable()
    {
        EventManager.LevelStartEvent.AddListener(LevelStart);
    }

    private void OnDisable()
    {
        EventManager.LevelStartEvent.RemoveListener(LevelStart);
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
        animator.SetTrigger(Run);
    }
}