using System.Text.RegularExpressions;

namespace GoDecola.API.Utils
{
    public static class ValidationUtils
    {
        public static bool IsValidCPF(string cpf)
        {
            if (string.IsNullOrWhiteSpace(cpf))
                return false;

            cpf = new string(cpf.Where(char.IsDigit).ToArray());

            if (cpf.Length != 11 || cpf.Distinct().Count() == 1)
                return false;

            var multiplicador1 = new int[9] { 10, 9, 8, 7, 6, 5, 4, 3, 2 };
            var multiplicador2 = new int[10] { 11, 10, 9, 8, 7, 6, 5, 4, 3, 2 };

            var tempCpf = cpf.Substring(0, 9);
            var soma = 0;

            for (int i = 0; i < 9; i++)
                soma += int.Parse(tempCpf[i].ToString()) * multiplicador1[i];

            var resto = soma % 11;
            var digito = resto < 2 ? 0 : 11 - resto;

            tempCpf += digito;
            soma = 0;

            for (int i = 0; i < 10; i++)
                soma += int.Parse(tempCpf[i].ToString()) * multiplicador2[i];

            resto = soma % 11;
            digito = resto < 2 ? 0 : 11 - resto;

            return cpf.EndsWith(digito.ToString());
        }

        public static bool IsValidRNE(string rne)
        {
            if (string.IsNullOrWhiteSpace(rne))
                return false;

            rne = rne.Trim();

            // aceita letras e números mínimo 9 caracteres
            return Regex.IsMatch(rne, @"^[A-Za-z0-9]{9,12}$");
        }

        public static bool IsValidPassport(string passport)
        {
            if (string.IsNullOrWhiteSpace(passport))
                return true;

            passport = passport.Trim().ToUpper();

            // 2 letras + 6 números
            return Regex.IsMatch(passport, @"^[A-Z]{2}\d{6}$");
        }
    }
}
