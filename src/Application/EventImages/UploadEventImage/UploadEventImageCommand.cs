using Application.Abstractions;
using Application.Dtos;
using Microsoft.AspNetCore.Http;

namespace Application.EventImages.UploadEventImage;

public record UploadEventImageCommand(
    Guid EventId,
    IFormFile File
) : ICommand<ImageDto>;
