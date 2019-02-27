using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using FluxoDeCaixa.Application.Dominio;
using MongoDB.Driver;

namespace FluxoDeCaixa.Application.Repositorio
{
    public class Repositorio<TEntity> : IRepositorio<TEntity> where TEntity : Entidade
    {
        public IMongoDatabase database;

        public Repositorio(IMongoDatabase db)
        {
            database = db;

            var consolidado = database.GetCollection<ConsolidadoFluxo>("ConsolidadoFluxo");
            bool existsIndex = false;

            var consolidadoIndexes = consolidado.Indexes.List();
            while (consolidadoIndexes.MoveNext())
            {
                var currentIndex = consolidadoIndexes.Current;
                foreach (var doc in currentIndex)
                {
                    var docNames = doc.Names;
                    foreach (string name in docNames)
                    {
                        var value = doc.GetValue(name);
                        if (value.ToString().Contains("Data"))
                            existsIndex = true;
                    }
                }
            }

            if (!existsIndex)
            {
                var lancamento = database.GetCollection<LancamentoFinanceiro>("LancamentoFinanceiro");
                consolidado.Indexes.CreateOne(Builders<ConsolidadoFluxo>.IndexKeys.Ascending(x => x.Data));
                lancamento.Indexes.CreateOne(Builders<LancamentoFinanceiro>.IndexKeys.Text(x => x.Data));
            }
        }

        public async Task<TEntity> Salvar_Async(TEntity entidade)
        {
            var collection = database.GetCollection<TEntity>(typeof(TEntity).Name);

            await collection.ReplaceOneAsync(x => x.Id == entidade.Id, entidade, new UpdateOptions
            {
                IsUpsert = true,
            });

            return entidade;
        }

        public async Task<IEnumerable<TEntity>> Buscar_Async(FilterDefinition<TEntity> filtro) => await database.GetCollection<TEntity>(typeof(TEntity).Name).Find(filtro).ToListAsync();

        public async Task<TEntity> BuscarPor_Async(FilterDefinition<TEntity> filtro) => await database.GetCollection<TEntity>(typeof(TEntity).Name).Find(filtro).FirstOrDefaultAsync();

    }
}
