using UnityEngine;

public class Bounder : MonoBehaviour
{
    public GameObject automobilePrefab; 
    [HideInInspector] public double bestValue = 0d;
    private float smoothSpeed = 0.125f;
    private GameObject bestCarTemp, bestCar;
    private Color baseColor, newColor, prevBestColor;

    void Start()
    {
        baseColor = automobilePrefab.GetComponent<SpriteRenderer>().color;
        newColor = new Color((float)154/255, (float)205/255, (float)50/255);
        prevBestColor = new Color((float)255/255, (float)140/255, 0);
    }

    void FixedUpdate()
    {
        Collider[] hitCollider = Physics.OverlapSphere(transform.position, transform.localScale.x / 2);
        bestValue = 0f;
        foreach (Collider col in hitCollider)
        {
            if (col.TryGetComponent<Automobile>(out Automobile automobile))
            {
                if (automobile.automobileValue > bestValue)
                {
                    bestValue = automobile.automobileValue;
                    bestCarTemp = col.gameObject;
                }
            }
        }

        if (bestCarTemp != bestCar && bestCar != null)
        {
            if (bestCar.GetComponent<SpriteRenderer>().color != prevBestColor)
            {
                bestCar.GetComponent<SpriteRenderer>().color = baseColor;
            }
            bestCar = bestCarTemp;
        }
        else if (bestCarTemp != bestCar && bestCar == null)
        {
            bestCar = bestCarTemp;
        }
        if (bestCar != null)
        {
            Vector3 smoothedPosition = Vector3.Lerp(transform.position, bestCar.transform.position, smoothSpeed);
            transform.position = smoothedPosition;
            
            if (bestCar.GetComponent<SpriteRenderer>().color == baseColor)
            {
                bestCar.GetComponent<SpriteRenderer>().color = newColor;
                bestCar.GetComponent<SpriteRenderer>().sortingOrder += 10;
            }
        }
    }
}
