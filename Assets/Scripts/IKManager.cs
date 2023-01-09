using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Globalization;
using System.ComponentModel;

public class IKManager : MonoBehaviour
{
    
    StreamReader strReader = new StreamReader("Assets\\Resources\\Data.csv");
    
    public Joint m_root;
    public Joint m_end;
    //public GameObject m_objetivo;
    //Si en vez de un objeto target, se quiere mover a un punto determinado que lo de el usuario
    // public float xObjetivo = 0;
    // public float yObjetivo = 0;
    // public float zObjetivo = 0;
    public GameObject m_objetivo;
    public float _toleranciaError = 0.05f; //(umbral) La diferencia mínima que puede haber entre la punta del brazo y el objetivo para considerar rotar las artículaciones (desplazar el brazo) (parámetro que determina el punto en el cual el compresor va a empezar actuar, si la señal rebasa el punto --> comienza a actuar)
    public float _toleranciaDiferenciaErrores; // = 0.001f; //La diferencia absoluta mínima que puede haber entre el error obtenido en un frame pasado y el error del actual para considerar seguir acercandose al objetivo o pasar al siguiente (porque el brazo no se está moviendo --> diferencia se acerca a 0)
    public float _intentosAntesDeSiguienteObjetivo; //= 400f; // Cuantas veces se puede no cumplir la _toleranciaDiferenciaErrores antes de pasar al siguiente objetivo
    public int _factorCorreccion = 5;
    public float velocidad = 10f;

    private int _contadorNoCambios = 0;
    private float _errorIteracionPasada = 0; //Auxiliar para mantener en memoria el error de la iteración pasada antes de asignar el error de la actual
    
    private float DistanciaEntreDosPuntos(Vector3 _p1, Vector3 _p2)
    {
        return Vector3.Distance(_p1, _p2);
    }

    // articulacionOrigen-----------articulacionFinal ~~~~~~~~~> Objetivo
    // Calcular la pendiente entre el vector que define la distancia entre m_end y m_objetivo
    // mediante derivada por deficion, que es, cuanto varia la distancia desde la articulación del
    // origen hasta el objetivo cuando se aplica una pequeña rotación al m_root (si origen rota 20°, final tambien lo hace):
    /*
        pendiente = ( distancia_con_rotacion(final, objetivo) - distancia_original(final, objetivo) ) / diferencia_angulo
    */
    private float calcularPendienteArticulaciones(Joint _articulacion)
    {
        float diferencialAngulo = 0.01f; //DeltaTheta
        float distanciaOriginal = DistanciaEntreDosPuntos(m_end.transform.position, m_objetivo.transform.position);

        //Rotar la articulacion;
        _articulacion.Rotar(diferencialAngulo);

        float distanciaDespuesRotacion = DistanciaEntreDosPuntos(m_end.transform.position, m_objetivo.transform.position);

        //Rotar a la posición original
        _articulacion.Rotar(-diferencialAngulo);

        return (distanciaDespuesRotacion - distanciaOriginal) / diferencialAngulo;
    }
    // Update is called once per frame
    void Update()
    {
        Joint articulacionActual = m_root;
        var error = DistanciaEntreDosPuntos(m_end.transform.position, m_objetivo.transform.position);
        var diferenciaError = Math.Abs(_errorIteracionPasada - error);
        _errorIteracionPasada = error;
       // Debug.Log("Diferencia Error: " + diferenciaError.ToString());
        if( diferenciaError < _toleranciaDiferenciaErrores)
        {
            _contadorNoCambios++;
            //Debug.Log("No cambios: " + _contadorNoCambios.ToString());
        }
        else
        {
            _contadorNoCambios = 0;
        }
        if(error > _toleranciaError && _contadorNoCambios < _intentosAntesDeSiguienteObjetivo)
        {   
            for(int i=0; i<_factorCorreccion; ++i)
            {
                while (articulacionActual != null)
                {
                    float pendiente = calcularPendienteArticulaciones(articulacionActual);
                    articulacionActual.Rotar(-pendiente * velocidad );
                    articulacionActual = articulacionActual.GetChild();
                }
            }
        }
        else
        {
            ObtenerNuevoObjetivo();
            _contadorNoCambios = 0;
        }
    }

    void Start()
    {
        //this.m_objetivo = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        ObtenerNuevoObjetivo();
    }

    void ObtenerNuevoObjetivo()
    {
        bool endOFfile = true;
        if(!endOFfile)
        {
            // Leer cada línea
            string data_string = strReader.ReadLine();
            if (data_string == null)
            {
                endOFfile = true;
                return;
            }
            // separar contenido
            var data_values = data_string.Split(',');

            // Obtener y Transformar valores de cada eje x,y,z a float
            if( float.TryParse(data_values[0], NumberStyles.Any, CultureInfo.InvariantCulture, out float x) && 
                float.TryParse(data_values[1], NumberStyles.Any, CultureInfo.InvariantCulture, out float y) && 
                float.TryParse(data_values[2], NumberStyles.Any, CultureInfo.InvariantCulture, out float z))
            {
                m_objetivo.transform.position = new Vector3(x, y, z);                
                Debug.Log("x: " + data_values[0].ToString() + " y: " + data_values[1].ToString() + " z: " + data_values[2].ToString());
            }
        }
        else
        {
            //TODO: Cortar animación/funcionamiento brazo
            // var animator = this.GetComponent<Animator>();
            // animator.StopPlayback();
            //Application.Quit();
            // if( UnityEditor.EditorApplication.isPlaying )
            // {
            //     UnityEditor.EditorApplication.isPlaying = false;
            // }
           // Debug.Break();
        }
    }
}
