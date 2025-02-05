﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Dapper;
using Microsoft.Data.Sqlite;

namespace CanWeFixItService
{
    public class DatabaseService : IDatabaseService
    {
        // See SQLite In-Memory example:
        // https://github.com/dotnet/docs/blob/main/samples/snippets/standard/data/sqlite/InMemorySample/Program.cs

        // Using a name and a shared cache allows multiple connections to access the same
        // in-memory database
        const string connectionString = "Data Source=DatabaseService;Mode=Memory;Cache=Shared";
        private SqliteConnection _connection;

        public DatabaseService(string connectionString)
        {
            _connection = new SqliteConnection(connectionString);
            _connection.Open();
        }

        public DatabaseService()
        {
            // The in-memory database only persists while a connection is open to it. To manage
            // its lifetime, keep one open connection around for as long as you need it.
            _connection = new SqliteConnection(connectionString);
            _connection.Open();
        }

        public async Task<IEnumerable<Instrument>> Instruments()
        {
            return await RunQueryAsync<Instrument>("SELECT * FROM instrument WHERE Active = 1");
        }

        public async Task<IEnumerable<MarketData>> MarketData()
        {
            return await RunQueryAsync<MarketData>("SELECT\r\nm.id,\r\nm.dataValue,\r\ni.id AS InstrumentId,\r\nm.active\r\nFROM marketdata m\r\nJOIN instrument i\r\nON m.sedol = i.sedol\r\nWHERE m.active = 1");
        }

        public async Task<IEnumerable<MarketValuation>> MarketValuation()
        {
            return await RunQueryAsync<MarketValuation>("SELECT\r\n'DataValueTotal' AS 'name',\r\nSUM(m.datavalue) AS total\r\nFROM marketdata m\r\nWHERE m.active = 1");
        }

        private async Task<IEnumerable<T>> RunQueryAsync<T>(string sqlQuery)
        {
            try
            {
                return await _connection.QueryAsync<T>(sqlQuery);
            }
            catch (SqliteException sqliteException)
            {
                Console.WriteLine("SQL Exception occurred.");
            }
            catch (Exception exception)
            {
                Console.WriteLine("Log exception and deal with it.");
            }

            return new List<T>();
        }

        /// <summary>
        /// This is complete and will correctly load the test data.
        /// It is called during app startup 
        /// </summary>
        public void SetupDatabase()
        {
            const string createInstruments = @"
                CREATE TABLE instrument
                (
                    id     int,
                    sedol  text,
                    name   text,
                    active int
                );
                INSERT INTO instrument
                VALUES (1, 'Sedol1', 'Name1', 0),
                       (2, 'Sedol2', 'Name2', 1),
                       (3, 'Sedol3', 'Name3', 0),
                       (4, 'Sedol4', 'Name4', 1),
                       (5, 'Sedol5', 'Name5', 0),
                       (6, '', 'Name6', 1),
                       (7, 'Sedol7', 'Name7', 0),
                       (8, 'Sedol8', 'Name8', 1),
                       (9, 'Sedol9', 'Name9', 0)";

            _connection.Execute(createInstruments);

            const string createMarketData = @"
                CREATE TABLE marketdata
                (
                    id        int,
                    datavalue int,
                    sedol     text,
                    active    int
                );
                INSERT INTO marketdata
                VALUES (1, 1111, 'Sedol1', 0),
                       (2, 2222, 'Sedol2', 1),
                       (3, 3333, 'Sedol3', 0),
                       (4, 4444, 'Sedol4', 1),
                       (5, 5555, 'Sedol5', 0),
                       (6, 6666, 'Sedol6', 1)";

            _connection.Execute(createMarketData);
        }
    }
}