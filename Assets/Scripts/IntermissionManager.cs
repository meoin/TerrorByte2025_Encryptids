using System.Data;
using UnityEngine;
using UnityEngine.SceneManagement;

public class IntermissionManager : MonoBehaviour
{
    public GameObject player;
    public GameObject mainCamera;
    public GameObject topBar;
    public GameObject bottomBar;
    public float distanceToTravel = 15f;
    public float cameraSlideDistance = 5f;
    private float cameraSlideTravelled = 0f;
    public float movementSpeed = 0.01f;
    public float pauseTimeTotal = 2f;
    private float pauseTimer = 0f;
    private float distanceTravelled;
    public float barDistanceToMove = 160f;
    private float barDistanceTravelled = 0f;
    public float waitBeforeBattle = 2f;
    private float timeWaitedBeforeBattle = 0f;
    private bool distanceReached = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        barDistanceToMove = Screen.height / 3;
    }

    // Update is called once per frame
    void Update()
    {
        if (distanceTravelled < distanceToTravel)
        {
            distanceTravelled += movementSpeed;
            transform.Translate(movementSpeed, 0f, 0f);
            BobPlayer();
        }
        else if (distanceTravelled >= distanceToTravel && !distanceReached)
        {
            Vector3 euler = new Vector3(0f, 0f, 0f);
            player.transform.rotation = Quaternion.Euler(euler);

            if (pauseTimer < pauseTimeTotal)
            {
                pauseTimer += Time.deltaTime;
            }
            else if (cameraSlideTravelled < cameraSlideDistance)
            {
                cameraSlideTravelled += movementSpeed / 4;
                mainCamera.transform.Translate(movementSpeed / 4, 0f, 0f);
            }
            else if (barDistanceTravelled < barDistanceToMove)
            {
                barDistanceTravelled += 1;
                topBar.transform.Translate(0f, -0.5f, 0f);
                bottomBar.transform.Translate(0f, 0.5f, 0f);
            }
            else if (timeWaitedBeforeBattle < waitBeforeBattle)
            {
                timeWaitedBeforeBattle += Time.deltaTime;
            }
            else if (barDistanceTravelled < (Screen.height/3) * 2)
            {
                barDistanceTravelled += 3;
                topBar.transform.Translate(0f, -4f, 0f);
                bottomBar.transform.Translate(0f, 4f, 0f);
            }
            else 
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
            }
        } 
    }

    void BobPlayer() 
    {
        float rotation = Mathf.Sin(Time.time * 10);
        Vector3 euler = new Vector3(0f, 0f, rotation * 10);
        player.transform.rotation = Quaternion.Euler(euler);
    }
}
