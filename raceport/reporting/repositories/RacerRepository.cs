using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Data.SqlClient;
using reporting.entities;
using reporting.helpers;


namespace reporting.repositories
{
    public class RacerRepository : IGenericRepository, IRacerRepository
    {
        private readonly String _connection;
        public RacerRepository(string connection) : base(connection)
        {
            _connection = connection;
        }

        async Task<IEnumerable<Racer>> SelectAsync(string ulid)
        {
            IEnumerable<Racer> result = new List<Racer>();
            string SQL = "_dapper_racer_getbyulid";
            using (IDbConnection db = new SqlConnection(_connection))
            {
                var parameters = new DynamicParameters();
                parameters.Add("@Ulid", ulid, DbType.String);
                result = await db.QueryAsync<Racer>(SQL, parameters, commandType: CommandType.StoredProcedure);
            }

            return result;
        }

        public async Task<IEnumerable<Racer>> GetAllAsync()
        {
            IEnumerable<Racer> result = new List<Racer>();
            string SQL = "_dapper_racer_getall";
            using (IDbConnection db = new SqlConnection(_connection))
            {
                var parameters = new DynamicParameters();
                result = await db.QueryAsync<Racer>(SQL, parameters, commandType: CommandType.StoredProcedure);
            }

            return result;
        }

        public async Task<Racer> FindAsync(string ulid)
        {
            IEnumerable<Racer> p = await SelectAsync(ulid);
            return p.FirstOrDefault();
        }

        public IEnumerable<Racer> FindByCondition(string condition) {
            IEnumerable<Racer> result = new List<Racer>();
            string SQL = "SELECT * FROM Racers ";
            
            if (condition.Length> 0) 
                SQL +=" WHERE " + condition;
            using (IDbConnection db = new SqlConnection(_connection))
            {
                var parameters = new DynamicParameters();
                result = db.Query<Racer>(SQL, parameters, commandType: CommandType.Text);
            }

            return result;
        }



        public Racer FindByName(string name)
        {
            return FindByCondition(
                ConditionHelper.BuildCondition("Name", name, "=", "NVARCHAR")).
                FirstOrDefault();
        }

        public Racer FindByNumber(int number)
        {
            return FindByCondition(
                ConditionHelper.BuildCondition("Number", number.ToString(), "=", "INT")).
                FirstOrDefault();
        }

        public IEnumerable<Racer> GetAll()
        {
            IEnumerable<Racer> result = new List<Racer>();
            string SQL = "_dapper_racer_getall";
            using (IDbConnection db = new SqlConnection(_connection))
            {
                var parameters = new DynamicParameters();
                result = db.Query<Racer>(SQL, parameters, commandType: CommandType.StoredProcedure);
            }

            return result;
        }

        public IEnumerable<Racer> Get(string ulid)
        {
            IEnumerable<Racer> result = new List<Racer>();
            string SQL = "_dapper_racer_getbyulid";
            using (IDbConnection db = new SqlConnection(_connection))
            {
                var parameters = new DynamicParameters();
                parameters.Add("@Ulid", ulid, DbType.String);
                result = db.Query<Racer>(SQL, parameters, commandType: CommandType.StoredProcedure);
            }

            return result;
        }

        public Racer Find(string ulid)
        {
            IEnumerable<Racer> result = new List<Racer>();
            string SQL = "_dapper_racer_getbyulid";
            using (IDbConnection db = new SqlConnection(_connection))
            {
                var parameters = new DynamicParameters();
                parameters.Add("@Ulid", ulid, DbType.String);
                result = db.Query<Racer>(SQL, parameters, commandType: CommandType.StoredProcedure);
            }

            return result.FirstOrDefault();
        }

        public async Task<string> AddAsync(Racer racer)
        {
            string SQL = "_dapper_racer_Insert";
            using (IDbConnection db = new SqlConnection(_connection))
            {
                var parameters = new {
                    racer.Id, 
                    racer.Name, 
                    racer.Number, 
                    racer.CarDetails, 
                    racer.RaceClass, 
                    racer.TeamIdOptional, 
                    racer.Notes, 
                    racer.dtcreated};
            return await db.ExecuteScalarAsync<string>(SQL, parameters, commandType: CommandType.StoredProcedure);
            }
        }

        public async Task<string> UpdateAsync(Racer racer)
        {
            string SQL = "_dapper_racer_Update";
            using (IDbConnection db = new SqlConnection(_connection))
            {
                var parameters = new { 
                    racer.Id, 
                    racer.Name, 
                    racer.Number, 
                    racer.CarDetails, 
                    racer.RaceClass, 
                    racer.TeamIdOptional, 
                    racer.Notes, 
                    racer.dtlastmodified };
                return await db.ExecuteScalarAsync<string>(SQL, parameters, commandType: CommandType.StoredProcedure);
            }
        }
    }
}