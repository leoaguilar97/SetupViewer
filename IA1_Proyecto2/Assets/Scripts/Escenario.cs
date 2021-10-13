using System;
using System.Collections.Generic;
using Mono.Data.Sqlite;
using System.Linq;

namespace Assets.Scripts
{
    public enum Piso: short
    {
        AGUA = 0,
        ALFOMBRA = 1,
        METAL = 2,
        EXAGONAL = 3,
        LADRILLO = 4,
        LAVA = 5
    }

    public enum Posicion: short {
        SUPERIOR_IZQUIERDA = 0,
        SUPERIOR_DERECHA = 1,
        CENTRO = 2,
        INFERIOR_IZQUIERDA = 3,
        INFERIOR_DERECHA = 4
    }

    public enum TipoObjeto: short
    {
        RICK = 1,
        MORTY = 2,
        PISTOLA = 3,
        PORTAL = 4,
        NAVE = 5
    }

    class Escenario
    {
        internal Dictionary<TipoObjeto, Posicion> Objetos { get; }

        private readonly static string STR_FORM = "Escenario: Piso {0} Objetos: {1}";
        private readonly static string OBJ_FORM = "{0} -> {1} ";

        public Piso Piso { get; private set; }

        private void Validar() {
            HashSet<Posicion> posicionesUsadas = new HashSet<Posicion>();

            foreach(TipoObjeto to in DbManager.OBJETOS)
            {
                if (!Objetos.ContainsKey(to))
                {
                    throw new Exception("No selecciono ninguna posicion para " + to.ToString());
                }

                Posicion pos = Objetos[to];
                if (!posicionesUsadas.Add(pos))
                {
                    throw new Exception("La posicion " + pos.ToString() + " ya fue utilizada por otro objeto anteriormente");
                }
            }
        }

        internal int ObtenerObjetoPorNombre(string name)
        {
            TipoObjeto to = TipoObjeto.RICK;

            switch (name)
            {
                case "O1_DD": to = TipoObjeto.RICK; break;
                case "O2_DD": to = TipoObjeto.MORTY; break;
                case "O3_DD": to = TipoObjeto.PISTOLA; break;
                case "O4_DD": to = TipoObjeto.PORTAL; break;
                case "O5_DD": to = TipoObjeto.NAVE; break;
            }

            return (int)Objetos[to];
        }

        public Escenario() {
            Piso = Piso.AGUA;
            Objetos = new Dictionary<TipoObjeto, Posicion>();
        }

        public void DefinirPiso(Piso piso) => Piso = piso;

        public void AgregarObjeto(TipoObjeto objeto, short posicionId)
        {
            try
            {
                Posicion pos = (Posicion)posicionId;
                if (Objetos.ContainsKey(objeto))
                {
                    Objetos[objeto] = pos;
                }
                else
                {
                    Objetos.Add(objeto, pos);
                }

                Logger.Log("Objeto " + objeto.ToString() + " asignado -> " + pos.ToString());
            }
            catch (InvalidCastException)
            {
                Logger.LogError("Error, la posicion seleccionada no existe: " + posicionId + " o el tipo de objeto: " + posicionId);
            }
            catch(Exception e)
            {
                Logger.LogError(e.Message, true);
            }
        }

        internal bool Modificar()
        {
            Logger.Log("Modificando escenario: " + ToString());
            try
            {
                Validar();
                Logger.Log("Datos a modificar validados correctamente");
                DbManager.Modificar(Piso, Objetos);

                return true;
            }
            catch (SqliteException)
            {
                Logger.LogError("El escenario a modificar no es correcto, o no existe", true);
            }
            catch (Exception e)
            {
                Logger.LogError(e.Message, true);
            }

            return false;
        }

        public bool Guardar() {
            Logger.Log("Guardando escenario: " + ToString());

            try
            {
                Validar();
                Logger.Log("Datos a guardar validados correctamente");
                DbManager.Guardar(Piso, Objetos);

                return true;
            }
            catch (SqliteException)
            {
                Logger.LogError("Ya existe un escenario asociado a ese piso", true);
            }
            catch(Exception e)
            {
                Logger.LogError(e.Message, true);
            }

            return false;
        }

        public TipoObjeto ObtenerObjetoPorPosicion(Posicion posicion) => Objetos.First(x => x.Value == posicion).Key;

        public override string ToString()
        {
            string objetos = "";
            foreach(TipoObjeto to in DbManager.OBJETOS)
            {
                objetos += string.Format(OBJ_FORM, to, this.Objetos[to]);
            }
            return string.Format(STR_FORM, Piso, objetos);
        }
    }
}
