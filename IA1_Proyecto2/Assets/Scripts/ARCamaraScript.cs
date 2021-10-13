using UnityEngine;
using UnityEngine.SceneManagement;
using Assets.Scripts;
using System.Collections.Generic;

public class ARCamaraScript : MonoBehaviour
{
    List<Escenario> escenarios;

    public void Start()
    {
        escenarios = ManejadorEscenarios.getInstance().Escenarios;

        foreach(Escenario escenario in escenarios)
        {
            Assets.Scripts.Logger.Log(escenario);        
        }
    }

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            SceneManager.LoadScene(1);
        }
    }
}
