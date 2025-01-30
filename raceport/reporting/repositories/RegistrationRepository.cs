using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using reporting.entities;
using System.Data.SqlClient;
using reporting.helpers;

namespace reporting.repositories
{
    public class RegistrationRepository : IGenericRepository, IRegistrationRepository
    {
        private readonly String _connection;
        public RegistrationRepository(string connection) : base(connection)
        {
            _connection = connection;
        }

        private string buildQuery(string optionalFilter = null)
        {
            // de-nullify parameter;
            optionalFilter = (optionalFilter == null ? "" : "AND " + optionalFilter);
            SQLSelect SQL = new SQLSelect
            {
                Table = "Registrations a",
                Fields = "*",
                Where = "a.Id IS NOT NULL " + optionalFilter
            };
            string command = SQL.CommandText();

            string whereClause = "b.Id IN (" + SQL.CommandText(fields: "a.RacerId ") + ")";
            string whereClause2 = "c.Id IN (" + SQL.CommandText(fields: "a.RoundId ") + ")";
            command += ";" + SQL.CommandText(
                table: "Racers b",
                fields: "*",
                where: whereClause + " AND b.ID IS NOT NULL"
                );



            command += ";" + SQL.CommandText(
                table: "Rounds c",
                fields: "*",
                where: whereClause2 + " AND c.ID IS NOT NULL"
                );

            return command;
        }
        private List<Registration> GetManyMultiple(string query)
        {
            using (IDbConnection db = new SqlConnection(_connection))
            {
                using (var multi = db.QueryMultiple(query))
                {
                    var rows = multi.Read<Registration>().ToList();
                    var racers = multi.Read<Racer>().ToList();
                    var rounds = multi.Read<Round>().ToList();

                    foreach (Registration row in rows)
                    {
                        row.Round = rounds.Where(c => c.Id == row.RoundId).FirstOrDefault();
                        row.Participant = racers.Where(c => c.Id == row.RacerId).FirstOrDefault();
                    }

                    return rows;
                }
            }
        }
        async Task<IEnumerable<Registration>> SelectAsync(string ulid)
        {
            IEnumerable<Registration> result = new List<Registration>();
            string SQL = "_dapper_Registration_getbyulid";
            using (IDbConnection db = new SqlConnection(_connection))
            {
                var parameters = new DynamicParameters();
                parameters.Add("@Ulid", ulid, DbType.String);
                result = await db.QueryAsync<Registration>(SQL, parameters, commandType: CommandType.StoredProcedure);
            }

            return result;
        }


        async Task<IEnumerable<Registration>> IRegistrationRepository.GetAllAsync()
        {
            IEnumerable<Registration> result = new List<Registration>();
            string SQL = "_dapper_Registration_getall";
            using (IDbConnection db = new SqlConnection(_connection))
            {
                var parameters = new DynamicParameters();
                result = await db.QueryAsync<Registration>(SQL, parameters, commandType: CommandType.StoredProcedure);
            }

            return result;
        }

        public IEnumerable<Registration> GetItemsByRoundId(string id)
        {
            using (IDbConnection db = new SqlConnection(_connection))
            {
                var query = buildQuery(string.Format("a.RoundId = '{0}'", id));

                var list = GetManyMultiple(query);
                return list;
            }
        }


        public async Task<Registration> FindAsync(string ulid)
        {
            IEnumerable<Registration> p = await SelectAsync(ulid);
            return p.FirstOrDefault();
        }

        public async Task<string> AddAsync(Registration Registration)
        {
            string SQL = "_dapper_Registration_Insert";
            using (IDbConnection db = new SqlConnection(_connection))
            {
                var parameters = new
                {
                    Registration.Id,
                    Registration.RaceClass,
                    Registration.RoundId,
                    Registration.RacerId,
                    Registration.dtcreated
                };
                return await db.ExecuteScalarAsync<string>(SQL, parameters, commandType: CommandType.StoredProcedure);
            }
        }

        public async Task<string> UpdateAsync(Registration Registration)
        {
            string SQL = "_dapper_Registration_Update";
            using (IDbConnection db = new SqlConnection(_connection))
            {
                var parameters = new
                {
                    Registration.Id,
                    Registration.RaceClass,
                    Registration.RoundId,
                    Registration.RacerId,
                    Registration.dtlastmodified
                };
                return await db.ExecuteScalarAsync<string>(SQL, parameters, commandType: CommandType.StoredProcedure);
            }
        }


        public IEnumerable<Registration> GetAll()
        {
            IEnumerable<Registration> result = new List<Registration>();
            string SQL = "_dapper_Registration_getall";
            using (IDbConnection db = new SqlConnection(_connection))
            {
                var parameters = new DynamicParameters();
                result = db.Query<Registration>(SQL, parameters, commandType: CommandType.StoredProcedure);
            }

            return result;
        }


        public IEnumerable<Registration> FindByCondition(string condition)
        {
            IEnumerable<Registration> result = new List<Registration>();
            string SQL = "SELECT * FROM Registrations ";

            if (condition.Length > 0)
                SQL += " WHERE " + condition;
            using (IDbConnection db = new SqlConnection(_connection))
            {
                var parameters = new DynamicParameters();
                result = db.Query<Registration>(SQL, parameters, commandType: CommandType.Text);
            }

            return result;
        }



        public Registration FindByName(string name)
        {
            return FindByCondition(
                ConditionHelper.BuildCondition("Name", name, "=", "NVARCHAR")).
                FirstOrDefault();
        }

        public Registration Find(string ulid)
        {
            IEnumerable<Registration> result = new List<Registration>();
            string SQL = "_dapper_Registration_getbyulid";
            using (IDbConnection db = new SqlConnection(_connection))
            {
                var parameters = new DynamicParameters();
                parameters.Add("@Ulid", ulid, DbType.String);
                result = db.Query<Registration>(SQL, parameters, commandType: CommandType.StoredProcedure);
            }

            return result.FirstOrDefault();
        }
    }
}