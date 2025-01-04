using Application.Abstractions;
using Application.Dtos;
using AutoMapper;
using Domain.Entities;

namespace Application;

public class ImageUrlValueResolver(
    IS3Client _s3Client
) : IValueResolver<Image, ImageDto, string>
{
    public string Resolve(
        Image source, 
        ImageDto destination,
        string destMember, 
        ResolutionContext context)
    {
        if (string.IsNullOrEmpty(source.BucketName) ||
            string.IsNullOrEmpty(source.ObjectKey))
        {
            return string.Empty;
        }

        return _s3Client.GetPublicFileUrl(source.BucketName, source.ObjectKey);
    }
}
