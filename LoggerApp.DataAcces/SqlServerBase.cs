using LoggerApp.Utility;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoggerApp.DataAccess
{
    public class SqlServerBase : IDisposable
    {
        private SqlCommand cmd = null;
        private SqlConnection cnn = null;
        private SqlTransaction tr = null;
        private SqlDataReader reader = null;

        public SqlServerBase()
        {

        }
        public SqlTransaction Transaction
        {
            get
            {
                return tr;
            }
        }

        public SqlConnection Connection
        {
            get
            {
                return cnn;
            }
        }

        public SqlServerBase(SqlConnection sqlConnection, SqlTransaction sqlTransaction)
        {
            if (sqlConnection != null)
            {
                cnn = sqlConnection;
                if (sqlTransaction != null)
                    tr = sqlTransaction;
            }
        }

        private SqlConnection CreateConnection()
        {
            try
            {
                if (ConfigurationManager.ConnectionStrings["Database1Entities"] == null)
                    throw new Exception("No se ha establecido la cadena de conexión Sql Server en el archivo de configuración");

                string oradb = ConfigurationManager.ConnectionStrings["Database1Entities"].ConnectionString;

                cnn = new SqlConnection { ConnectionString = oradb };
                return cnn;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void CreateCommand(string procedureName)
        {
            cmd = new SqlCommand(procedureName);
            cmd.CommandType = CommandType.StoredProcedure;
            if (cnn == null)
                CreateConnection();
            cmd.Connection = cnn;
            if (tr != null)
                cmd.Transaction = tr;
        }

        public void CreateInputParameter(string parameterName, object value)
        {
            SqlParameter parametro = new SqlParameter()
            {
                ParameterName = "@" + parameterName,
                Direction = ParameterDirection.Input,
                Value = value
            };
            cmd.Parameters.Add(parametro);
        }

        public void CreateOututParameter(string parameterName, SqlDbType sqlDbType, int tamano = 0)
        {
            SqlParameter parametro = new SqlParameter()
            {
                ParameterName = "@" + parameterName,
                Direction = ParameterDirection.Output,
                SqlDbType = sqlDbType,
                Size = tamano

            };
            cmd.Parameters.Add(parametro);
        }

        public void ClearParameters()
        {
            cmd.Parameters.Clear();
        }

        public int ExecuteNonQuery()
        {
            try
            {
                if (cnn.State != ConnectionState.Open)
                    cnn.Open();
                int Resultado = cmd.ExecuteNonQuery();
                if (tr == null)
                    CloseConnection();
                return Resultado;
            }
            catch (Exception ex)
            {
                this.Dispose();
                throw ex;
            }
        }

        public int ExecuteScalar()
        {
            int Result = -1;
            try
            {
                if (cnn.State != ConnectionState.Open)
                    cnn.Open();
                Result = Convert.ToInt32(cmd.ExecuteScalar());
                CloseConnection();
                return Result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public DataTable ExecuteQuery()
        {
            try
            {
                SqlDataAdapter adaptador = new SqlDataAdapter(cmd);
                if (cnn.State != ConnectionState.Open)
                    cnn.Open();
                adaptador.SelectCommand = cmd;
                DataTable resultado = new DataTable();
                adaptador.Fill(resultado);
                CloseConnection();
                return resultado;
            }
            catch (Exception ex)
            {
                this.Dispose();
                throw ex;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public SqlDataReader ExecuteReader()
        {
            try
            {
                if (cnn.State != ConnectionState.Open)
                    cnn.Open();
                reader = cmd.ExecuteReader();
                return reader;
            }
            catch (Exception ex)
            {
                this.Dispose();
                throw ex;
            }
        }

        /// <summary>
        /// Cierra conexión con la base de datos
        /// </summary>
        private void CloseConnection()
        {
            if (cnn != null)
                cnn.Close();
        }

        /// <summary>
        /// Destruye los objetos
        /// </summary>
        public void Dispose()
        {
            if (reader != null && !reader.IsClosed)
            {
                reader.Close();
            }
            if (cnn != null)
            {
                if (cnn.State != ConnectionState.Open)
                    cnn.Close();
                cnn.Dispose();
                cnn = null;
            }
            if (tr != null)
            {
                tr.Dispose();
                if (cmd != null)
                    cmd.Transaction = null;
            }
            if (cmd != null)
            {
                cmd.Dispose();
                cmd = null;
            }
        }

        /// <summary>
        /// Inicia una transacción.
        /// </summary>
        protected void BeginTransaction()
        {
            try
            {
                if (cnn == null)
                    CreateConnection();
                if (cnn.State != ConnectionState.Open)
                    cnn.Open();
                tr = cnn.BeginTransaction(IsolationLevel.ReadCommitted);
            }
            catch (Exception ex)
            {
                throw new Exception("IniciarTransaccion => " + ex.Message, ex);
            }
        }

        /// <summary>
        /// Confirma la transacción actual.
        /// </summary>
        protected void CommitTransaction()
        {
            tr.Commit();
            CloseConnection();
        }

        /// <summary>
        /// Cancela una transacción de base de datos
        /// </summary>
        protected void RollbackTransaction()
        {
            try
            {
                if (cnn != null && cnn.State == ConnectionState.Open && tr != null)
                    tr.Rollback();
                CloseConnection();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="name"></param>
        /// <returns></returns>
        public T GetParameterValue<T>(string name)
        {
            SqlParameter parameter = cmd.Parameters.Cast<SqlParameter>().Where(o => o.ParameterName == "@" + name).FirstOrDefault();
            if (parameter == null)
                throw new Exception("El parametro '" + name + "' no existe.");
            if (parameter.Value == DBNull.Value)
                return default(T);
            T obj = (T)parameter.Value;
            return obj;
        }

        /// <summary>
        /// metodo que convierte las propiedades de un objeto a SqlParameter
        /// </summary>
        /// <param name="Obj">Objeto a convertir</param>
        /// <param name="ParameterOut">Parametro de salida tipo Int</param>
        public void ConvertPropertiesToSQLParameters(object Obj, string ParameterOut = null, SqlDbType tipoOUT = SqlDbType.Int)
        {
            try
            {
                Dictionary<string, string> Parametros = HelperGeneric.MapObjectToDictionary(Obj);
                foreach (var item in Parametros)
                {
                    SqlParameter a = new SqlParameter("@" + item.Key, item.Value);
                    cmd.Parameters.Add(a);
                }
                if (ParameterOut != null)
                {
                    SqlParameter a = new SqlParameter();
                    if (tipoOUT == SqlDbType.VarChar)
                    {
                        a = new SqlParameter("@" + ParameterOut, tipoOUT, 36);
                    }
                    else
                    {
                        a = new SqlParameter("@" + ParameterOut, tipoOUT);
                    }
                    a.Direction = ParameterDirection.Output;
                    cmd.Parameters.Add(a);
                }
            }
            catch (Exception ex)
            {
                throw new Exception("No fue posible crear el objeto, error " + ex.Message);
            }
        }

        public void ConvertPropertiesToSQLParameters(object Obj, SqlDbType TipoParametroOut, params string[] ParameterOuts)
        {
            try
            {
                Dictionary<string, string> Parametros = HelperGeneric.MapObjectToDictionary(Obj);
                foreach (var item in Parametros)
                {
                    SqlParameter a = new SqlParameter("@" + item.Key, item.Value);
                    cmd.Parameters.Add(a);
                }
                if (ParameterOuts != null)
                {
                    foreach (string item in ParameterOuts)
                    {
                        SqlParameter a = new SqlParameter("@" + item, TipoParametroOut);
                        a.Size = 1024;
                        a.Direction = ParameterDirection.Output;
                        cmd.Parameters.Add(a);
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("No fue posible crear el objeto, error " + ex.Message);
            }
        }
    }
}
