using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Succubus.Application.Common;
using Succubus.Application.Common.Exceptions;
using Succubus.Application.Common.Models;
using Succubus.Application.Images;
using Succubus.Application.Images.Commands.Create;
using Succubus.Core.Entities;
using Succubus.Infrastructure.Database;
using System;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Succubus.Infrastructure.Images
{
    public class ImageService : IImageService
    {
        private readonly Context Context;
        private readonly IHostingEnvironment Environment;

        public ImageService(Context context, IHostingEnvironment environment)
        {
            Context = context;
            Environment = environment;
        }

        public async Task<Response<string>> UploadImageAsync(CreateImageCommand.Request request, CancellationToken cancellationToken)
        {
            var cosplayer = await Context.Cosplayers
                .Include(x => x.Sets)
                    .ThenInclude(s => s.Images)
                .FirstOrDefaultAsync(x => x.Id == request.CosplayerId, cancellationToken);

            if (cosplayer is null)
            {
                return new Response<string>
                {
                    Result = Result.Error,
                    Exception = new NotFoundException(request.CosplayerId, request),
                    ErrorCode = 0,
                    ErrorMessage = "Cosplayer not found."
                };
            }

            var set = cosplayer.Sets.FirstOrDefault(x => x.Id == request.SetId);

            if (set is null)
            {
                return new Response<string>
                {
                    Result = Result.Error,
                    Exception = new NotFoundException(request.SetId, request),
                    ErrorCode = 0,
                    ErrorMessage = "Set not found."
                };
            }

            if (request.File is null)
            {
                return new Response<string>
                {
                    Result = Result.Error,
                    Exception = new NullReferenceException(),
                    ErrorCode = 0,
                    ErrorMessage = "File was null."
                };
            }

            var filename = GetUniqueFilename(cosplayer, set, request.File.FileName);
            var folder = GetFolder(cosplayer, set);
            var filePath = Path.Combine(Environment.WebRootPath, "Images", folder, filename);

            await request.File.CopyToAsync(new FileStream(filePath, FileMode.Create), cancellationToken);

            try
            {
                var e = Context.Images.Add(new Image
                {
                    Name = Path.GetFileNameWithoutExtension(filename),
                    Number = set.CurrentIndex,
                    File = filename,
                    Folder = GetFolder(cosplayer, set),
                    AbsolutePath = filePath,
                    Url = Path.Combine("Images", folder, filename),
                    Set = set,
                    Cosplayer = cosplayer
                });

                set.CurrentIndex += 1;

                Context.Update(set);

                await Context.SaveChangesAsync(cancellationToken);

                return new Response<string>
                {
                    Result = Result.Success,
                    Content = e.Entity.Id
                };
            }
            catch (DbUpdateConcurrencyException ex)
            {
                return new Response<string>
                {
                    Result = Result.Error,
                    Exception = ex,
                    ErrorCode = 0,
                    ErrorMessage = "Error while updating the database."
                };

            }
        }

        private static string GetFolder(Cosplayer cosplayer, Set set) => Path.Combine(cosplayer.Name, set.Name);

        private static string GetUniqueFilename(Cosplayer cosplayer, Set set, string filename) => $"{cosplayer.Name}_{set.Name}_{set.CurrentIndex}{Path.GetExtension(filename)}";
    }
}
