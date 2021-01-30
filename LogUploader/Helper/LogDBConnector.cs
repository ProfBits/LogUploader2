using Dapper;
using LogUploader.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogUploader.Helper
{
    static class LogDBConnector
    {
        public static string DBConnectionString { get; set; }

        #region Create

        public static void CreateTable() => CreateTable(DBConnectionString);
        public static void CreateTable(string connectionString)
        {
            using (IDbConnection cnn = new SQLiteConnection(connectionString))
            {
                var sqlStatement = @"CREATE TABLE 'LogData' (
    'ID'    INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT UNIQUE,
	'BossID'    INTEGER NOT NULL DEFAULT 0,
	'EvtcPath'  TEXT,
	'JsonPath'  TEXT,
	'HtmlPath'  TEXT,
	'Link'  TEXT,
	'SizeKb'    INTEGER NOT NULL DEFAULT 0,
	'TimeStamp' INTEGER NOT NULL DEFAULT 0,
	'DurationMs'    INTEGER NOT NULL DEFAULT 0,
	'Flags' INTEGER NOT NULL DEFAULT 0,
	'RemainingHealth'   REAL NOT NULL DEFAULT 100
)";
                cnn.Execute(sqlStatement);
            }
        }


        #endregion
        #region Get

        public static List<DBLog> GetAll() => GetAll(DBConnectionString);
        public static List<DBLog> GetAll(string connectionString)
        {
            using (IDbConnection cnn = new SQLiteConnection(connectionString))
            {
                return cnn.Query<DBLog>("SELECT * FROM [LogData]", new DynamicParameters()).ToList();
                 
            }
        }

        public static DBLog GetByID(int id) => GetByID(DBConnectionString, id);
        public static DBLog GetByID(string connectionString, int id)
        {
            using (IDbConnection cnn = new SQLiteConnection(connectionString))
            {
                return cnn.Query<DBLog>("SELECT * FROM [LogData] WHERE ID = @ID", new { ID = id}).FirstOrDefault();
            }
        }
        public static DBLog GetNewest() => GetNewest(DBConnectionString);
        public static DBLog GetNewest(string connectionString)
        {
            using (IDbConnection cnn = new SQLiteConnection(connectionString))
            {
                var max = cnn.ExecuteScalar("SELECT MAX(TimeStamp) FROM [LogData];");
                if (max == null)
                    return null;
                return cnn.Query<DBLog>("SELECT * FROM [LogData] WHERE TimeStamp = @TimeStamp", new { TimeStamp = (long)max }).FirstOrDefault();
            }
        }

        public static List<DBLog> GetByEvtcPaht(string evtc) => GetByEvtcPaht(DBConnectionString, evtc);
        public static List<DBLog> GetByEvtcPaht(string connectionString, string evtc)
        {
            using (IDbConnection cnn = new SQLiteConnection(connectionString))
            {
                var t = cnn.Query<DBLog>($"SELECT * FROM [LogData] WHERE EvtcPath LIKE '{evtc}'").ToList();
                return t;
            }
        }

        public static List<DBLog> GetByBossIdWithPath(long id) => GetByBossIdWithPath(DBConnectionString, id);
        public static List<DBLog> GetByBossIdWithPath(string connectionString, long id)
        {
            using (IDbConnection cnn = new SQLiteConnection(connectionString))
            {
                var t = cnn.Query<DBLog>($"SELECT * FROM [LogData] WHERE BossID == {id} AND NOT EvtcPath IS NULL").ToList();
                return t;
            }
        }

        public static int GetCount() => GetCount(DBConnectionString);
        public static int GetCount(string connectionString)
        {
            using (IDbConnection cnn = new SQLiteConnection(connectionString))
            {
                return (int)(long)cnn.ExecuteScalar("SELECT COUNT(*) FROM [LogData]");
            }
        }

        #endregion
        #region Insert

        public static int Insert(IDBLog log) => Insert(DBConnectionString, log);
        public static int Insert(string connectionString, IDBLog log)
        {
            using (IDbConnection cnn = new SQLiteConnection(connectionString))
            {
                var sqlStatement = @"
INSERT INTO [LogData]
(BossID
,EvtcPath
,JsonPath
,HtmlPath
,Link
,SizeKb
,TimeStamp
,DurationMs
,Flags
,RemainingHealth)
VALUES (@BossID
,@EvtcPath
,@JsonPath
,@HtmlPath
,@Link
,@SizeKb
,@TimeStamp
,@DurationMs
,@Flags
,@RemainingHealth);

SELECT CAST(last_insert_rowid() as int);";
                var newID = cnn.ExecuteScalar(sqlStatement, log);

                return (int)(long) newID;
            }
        }

        #endregion
        #region InsertBulk

        public static void InsertBulk(IList<IDBLog> logs) => InsertBulk(DBConnectionString, logs);
        public static void InsertBulk(string connectionString, IList<IDBLog> logs)
        {
            var sqls = GetSqlsInBatches(logs);
            using (IDbConnection cnn = new SQLiteConnection(connectionString))
            {
                foreach (var sql in sqls)
                {
                    cnn.ExecuteAsync(sql);
                }
            }
        }
        private static IEnumerable<string> GetSqlsInBatches(IList<IDBLog> persons)
        {
            var insertSql = @"
INSERT INTO [LogData]
(BossID
,EvtcPath
,JsonPath
,HtmlPath
,Link
,SizeKb
,TimeStamp
,DurationMs
,Flags
,RemainingHealth)
VALUES ";
            var valuesSql = "({0}, '{1}', '{2}', '{3}', '{4}', {5}, {6}, {7}, {8}, {9})";
            var batchSize = 1000;

            var sqlsToExecute = new List<string>();
            var numberOfBatches = (int)Math.Ceiling((double)persons.Count / batchSize);
            var invariant = System.Globalization.CultureInfo.InvariantCulture;

            for (int i = 0; i < numberOfBatches; i++)
            {
                var userToInsert = persons.Skip(i * batchSize).Take(batchSize);
                var valuesToInsert = userToInsert.Select(log => string.Format(valuesSql,
                    log.BossID,
                    log.EvtcPath,
                    log.JsonPath,
                    log.HtmlPath,
                    log.Link,
                    log.SizeKb,
                    log.TimeStamp,
                    log.DurationMs,
                    log.Flags,
                    log.RemainingHealth.ToString(invariant)
                    ));
                sqlsToExecute.Add(insertSql + string.Join(",", valuesToInsert));
            }

            return sqlsToExecute;
        }

        #endregion
        #region Update

        private static readonly object UpdateLock = new object();
        public static void Update(IDBLog log) => Update(DBConnectionString, log);
        public static void Update(string connectionString, IDBLog log)
        {
            using (IDbConnection cnn = new SQLiteConnection(connectionString))
            {
                var sqlStatement = @"
UPDATE [LogData] 
SET BossID = @BossID
,EvtcPath = @EvtcPath
,JsonPath = @JsonPath
,HtmlPath = @HtmlPath
,Link = @Link
,SizeKb = @SizeKb
,TimeStamp = @TimeStamp
,DurationMs = @DurationMs
,Flags = @Flags
,RemainingHealth = @RemainingHealth
WHERE ID = @ID"; 
                
                lock(UpdateLock)
                    cnn.Execute(sqlStatement, log);
            }
        }

        #endregion
        #region Delete

        public static void Delete(IDBLog log) => Delete(DBConnectionString, log);
        public static void Delete(string connectionString, IDBLog log)
        {
            using (IDbConnection cnn = new SQLiteConnection(connectionString))
            {
                var sqlStatement = "DELETE FROM LogData WHERE ID = @ID";
                cnn.Execute(sqlStatement, log);
            }
        }

        #endregion
    }
}
