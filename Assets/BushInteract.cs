using UnityEngine;
using TMPro;
public class BushInteract : MonoBehaviour
{
    public GameObject promptUI; // O texto ou ícone na UI (ex: “Press E”)
    public BurrowPoint burrowPoint; // Referência ao buraco (entrada)

    private bool isPlayerInRange = false;
    private CompanionAI guaxinim;

    private void Start()
    {
        if (promptUI != null)
            promptUI.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInRange = true;
            promptUI.SetActive(true);
            
            guaxinim = other.GetComponent<PlayerController>().guaxinim;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInRange = false;
            promptUI.SetActive(false);
        }
    }

    private void Update()
    {
        if (isPlayerInRange && Input.GetKeyDown(KeyCode.E))
        {
            if (burrowPoint != null && guaxinim != null)
            {
                burrowPoint.Activate(guaxinim);
                promptUI.SetActive(false); // Esconde o prompt após usar
            }
        }
    }
    
}
