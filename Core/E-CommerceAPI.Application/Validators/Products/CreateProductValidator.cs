using E_CommerceAPI.Application.ViewModels.Product;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_CommerceAPI.Application.Validators.Products
{
    public class CreateProductValidator : AbstractValidator<CreateProductVM>
    {
        public CreateProductValidator()
        {
            RuleFor(p => p.Name).NotEmpty().NotNull().WithMessage("Lütfen ürün adını giriniz.")
                                .MaximumLength(150)
                                .MinimumLength(5).WithMessage("Ürün adı 5-150 karakter arasında olmalıdır.");

            RuleFor(p => p.Stock).NotEmpty()
                                 .NotNull().WithMessage("Lütfen stok bilgisi giriniz.")
                                 .Must(s => s >= 0).WithMessage("Stok Bilgisi 0'dan küçük olamaz.");

            RuleFor(p=>p.Price).NotEmpty()
                               .NotNull().WithMessage("Lütfen fiyat bilgisi giriniz.")
                               .Must(p => p >= 0).WithMessage("Fiyat Bilgisi 0'dan küçük olamaz.");
        }
    }
}
