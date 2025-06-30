using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;

public class JumpSequence : MonoBehaviour
{
    [Header("Jump Path")]
    public List<Transform> jumpPoints;
    public float jumpDuration = 0.4f;

    public float jumpArcHeight = 2f;

    private int currentIndex = 0;
    private bool isJumping = false;

    [Header("Prompt UI")]
    public GameObject promptUI;

    [Header("Character")]
    public Transform character;

    [Header("Events")]
    public UnityEvent OnSequenceStart;
    public UnityEvent OnSequenceEnd;

    private bool playerInRange = false;
    private bool hasTriggered = false;

    private void Start()
    {
        if (promptUI != null)
            promptUI.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !hasTriggered)
        {
            playerInRange = true;
            if (promptUI != null) promptUI.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
            if (promptUI != null) promptUI.SetActive(false);
        }
    }

    private void Update()
    {
        if (isJumping) return;
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (currentIndex < jumpPoints.Count - 1)
            {
                StartCoroutine(JumpTo(character, jumpPoints[currentIndex + 1].position));
                currentIndex++;
            }
        }
        else if (Input.GetKeyDown(KeyCode.Q)) // Desce
        {
            if (currentIndex > 0)
            {
                StartCoroutine(JumpTo(character, jumpPoints[currentIndex - 1].position));
                currentIndex--;
            }
        }
    }

    private IEnumerator PlayJumpSequence()
    {
        OnSequenceStart?.Invoke(); // Evento utilizavel para ativar codigo antes da sequencia comecar

        foreach (Transform point in jumpPoints)
        {
            yield return JumpTo(character, point.position);
        }

        OnSequenceEnd?.Invoke(); // Evento utilizavel para ativar codigo depois da sequencia acabar
        hasTriggered = false;
    }

    private IEnumerator JumpTo(Transform character, Vector3 destination)
    {
        isJumping = true;
        Vector3 start = character.position;
        float t = 0f;
        while (t < 1f)
        {
            t += Time.deltaTime / jumpDuration;
            float height = Mathf.Sin(t * Mathf.PI) * jumpArcHeight;
            character.position = Vector3.Lerp(start, destination, t) + Vector3.up * height;
            yield return null;
        }
        character.position = destination + new Vector3(0,2,0);
        isJumping = false;
    }
}