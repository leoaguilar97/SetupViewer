using Mono.Data.Sqlite;
using System;
using System.Collections.Generic;
using System.Data;
using UnityEngine;

namespace Assets.Scripts
{
    class DbManager
    {

        private readonly static string CONEXION = "URI=file:" + Application.dataPath + "/db.db";
        
        private readonly static string COLUMNAS = "Piso, OBJETO_1, OBJETO_2, OBJETO_3, OBJETO_4, OBJETO_5";
                
        private readonly static string SELECT_ALL = "SELECT " + COLUMNAS + " FROM Escenario";
        
        private readonly static string INSERT = "INSERT INTO Escenario (" + COLUMNAS + ") VALUES (@piso, @o1, @o2, @o3, @o4, @o5)";

        private readonly static string UPDATE = "UPDATE Escenario SET OBJETO_1 = @o1, OBJETO_2 = @o2, OBJETO_3 = @o3, OBJETO_4 = @o4, OBJETO_5 = @o5 WHERE Piso = @piso";

        private readonly static string DELETE_ONE = "DELETE FROM Escenario WHERE Piso = @piso";

        public readonly static TipoObjeto[] OBJETOS = { TipoObjeto.RICK, TipoObjeto.MORTY, TipoObjeto.PISTOLA, TipoObjeto.PORTAL, TipoObjeto.NAVE };

        internal static void Guardar(Piso piso, Dictionary<TipoObjeto, Posicion> objetos)
        {
            using IDbConnection dbconn = new SqliteConnection(CONEXION);
            dbconn.Open();

            using IDbCommand dbCommand = dbconn.CreateCommand();
            dbCommand.CommandText = INSERT;

            dbCommand.Parameters.Add(new SqliteParameter("@piso", piso));

            int i = 1;
            foreach (TipoObjeto to in OBJETOS)
            {
                dbCommand.Parameters.Add(new SqliteParameter("@o" + i++, objetos[to]));
            }

            int status = dbCommand.ExecuteNonQuery();
            if (status == 0)
            {
                throw new Exception("Error almacenando datos");
            }
            else
            {
                Logger.Log("Datos almacenados correctamente");
            }
        }

        internal static void Modificar(Piso piso, Dictionary<TipoObjeto, Posicion> objetos)
        {
            using IDbConnection dbconn = new SqliteConnection(CONEXION);
            dbconn.Open();

            using IDbCommand dbCommand = dbconn.CreateCommand();
            dbCommand.CommandText = UPDATE;

            dbCommand.Parameters.Add(new SqliteParameter("@piso", piso));

            int i = 1;
            foreach (TipoObjeto to in OBJETOS)
            {
                dbCommand.Parameters.Add(new SqliteParameter("@o" + i++, objetos[to]));
            }

            int status = dbCommand.ExecuteNonQuery();
            if (status == 0)
            {
                throw new Exception("Error modificando datos");
            }
            else
            {
                Logger.Log("Datos modificados correctamente");
            }
        }

        internal static List<Escenario> CargarTodo()
        {
            List<Escenario> result = new List<Escenario>();
            try
            {
                using IDbConnection dbconn = new SqliteConnection(CONEXION);
                dbconn.Open();

                using IDbCommand dbCommand = dbconn.CreateCommand();
                dbCommand.CommandText = SELECT_ALL;

                IDataReader reader = dbCommand.ExecuteReader();

                while (reader.Read())
                {
                    Escenario escenario = new Escenario();
                    escenario.DefinirPiso((Piso)reader.GetInt32(0));
                    escenario.AgregarObjeto(TipoObjeto.RICK, (short)reader.GetInt32(1));
                    escenario.AgregarObjeto(TipoObjeto.MORTY, (short)reader.GetInt32(2));
                    escenario.AgregarObjeto(TipoObjeto.PISTOLA, (short)reader.GetInt32(3));
                    escenario.AgregarObjeto(TipoObjeto.PORTAL, (short)reader.GetInt32(4));
                    escenario.AgregarObjeto(TipoObjeto.NAVE, (short)reader.GetInt32(5));

                    Logger.Log("Escenario cargado > " + escenario);

                    result.Add(escenario);
                }
            }
            catch (Exception e)
            {
                Logger.LogError("Error cargando escenarios");
                Logger.LogError(e.Message);
            }

            return result;
        }

        internal static bool Eliminar(Piso piso)
        {
            try
            {
                using IDbConnection dbconn = new SqliteConnection(CONEXION);
                dbconn.Open();

                using IDbCommand dbCommand = dbconn.CreateCommand();
                dbCommand.CommandText = DELETE_ONE;
                dbCommand.Parameters.Add(new SqliteParameter("@piso", piso));

                dbCommand.ExecuteNonQuery();

                return true;
            }
            catch (Exception e)
            {
                Logger.LogError(e.Message);
            }
            return false;
        }
    }
}
