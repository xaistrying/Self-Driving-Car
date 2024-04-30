using UnityEngine;

public class Generate : MonoBehaviour
{
    public GameObject automobilePrefab;
    public int quantity;

    private NeuralNetwork bestBrain;
    private double bestValue = 0d;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            GameObject poolHolder = new GameObject(automobilePrefab.name + "Pool");
            if (transform.Find(poolHolder.name))
            {
                foreach (Transform automobile in transform.Find(poolHolder.name).transform)
                {
                    double value = automobile.gameObject.GetComponent<Automobile>().automobileValue;
                    if (value > bestValue)
                    {
                        bestBrain = NeuralNetwork.PassNetwork(automobile.gameObject.GetComponent<Automobile>().brain);
                        bestValue = value;
                    }
                }
                DestroyImmediate(transform.Find(poolHolder.name).gameObject);
            }

            transform.Find("Bounder").localPosition = new Vector3(0, 0);
            transform.Find("Bounder").GetComponent<Bounder>().bestValue = 0d;

            poolHolder.transform.parent = this.transform;
            for (int i = 0; i < quantity; i++)
            {
                GameObject obj = Instantiate(automobilePrefab, transform.position, Quaternion.identity);
                obj.transform.parent = poolHolder.transform;
                if (bestBrain != null)
                {
                    obj.GetComponent<Automobile>().brain = NeuralNetwork.PassNetwork(bestBrain);
                    if (i == 0)
                    {
                        obj.GetComponent<SpriteRenderer>().color = new Color((float)255/255, (float)140/255, 0);
                        obj.GetComponent<SpriteRenderer>().sortingOrder += 10;
                    }
                    if (i != 0)
                    {
                        obj.GetComponent<Automobile>().brain = NeuralNetwork.PassNetwork(NeuralNetwork.Mutate(bestBrain, 0.2));
                    }
                }
            }
        }
    }
}
