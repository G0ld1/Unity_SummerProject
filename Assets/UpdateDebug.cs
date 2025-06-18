using UnityEngine;
using TMPro;

public class UpdateDebug : MonoBehaviour
{

    public TMP_Text currentSpeed;
    public PlayerMovement pm;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
       
    }

    // Update is called once per frame
    void Update()
    {
        currentSpeed.text = "Current Speed:" + pm.currentSpeed;
    }
}
