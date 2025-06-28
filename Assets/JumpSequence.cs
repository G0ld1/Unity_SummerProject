using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class JumpSequence : MonoBehaviour
{
   public List<Transform> jumpPoints;
    public GameObject promptUI;

    public Transform player;
    public Transform sister;
    public Transform raccoon;

    public float jumpDuration = 0.4f;

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
            Debug.Log("Bateu");
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
        if (playerInRange && Input.GetKeyDown(KeyCode.E) && !hasTriggered)
        {
            hasTriggered = true;
            if (promptUI != null) promptUI.SetActive(false);
            StartCoroutine(PlayJumpSequence());
        }
    }

    private IEnumerator PlayJumpSequence()
    {
        foreach (Transform point in jumpPoints)
        {
            yield return JumpTo(player, point.position);
        }

        // Agora trata dos outros personagens
        yield return new WaitForSeconds(0.5f);
        StartCoroutine(SendCharacterAcross(sister));
        StartCoroutine(SendCharacterAcross(raccoon));
    }

    private IEnumerator JumpTo(Transform character, Vector3 destination)
    {
        Vector3 start = character.position;
        float t = 0f;
        while (t < 1f)
        {
            t += Time.deltaTime / jumpDuration;
            float height = Mathf.Sin(t * Mathf.PI) * 1.5f;
            character.position = Vector3.Lerp(start, destination, t) + Vector3.up * height;
            yield return null;
        }
        character.position = destination;
    }

    private IEnumerator SendCharacterAcross(Transform character)
    {
        UnityEngine.AI.NavMeshAgent agent = character.GetComponent<UnityEngine.AI.NavMeshAgent>();
        if (agent != null) agent.enabled = false;
        // Encontra o ponto de partida mais próximo do personagem
        int startIndex = 0;
        float closestDistance = float.MaxValue;

        for (int i = 0; i < jumpPoints.Count; i++)
        {
            float dist = Vector3.Distance(character.position, jumpPoints[i].position);
            if (dist < closestDistance)
            {
                closestDistance = dist;
                startIndex = i;
            }
        }

        // Faz os saltos a partir do ponto encontrado até ao fim da sequência
        for (int i = startIndex + 1; i < jumpPoints.Count; i++)
        {
            yield return new WaitForSeconds(0.3f);
            yield return JumpTo(character, jumpPoints[i].position);
        }
    
     if (agent != null) agent.enabled = true;
}
}
