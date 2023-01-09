using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Premio : MonoBehaviour
{ public GameObject pinza;
    private RedMovement redmovement;
    bool isPressing;
    void Start()
    {
        redmovement = pinza.GetComponent<RedMovement>();
    }

    private void Update()
    {
       isPressing = redmovement.pinzapressing;
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("pinzas") )
        {
            Debug.Log("Colisión con MLAgent");
            if (isPressing == false)
            Invoke("MoverPosicionInicial", 8);
        }
    }

    public void MoverPosicionInicial()
    {
        bool posicionEncontrada = false;
        int intentos = 100;
        Vector3 posicionPotencial = Vector3.zero;
        while (!posicionEncontrada && intentos >= 0)
        {
            intentos--;
            posicionPotencial = new Vector3(
                 UnityEngine.Random.Range(1.5f, 2.8f),
                0.86f,
                UnityEngine.Random.Range(-2.2f, 2.2f));
            //en el caso de que tengamos mas cosas en el escenario checker que no choca
            Collider[] colliders = Physics.OverlapSphere(posicionPotencial, 0.05f);
            if (colliders.Length == 0)
            {
                transform.position = posicionPotencial;
                posicionEncontrada = true;
            }
        }
    }
}
