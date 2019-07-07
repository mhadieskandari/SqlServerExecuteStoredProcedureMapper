using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using AutoMapper;
using AutoMapper.Data;
using Microsoft.EntityFrameworkCore;

namespace SqlServerExecuteStoredProcedureMapper
{
    public static class SqlExtenion
    {
        public static List<TEntity> ExecuteSqlMapper<TEntity>(this DbContext dbContext, string storedProcedureName, List<SqlParameter> sqlParameters = null)
        {
            List<TEntity> entities = null;
            using (var command = dbContext.Database.GetDbConnection().CreateCommand())
            {
                command.CommandText = storedProcedureName;
                command.CommandType = CommandType.StoredProcedure;
                if (sqlParameters != null && sqlParameters.Count > 0)
                {
                    foreach (var param in sqlParameters)
                        command.Parameters.Add(param);
                }
                dbContext.Database.OpenConnection();

                var dataReader = command.ExecuteReader();

                if (dataReader.HasRows)
                {
                    Mapper.Reset();
                    Mapper.Initialize(cfg =>
                    {
                        cfg.AddDataReaderMapping();
                    });
                    entities = Mapper.Map<IDataReader, List<TEntity>>(dataReader);
                    dbContext.Database.CloseConnection();
                }
            }
            return entities;
        }
        /// <summary>
        /// call stored procedure with parameters and return your entity list 
        /// </summary>
        /// <typeparam name="TEntity">your entity return type</typeparam>
        /// <param name="dbContext">dbContext</param>
        /// <param name="storedProcedureName">sp name</param>
        /// <param name="sqlParameters">sp parameters</param>
        /// <returns>linst of TEntity</returns>
        public static List<TEntity> ExecuteSqlMapper<TEntity>(this DbContext dbContext, string storedProcedureName, params SqlParameter[] sqlParameters)
        {
            List<TEntity> entities = null;
            using (var command = dbContext.Database.GetDbConnection().CreateCommand())
            {
                command.CommandText = storedProcedureName;
                command.CommandType = CommandType.StoredProcedure;
                if (sqlParameters != null && sqlParameters.Length > 0)
                {
                    foreach (var param in sqlParameters)
                        command.Parameters.Add(param);
                }
                dbContext.Database.OpenConnection();

                var dataReader = command.ExecuteReader();
                
                if (dataReader.HasRows)
                {
                    Mapper.Reset();
                    Mapper.Initialize(cfg =>
                    {
                        cfg.AddDataReaderMapping();
                    });
                    entities = Mapper.Map<IDataReader, List<TEntity>>(dataReader);
                    dbContext.Database.CloseConnection();
                }
            }
            return entities;
        }
        /// <summary>
        /// call stored procedure with parameters and return your long 
        /// </summary>
        /// <param name="dbContext">dbContext</param>
        /// <param name="storedProcedureName">sp name</param>
        /// <param name="sqlParameters">sp parameters</param>
        /// <returns>long</returns>
        public static long? ExecuteStoredProcedureScalar(this DbContext dbContext, string storedProcedureName, params SqlParameter[] sqlParameters)
        {
            using (var command = dbContext.Database.GetDbConnection().CreateCommand())
            {
                command.CommandText = storedProcedureName;
                command.CommandType = CommandType.StoredProcedure;
                if (sqlParameters != null && sqlParameters.Length > 0)
                {
                    foreach (var param in sqlParameters)
                        command.Parameters.Add(param);
                }
                dbContext.Database.OpenConnection();

                var ret = command.ExecuteScalar();
                return long.TryParse(ret.ToString(), out long res) ? (long?) res : null;
            }
        }

        //public static DbCommand LoadStoredProcedure(this DbContext context, string storedProcName)
        //{
        //    var cmd = context.Database.GetDbConnection().CreateCommand();
        //    cmd.CommandText = storedProcName;
        //    cmd.CommandType = System.Data.CommandType.StoredProcedure;
        //    return cmd;
        //}


        //public static DbCommand WithSqlParams(this DbCommand cmd, params (string, object)[] nameValues)
        //{
        //    foreach (var pair in nameValues)
        //    {
        //        var param = cmd.CreateParameter();
        //        param.ParameterName = pair.Item1;
        //        param.Value = pair.Item2 ?? DBNull.Value;
        //        cmd.Parameters.Add(param);
        //    }
        //    return cmd;
        //}

        //public static IList<T> ExecuteStoredProcedure<T>(this DbCommand command) where T : class
        //{

        //    using (command)
        //    {
        //        if (command.Connection.State == System.Data.ConnectionState.Closed)
        //            command.Connection.Open();

        //        try
        //        {
        //            using (var reader = command.ExecuteReader())
        //            {
        //                return reader.MapToList<T>();
        //            }
        //        }
        //        finally
        //        {
        //            command.Connection.Close();
        //        }

        //    }
        //}

        //public static async Task<IList<T>> ExecuteStoredProcedureAsync<T>(this DbCommand command) where T : class
        //{
        //    using (command)
        //    {
        //        if (command.Connection.State == System.Data.ConnectionState.Closed)
        //            await command.Connection.OpenAsync();
        //        try
        //        {
        //            using (var reader = command.ExecuteReader())
        //            {
        //                return reader.MapToList<T>();
        //            }
        //        }
        //        finally
        //        {
        //            command.Connection.Close();
        //        }
        //    }
        //}

        //private static IList<T> MapToList<T>(this DbDataReader dr)
        //{
        //    var objList = new List<T>();
        //    var props = typeof(T).GetRuntimeProperties();
        //    var colMapping = dr.GetColumnSchema()
        //        .Where(x => props.Any(y => y.Name.ToLower() == x.ColumnName.ToLower()))
        //        .ToDictionary(key => key.ColumnName.ToLower());
        //    if (dr.HasRows)
        //    {
        //        while (dr.Read())
        //        {
        //            T obj = Activator.CreateInstance<T>();
        //            foreach (var prop in props)
        //            {
        //                var val = dr.GetValue(colMapping[prop.Name.ToLower()].ColumnOrdinal.Value);
        //                prop.SetValue(obj, val == DBNull.Value ? null : val);
        //            }
        //            objList.Add(obj);
        //        }
        //    }
        //    return objList;
        //}

    }
}
