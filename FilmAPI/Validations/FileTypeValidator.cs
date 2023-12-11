using System.ComponentModel.DataAnnotations;

namespace FilmAPI.Validations
{
    public class FileTypeValidator : ValidationAttribute 
    {
        private readonly string[] typesValid;

        public FileTypeValidator(string[] typesValid)
        {
            this.typesValid = typesValid;
        }
        public FileTypeValidator(GroupTypeFile groupTypeFile)
        {
            if (groupTypeFile == GroupTypeFile.Image)
            {
                typesValid = new string[] { "image/jpeg", "image/png", "image/gif" };
            }
        }

        protected override ValidationResult IsValid(object value,ValidationContext validationContext)
        {
            if (value == null) { return ValidationResult.Success; }
            IFormFile formFile = value as IFormFile;
            if (formFile == null) { return ValidationResult.Success; }
            if (!typesValid.Contains(formFile.ContentType))
            {
                return new ValidationResult($"the type of the file must be {string.Join(", ", typesValid)}");
            }
            return ValidationResult.Success;
        }
    }
}
