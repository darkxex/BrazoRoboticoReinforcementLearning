using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Sensors;
using Unity.MLAgents.Actuators;
public class RedMovement : Agent
{
    public GameObject pinza;
    private PinzasScript pinzascript;
    private Premio premioscript;
    public bool touchingcube;
    public GameObject cubb;
    public GameObject refcub;
    public bool pinzapressing;
    Rigidbody rb;
    public bool _training = true;
    [SerializeField] float movementSpeed = 6f;
    float baseY = 0f;
    float lastvalue = 10f;
    float lasty = 0f;
    public GameObject j1, j2, j3,j4,j5,j6;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        pinzascript = pinza.GetComponent<PinzasScript>();
        pinzapressing = false;
        premioscript = cubb.GetComponent<Premio>();
    }

    // Update is called once per frame
    void Update()
    {
        baseY = 0f;
        if (Input.GetKey(KeyCode.Q))
        {
            baseY = 1f;
        }
        if (Input.GetKey(KeyCode.E))
        {
            baseY = -1f;
        }

        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        rb.velocity = new Vector3(horizontalInput * movementSpeed, baseY * movementSpeed, verticalInput * movementSpeed);
        touchingcube = pinzascript.isTouchingCube;
        
       float result = Vector3.Distance(cubb.transform.position, transform.position);
        
        //Debug.Log("antiguo: "+ lastvalue + "nuevo: "+ result);
        if (result < lastvalue )
        { AddReward(0.05f);
            lastvalue = result;
        }
        else
        {
            AddReward(-0.5f);
            lastvalue = result;
        }
        if (touchingcube)
        {
            if (Input.GetKey(KeyCode.Z))
            {
                pinzapressing = true;

            }

            if (Input.GetKey(KeyCode.X))
            {
                pinzapressing = false;
            }

            if (_training)
            {
                AddReward(1f);
            }
            if (pinzapressing)
            {
                AddReward(5f);
                Debug.Log("Agarrando Cubo.");
                cubb.transform.position = refcub.transform.position;

                if (transform.position.y > lasty)
                { AddReward(0.05f); }
                else
                { AddReward(-0.5f); }
            }
            else
            { AddReward(-5f);
               Debug.Log("Soltando Cubo.");
            }


        }
    }

    public override void Initialize()
    {
        rb = GetComponent<Rigidbody>();
        //MaxStep forma parte de la clase Agent
        if (!_training) MaxStep = 0;
    }
    public override void OnEpisodeBegin()
    {
        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
        pinzapressing = false;
        MoverPosicionInicial();
        j1.transform.rotation = Quaternion.Euler(new Vector3(0, 0, 0));
        j2.transform.rotation = Quaternion.Euler(new Vector3(0, 0, 0));
        j3.transform.rotation = Quaternion.Euler(new Vector3(0, 0, 0));
        j4.transform.rotation = Quaternion.Euler(new Vector3(0, 0, 0));
        j5.transform.rotation = Quaternion.Euler(new Vector3(0, 0, 0));
        j6.transform.rotation = Quaternion.Euler(new Vector3(0, 0, 0));
        premioscript.MoverPosicionInicial();
    }
    public override void OnActionReceived(ActionBuffers actions)
    {
        //Construimos un vector con el vector recibido.

        Vector3 movimiento;
        if (pinzapressing == true)
            movimiento = new Vector3(actions.ContinuousActions[0],
          actions.ContinuousActions[2], actions.ContinuousActions[1]);
        else
        movimiento = new Vector3(actions.ContinuousActions[0],
            0 ,actions.ContinuousActions[1]);

        float agarreAction = actions.ContinuousActions[3];
      
        if (touchingcube)
        {
            
            if (agarreAction > 0)
            { pinzapressing = true; }
            else
            { pinzapressing = false; }
        }

        //Sumamos el vector construido al rigidbody como fuerza 
        rb.AddForce(movimiento * movementSpeed * Time.deltaTime);
    }
    public override void CollectObservations(VectorSensor sensor)
    {
        //Calcular cuanto nos queda hasta el objetivo
        Vector3 alObjetivo = cubb.transform.position - transform.position;

        //Un vector ocupa 3 observaciones. 
        sensor.AddObservation(alObjetivo.normalized);
        float catching = 0;
        if (pinzapressing)
            catching = 1f;
        else
        { catching = -1f; }
        sensor.AddObservation(catching);
    }
    public override void Heuristic(in ActionBuffers actionsOut)
    {
        /*var continuousActionsOut = actionsOut.ContinuousActions;
        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");
        Vector3 movement = new Vector3(moveHorizontal, 0f, moveVertical);
        continuousActionsOut[0] = movement.x;
        continuousActionsOut[1] = movement.z;*/
    }
    private void MoverPosicionInicial()
    {
        bool posicionEncontrada = false;
        int intentos = 100;
        Vector3 posicionPotencial = Vector3.zero;
        while (!posicionEncontrada && intentos >= 0)
        {
            intentos--;
            posicionPotencial = new Vector3(
                 2.25f,
                0.86f,
                2.25f);
            //en el caso de que tengamos mas cosas en el escenario checker que no choca
            Collider[] colliders = Physics.OverlapSphere(posicionPotencial, 0.05f);
            if (colliders.Length == 0)
            {
                transform.position = posicionPotencial;
                posicionEncontrada = true;
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (_training)
        {
       
            if (other.CompareTag("borders"))
            {
                AddReward(-0.5f);
            }
        }
    }
    private void OnTriggerStay(Collider other)
    {
        if (_training)
        {
            if (other.CompareTag("borders"))
            {
                AddReward(-1f);
            }
        }
    }

}
