using System;
using System.Collections.Generic;

namespace Assets.Scripts
{
    class ManejadorEscenarios
    {
        private static ManejadorEscenarios instance;
        private bool modificando = false;

        internal List<Escenario> Escenarios { get; private set; }
        public Escenario EscenarioActual { get; internal set; }

        internal int Contar()
        {
            return Escenarios.Count;
        }

        private ManejadorEscenarios()
        {
            Escenarios = new List<Escenario>();
        }

        internal void CancelarModificacion()
        {
            modificando = false;
            EscenarioActual = null;
        }

        public static ManejadorEscenarios getInstance()
        {
            if (instance == null)
            {
                instance = new ManejadorEscenarios();
            }

            return instance;            
        }

        public void CargarEscenarios()
        {
            Logger.Log("Cargando escenarios de la DB");
            Escenarios = DbManager.CargarTodo();
        }

        public void CrearEscenario(Dictionary<String, short> valores)
        {
            if (Escenarios.Count == 6) {
                Logger.LogError("No puedes crear más de 6 escenarios", true);
                return;
            }

            Piso piso = (Piso)valores["Piso_DD"];
            foreach(Escenario e in Escenarios)
            {
                if (e.Piso == piso)
                {
                    Logger.LogError("Ya hay un escenario asociado a este piso", true);
                    return;
                }
            }

            Escenario escenario = new Escenario();
            escenario.DefinirPiso(piso);
            escenario.AgregarObjeto(TipoObjeto.RICK, valores["O1_DD"]);
            escenario.AgregarObjeto(TipoObjeto.MORTY, valores["O2_DD"]);
            escenario.AgregarObjeto(TipoObjeto.PISTOLA, valores["O3_DD"]);
            escenario.AgregarObjeto(TipoObjeto.PORTAL, valores["O4_DD"]);
            escenario.AgregarObjeto(TipoObjeto.NAVE, valores["O5_DD"]);

            if (escenario.Guardar())
            {
                Escenarios.Add(escenario);

                Logger.Log("CREADO: " + escenario, true);
            }
        }

        internal bool ModificarEscenario(Dictionary<string, short> valores)
        {
            EscenarioActual.AgregarObjeto(TipoObjeto.RICK, valores["O1_DD"]);
            EscenarioActual.AgregarObjeto(TipoObjeto.MORTY, valores["O2_DD"]);
            EscenarioActual.AgregarObjeto(TipoObjeto.PISTOLA, valores["O3_DD"]);
            EscenarioActual.AgregarObjeto(TipoObjeto.PORTAL, valores["O4_DD"]);
            EscenarioActual.AgregarObjeto(TipoObjeto.NAVE, valores["O5_DD"]);
            return EscenarioActual.Modificar();
        }

        public Escenario ObtenerEscenario(int posicion)
        {
            return Escenarios[posicion];
        }

        public bool EliminarEscenario(int value)
        {
            if (value < 0 || value >= Escenarios.Count)
            {
                Logger.LogError("El escenario a eliminar no es valido");
                return false;
            }

            Escenario escenario = Escenarios[value];

            if (DbManager.Eliminar(escenario.Piso))
            {
                Logger.Log(string.Format("Escenario {0} eliminado correctamente.", escenario), true);
                Escenarios.RemoveAt(value);
                return true;
            }
            else
            {
                Logger.Log(string.Format("No fue posible eliminar el escenario {0}.", escenario), true);
                return false;
            }
        }

        internal void IniciarModificacion(int posicion)
        {
            modificando = false;
            if (Escenarios.Count == 0 || posicion < 0 || posicion >= Escenarios.Count)
            {
                Logger.LogError("No se puede modificar el escenario seleccionado");
                return;
            }

            EscenarioActual = ObtenerEscenario(posicion);
            if (EscenarioActual == null)
            {
                Logger.LogError("No se puede modificar el escenario seleccionado");
                return;
            }

            modificando = true;
        }

        internal bool EstaModificando() => modificando && EscenarioActual != null;
    }
}
