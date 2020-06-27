using GamesApi.Models;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GamesApi
{
    public class Services
    {
        public class GameService
        {
            private readonly IMongoCollection<Game> _games;

            public GameService(IGamestoreDatabaseSettings settings)
            {
                var client = new MongoClient(settings.ConnectionString);
                var database = client.GetDatabase(settings.DatabaseName);

                _games = database.GetCollection<Game>(settings.GamesCollectionName);
            }

            public async Task<IEnumerable<Game>> GetAll(string category = null)
            {
                try
                {
                    if (category == null)
                        return await _games.Find(game => true).ToListAsync();
                    else 
                        return await _games.Find(game => true && game.Category.ToLower() == category.ToLower()).ToListAsync();
                }
                catch (Exception ex)
                {
                    // log or manage the exception
                    throw ex;
                }
            }
                   
            public async Task<Game> Get(string id)
            {
                return await _games.Find<Game>(game => game.Id == id).FirstOrDefaultAsync();
            }

            public async Task<Game> Create(Game game)
            {
                game.CreatedOn = DateTime.Now;

                await _games.InsertOneAsync(game);
                return game;
            }

            public void Update(string id, Game gameIn)
            {
                gameIn.UpdatedOn = DateTime.Now;
                _games.ReplaceOne(game => game.Id == id, gameIn);
            }             

            public void Remove(Game gameIn) =>
                _games.DeleteOne(game => game.Id == gameIn.Id);

            public async Task<bool> Remove(string id)
            {
                DeleteResult actionResult = await _games.DeleteOneAsync(game => game.Id == id);

                return actionResult.IsAcknowledged
                && actionResult.DeletedCount > 0;
            }
        }
    }
}
