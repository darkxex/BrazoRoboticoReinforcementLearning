using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Joint : MonoBehaviour
{
    public Joint m_child;
    public string DireccionRotacion;
    //public float AnguloMinimo = 0;
    //public float AnguloMaximo = 359.99f;

    public Joint GetChild()
    {
        return m_child;
    }

    // Start is called before the first frame update
    public void Rotar(float _angle) 
    {
        
        if(DireccionRotacion.ToUpper() == "UP")
        {
            this.transform.Rotate(Vector3.down * _angle);
            // var vectorRotacionAplicar = Vector3.up * _angle; //Rotacion que se le aplicara al objeto en un momento dado (En forma de Vector3)
            // var vectorRotacionActual = this.transform.eulerAngles; // El angulo de rotación actual que tiene el objeto en un momento dato(En forma de Vector3)
            // var vectorRotacionFinal= vectorRotacionActual + vectorRotacionAplicar; //Rotación final que pudiera tener el objeto(En forma de Vector3)

            // var quaternionFinal = Quaternion.Euler(vectorRotacionFinal); //Se transofrma el Vector3 de vectorRotacionFinal en un Quaternion con valores de rotación en grados
            // var anguloFinal = quaternionFinal.eulerAngles.y; //Se obtiene solo el angulo de rotación en el Eje y
            // //Debug.Log(anguloFinal.ToString());
            // anguloFinal = Mathf.Clamp(Math.Abs(anguloFinal), Math.Abs(AnguloMinimo), Math.Abs(AnguloMaximo));//Se verifica que el angulo de rotación final en el eje Y no supere los límites de angulos en que puede girar el objeto real
            // this.transform.eulerAngles = new Vector3(quaternionFinal.eulerAngles.x, anguloFinal, quaternionFinal.eulerAngles.z); //Se actualiza el grado actual de rotación del objeto al angulo final de rotación en el eje Y mediante una representación Vector3
        }
        else if(DireccionRotacion.ToUpper() == "DOWN")
        {
            this.transform.Rotate(Vector3.down * _angle);
            // var vectorRotacionActual = this.transform.eulerAngles; // El angulo de rotación actual que tiene el objeto en un momento dato(En forma de Vector3)

            // var quaternion = Quaternion.Euler(vectorRotacionActual); //Se transofrma el Vector3 de vectorRotacionFinal en un Quaternion con valores de rotación en grados
            // var anguloFinal = quaternion.y; //Se obtiene solo el angulo de rotación en el Eje y
            // Debug.Log(anguloFinal.ToString());
        }
        else if(DireccionRotacion.ToUpper() == "FORWARD")
        {
            this.transform.Rotate(Vector3.forward * _angle);
            var vectorRotacionActual = this.transform.eulerAngles; // El angulo de rotación actual que tiene el objeto en un momento dato(En forma de Vector3)

            var quaternion = Quaternion.Euler(vectorRotacionActual); //Se transofrma el Vector3 de vectorRotacionFinal en un Quaternion con valores de rotación en grados
            var anguloFinal = quaternion.z; //Se obtiene solo el angulo de rotación en el Eje y
            //Debug.Log(anguloFinal.ToString());
        }
        else if(DireccionRotacion.ToUpper() == "BACK")
        {
            this.transform.Rotate(Vector3.back * _angle);
        }
        else if(DireccionRotacion.ToUpper() == "RIGHT")
        {
            this.transform.Rotate(Vector3.right * _angle);
        }
        else if(DireccionRotacion.ToUpper() == "LEFT")
        {
            this.transform.Rotate(Vector3.left * _angle);
        }
    }
}
