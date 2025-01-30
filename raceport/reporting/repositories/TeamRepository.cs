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
    public class TeamRepository : IGenericRepository, ITeamRepository
    {
        private readonly String _connection;
        public TeamRepository(string connection) : base(connection)
        {
            _connection = connection;
        }

        async Task<IEnumerable<Team>> SelectAsync(string ulid)
        {
            IEnumerable<Team> result = new List<Team>();
            string SQL = "_dapper_Team_getbyulid";
            using (IDbConnection db = new SqlConnection(_connection))
            {
                var parameters = new DynamicParameters();
                parameters.Add("@Ulid", ulid, DbType.String);
                result = await db.QueryAsync<Team>(SQL, parameters, commandType: CommandType.StoredProcedure);
            }

            return result;
        }


        async Task<IEnumerable<Team>> ITeamRepository.GetAllAsync()
        {
            IEnumerable<Team> result = new List<Team>();
            string SQL = "_dapper_Team_getall";
            using (IDbConnection db = new SqlConnection(_connection))
            {
                var parameters = new DynamicParameters();
                result = await db.QueryAsync<Team>(SQL, parameters, commandType: CommandType.StoredProcedure);
            }

            return result;
        }

        public async Task<Team> FindAsync(string ulid)
        {
            IEnumerable<Team> p = await SelectAsync(ulid);
            return p.FirstOrDefault();
        }

        public async Task<string> AddAsync(Team team)
        {
            string SQL = "_dapper_team_Insert";
            using (IDbConnection db = new SqlConnection(_connection))
            {
                var parameters = new
                {
                    team.Id,
                    team.Name,
                    team.Description,
                    team.dtcreated
                };
                return await db.ExecuteScalarAsync<string>(SQL, parameters, commandType: CommandType.StoredProcedure);
            }
        }

        public async Task<string> UpdateAsync(Team team)
        {
            string SQL = "_dapper_team_Update";
            using (IDbConnection db = new SqlConnection(_connection))
            {
                var parameters = new
                {
                    team.Id,
                    team.Name,
                    team.Description,
                    team.dtlastmodified
                };
                return await db.ExecuteScalarAsync<string>(SQL, parameters, commandType: CommandType.StoredProcedure);
            }
        }


        public IEnumerable<Team> GetAll()
        {
            IEnumerable<Team> result = new List<Team>();
            string SQL = "_dapper_team_getall";
            using (IDbConnection db = new SqlConnection(_connection))
            {
                var parameters = new DynamicParameters();
                result = db.Query<Team>(SQL, parameters, commandType: CommandType.StoredProcedure);
            }

            return result;
        }


        public IEnumerable<Team> FindByCondition(string condition)
        {
            IEnumerable<Team> result = new List<Team>();
            string SQL = "SELECT * FROM Teams ";

            if (condition.Length > 0)
                SQL += " WHERE " + condition;
            using (IDbConnection db = new SqlConnection(_connection))
            {
                var parameters = new DynamicParameters();
                result = db.Query<Team>(SQL, parameters, commandType: CommandType.Text);
            }

            return result;
        }



        public Team FindByName(string name)
        {
            return FindByCondition(
                ConditionHelper.BuildCondition("Name", name, "=", "NVARCHAR")).
                FirstOrDefault();
        }

        public Team Find(string ulid)
        {
            IEnumerable<Team> result = new List<Team>();
            string SQL = "_dapper_team_getbyulid";
            using (IDbConnection db = new SqlConnection(_connection))
            {
                var parameters = new DynamicParameters();
                parameters.Add("@Ulid", ulid, DbType.String);
                result = db.Query<Team>(SQL, parameters, commandType: CommandType.StoredProcedure);
            }

            return result.FirstOrDefault();
        }
    }
}