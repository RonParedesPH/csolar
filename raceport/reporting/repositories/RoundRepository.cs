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
    public class RoundRepository : IGenericRepository, IRoundRepository
    {
        private readonly String _connection;
        public RoundRepository(string connection) : base(connection)
        {
            _connection = connection;
        }

        

        async Task<IEnumerable<Round>> SelectAsync(string ulid)
        {
            IEnumerable<Round> result = new List<Round>();
            string SQL = "_dapper_Round_getbyulid";
            using (IDbConnection db = new SqlConnection(_connection))
            {
                var parameters = new DynamicParameters();
                parameters.Add("@Ulid", ulid, DbType.String);
                result = await db.QueryAsync<Round>(SQL, parameters, commandType: CommandType.StoredProcedure);
            }

            return result;
        }


        async Task<IEnumerable<Round>> IRoundRepository.GetAllAsync()
        {
            IEnumerable<Round> result = new List<Round>();
            string SQL = "_dapper_Round_getall";
            using (IDbConnection db = new SqlConnection(_connection))
            {
                var parameters = new DynamicParameters();
                result = await db.QueryAsync<Round>(SQL, parameters, commandType: CommandType.StoredProcedure);
            }

            return result;
        }

        

            public async Task<Round> FindAsync(string ulid)
        {
            IEnumerable<Round> p = await SelectAsync(ulid);
            return p.FirstOrDefault();
        }

        public async Task<string> AddAsync(Round Round)
        {
            string SQL = "_dapper_Round_Insert";
            using (IDbConnection db = new SqlConnection(_connection))
            {
                var parameters = new
                {
                    Round.Id,
                    Round.Name,
                    Round.Description,
                    Round.EventDate,
                    Round.Venue,
                    Round.SeasonId,
                    Round.dtcreated
                };
                return await db.ExecuteScalarAsync<string>(SQL, parameters, commandType: CommandType.StoredProcedure);
            }
        }

        public async Task<string> UpdateAsync(Round Round)
        {
            string SQL = "_dapper_Round_Update";
            using (IDbConnection db = new SqlConnection(_connection))
            {
                var parameters = new
                {
                    Round.Id,
                    Round.Name,
                    Round.Description,
                    Round.EventDate,
                    Round.Venue,
                    Round.SeasonId,
                    Round.dtlastmodified
                };
                return await db.ExecuteScalarAsync<string>(SQL, parameters, commandType: CommandType.StoredProcedure);
            }
        }


        public IEnumerable<Round> GetAll()
        {
            IEnumerable<Round> result = new List<Round>();
            string SQL = "_dapper_Round_getall";
            using (IDbConnection db = new SqlConnection(_connection))
            {
                var parameters = new DynamicParameters();
                result = db.Query<Round>(SQL, parameters, commandType: CommandType.StoredProcedure);
            }

            return result;
        }


        public IEnumerable<Round> FindByCondition(string condition)
        {
            IEnumerable<Round> result = new List<Round>();
            string SQL = "SELECT * FROM Rounds ";

            if (condition.Length > 0)
                SQL += " WHERE " + condition;
            using (IDbConnection db = new SqlConnection(_connection))
            {
                var parameters = new DynamicParameters();
                result = db.Query<Round>(SQL, parameters, commandType: CommandType.Text);
            }

            return result;
        }



        public Round FindByName(string name)
        {
            return FindByCondition(
                ConditionHelper.BuildCondition("Name", name, "=", "NVARCHAR")).
                FirstOrDefault();
        }

        public Round Find(string ulid)
        {
            IEnumerable<Round> result = new List<Round>();
            string SQL = "_dapper_Round_getbyulid";
            using (IDbConnection db = new SqlConnection(_connection))
            {
                var parameters = new DynamicParameters();
                parameters.Add("@Ulid", ulid, DbType.String);
                result = db.Query<Round>(SQL, parameters, commandType: CommandType.StoredProcedure);
            }

            return result.FirstOrDefault();
        }
    }
}