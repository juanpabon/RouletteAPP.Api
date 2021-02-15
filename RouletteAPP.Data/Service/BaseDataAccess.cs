using RouletteAPP.Data.Helpers;
using RouletteAPP.Data.Service.Abstract;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using System.Threading.Tasks;

namespace RouletteAPP.Service.Data
{
    public class BaseDataAccess : IBaseDataAccess
    {
        private static SqlConnection _sqlConnection;
        public BaseDataAccess()
        {
        }
        private static SqlConnection GetConnection()
        {
            try
            {
                _sqlConnection = new SqlConnection(SqlHelper.ConnectionString);
                return _sqlConnection;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        protected static SqlCommand GetCommand(string commandText, CommandType _commmandType)
        {
            _sqlConnection = new SqlConnection();
            _sqlConnection = GetConnection();
            SqlCommand _sqlCommand = new SqlCommand(commandText, _sqlConnection as SqlConnection)
            {
                CommandType = _commmandType
            };
            return _sqlCommand;
        }
        public SqlParameter GetParameter(string parameter, Object value)
        {
            SqlParameter _parameterObject = new SqlParameter(parameter, value != null ? value : DBNull.Value);
            _parameterObject.Direction = ParameterDirection.Input;
            return _parameterObject;
        }
        public SqlParameter GetParameterOut(string parameter, SqlDbType type, object value = null, ParameterDirection parameterDirection = ParameterDirection.InputOutput)
        {
            SqlParameter _parameterObject = new SqlParameter(parameter, type);
            if (type == SqlDbType.NVarChar || type == SqlDbType.VarChar || type == SqlDbType.NText || type == SqlDbType.Text)
            {
                _parameterObject.Size = -1;
            }
            _parameterObject.Direction = parameterDirection;
            if (value != null)
            {
                _parameterObject.Value = value;
            }
            else
            {
                _parameterObject.Value = DBNull.Value;
            }
            return _parameterObject;
        }
        public async Task<int> ExcecuteNonQueryAsync(string procedureName, List<SqlParameter> parameters, CommandType commandType = CommandType.StoredProcedure)
        {
            int returnValue;
            try
            {
                SqlCommand command = GetCommand(procedureName, commandType);
                await command.Connection.OpenAsync();
                if (parameters != null && parameters.Count > 0)
                {
                    command.Parameters.AddRange(parameters.ToArray());
                }
                returnValue = await command.ExecuteNonQueryAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return 0;
            }
            return returnValue;
        }
        public async Task<object> ExecuteScalarAsync(string procedureName, List<SqlParameter> parameters, CommandType commandType = CommandType.StoredProcedure, string parameterOutput = null)
        {
            object returnValue;
            try
            {
                SqlCommand command = GetCommand(procedureName, commandType);
                await command.Connection.OpenAsync();
                if (parameters != null && parameters.Count > 0)
                {
                    command.Parameters.AddRange(parameters.ToArray());
                }
                returnValue = await command.ExecuteScalarAsync();
                if (!(string.IsNullOrWhiteSpace(parameterOutput)))
                {
                    returnValue = command.Parameters[parameterOutput].Value;
                }
                command.Connection.Dispose();
                command.Connection.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return null;
            }
            return returnValue;
        }
        public async Task<SqlDataReader> GetDataReaderAsync(string procedureName, List<SqlParameter> parameters, CommandType commandType = CommandType.StoredProcedure)
        {
            SqlDataReader dr;
            try
            {
                SqlCommand command = GetCommand(procedureName, commandType);
                await command.Connection.OpenAsync();
                if (parameters != null && parameters.Count > 0)
                {
                    command.Parameters.AddRange(parameters.ToArray());
                }
                dr = await command.ExecuteReaderAsync(CommandBehavior.CloseConnection);
                command.Connection.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return null;
            }
            return dr;
        }
    }
}
