# NewsBus
NewsBus is a news aggregation portal, similar to https://feedly.com/.
It is a sample project that illustrates how to achive a scallable architecture by using the following technologies:
* Azure Service Bus
* Azure Cosmos DB
* Azure Blob Storage
* ASP.NET Core
* Docker
* [To be done] Azure Kubernetes Service
### Projects description
* NewsBus.Domain — domain layer
* NewsBus.Application — business layer
* NewsBus.Infrastructure — infrastructure layer
* NewsBus.WatcherService — a service that monitors rss feeds and when it finds new content it sends the links to the download queue
* NewsBus.DownloaderService — a service that takes links from the download queue, downloads them, parses them and then stores its content/metadata to a blob storage/cosmos db.
* [To be done] NewsBus.Portal — provides Web UI interface for the end users and exposes Web API to the Internet
