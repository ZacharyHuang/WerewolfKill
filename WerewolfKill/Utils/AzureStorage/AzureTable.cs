using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;
using System.Threading.Tasks;

namespace WerewolfKill.Utils.AzureStorage
{
    public class AzureTable
    {
        private CloudTable m_table;
        public AzureTable(CloudTable table)
        {
            m_table = table;
        }

        public async Task CreateAsync<T>(T entity) where T : TableEntity
        {
            var res = await m_table.ExecuteAsync(TableOperation.Insert(entity));
            if (res.HttpStatusCode == 409) throw new EntityAlreadyExists();
        }

        public async Task UpdateAsync<T>(T entity) where T : TableEntity
        {
            var res = await m_table.ExecuteAsync(TableOperation.InsertOrReplace(entity));
        }

        public async Task UpdatePatialAsync<T>(T entity) where T : TableEntity
        {
            var res = await m_table.ExecuteAsync(TableOperation.InsertOrMerge(entity));
        }

        public async Task DeleteAsync<T>(T entity) where T : TableEntity
        {
            entity.ETag = "*";
            var res = await m_table.ExecuteAsync(TableOperation.Delete(entity));
        }
        
        public async Task<T> FindAsync<T>(string partitionKey, string rowKey) where T : TableEntity
        {
            var res = await m_table.ExecuteAsync(TableOperation.Retrieve<T>(partitionKey, rowKey));
            return res.Result as T;
        }
        public async Task<IList<T>> FindByPartitionKeyAsync<T>(string partitionKey) where T : TableEntity, new()
        {
            string tableQuery = TableQuery.GenerateFilterCondition("PartitionKey", QueryComparisons.Equal, partitionKey);
            return await QueryAsync<T>(tableQuery);
        }
        public async Task<IList<T>> QueryAsync<T>(string tableQuery) where T : TableEntity, new()
        {
            List<T> res = new List<T>();
            TableQuerySegment<T> querySegment = null;

            while (querySegment == null || querySegment.ContinuationToken != null)
            {
                querySegment = await m_table.ExecuteQuerySegmentedAsync(
                    new TableQuery<T>().Where(tableQuery),
                    querySegment != null ? querySegment.ContinuationToken : null
                    );
                res.AddRange(querySegment.Results);
            }

            return res;
        }
    }
}