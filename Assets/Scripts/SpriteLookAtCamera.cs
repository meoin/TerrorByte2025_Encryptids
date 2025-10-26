using UnityEngine;

public class SpriteLookAtCamera : MonoBehaviour
{
    public GameObject parent;
    private bool shot = false;
    private bool dead = false;
    private float fallRotation = 0f;
    private float spinRotation = 180f;
    public bool canBeShot = true;
    public bool animated = false;
    public MinigameManager minigameManager;
    public Animator animator;
    public string animationName;
    private Vector3 startPosition;
    private Quaternion startRotation;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (animated) 
        {
            animator.enabled = false;
            startPosition = parent.transform.position;
            startRotation = parent.transform.rotation;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (dead)
        {
            
            fallRotation = Mathf.Min(fallRotation + 3, 90f);
            Vector3 euler = new Vector3(fallRotation, 0f, 0f);
            parent.transform.rotation = Quaternion.Euler(euler);
        }
        else if (shot)
        {
            spinRotation = spinRotation + 9f;
            Vector3 euler = new Vector3(0f, spinRotation, 0f);
            transform.rotation = Quaternion.Euler(euler);
        }
        else 
        {
            fallRotation = 0f;
            Vector3 euler = new Vector3(0f, 0f, 0f);
            parent.transform.rotation = Quaternion.Euler(euler);
            transform.LookAt(Camera.main.transform);
            transform.Rotate(0, 180, 0);
        }

        if (animated) 
        {
            if (minigameManager.gameStarted && !animator.isActiveAndEnabled && (minigameManager.GetPlayerWins() != minigameManager.winsRequired)) 
            {
                animator.enabled = true;
            }
        }
        
    }

    public void Shot() 
    {
        canBeShot = false;
        shot = true;
        if (animated)
        {
            animator.enabled = false;
        }
    }

    public void FallOver() 
    {
        if (animated)
        {
            animator.enabled = false;
        }
        transform.rotation = Quaternion.Euler(new Vector3(0f, 0f, 0f));
        dead = true;
    }

    public void StandBackUp() 
    {
        canBeShot = true;
        dead = false;
        shot = false;

        if (animated)
        {
            animator.enabled = true;
            animator.Play(animationName, 1, 0f);
            parent.transform.position = startPosition;
            parent.transform.rotation = startRotation;
        }
    }
}
