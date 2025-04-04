using BookPro.Common.Config;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Localization;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Options;
using System.Globalization;

namespace BookPro.Common.Localization;

public class ManagerResourceLenguaje : IManagerResourceLenguaje
{
    private readonly IHttpContextAccessor _contextAccessor;
    private readonly IStringLocalizer _localizer;
    private readonly SettingsResource _resource;

    public ManagerResourceLenguaje (IHttpContextAccessor contextAccessor, IStringLocalizerFactory localizerFactory, IOptions<SettingsResource> configuration)
    {
        _contextAccessor = contextAccessor;
        _resource = configuration.Value;

        var culture = _contextAccessor.HttpContext?.Features.Get<IRequestCultureFeature>()?.RequestCulture.Culture.Name ?? "en";

        CultureInfo.CurrentCulture = new CultureInfo(culture);
        CultureInfo.CurrentUICulture = new CultureInfo(culture);

        _localizer = localizerFactory.Create(_resource.BaseName, _resource.Location);
    }

    public string GetMessage<TEnum>(TEnum key)
    {
        var result = _localizer[key.ToString()];

        return result.Value;
    }
}
