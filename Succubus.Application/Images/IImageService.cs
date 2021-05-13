using Succubus.Application.Common.Models;
using Succubus.Application.Images.Commands.Create;
using System.Threading;
using System.Threading.Tasks;

namespace Succubus.Application.Images
{
    public interface IImageService
    {
        Task<Response<string>> UploadImageAsync(CreateImageCommand.Request request, CancellationToken cancellationToken = new());
    }
}
