using FluentValidation;

namespace Application.EventImages.UploadEventImage;

public class UploadEventImageCommandValidator : AbstractValidator<UploadEventImageCommand>
{
    public UploadEventImageCommandValidator()
    {
        RuleFor(e => e.File).NotNull();

        RuleFor(e => e.File.Length).GreaterThan(0);

        RuleFor(e => e.File.ContentType).Must(c => c.StartsWith("image/"));
    }
}
