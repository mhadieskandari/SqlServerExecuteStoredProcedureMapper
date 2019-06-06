using AutoMapper;
using AutoMapper.Data;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace Microsoft.EntityFrameworkCore.Extensions
{
    public static class SqlExtenion
    {
        public static List<TEntity> ExecuteSqlMapper<TEntity>(this DbContext dbContext, string StoredProcedureName, List<SqlParameter> sqlParameters = null)
        {
            List<TEntity> entities = null;
            using (var command = dbContext.Database.GetDbConnection().CreateCommand())
            {
                command.CommandText = StoredProcedureName;
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
    }
}
