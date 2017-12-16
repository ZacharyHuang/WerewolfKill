using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace WerewolfKill.Utils.AzureStorage
{
    public class AzureBlobContainer
    {
        private CloudBlobContainer m_container;

        public AzureBlobContainer(CloudBlobContainer container)
        {
            m_container = container;
        }

        public string EndpointUrl { get { return m_container.StorageUri.PrimaryUri.AbsoluteUri; } }
        
        public async Task UploadBlob(string blobName, byte[] bytes)
        {
            await m_container
                .GetBlockBlobReference(blobName)
                .UploadFromByteArrayAsync(bytes, 0, bytes.Length);
        }
    }
}