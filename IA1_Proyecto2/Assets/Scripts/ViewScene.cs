using UnityEngine;
using Assets.Scripts;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ViewScene : MonoBehaviour
{

    private void RevisarCorrecto()
    {
        if (ManejadorEscenarios.getInstance().Contar() == 0)
        {
            Assets.Scripts.Logger.LogError("No existen escenarios actualmente, debes crear al menos uno.");
            SceneManager.LoadScene("CrearEscenario");
        }
    }

    private void ActualizarUI(Escenario escenario)
    {
        //TODO: Cargar Imagen de Piso de Escenario dependiendo cual es
        RawImage imagenPiso = GameObject.Find("ImagenPiso").GetComponent<RawImage>();
        //TODO: imagenPiso.texture = algo;

        Text text1 = GameObject.Find("IS_TEXT").GetComponent<Text>(); // izquierda superior
        Text text2 = GameObject.Find("II_TEXT").GetComponent<Text>(); // izquierda inferior
        Text text3 = GameObject.Find("CE_TEXT").GetComponent<Text>(); // centro
        Text text4 = GameObject.Find("DS_TEXT").GetComponent<Text>(); // derecha superior
        Text text5 = GameObject.Find("DI_TEXT").GetComponent<Text>(); // derecha inferior

        text1.text = escenario.ObtenerObjetoPorPosicion(Posicion.SUPERIOR_IZQUIERDA).ToString();
        text2.text = escenario.ObtenerObjetoPorPosicion(Posicion.INFERIOR_IZQUIERDA).ToString();
        text3.text = escenario.ObtenerObjetoPorPosicion(Posicion.CENTRO).ToString();
        text4.text = escenario.ObtenerObjetoPorPosicion(Posicion.SUPERIOR_DERECHA).ToString();
        text5.text = escenario.ObtenerObjetoPorPosicion(Posicion.INFERIOR_DERECHA).ToString();
    }

    private void CargarDropdown()
    {
        RevisarCorrecto();

        ManejadorEscenarios.getInstance().CargarEscenarios();
        Dropdown piso_dd = GameObject.Find("Piso_DD").GetComponent<Dropdown>();
        if (piso_dd != null)
        {
            piso_dd.options.Clear();
            var escenarios = ManejadorEscenarios.getInstance().Escenarios;
            int i = 1;
            foreach (Escenario escenario in escenarios)
            {
                piso_dd.options.Add(new Dropdown.OptionData("Escenario #" + i++ + " - Piso " + escenario.Piso.ToString()));
            }

            piso_dd.value = 0;
            OnDropdownChange(piso_dd);
        }
    }

    public void OnDropdownChange(Dropdown d)
    {
        Escenario escenario = ManejadorEscenarios.getInstance().ObtenerEscenario(d.value);

        ActualizarUI(escenario);
    }

    public void Start()
    {
        CargarDropdown();
        RevisarCorrecto();
    }

    public void EliminarEscenario(Dropdown d)
    {
        bool eliminado = ManejadorEscenarios.getInstance().EliminarEscenario(d.value);

        if (eliminado)
        {
            CargarDropdown();
            RevisarCorrecto();
        }
    }

    public void CambiarAMenuInicial()
    {
        SceneManager.LoadScene("MainMenu");
    }

    public void ActualizarEscenario(Dropdown d)
    {
        ManejadorEscenarios.getInstance().IniciarModificacion(d.value);
        SceneManager.LoadScene("CrearEscenario");
    }

    public void AbrirEscenario()
    {
        SceneManager.LoadScene("Escenario");
    }
}
