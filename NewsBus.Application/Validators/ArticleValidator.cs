using System;
using FluentValidation;
using NewsBus.Domain.Models;

namespace NewsBus.Application.Validators
{
    public class ArticleValidator : AbstractValidator<Article>
    {
        public ArticleValidator()
        {
            RuleFor(x => x.Id).Must(BeAValidGuid);
            RuleFor(x => x.Url).Must(BeAValidUrl);
            RuleFor(x => x.Title).NotEmpty();
        }

        private bool BeAValidGuid(string guid)
        {
            if (Guid.TryParse(guid, out Guid value))
            {
                return Guid.Empty != value;
            }
            return false;
        }

        private bool BeAValidUrl(Uri url)
        {
            Uri validUrl = ValidateUrl(url.ToString());
            return validUrl != null;
        }

        public static Uri ValidateUrl(string url)
        {
            if (!Uri.IsWellFormedUriString(url, UriKind.Absolute))
            {
                return null;
            }

            UriBuilder uriBuilder = new UriBuilder(url);
            if (string.Equals(uriBuilder.Scheme, Uri.UriSchemeHttp, StringComparison.OrdinalIgnoreCase) || 
                string.Equals(uriBuilder.Scheme, Uri.UriSchemeHttps, StringComparison.OrdinalIgnoreCase))
            {
                return uriBuilder.Uri;
            }

            return null;
        }
    }
}