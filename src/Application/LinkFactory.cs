using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace Application;

public class LinkFactory(
    IHttpContextAccessor _httpContextAccessor,
    LinkGenerator _linkGenerator)
{
    public string? GenerateGetAllEventsUri()
    {
        var uri = _linkGenerator.GetUriByName(
            _httpContextAccessor.HttpContext!,
            "GetAllEvents"
        );

        return uri;
    }

    public string? GenerateGetEventByIdUri(Guid eventId)
    {
        var uri = _linkGenerator.GetUriByName(
            _httpContextAccessor.HttpContext!,
            "GetEventById",
            new { id = eventId }
        );

        return uri;
    }
}
