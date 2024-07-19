using Blog.Models;
using FluentValidation;

namespace Blog.Validation
{
    public class PostValidator : AbstractValidator<Post>
    {
        public PostValidator() 
        {
            RuleFor(p => p.Title)
                .NotEmpty()
                .WithMessage("Tiêu đề là bắt buộc");
            RuleFor(p => p.Description)
                .NotEmpty()
                .WithMessage("Mô tả là bắt buộc");
            RuleFor(p => p.Content)
                .NotEmpty()
                .WithMessage("Nội dung là bắt buộc");
            RuleFor(p => p.PublishDate)
                .NotEmpty()
                .WithMessage("Ngày xuất bản là bắt buộc");
            RuleFor(p => p.IsPublic)
                .NotNull()
                .WithMessage("Trạng thái công khai là bắt buộc");
            RuleFor(p => p.Category)
                .NotEmpty()
                .WithMessage("Danh mục là bắt buộc");
        }
    }
}
