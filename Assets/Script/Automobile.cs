using System.Collections;
using UnityEngine;

public class Automobile : MonoBehaviour
{
    public float moveSpeed;
    public float rotateSpeed;
    public Transform point;

    private Rigidbody rb;
    private Vector3 movement;
    private Collider coll;
    private bool gameOver = false;
    private LineRenderer line;
    private int rayCount = 5;
    private float raySpread = Mathf.PI/2;

    private enum controlType{ NORMAL, AI }
    [SerializeField] private controlType useBrain;
    public NeuralNetwork brain;
    private double[] raysLength;

    [HideInInspector] public float automobileValue; 
    public bool mutated = false;

    void Start()
    {
        rb = gameObject.GetComponent<Rigidbody>();
        coll = gameObject.GetComponent<Collider>();
        line = gameObject.GetComponent<LineRenderer>();
        line.positionCount = rayCount * 2;

        point.gameObject.SetActive(true);
        for (int i = 0; i < rayCount-1; i++)
        {
            Transform obj = Instantiate(point, transform);
            obj.name = point.name;
        }

        if (useBrain == controlType.AI)
        {
            if (brain == null) { brain = new NeuralNetwork(rayCount, 5, 2); }
            raysLength = new double[rayCount];
        }
    }
    
    void Update()
    {
        for (int i = 0; i < rayCount; i++)
        {
            float rayAngle = (raySpread/2) + (-raySpread/2 - raySpread/2) * i/(rayCount-1); // Lerp Calculate
            Ray ray = new Ray(
                transform.position, 
                transform.TransformDirection(new Vector2(Mathf.Sin(rayAngle), Mathf.Cos(rayAngle)))
            );
            RaycastHit hitInfo;
            line.SetPosition(i*2, transform.position);
            if (Physics.Raycast(ray, out hitInfo))
            {
                Debug.DrawLine(ray.origin, hitInfo.point, Color.red);
                line.SetPosition(i*2+1, hitInfo.point);
                gameObject.transform.GetChild(i).position = hitInfo.point;
                if (useBrain == controlType.AI)
                {
                    raysLength[i] = Vector2.Distance(hitInfo.point, transform.position);
                }
            }
        }

        if (gameOver == false)
        {
            if (useBrain == controlType.NORMAL)
            {
                movement.x = Input.GetAxis("Horizontal");
                movement.y = Input.GetAxis("Vertical"); 
            }
            else
            {
                var output = brain.CalculatedOutputs(raysLength);
                movement.x = (float)output[0];
                movement.y = (float)output[1]; 
                automobileValue += movement.y;
            }
        }
        else
        {
            movement = Vector3.zero;
        }
    }

    void FixedUpdate()
    {
        rb.MovePosition(rb.position + rb.rotation * new Vector3(0, movement.y) * moveSpeed * Time.fixedDeltaTime);

        if (movement.y != 0)
        {
            Quaternion deltaRotation = Quaternion.Euler(new Vector3(0, 0, -Mathf.Sign(movement.y) * movement.x) * rotateSpeed * Time.fixedDeltaTime);
            rb.MoveRotation(rb.rotation * deltaRotation);
        }
    }

    void OnTriggerStay(Collider other) 
    {
        if (other.gameObject.name == "Road Mesh Holder" && gameOver == false)
        {
            if (!(coll.bounds.Contains(other.bounds.max) && coll.bounds.Contains(other.bounds.min)))
            {
                gameOver = true;
                StartCoroutine(ChangeColor());
            }
        }
    }

    IEnumerator ChangeColor()
    {
        Color newColor = this.GetComponent<SpriteRenderer>().color;
        while (newColor.a > 0f)
        {
            newColor.a -= 0.1f;
            this.GetComponent<SpriteRenderer>().color = newColor; 
            newColor = this.GetComponent<SpriteRenderer>().color; 
            yield return new WaitForSeconds(0.01f);
        }
        this.gameObject.SetActive(false);
    }
}
