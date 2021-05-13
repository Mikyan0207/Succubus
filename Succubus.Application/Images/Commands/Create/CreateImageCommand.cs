using MediatR;
using Microsoft.AspNetCore.Http;
using Succubus.Application.Common.Models;
using Succubus.Application.Interfaces;
using System.Threading;
using System.Threading.Tasks;

namespace Succubus.Application.Images.Commands.Create
{
    public static class CreateImageCommand
    {
        public class Request : IRequest<Response<string>>
        {
            public string CosplayerId { get; set; } = string.Empty;

            public string SetId { get; set; } = string.Empty;

            // NOTE(Mikyan): Pass a stream to the request?
            public IFormFile? File { get; set; }

            public override string ToString()
            {
                return $"{CosplayerId}::{SetId}";
            }
        }

        public class Handler : IRequestHandler<Request, Response<string>>
        {
            private readonly IContext Context;
            private readonly IImageService ImageService;

            public Handler(IContext context, IImageService imageService)
            {
                Context = context;
                ImageService = imageService;
            }

            public Task<Response<string>> Handle(Request request, CancellationToken cancellationToken)
            {
                return ImageService.UploadImageAsync(request, cancellationToken);
            }
        }
    }
}
