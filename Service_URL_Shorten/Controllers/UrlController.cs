using Microsoft.AspNetCore.Mvc;
using MediatR;
using Service_URL_Shorten.Queries;
using Service_URL_Shorten.Commands;
namespace Service_URL_Shorten.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UrlController : ControllerBase
    {
        private readonly IMediator _mediator;
        public UrlController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("shorten")]
        public async Task<IActionResult> ShortenUrl([FromBody] CreateShortUrlCommand command)
        {
            var result = await _mediator.Send(command);
            return Ok(new
            {
                Url = $"{Request.Scheme}://{Request.Host}/api/url/{result.ShortCode}",
                originalUrl = result.OriginalUrl
            });
        }

        [HttpGet("{code}")]
        public async Task<IActionResult> RedirectToOriginal(string code)
        {
            var query = new GetOriginalUrlQuery { ShortCode = code };
            var result = await _mediator.Send(query);
            if (result == null) return NotFound("Short URL not found");
            return Redirect(result.OriginalUrl);

        }
    }
}
