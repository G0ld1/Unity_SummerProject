using UnityEngine;
using UnityEngine.AI;
using System.Collections;
public class CompanionAI : MonoBehaviour
{
    private NavMeshAgent agent;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    public void MoveTo(Vector3 position)
    {
        agent.SetDestination(position);
    }

    public IEnumerator BurrowTo(BurrowPoint burrow)
    {
        // 1. Vai até o ponto de entrada
        agent.SetDestination(burrow.entryPoint.position);

        // 2. Espera até chegar
        while (Vector3.Distance(transform.position, burrow.entryPoint.position) > 0.2f)
            yield return null;

        // 3. Desaparece
        gameObject.SetActive(false); // ou joga uma animação de entrar

        yield return new WaitForSeconds(1f); // espera simbólica

        // 4. Teleporta para o ponto de saída
        transform.position = burrow.linkedExit.exitPoint.position;

        // 5. Reaparece
        gameObject.SetActive(true);
    }
}
