using UnityEngine;
using UnityEngine.SceneManagement;

public class ChangeMainMenuScenes : MonoBehaviour
{
    public void CambiarAVistaVer()
    { 
        SceneManager.LoadScene("VerEscenario");
    }

    public void CambiarAVistaCrear()
    {
        SceneManager.LoadScene("CrearEscenario");
    }

    public void OnApplicationQuit()
    {
        Assets.Scripts.Logger.CrearBitacora();
    }
}
