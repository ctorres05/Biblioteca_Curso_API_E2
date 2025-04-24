using System.ComponentModel.DataAnnotations;

namespace BibliotecaApi.Validaciones
{
    public class PrimeraLetraMayusculaAttribute : ValidationAttribute
    {
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            if (value == null || string.IsNullOrEmpty(value.ToString()))
            {
                //return new ValidationResult("El campo no puede ser nulo");
                return ValidationResult.Success;

            }
            //var primerCaracter = value.ToString()[0].ToString();   /*// Obtener la primera letra*/
            var primerCaracter = value.ToString()!;
            
            //primerCaracter = primerCaracter.Substring(0, 1); 
            
            primerCaracter = primerCaracter[0].ToString(); /*// Obtener la primera letra*/  
            
            if (primerCaracter != primerCaracter.ToUpper())
            {
                return new ValidationResult("La primera letra debe ser mayúscula del campo {value}");
            }
            return ValidationResult.Success;
        }
    }
            
}
