using logger;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using reporting.repositories;
using reporting.helpers;

namespace reporting.unitofwork
{
    // ctrl + . ile implement ediyoruz.
    // interface 'ini implement ettik.
    public class UnitOfWork : IUnitOfWork
    {
        private IDbConnection _connection;
        private IDbTransaction _transaction;
        private string _connectionString;

        //private IItemCategoryRepository _itemCategories;
        //private IItemRepository _items;

        private IRacerRepository _racers;
        private IRaceClassRepository _raceClass;
        private ITeamRepository _team;
        private IRoundRepository _round;
        private IRegistrationRepository _registration;


        private bool _dispose;

        public IRacerRepository Racers
        {
            get { return _racers ?? (_racers = new RacerRepository(_connectionString)); }
        }

        public IRaceClassRepository RaceClass
        {
            get { return _raceClass ?? (_raceClass = new RaceClassRepository(_connectionString)); }
        }

        public ITeamRepository Teams
        {
            get { return _team ?? (_team = new TeamRepository(_connectionString)); }
        }
        public IRoundRepository Rounds
        {
            get { return _round ?? (_round = new RoundRepository(_connectionString)); }
        }
        public IRegistrationRepository Registrations
        {
            get { return _registration ?? (_registration = new RegistrationRepository(_connectionString));  }
        }

        // ctor + tab tab
        public UnitOfWork(string connectionString)
        {
            _connectionString = connectionString;
            // SqlConnection için ctrl + . yapıp gerekli kütüphaneyi indirmeniz gerekiyor. =>  using System.Data.SqlClient;
            //_connection = new SqlConnection(connectionString);
            //_connection.Open();

        }

        public bool BeginTransaction()
        {
            _transaction = _connection.BeginTransaction();
            return true;
       }


        public ResultOfAction Commit()
        {
            ResultOfAction result = new ResultOfAction();
            try
            {
                // burda hata yaptık commit yapması gerekirken sürekli hataya düşecek burası yorgunluk böyle birşey :)))  hiçbir türlü commit olmaz düzeltiyoruz :)))

                _transaction.Commit();  // olması gereken bu 
                result.Success = true;
            }
            catch (Exception ex)
            {
                result.Errors.Add(ExceptionHelper.RollUp(ex));
                log.LogMessage(log.TracingLevel.ERROR, ExceptionHelper.Verbose(ex));
                // yapılanları geri al 
                _transaction.Rollback();
                //throw;
            }
            finally
            {
                // her ne olursa olsun bu işlemleri yap anlamında => ister try a girsib ister catch'e 
                _transaction.Dispose();
                _transaction = _connection.BeginTransaction();
                resetRepositories();
            }
            return result;
        }

        private void resetRepositories()
        {
            // repositoryleri temizliyoruz.
            _racers = null;
        }

        public void Dispose()
        {
            dispose(true);
            GC.SuppressFinalize(this);
        }

        // burada çok fazla if kullandık ilerleyen süreçlerde buralarında üzerinden geçmeliyiz. ne kadar az if if o kadar iyi :) s
        private void dispose(bool disposing)
        {
            if (!_dispose)
            {
                if (disposing)
                {
                    if (_transaction != null)
                    {
                        _transaction.Dispose();
                        _transaction = null;
                    }
                    if (_connection != null)
                    {
                        _connection.Dispose();
                        _connection = null;
                    }
                }
                _dispose = true;
            }
        }
        ~UnitOfWork()
        {
            dispose(false);
        }


    }
}
