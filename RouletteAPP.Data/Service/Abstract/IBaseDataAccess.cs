using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using System.Threading.Tasks;

namespace RouletteAPP.Data.Service.Abstract
{
    public interface IBaseDataAccess
    {
        SqlParameter GetParameter(string parameter, Object value);
        SqlParameter GetParameterOut(string parameter, SqlDbType type, object value = null, ParameterDirection parameterDirection = ParameterDirection.InputOutput);
        Task<int> ExcecuteNonQueryAsync(string procedureName, List<SqlParameter> parameters, CommandType commandType = CommandType.StoredProcedure);
        Task<object> ExecuteScalarAsync(string procedureName, List<SqlParameter> parameters, CommandType commandType = CommandType.StoredProcedure, string parameterOutput = null);
        Task<SqlDataReader> GetDataReaderAsync(string procedureName, List<SqlParameter> parameters, CommandType commandType = CommandType.StoredProcedure);
    }
}
