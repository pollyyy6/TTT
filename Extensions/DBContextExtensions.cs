using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Data;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Internal;

namespace Extensions
{
	public static class DBContextExtensions
	{
		public static int EnsureDeleted<T>(this DatabaseFacade db, DbSet<T> set) where T : class
		{
			int res = 0;
			try
			{
				TableDescription Table = GetTableName(set);
				res = db.ExecuteSqlRaw($"DROP TABLE [{Table.Schema}].[{Table.TableName}];");
			}
			catch (Exception)
			{
			}
			return res;
		}

		public static int EnsureDeleted(this DatabaseFacade db, String TableName, String Schema)
		{
			TableDescription Table = new TableDescription();
			Table.Schema = Schema;
			Table.TableName = TableName;
			int res = 0;
			try
			{
				res = db.ExecuteSqlRaw($"DROP TABLE [{Table.Schema}].[{Table.TableName}];");
			}
			catch (Exception)
			{
				
			}
			
			return res;
		}
		public static TableDescription GetTableName<T>(this DbSet<T> dbSet) where T : class
		{

			var dbContext = dbSet.GetDbContext();

			var model = dbContext.Model;
			var entityTypes = model.GetEntityTypes();
			var entityType = entityTypes.First(t => t.ClrType == typeof(T));
			var tableNameAnnotation = entityType.GetAnnotation("Relational:TableName");
			var tableSchemaAnnotation = entityType.GetAnnotation("Relational:Schema");
			var tableName = tableNameAnnotation.Value.ToString();
			var schemaName = tableSchemaAnnotation.Value.ToString();
			return new TableDescription { Schema = schemaName, TableName = tableName };
		}

		public static DbContext GetDbContext<T>(this DbSet<T> dbSet) where T : class
		{
			var infrastructure = dbSet as IInfrastructure<IServiceProvider>;
			var serviceProvider = infrastructure.Instance;
			var currentDbContext = serviceProvider.GetService(typeof(ICurrentDbContext)) as ICurrentDbContext;
			return currentDbContext.Context;
		}

		//public static void Truncate<TEntity>(this Table<TEntity> table) where TEntity : class
		//{
		//	var rowType = table.GetType().GetGenericArguments()[0];
		//	var tableName = table.Context.Mapping.GetTable(rowType).TableName;
		//	var sqlCommand = String.Format("TRUNCATE TABLE {0}", tableName);
		//	table.Context.ExecuteCommand(sqlCommand);
		//}

	}

	public class TableDescription
	{
		public String Schema { get; set; }
		public String TableName { get; set; }
	}
}
