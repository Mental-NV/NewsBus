using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using NewsBus.Domain.Models;

namespace NewsBus.Domain
{
    public interface IDownloadQueueSender : IAsyncDisposable
    {
        Task SendAsync(MetaArticle article);
    }
}