using System;
using System.Collections.Generic;
using System.Data;
//using Oracle.DataAccess.Client;
using System.Data.OracleClient;
using System.Data.Common;


namespace TYSystem.BaseFramework.Dapper.OracleDBHelper
{
    /// <summary>
    /// ��װ���ݿ�Ļ�������
    /// </summary>
    /// <remarks>    
    public class SQLHelper
    {
        #region ˽�з����͹���

        //sql
        private static void PrepareCommand(OracleCommand command, OracleConnection connection, OracleTransaction transaction, CommandType commandType, string commandText, out bool mustCloseConnection, List<IDataParameter> commandParameters, int? commandTimeout = null)
        {
            // If the provided connection is not open, we will open it
            if (connection.State != ConnectionState.Open)
            {
                mustCloseConnection = true;
                connection.Open();
            }
            else
            {
                mustCloseConnection = false;
            }
            // Associate the connection with the command
            command.Connection = connection;

            // Set the command text (stored procedure name or SQL statement)
            command.CommandText = commandText;

            // If we were provided a transaction, assign it
            if (transaction != null)
            {
                command.Transaction = transaction;
            }
            if (commandTimeout != null)
            {
                command.CommandTimeout = Convert.ToInt32(commandTimeout);
            }
            // Set the command type
            command.CommandType = commandType;
            // Attach the command parameters if they are provided
            if (commandParameters != null)
            {
                AttachParameters(command, commandParameters);
            }
            return;
        }



        //ͨ��
        private static void AttachParameters(OracleCommand command, List<IDataParameter> commandParameters)
        {

            if (commandParameters != null && commandParameters.Count > 0)
            {
                foreach (OracleParameter p in commandParameters)
                {
                    if (p != null)
                    {
                        // Check for derived output value with no value assigned
                        if ((p.Direction == ParameterDirection.InputOutput ||
                            p.Direction == ParameterDirection.Input) &&
                            (p.Value == null))
                        {
                            p.Value = DBNull.Value;
                        }
                        command.Parameters.Add(p);
                    }
                }
            }
        }

        #endregion

        #region transaction ������
        /// <summary>
        /// ��ʼ����
        /// </summary>
        /// <param name="conn">���ݿ�����</param>
        /// <param name="Iso">ָ�����ӵ�����������Ϊ</param>
        /// <returns>��ǰ����</returns>  
        public static IDbTransaction BeginTransaction(OracleConnection conn, IsolationLevel Iso)
        {
            conn.Open();
            return conn.BeginTransaction(Iso);
        }

        /// <summary>
        /// ��ʼ����
        /// </summary>
        /// <param name="conn">���ݿ�����</param>
        /// <returns>��ǰ����</returns>
        public static IDbTransaction BeginTransaction(OracleConnection conn)
        {
            conn.Open();
            return conn.BeginTransaction();
        }

        /// <summary>
        /// ��������ȷ�ϲ���
        /// </summary>
        /// <param name="Transaction">Ҫ����������</param>
        public static void endTransactionCommit(IDbTransaction Transaction)
        {
            using (DbConnection con = (DbConnection)Transaction.Connection)
            {
                Transaction.Commit();
            }
        }

        /// <summary>
        /// �������񣬻ع�����
        /// </summary>
        /// <param name="Transaction">Ҫ����������</param>
        public static void endTransactionRollback(IDbTransaction Transaction)
        {
            using (DbConnection con = (DbConnection)Transaction.Connection)
            {
                Transaction.Rollback();
            }
        }

        #endregion

        #region ExecuteNonQuery


        /// <summary>
        /// ִ�Уӣѣ������ߴ洢���� ,�����ز���,ֻ����Ӱ������
        /// </summary>
        /// <param name="connection">Ҫִ�Уӣѣ���������</param>
        /// <param name="commandText">�ӣѣ������ߴ洢������</param>
        /// <param name="commandParameters">�ӣѣ������ߴ洢���̲���</param>
        /// <param name="commandType">�ӣѣ��������</param>
        /// <param name="commandTimeout">��ʱʱ��</param>
        /// <returns>Ӱ�������</returns>
        public static int ExecuteNonQuery(OracleConnection connection, string commandText, List<IDataParameter> commandParameters = null, CommandType commandType = CommandType.Text, int? commandTimeout = null)
        {
            int retval = 0;
            //Ҫ������
            OracleCommand cmd = new OracleCommand();
            bool mustCloseConnection = false;
            PrepareCommand(cmd, connection, (OracleTransaction)null, commandType, commandText, out mustCloseConnection, commandParameters, commandTimeout);
            retval = cmd.ExecuteNonQuery();
            cmd.Parameters.Clear();
            if (mustCloseConnection)
                connection.Close();
            return retval;
        }


        /// <summary>
        ///  ִ�Уӣѣ������ߴ洢���� ,�����ز���,ֻ����Ӱ������(ͨ��)
        /// </summary>
        /// <param name="transaction">������ڵ�����</param>
        /// <param name="commandType">�ӣѣ��������</param>
        /// <param name="commandText">�ӣѣ������ߴ洢������</param>
        /// <param name="commandParameters">�ӣѣ������ߴ洢���̲���</param>
        /// <returns>Ӱ�������</returns>
        public static int ExecuteNonQuery(IDbTransaction transaction, string commandText, List<IDataParameter> commandParameters = null, CommandType commandType = CommandType.Text, int? commandTimeout = null)
        {
            //Ҫ������  
            OracleCommand cmd = new OracleCommand();
            bool mustCloseConnection = false;
            PrepareCommand(cmd, ((OracleTransaction)transaction).Connection, (OracleTransaction)transaction, commandType, commandText, out mustCloseConnection, commandParameters, commandTimeout);
            int retval = cmd.ExecuteNonQuery();
            cmd.Parameters.Clear();
            return retval;
        }
        #endregion

        #region ExecuteDataset

        /// <summary>
        /// ִ�Уӣѣ������ߴ洢���� ,���ز���dataset
        /// </summary>
        /// <param name="connection">Ҫִ�Уӣѣ���������</param>
        /// <param name="commandText">�ӣѣ������ߴ洢������</param>
        /// <param name="commandParameters">�ӣѣ������ߴ洢���̲���</param>
        /// <param name="commandType">�ӣѣ��������</param>
        /// <param name="commandTimeout">��ʱʱ��</param>
        /// <returns>ִ�н����</returns>
        public static DataSet ExecuteDataset(OracleConnection connection, string commandText, List<IDataParameter> commandParameters = null, CommandType commandType = CommandType.Text, int? commandTimeout = null)
        {
            OracleCommand cmd = new OracleCommand();
            bool mustCloseConnection = false;
            PrepareCommand(cmd, connection, (OracleTransaction)null, commandType, commandText, out mustCloseConnection, commandParameters, commandTimeout);
            using (OracleDataAdapter da = new OracleDataAdapter(cmd))
            {
                DataSet ds = new DataSet();
                da.Fill(ds);
                cmd.Parameters.Clear();
                if (mustCloseConnection)
                    connection.Close();
                return ds;
            }
        }

        /// <summary>
        /// ִ�Уӣѣ������ߴ洢���� ,���ز���dataset
        /// </summary>
        /// <param name="transaction">������ڵ�����</param>
        /// <param name="commandText">�ӣѣ������ߴ洢������</param>
        /// <param name="commandParameters">�ӣѣ������ߴ洢���̲���</param>
        /// <param name="commandType">�ӣѣ��������</param>
        /// <param name="commandTimeout">��ʱʱ��</param>
        /// <returns>ִ�н����</returns>
        public static DataSet ExecuteDataset(IDbTransaction transaction, string commandText, List<IDataParameter> commandParameters = null, CommandType commandType = CommandType.Text, int? commandTimeout = null)
        {
            OracleCommand cmd = new OracleCommand();
            bool mustCloseConnection = false;
            PrepareCommand(cmd, (OracleConnection)transaction.Connection, (OracleTransaction)transaction, commandType, commandText, out mustCloseConnection, commandParameters, commandTimeout);
            using (OracleDataAdapter da = new OracleDataAdapter(cmd))
            {
                DataSet ds = new DataSet();
                da.Fill(ds);
                cmd.Parameters.Clear();
                return ds;
            }
        }
        #endregion

        #region ExecuteReader

        //ͨ��
        private static OracleDataReader ExecuteReader(OracleConnection connection, OracleTransaction transaction, CommandType commandType, string commandText, bool isClose, List<IDataParameter> commandParameters = null, int? commandTimeout = null)
        {
            bool mustCloseConnection = false;
            OracleCommand cmd = new OracleCommand();
            PrepareCommand(cmd, connection, transaction, commandType, commandText, out mustCloseConnection, commandParameters, commandTimeout);
            OracleDataReader dataReader = null;
            if (isClose)
            {
                dataReader = cmd.ExecuteReader(CommandBehavior.CloseConnection);
            }
            else
            {
                dataReader = cmd.ExecuteReader();
            }
            bool canClear = true;
            foreach (IDataParameter commandParameter in cmd.Parameters)
            {
                if (commandParameter.Direction != ParameterDirection.Input)
                    canClear = false;
            }
            if (canClear)
            {
                cmd.Parameters.Clear();
            }
            return dataReader;
        }


        /// <summary>
        /// ִ�Уӣѣ������ߴ洢���� ,���ز���datareader(ͨ��)
        /// <remarks >
        /// ��Ҫ��ʾ�ر�����
        /// </remarks>
        /// </summary>
        /// <param name="connection">Ҫִ�Уӣѣ���������</param>
        /// <param name="commandType">�ӣѣ��������</param>
        /// <param name="commandText">�ӣѣ������ߴ洢������</param>
        /// <param name="commandParameters">�ӣѣ������ߴ洢���̲���</param>
        /// <returns>DataReader</returns>
        public static OracleDataReader ExecuteReader(OracleConnection connection, string commandText, List<IDataParameter> commandParameters = null, CommandType commandType = CommandType.Text, int? commandTimeout = null)
        {
            bool mustCloseConnection = true;
            return ExecuteReader(connection, (OracleTransaction)null, commandType, commandText, mustCloseConnection, commandParameters, commandTimeout);
        }


        /// <summary>
        /// ִ�Уӣѣ������ߴ洢���� ,���ز���datareader
        /// <remarks >
        /// ��Ҫ��ʾ�ر�����
        /// </remarks>
        /// </summary>
        /// <param name="transaction">����</param>
        /// <param name="commandType">�ӣѣ��������</param>
        /// <param name="commandText">�ӣѣ������ߴ洢������</param>
        /// <param name="commandParameters">�ӣѣ������ߴ洢���̲���</param>
        /// <returns>DataReader</returns>
        public static OracleDataReader ExecuteReader(IDbTransaction transaction, string commandText, List<IDataParameter> commandParameters = null, CommandType commandType = CommandType.Text, int? commandTimeout = null)
        {
            bool mustCloseConnection = false;
            return ExecuteReader((OracleConnection)transaction.Connection, (OracleTransaction)transaction, commandType, commandText, mustCloseConnection, commandParameters, commandTimeout);
        }

        #endregion

        #region ExecuteScalar


        /// <summary>
        /// ִ�Уӣѣ������ߴ洢���� ,���ز���object����һ�У���һ�е�ֵ(ͨ��)
        /// </summary>
        /// <param name="connection">Ҫִ�Уӣѣ���������</param>
        /// <param name="commandType">�ӣѣ��������</param>
        /// <param name="commandText">�ӣѣ������ߴ洢������</param>
        /// <param name="commandParameters">�ӣѣ������ߴ洢���̲���</param>
        /// <returns>ִ�н������һ�У���һ�е�ֵ</returns>��
        public static object ExecuteScalar(OracleConnection connection, string commandText, List<IDataParameter> commandParameters = null, CommandType commandType = CommandType.Text, int? commandTimeout = null)
        {
            object retval = null;
            OracleCommand cmd = new OracleCommand();
            bool mustCloseConnection = false;
            PrepareCommand(cmd, connection, (OracleTransaction)null, commandType, commandText, out mustCloseConnection, commandParameters, commandTimeout);
            retval = cmd.ExecuteScalar();
            cmd.Parameters.Clear();
            if (mustCloseConnection)
                connection.Close();
            return retval;
        }



        /// <summary>
        ///  ִ�Уӣѣ������ߴ洢���� ,���ز���object����һ�У���һ�е�ֵ
        /// </summary>
        /// <param name="transaction">������ڵ�����</param>
        /// <param name="commandType">�ӣѣ��������</param>
        /// <param name="commandText">�ӣѣ������ߴ洢������</param>
        /// <param name="commandParameters">�ӣѣ������ߴ洢���̲���</param>
        /// <returns>ִ�н������һ�У���һ�е�ֵ</returns>
        public static object ExecuteScalar(IDbTransaction transaction, string commandText, List<IDataParameter> commandParameters = null, CommandType commandType = CommandType.Text, int? commandTimeout = null)
        {
            object retval = null;
#pragma warning disable CS0618 // ���ͻ��Ա�ѹ�ʱ
            OracleCommand cmd = new OracleCommand();
#pragma warning restore CS0618 // ���ͻ��Ա�ѹ�ʱ
            bool mustCloseConnection = false;
            PrepareCommand(cmd, ((OracleTransaction)transaction).Connection, (OracleTransaction)transaction, commandType, commandText, out mustCloseConnection, commandParameters, commandTimeout);
            retval = cmd.ExecuteScalar();
            cmd.Parameters.Clear();
            return retval;
        }

        #endregion


    }
}
