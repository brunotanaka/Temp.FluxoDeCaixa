using FluxoDeCaixa.Application.Dominio;
using MongoDB.Driver;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FluxoDeCaixa.Application.Repositorio
{
    public interface IRepositorio<TEntity> where TEntity : Entidade
    {
        Task<TEntity> Salvar_Async(TEntity entidade);
        Task<IEnumerable<TEntity>> Buscar_Async(FilterDefinition<TEntity> filtro);
        Task<TEntity> BuscarPor_Async(FilterDefinition<TEntity> filtro);
    }
}
