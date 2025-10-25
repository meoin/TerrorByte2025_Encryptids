using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using System;
using static UnityEditor.PlayerSettings;

public class CrosshairMovement : MonoBehaviour
{
    public float maxMoveSpeed = 1f;
    private float moveSpeed = 3f;
    public float maxDistance = 200f;
    private float timer = 0f;
    private float intervalTimer = 0f;
    public float interval = 3f;
    private Vector3 centerPosition;
    private Vector3 floatTowardsPosition;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        centerPosition = transform.position;
        SetRandomTarget();
    }

    // Update is called once per frame
    void Update()
    {
        AdjustMoveSpeed();

        PlayerInput();
        FloatCursorTowardsRandomTarget();
        CrosshairIsOverOpponent();
        //FigureEight(80f);
        //RandomForce(30f);

        intervalTimer += Time.deltaTime;
        while (intervalTimer >= interval)
        {
            SetRandomTarget();
            intervalTimer -= interval;
        }

        if (Input.GetKeyDown(KeyCode.Space)) 
        {
            if (CrosshairIsOverOpponent()) 
            {
                Debug.Log("YOU SHOT HIM!");
            }
        }
    }

    void AdjustMoveSpeed() 
    {
        float distancePercent = Vector3.Distance(centerPosition, transform.position) / maxDistance;

        moveSpeed = Math.Max(maxMoveSpeed * distancePercent, maxMoveSpeed / 2);
        Debug.Log($"Speed: {moveSpeed} ({distancePercent}%)");
    }

    void FloatCursorTowardsRandomTarget() 
    {
        //Debug.Log($"Moving towards X: {floatTowardsPosition.x} - Y: {floatTowardsPosition.y}");
        transform.position = Vector3.MoveTowards(transform.position, floatTowardsPosition, moveSpeed*0.5f);
    }

    void SetRandomTarget() 
    {
        float angle = UnityEngine.Random.Range(0, 8) * 45f * Mathf.Deg2Rad;
        float xOffset = Mathf.Cos(angle) * maxDistance;
        float yOffset = Mathf.Sin(angle) * maxDistance;

        floatTowardsPosition = new Vector3(centerPosition.x + xOffset, centerPosition.y + yOffset);
    }

    void RandomForce(float intensity) 
    {
        float x = UnityEngine.Random.Range(-1f, 1f);
        float y = UnityEngine.Random.Range(-1, 1f);

        transform.Translate(new Vector3(x * intensity, y * intensity));

        if (Vector3.Distance(centerPosition, transform.position) > maxDistance) 
        {
            transform.position = Vector3.MoveTowards(transform.position, centerPosition, intensity);
        }
    }

    void FigureEight(float intensity) 
    {
        float speed = 2.0f;
        float radiusX = 2.0f * intensity; // Radius of the loop along X-axis
        float radiusY = 2.0f * intensity; // Radius of the loop along Y-axis (for the '8' shape)

        timer += Time.deltaTime * speed;

        // Calculate X and Y positions using sine and cosine
        float x = Mathf.Cos(timer) * radiusX;
        float y = Mathf.Sin(timer * 2) * radiusY; // Double the frequency for Y to create the '8'

        Vector3 targetPosition = new Vector3(x, y);

        // Set the object's position
        transform.position = Vector3.MoveTowards(transform.position, centerPosition + targetPosition, 10f);
    }

    public bool CrosshairIsOverOpponent() 
    {
        Ray ray = Camera.main.ScreenPointToRay(transform.position);
        Debug.DrawRay(ray.origin, ray.direction * 10, Color.yellow);

        RaycastHit2D hit = Physics2D.GetRayIntersection(ray);
        if (hit.collider != null)
        {
            Debug.Log("Hit:" + hit.collider.name);
            string hitTag = hit.collider.gameObject.tag;

            if (hitTag == "Opponent")
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        else 
        {
            return false;
        }
    }

    void PlayerInput() 
    {
        int xMove = 0;
        int yMove = 0;

        if (Input.GetKey(KeyCode.RightArrow)) xMove += 1;
        if (Input.GetKey(KeyCode.LeftArrow)) xMove -= 1;
        if (Input.GetKey(KeyCode.DownArrow)) yMove -= 1;
        if (Input.GetKey(KeyCode.UpArrow)) yMove += 1;

        Vector3 moveDirection = new Vector3(xMove, yMove);
        moveDirection = moveDirection.normalized * moveSpeed;

        transform.position += moveDirection;
    }
}
