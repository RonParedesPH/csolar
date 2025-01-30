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
    public class RaceClassRepository : IGenericRepository, IRaceClassRepository
    {
        private readonly String _connection;
        public RaceClassRepository(string connection) : base(connection)
        {
            _connection = connection;
        }


        async Task<IEnumerable<RaceClass>> SelectAsync(string ulid)
        {
            IEnumerable<RaceClass> result = new List<RaceClass>();
            string SQL = "_dapper_class_getbyulid";
            using (IDbConnection db = new SqlConnection(_connection))
            {
                var parameters = new DynamicParameters();
                parameters.Add("@Ulid", ulid, DbType.String);
                result = await db.QueryAsync<RaceClass>(SQL, parameters, commandType: CommandType.StoredProcedure);
            }

            return result;
        }


        async Task<IEnumerable<RaceClass>> IRaceClassRepository.GetAllAsync()
        {
            IEnumerable<RaceClass> result = new List<RaceClass>();
            string SQL = "_dapper_class_getall";
            using (IDbConnection db = new SqlConnection(_connection))
            {
                var parameters = new DynamicParameters();
                result = await db.QueryAsync<RaceClass>(SQL, parameters, commandType: CommandType.StoredProcedure);
            }

            return result;
        }

        public async Task<RaceClass> FindAsync(string ulid)
        {
            IEnumerable<RaceClass> p = await SelectAsync(ulid);
            return p.FirstOrDefault();
        }

        public async Task<string> AddAsync(RaceClass raceClass)
        {
            string SQL = "_dapper_class_Insert";
            using (IDbConnection db = new SqlConnection(_connection))
            {
                var parameters = new
                {
                    raceClass.Id,
                    raceClass.Code,
                    raceClass.Description,
                    raceClass.dtcreated
                };
                return await db.ExecuteScalarAsync<string>(SQL, parameters, commandType: CommandType.StoredProcedure);
            }
        }

        public async Task<string> UpdateAsync(RaceClass raceClass)
        {
            string SQL = "_dapper_Class_Update";
            using (IDbConnection db = new SqlConnection(_connection))
            {
                var parameters = new
                {
                    raceClass.Id,
                    raceClass.Code,
                    raceClass.Description,
                    raceClass.dtlastmodified
                };
                return await db.ExecuteScalarAsync<string>(SQL, parameters, commandType: CommandType.StoredProcedure);
            }
        }

        public IEnumerable<RaceClass> GetAll()
        {
            IEnumerable<RaceClass> result = new List<RaceClass>();
            string SQL = "_dapper_class_getall";
            using (IDbConnection db = new SqlConnection(_connection))
            {
                var parameters = new DynamicParameters();
                result = db.Query<RaceClass>(SQL, parameters, commandType: CommandType.StoredProcedure);
            }

            return result;
        }


        public IEnumerable<RaceClass> FindByCondition(string condition)
        {
            IEnumerable<RaceClass> result = new List<RaceClass>();
            string SQL = "SELECT * FROM Class ";

            if (condition.Length > 0)
                SQL += " WHERE " + condition;
            using (IDbConnection db = new SqlConnection(_connection))
            {
                var parameters = new DynamicParameters();
                result = db.Query<RaceClass>(SQL, parameters, commandType: CommandType.Text);
            }

            return result;
        }



        public RaceClass FindByCode(string code)
        {
            return FindByCondition(
                ConditionHelper.BuildCondition("Code", code, "=", "NVARCHAR")).
                FirstOrDefault();
        }

        public RaceClass Find(string ulid)
        {
            IEnumerable<RaceClass> result = new List<RaceClass>();
            string SQL = "_dapper_Class_getbyulid";
            using (IDbConnection db = new SqlConnection(_connection))
            {
                var parameters = new DynamicParameters();
                parameters.Add("@Ulid", ulid, DbType.String);
                result = db.Query<RaceClass>(SQL, parameters, commandType: CommandType.StoredProcedure);
            }

            return result.FirstOrDefault();
        }
    }
}