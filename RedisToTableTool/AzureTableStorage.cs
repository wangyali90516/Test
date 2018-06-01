using System;
using System.Configuration;
using System.Net;
using System.Threading.Tasks;
using Jinyinmao.ActivityCenter.AzureStorage.Interface;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.RetryPolicies;
using Microsoft.WindowsAzure.Storage.Table;

namespace Jinyinmao.ActivityCenter.AzureStorage
{
    /// <summary>
    ///     The IAzureTableStorage Class.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <seealso cref="IAzureTableStorage{T}" />
    public class AzureTableStorage<T> : IAzureTableStorage<T> where T : TableEntity, new()
    {
        /// <summary>
        ///     The cloud table
        /// </summary>
        private readonly CloudTable cloudTable;

        /// <summary>
        ///     Initializes a new instance of the <see cref="AzureTableStorage{T}" /> class.
        /// </summary>
        /// <param name="storageConnectionString">The storage connection string.</param>
        public AzureTableStorage(string storageConnectionString)
        {
            CloudStorageAccount cloudStorageAccount = CloudStorageAccount.Parse(ConfigurationManager.AppSettings["BlobConnectionString"]);
            TableRequestOptions requestOptions = new TableRequestOptions { RetryPolicy = new ExponentialRetry(TimeSpan.FromSeconds(1), 3) };
            CloudTableClient cloudTableClient = cloudStorageAccount.CreateCloudTableClient();
            cloudTableClient.DefaultRequestOptions = requestOptions;

            //禁用Nagle算法
            ServicePointManager.UseNagleAlgorithm = false;

            this.cloudTable = cloudTableClient.GetTableReference(typeof(T).Name);
            this.cloudTable.CreateIfNotExists();
        }

        #region IAzureTableStorage<T> Members

        /// <summary>
        ///     Delete the specified entity.
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public bool Delete(T entity)
        {
            if (entity == null) return false;
            TableResult result = this.cloudTable.Execute(TableOperation.Delete(entity));
            return 300 > result.HttpStatusCode && result.HttpStatusCode >= 200;
        }

        /// <summary>
        ///     Delete the asynchronous entity.
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public async Task<bool> DeleteAsync(T entity)
        {
            if (entity == null) return false;
            TableResult result = await this.cloudTable.ExecuteAsync(TableOperation.Delete(entity));
            return 300 > result.HttpStatusCode && result.HttpStatusCode >= 200;
        }

        /// <summary>
        ///     Inserts the specified entity.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <returns></returns>
        public bool Insert(T entity)
        {
            if (entity == null) return false;
            TableResult result = this.cloudTable.Execute(TableOperation.Insert(entity));
            return 300 > result.HttpStatusCode && result.HttpStatusCode >= 200;
        }

        /// <summary>
        ///     Inserts the asynchronous.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <returns></returns>
        public async Task<bool> InsertAsync(T entity)
        {
            if (entity == null) return false;
            TableResult result = await this.cloudTable.ExecuteAsync(TableOperation.Insert(entity));
            return 300 > result.HttpStatusCode && result.HttpStatusCode >= 200;
        }

        /// <summary>
        ///     Inserts the or update.
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public bool InsertOrUpdate(T entity)
        {
            if (entity == null) return false;
            TableResult result = this.cloudTable.Execute(TableOperation.InsertOrMerge(entity));
            return 300 > result.HttpStatusCode && result.HttpStatusCode >= 200;
        }

        /// <summary>
        ///     Inserts the or update asynchronous.
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public async Task<bool> InsertOrUpdateAsync(T entity)
        {
            if (entity == null) return false;
            TableResult result = await this.cloudTable.ExecuteAsync(TableOperation.InsertOrMerge(entity));
            return 300 > result.HttpStatusCode && result.HttpStatusCode >= 200;
        }

        /// <summary>
        ///     Inserts the or update.
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public bool Merge(T entity)
        {
            if (entity == null) return false;
            TableResult result = this.cloudTable.Execute(TableOperation.Merge(entity));
            return 300 > result.HttpStatusCode && result.HttpStatusCode >= 200;
        }

        /// <summary>
        ///     Inserts the or update asynchronous.
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public async Task<bool> MergeAsync(T entity)
        {
            if (entity == null) return false;
            TableResult result = await this.cloudTable.ExecuteAsync(TableOperation.Merge(entity));
            return 300 > result.HttpStatusCode && result.HttpStatusCode >= 200;
        }

        /// <summary>
        ///     Queries the specified partition key.
        /// </summary>
        /// <param name="partitionKey">The partition key.</param>
        /// <param name="rowKey">The row key.</param>
        /// <returns></returns>
        /// <exception cref="System.NotImplementedException"></exception>
        public T Query(string partitionKey, string rowKey)
        {
            TableOperation retrieveOperation = TableOperation.Retrieve<T>(partitionKey, rowKey);
            TableResult retrievedResult = this.cloudTable.Execute(retrieveOperation);
            return retrievedResult.Result as T;
        }

        /// <summary>
        ///     Queries the asynchronous.
        /// </summary>
        /// <param name="partitionKey">The partition key.</param>
        /// <param name="rowKey">The row key.</param>
        /// <returns></returns>
        /// <exception cref="System.NotImplementedException"></exception>
        public async Task<T> QueryAsync(string partitionKey, string rowKey)
        {
            TableOperation retrieveOperation = TableOperation.Retrieve<T>(partitionKey, rowKey);
            TableResult retrievedResult = await this.cloudTable.ExecuteAsync(retrieveOperation);
            return retrievedResult.Result as T;
        }

        /// <summary>
        ///     Inserts the or update.
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public bool Update(T entity)
        {
            if (entity == null) return false;
            TableResult result = this.cloudTable.Execute(TableOperation.Replace(entity));
            return 300 > result.HttpStatusCode && result.HttpStatusCode >= 200;
        }

        /// <summary>
        ///     Inserts the or update asynchronous.
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public async Task<bool> UpdateAsync(T entity)
        {
            if (entity == null) return false;
            TableResult result = await this.cloudTable.ExecuteAsync(TableOperation.Replace(entity));
            return 300 > result.HttpStatusCode && result.HttpStatusCode >= 200;
        }

        #endregion IAzureTableStorage<T> Members
    }
}