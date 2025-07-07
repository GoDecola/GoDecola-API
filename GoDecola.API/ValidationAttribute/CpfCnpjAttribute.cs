namespace GoDecola.API.ValidationAttribute;
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

public class CpfCnpjAttribute : ValidationAttribute
{
    public override bool IsValid(object? value)
    {
        if (value == null) return false;

        string documento = Regex.Replace(value.ToString()!, @"[^\d]", "");

        if (documento.Length == 11)
            return ValidaCPF(documento);
        else if (documento.Length == 14)
            return ValidaCNPJ(documento);
        else
            return false;
    }

    private bool ValidaCPF(string cpf)
    {
        if (new string(cpf[0], cpf.Length) == cpf) return false;

        int[] multiplicador1 = { 10, 9, 8, 7, 6, 5, 4, 3, 2 };
        int[] multiplicador2 = { 11, 10, 9, 8, 7, 6, 5, 4, 3, 2 };

        string tempCpf = cpf[..9];
        int soma = 0;

        for (int i = 0; i < 9; i++)
            soma += int.Parse(tempCpf[i].ToString()) * multiplicador1[i];

        int resto = soma % 11;
        int digito = resto < 2 ? 0 : 11 - resto;
        tempCpf += digito;
        soma = 0;

        for (int i = 0; i < 10; i++)
            soma += int.Parse(tempCpf[i].ToString()) * multiplicador2[i];

        resto = soma % 11;
        digito = resto < 2 ? 0 : 11 - resto;

        return cpf.EndsWith(tempCpf[9].ToString() + digito.ToString());
    }

    private bool ValidaCNPJ(string cnpj)
    {
        if (new string(cnpj[0], cnpj.Length) == cnpj) return false;

        int[] multiplicador1 = { 5, 4, 3, 2, 9, 8, 7, 6, 5, 4, 3, 2 };
        int[] multiplicador2 = { 6, 5, 4, 3, 2, 9, 8, 7, 6, 5, 4, 3, 2 };

        string tempCnpj = cnpj[..12];
        int soma = 0;

        for (int i = 0; i < 12; i++)
            soma += int.Parse(tempCnpj[i].ToString()) * multiplicador1[i];

        int resto = soma % 11;
        int digito = resto < 2 ? 0 : 11 - resto;
        tempCnpj += digito;
        soma = 0;

        for (int i = 0; i < 13; i++)
            soma += int.Parse(tempCnpj[i].ToString()) * multiplicador2[i];

        resto = soma % 11;
        digito = resto < 2 ? 0 : 11 - resto;

        return cnpj.EndsWith(tempCnpj[12].ToString() + digito.ToString());
    }
}
