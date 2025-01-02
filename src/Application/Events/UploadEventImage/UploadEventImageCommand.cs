using Application.Abstractions;
using Application.Dtos;
using Microsoft.AspNetCore.Http;

namespace Application.Events.UploadEventImage;

public record UploadEventImageCommand(
    Guid EventId,
    IFormFile File
) : ICommand<ImageDto>;
