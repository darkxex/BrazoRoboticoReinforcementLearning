using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PinzasScript : MonoBehaviour
{
    private Renderer m_Renderer;
    public bool isTouchingCube;
    // Start is called before the first frame update
    void Start()
    {
        m_Renderer = GetComponent<Renderer>();
        isTouchingCube = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    //Detect collisions between the GameObjects with Colliders attached
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "cubo")
        {
            isTouchingCube = true;
            Debug.Log("Entrando a la colisión.");
        }

    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "cubo")
        {
            isTouchingCube = false;
            Debug.Log("Saliendo de la colisión.");
        }
    }

}
