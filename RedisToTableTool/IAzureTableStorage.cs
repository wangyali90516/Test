using System.Threading.Tasks;
using Microsoft.WindowsAzure.Storage.Table;

namespace Jinyinmao.ActivityCenter.AzureStorage.Interface
{
    /// <summary>
    ///     The Interface IAzureTableStorage Class.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IAzureTableStorage<T> where T : TableEntity, new()
    {
        /// <summary>
        ///     Delete the specified entity.
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        bool Delete(T entity);

        /// <summary>
        ///     Delete the asynchronous entity.
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        Task<bool> DeleteAsync(T entity);

        /// <summary>
        ///     Inserts the specified entity.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <returns></returns>
        bool Insert(T entity);

        /// <summary>
        ///     Inserts the asynchronous.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <returns></returns>
        Task<bool> InsertAsync(T entity);

        /// <summary>
        ///     Inserts the or update.
        /// </summary>
        /// <returns></returns>
        bool InsertOrUpdate(T entity);

        /// <summary>
        ///     Inserts the or update asynchronous.
        /// </summary>
        /// <returns></returns>
        Task<bool> InsertOrUpdateAsync(T entity);

        /// <summary>
        ///     Inserts the or update.
        /// </summary>
        /// <returns></returns>
        bool Merge(T entity);

        /// <summary>
        ///     Inserts the or update asynchronous.
        /// </summary>
        /// <returns></returns>
        Task<bool> MergeAsync(T entity);

        /// <summary>
        ///     Queries the specified partition key.
        /// </summary>
        /// <param name="partitionKey">The partition key.</param>
        /// <param name="rowKey">The row key.</param>
        /// <returns></returns>
        T Query(string partitionKey, string rowKey);

        /// <summary>
        ///     Queries the asynchronous.
        /// </summary>
        /// <param name="partitionKey">The partition key.</param>
        /// <param name="rowKey">The row key.</param>
        /// <returns></returns>
        Task<T> QueryAsync(string partitionKey, string rowKey);

        /// <summary>
        ///     Inserts the or update.
        /// </summary>
        /// <returns></returns>
        bool Update(T entity);

        /// <summary>
        ///     Inserts the or update asynchronous.
        /// </summary>
        /// <returns></returns>
        Task<bool> UpdateAsync(T entity);
    }
}