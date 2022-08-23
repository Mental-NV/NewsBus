using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.ServiceModel.Syndication;
using System.Threading.Tasks;
using System.Xml;
using NewsBus.Application.Interfaces;
using NewsBus.Domain.Models;
using NewsBus.Application.Validators;

namespace NewsBus.Infrastructure
{
    public class RssLoader : IRssLoader
    {
        private readonly IArticleIdGenerator idGenerator;

        public RssLoader(IArticleIdGenerator idGenerator)
        {
            this.idGenerator = idGenerator ?? throw new ArgumentNullException(nameof(idGenerator));
        }

        public Task<IEnumerable<Article>> LoadAsync(Uri rssFeedUrl)
        {
            if (rssFeedUrl is null)
            {
                throw new ArgumentNullException(nameof(rssFeedUrl));
            }

            List<Article> result = new List<Article>();
            ArticleValidator validator = new ArticleValidator();

            using XmlReader reader = XmlReader.Create(rssFeedUrl.ToString());
            SyndicationFeed feed = SyndicationFeed.Load(reader);

            foreach (SyndicationItem item in feed.Items)
            {
                Uri url = ArticleValidator.ValidateUrl(item.Id) ?? item.Links?.FirstOrDefault()?.Uri;
                string id = idGenerator.Convert(url.ToString()).ToString();
                var article = new Article() 
                { 
                    Id = id, 
                    Url = url, 
                    Title = item.Title?.Text,
                    PublishDate = item.PublishDate,
                    Description = item.Summary?.Text
                };
                var validationResult = validator.Validate(article);
                if (validationResult.IsValid)
                {
                    result.Add(article);
                }
                else
                {
                    Trace.TraceWarning($"Article with url '{article.Url}' isn't valid: {validationResult?.Errors?.FirstOrDefault()?.ErrorMessage}");
                }
            }
            return Task.FromResult((IEnumerable<Article>)result);
        }
    }
}