using Cinemachine;
using UnityEngine;
using TMPro;

public class Manager : MonoBehaviour
{
    public TMP_Text countGeneration;
    private bool isSwitched = true;
    private int count = 0;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            count ++;
            countGeneration.text = "Generation: " + count.ToString();
        }

        if (Input.GetKeyDown(KeyCode.S))
        {
            isSwitched = !isSwitched;
            GetComponent<CinemachineVirtualCamera>().enabled = isSwitched;
        }

        if (Input.GetKeyDown(KeyCode.F))
        {
            var generateScipt = FindFirstObjectByType<Generate>();
            generateScipt.GetComponent<Generate>().quantity = 1;
        }
    }
}
