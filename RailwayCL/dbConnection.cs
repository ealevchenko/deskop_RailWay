using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;

namespace RailwayCL
{
    public class dbConnection
    {
        private SqlDataAdapter adapter;
        private SqlConnection conn;

        public dbConnection(String conStr)
        {
            adapter = new SqlDataAdapter();
            conn = new SqlConnection(conStr);
        }

        private SqlConnection openConnection() // открывает соединение (если нужно)
        {
            if (conn.State == ConnectionState.Closed || conn.State ==
            ConnectionState.Broken)
            {
                conn.Open();
            }
            return conn;
        }

        private void closeConnection() // закрывает соединение
        {
            if (conn.State == ConnectionState.Open)
            {
                conn.Close();
            }
        }

        public DataSet executeSelectQuery(String _query, SqlParameter[] sqlParameter)
        {
            SqlCommand comm = new SqlCommand();
            DataSet dataSet = new DataSet();

            try
            {
                comm.Connection = openConnection();
                comm.CommandText = _query;
                comm.Parameters.AddRange(sqlParameter);
                comm.ExecuteNonQuery();
                adapter.SelectCommand = comm;
                adapter.Fill(dataSet);
            }
            catch (SqlException ex)
            {
                if (ex.Number == 1205)
                {
                    throw new DeadlockException("Не удалось выполнить операцию! Повторите еще раз.");
                }
                else throw;
            }
            finally
            {
                closeConnection();
            }
            return dataSet;
        }

        public DataSet executeProc(String _query, SqlParameter[] sqlParameter)
        {
            SqlCommand comm = new SqlCommand();
            DataSet dataSet = new DataSet();

            try
            {
                comm.Connection = openConnection();
                comm.CommandText = _query;
                comm.CommandType = CommandType.StoredProcedure;
                comm.Parameters.AddRange(sqlParameter);
                SqlDataAdapter sqlda = new SqlDataAdapter(comm);

                sqlda.Fill(dataSet);
            }
            catch (SqlException ex)
            {
                if (ex.Number == 1205)
                {
                    throw new DeadlockException("Не удалось выполнить операцию! Повторите еще раз.");
                }
                else throw;
            }
            finally
            {
                closeConnection();
            }
            return dataSet;
        }

        public bool executeNonQueryCommand(String _query, SqlParameter[] sqlParameter)
        {
            SqlCommand comm = new SqlCommand();
            try
            {
                comm.Connection = openConnection();
                comm.CommandText = _query;
                comm.Parameters.AddRange(sqlParameter);
                comm.ExecuteNonQuery();
            }
            catch (SqlException ex)
            {
                if (ex.Number == 1205)
                {
                    throw new DeadlockException("Не удалось выполнить операцию! Повторите еще раз.");
                }
                else throw;
            }
            finally
            {
                closeConnection();
            }
            return true;
        }

        public object executeScalarCommand(String _query, SqlParameter[] sqlParameter, bool isProc)
        {
            SqlCommand comm = new SqlCommand();
            try
            {
                comm.Connection = openConnection();
                comm.CommandText = _query;
                if (isProc) comm.CommandType = CommandType.StoredProcedure;
                comm.Parameters.AddRange(sqlParameter);
                object res = comm.ExecuteScalar();
            }
            catch (SqlException ex)
            {
                if (ex.Number == 1205)
                {
                    throw new DeadlockException("Не удалось выполнить операцию! Повторите еще раз.");
                }
                else throw;
            }
            finally
            {
                closeConnection();
            }
            return true;
        }
    }

    [Serializable()]
    public class DeadlockException : System.Exception
    {
        public DeadlockException() : base() { }
        public DeadlockException(string message) : base(message) { }

        // A constructor is needed for serialization when an
        // exception propagates from a remoting server to the client. 
        protected DeadlockException(System.Runtime.Serialization.SerializationInfo info,
            System.Runtime.Serialization.StreamingContext context) { }
    }
}
