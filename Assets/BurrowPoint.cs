using UnityEngine;

public class BurrowPoint : MonoBehaviour
{
    public BurrowPoint linkedExit; //ligacao ao sitio onde o guaxinim vai sair

    public Transform entryPoint; //ponto de entrada

    public Transform exitPoint; //ponto de saida

    public void Activate(CompanionAI guaxinim)
    {
        guaxinim.StartCoroutine(guaxinim.BurrowTo(this));
    }
}
