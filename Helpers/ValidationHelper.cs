using System.Text.RegularExpressions;

namespace CoreventApp.Helpers;

public static partial class ValidationHelper
{
    public static bool IsValidCpf(string? cpf)
    {
        if (string.IsNullOrWhiteSpace(cpf))
            return false;

        var digits = Regex.Replace(cpf, @"\D", "");

        if (digits.Length != 11)
            return false;

        if (digits.All(c => c == digits[0]))
            return false;

        var numbers = digits.Select(c => c - '0').ToArray();

        var sum1 = 0;
        for (var i = 0; i < 9; i++)
            sum1 += numbers[i] * (10 - i);
        var digit1 = sum1 % 11;
        digit1 = digit1 < 2 ? 0 : 11 - digit1;

        if (numbers[9] != digit1)
            return false;

        var sum2 = 0;
        for (var i = 0; i < 10; i++)
            sum2 += numbers[i] * (11 - i);
        var digit2 = sum2 % 11;
        digit2 = digit2 < 2 ? 0 : 11 - digit2;

        return numbers[10] == digit2;
    }

    [GeneratedRegex(@"^(\+55)?(\(?\d{2}\)?)[\s-]?(9\d{4}|\d{4})-?\d{4}$")]
    private static partial Regex PhoneRegex();

    public static bool IsValidPhone(string? phone)
    {
        if (string.IsNullOrWhiteSpace(phone))
            return false;

        return PhoneRegex().IsMatch(phone.Trim());
    }
}
