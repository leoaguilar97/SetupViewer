using Assets.Scripts;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CreateScene : MonoBehaviour
{
    Dictionary<string, short> valores = new Dictionary<string, short>() {
        {"Piso_DD", 0},
        {"O1_DD", 0},
        {"O2_DD", 1},
        {"O3_DD", 2},
        {"O4_DD", 3},
        {"O5_DD", 4}
    };

    public void CambiarAMenuInicial()
    { 
        SceneManager.LoadScene("MainMenu");
        ManejadorEscenarios.getInstance().CancelarModificacion();
    }

    public void CambiarValorDropDown(Dropdown dd)
    {
        valores[dd.name] = (short)dd.value;
    }

    public void Start()
    {
        Text titulo = GameObject.Find("Titulo").GetComponent<Text>();
        Dropdown piso_dd = GameObject.Find("Piso_DD").GetComponent<Dropdown>();
        if (ManejadorEscenarios.getInstance().EstaModificando())
        {
            Escenario ea = ManejadorEscenarios.getInstance().EscenarioActual;
            titulo.text = "MODIFICAR ESCENARIO - PISO " + ea.Piso.ToString();
            piso_dd.enabled = false;
            piso_dd.value = (int)ea.Piso;

            foreach (string name in valores.Keys)
            {
                if (name != "Piso_DD")
                {
                    Dropdown dd = GameObject.Find(name).GetComponent<Dropdown>();
                    dd.value = ea.ObtenerObjetoPorNombre(name);
                }
            }

        }
        else
        {
            titulo.text = "CREAR ESCENARIO";
            piso_dd.enabled = true;
            valores = new Dictionary<string, short>() {
                {"Piso_DD", 0},
                {"O1_DD", 0},
                {"O2_DD", 1},
                {"O3_DD", 2},
                {"O4_DD", 3},
                {"O5_DD", 4}
            };
        }
    }

    public void CrearEscena()
    {
        if (ManejadorEscenarios.getInstance().EstaModificando())
        {
            if (ManejadorEscenarios.getInstance().ModificarEscenario(valores))
            {
                SceneManager.LoadScene("VerEscenario");
                ManejadorEscenarios.getInstance().CancelarModificacion();
                Assets.Scripts.Logger.Log("Escenario modificado correctamente", true);
            }
        }
        else
        {
            ManejadorEscenarios.getInstance().CrearEscenario(valores);
        }
    }

    public void CrearYVerEscena()
    {
        CrearEscena();

        if (!ManejadorEscenarios.getInstance().EstaModificando())
        {
            SceneManager.LoadScene("VerEscenario");
        }
    }
}
