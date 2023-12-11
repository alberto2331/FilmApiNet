using System.ComponentModel.DataAnnotations;

namespace FilmAPI.Validations
{
    public class WeightFileValidation : ValidationAttribute
    {
        private readonly int maximumWeightInMegabits;

        public WeightFileValidation(int maximumWeightInMegabits)
        {
            this.maximumWeightInMegabits = maximumWeightInMegabits;
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value == null) { return ValidationResult.Success; }
            IFormFile formFile = value as IFormFile;
            if (formFile == null) { return ValidationResult.Success; }
            if (formFile.Length > maximumWeightInMegabits * 1024 * 1024) {
                return new ValidationResult($"the weight of the file exceeds {maximumWeightInMegabits} megabytes");
            }
            return ValidationResult.Success;
        }
    }
}
