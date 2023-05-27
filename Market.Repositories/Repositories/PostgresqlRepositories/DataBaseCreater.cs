using Dapper;
using Market.Entities.Configs;
using Market.Repositories.Interfaces;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Market.Repositories.Repositories.PostgresqlRepositories
{
    public class DataBaseCreater
    {
        /// <summary>
        /// запросы на создания таблиц, Базы данных и тд.
        /// </summary>
        private List<string> _commandsCreate;
        bool _dataBaseExist = false;
        public NpgsqlConnection Connection { get; private set; }
        private string _databaseName = "Market2";
        private LoggerLib.Interfaces.ILogger _logger;

        private List<ITableCreater> _creaters = new List<ITableCreater>();

        public DataBaseCreater(Configs config, LoggerLib.Interfaces.ILogger logger)
        {
            _logger = logger;
            _commandsCreate = new List<string>();
            Console.WriteLine($"Host={config.DataBaseConfig.Host}:{config.DataBaseConfig.Port}; Username={config.DataBaseConfig.Username};Password={config.DataBaseConfig.Password}");
            Connection = new NpgsqlConnection($"Host={config.DataBaseConfig.Host}:{config.DataBaseConfig.Port}; Username={config.DataBaseConfig.Username};Password={config.DataBaseConfig.Password}");

            Connection.Open();
            CreateDataBase().GetAwaiter().GetResult();
            Connection.Close();
            Connection.Dispose();

            //Connection = new NpgsqlConnection($"Host=192.168.133.128;Port=5432;Database = Market; Username=postgres;Password=123qwe45asd");
            Connection = new NpgsqlConnection($"Host={config.DataBaseConfig.Host}:{config.DataBaseConfig.Port};Database = {_databaseName}; Username={config.DataBaseConfig.Username};Password={config.DataBaseConfig.Password}");



            ////категории товаров
            _creaters.Add(new CategoriesRepository.TableCreater(Connection));
            //подкатегории 
            _creaters.Add(new SubcategoryRepository.TableCreater(Connection));
            //            ///таблица с товарами
            _creaters.Add(new ProductsRepository.TableCreater(Connection));
            /// таблица с типами характеристик конкретного товара для телефона например
            /// "Сотовая связь","Камера"
            _creaters.Add(new TypeСharacteristicsRepository.TableCreater(Connection));
            //// характеристики  свойственные для конкретного типа товара
            ///например  для телефона есть тип характеристики "Сотовая связь" и в этой таблице будет содержаться 
            ///то что к ней относится: "Количество физических SIM-карт", "Стандарт связи" и тд
            _creaters.Add(new СharacteristicsRepository.TableCreater(Connection));
            ///// таблица с пользователями
            _creaters.Add(new UsersRepository.TableCreater(Connection));
            //            /// комментарии
            _creaters.Add(new CommentsRepository.TableCreater(Connection));

            
            _creaters.Add(new CommentsLikesRepository.TableCreater(Connection));
        }


        /// <summary>
        /// создание таблиц
        /// </summary>
        public void Create()
        {
            //if (_dataBaseExist) return;
            foreach (var t in _creaters)
            {
                try
                {
                    //Connection.Execute(t);
                    t.Create();
                }
                catch (Exception ex) { }
            }

        }

        public async Task CreateDataBase()
        {
            try
            {
                Console.WriteLine($"Попытка создания БД");
                _logger.Info("Создание базы данных");
                string sql = @"CREATE DATABASE " + $"\"{_databaseName}\"";
                    


                Connection.Execute(sql);
                _logger.Info("Добавление тестовых данных");



            }
            catch (Exception ex)
            {
                _logger.Error($"Ошибка при создании БД  {ex.Message}");
            }

        }


    }

}

