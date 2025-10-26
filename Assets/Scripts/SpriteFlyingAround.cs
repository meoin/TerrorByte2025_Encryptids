using UnityEngine;

public class SpriteFlyingAround : MonoBehaviour
{
    public float flightIntensity = 100f;
    public float flightSpeed = 10f;
    private float timer = 0f;
    private Vector3 centerPosition;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        centerPosition = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        FigureEight(flightIntensity);
    }

    void FigureEight(float intensity)
    {
        float speed = flightSpeed;
        float radiusX = 2.0f * intensity; // Radius of the loop along X-axis
        float radiusY = 1.0f * intensity; // Radius of the loop along Y-axis (for the '8' shape)

        timer += Time.deltaTime * speed;

        // Calculate X and Y positions using sine and cosine
        float x = Mathf.Cos(timer) * radiusX;
        float y = Mathf.Sin(timer * 2) * radiusY; // Double the frequency for Y to create the '8'

        Vector3 targetPosition = new Vector3(x, y);

        // Set the object's position
        transform.position = Vector3.MoveTowards(transform.position, centerPosition + targetPosition, flightSpeed);
    }
}
