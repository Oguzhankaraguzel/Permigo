using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net.Mail;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Application.Abstractions.Services;
using SharedKernel.Extensions.Linq;

namespace Infrastructure.Conceretes.Services;
public class TextUtilityService : ITextUtilityService
{
    /// <summary>
    /// Corrects the capitalization and removes spaces from a given name. (naME = Name).
    /// </summary>
    /// <param name="name">The name to be corrected.</param>
    /// <returns>The name with corrected capitalization.</returns>
    public string NameCorrection(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
        { 
            return name;
        }
        name = name.Trim();

        var sb = new StringBuilder(name.Length);
        bool isFirstChar = true;

        foreach (char c in name)
        {
            if (c != ' ')
            {
                if (isFirstChar)
                {
                    sb.Append(char.ToUpperInvariant(c));
                    isFirstChar = false;
                }
                else
                {
                    sb.Append(char.ToLowerInvariant(c));
                }
            }
        }

        return sb.ToString();
    }

    /// <summary>
    /// Corrects the capitalization of multiple names and concatenates them into a single string.
    /// </summary>
    /// <param name="names">The names to be corrected.</param>
    /// <returns>The corrected names concatenated with spaces.</returns>
    public string NameCorrection(params string[] names)
    {
        if (names == null || names.Length == 0)
        { 
            return string.Empty;
        }
        var sb = new StringBuilder();

        foreach (string name in names)
        {
            if (!string.IsNullOrWhiteSpace(name))
            {
                if (sb.Length > 0)
                { 
                    sb.Append(' ');
                }
                sb.Append(NameCorrection(name));
            }
        }

        return sb.ToString();
    }

    /// <summary>
    /// Corrects the capitalization and removes spaces from a given sir name.
    /// </summary>
    /// <param name="sirName">The sir name to be corrected.</param>
    /// <returns>The corrected sir name without spaces and with uppercase letters.</returns>
    public string SirNameCorrection(string sirName)
    {
        if (string.IsNullOrWhiteSpace(sirName))
        {
            return sirName;
        }
        return sirName.Replace(" ", "").ToUpperInvariant();
    }

    /// <summary>
    /// Corrects the capitalization and removes spaces from one or more sir names.
    /// </summary>
    /// <param name="sirNames">The sir names to be corrected.</param>
    /// <returns>The corrected sir names without spaces and with uppercase letters.</returns>
    public string SirNameCorrection(params string[] sirNames)
    {
        if (sirNames == null || sirNames.Length == 0)
        {
            return string.Empty;
        }
        var sb = new StringBuilder();

        foreach (string sirName in sirNames)
        {
            if (!string.IsNullOrWhiteSpace(sirName))
            {
                if (sb.Length > 0)
                {
                    sb.Append(' ');
                }
                sb.Append(SirNameCorrection(sirName));
            }
        }

        return sb.ToString();
    }

    /// <summary>
    /// Corrects the capitalization and spaces in the given name and sir name, and concatenates them to form a full name.
    /// </summary>
    /// <param name="name">The name to be corrected.</param>
    /// <param name="sirName">The sir name to be corrected.</param>
    /// <returns>The corrected full name with proper capitalization and spacing.</returns>
    public string FullNameCorrection(string name, string sirName)
    {
        if (string.IsNullOrWhiteSpace(name) && string.IsNullOrWhiteSpace(sirName))
        {
            return string.Empty;
        }
        var sb = new StringBuilder();

        if (!string.IsNullOrWhiteSpace(name))
        {
            sb.Append(NameCorrection(name));
        }

        if (!string.IsNullOrWhiteSpace(sirName))
        {
            if (sb.Length > 0)
            {
                sb.Append(' ');
            }
            sb.Append(SirNameCorrection(sirName));
        }

        return sb.ToString();
    }

    /// <summary>
    /// Corrects the capitalization and spacing of the given names and sir names, and concatenates them to form a full name.
    /// </summary>
    /// <param name="names">The names to be corrected.</param>
    /// <param name="sirNames">The sir names to be corrected.</param>
    /// <returns>The corrected full name with proper capitalization and spacing.</returns>
    public string FullNameCorrection(string[] names, string[] sirNames)
    {
        bool checkNames = names.IsNullOrEmpty();
        bool checksirNames = sirNames.IsNullOrEmpty();

        if (checkNames && checksirNames)
        {
            return string.Empty;
        }
        var sb = new StringBuilder();

        if (!checkNames)
        {
            foreach (string name in names)
            {
                if (!string.IsNullOrWhiteSpace(name))
                {
                    if (sb.Length > 0)
                    {
                        sb.Append(' ');
                    }
                    sb.Append(NameCorrection(name));
                }
            }
        }

        if (!checksirNames)
        {
            foreach (string sirName in sirNames)
            {
                if (!string.IsNullOrWhiteSpace(sirName))
                {
                    if (sb.Length > 0)
                    {
                        sb.Append(' ');
                    }
                    sb.Append(SirNameCorrection(sirName));
                }
            }
        }

        return sb.ToString();
    }

    /// <summary>
    /// Standardizes and normalizes characters in the input string by removing diacritic marks.
    /// </summary>
    /// <param name="input">The input string to be standardized.</param>
    /// <returns>The standardized string without diacritic marks.</returns>
    public string StandardizeCharacters(string input)
    {
        if (string.IsNullOrWhiteSpace(input))
        {
            return input;
        }
        var sb = new StringBuilder(input.Length);
        string normalizedString = input.Trim().Normalize(NormalizationForm.FormD);

        foreach (char c in normalizedString)
        {
            if (CharUnicodeInfo.GetUnicodeCategory(c) != UnicodeCategory.NonSpacingMark)
            {
                sb.Append(c);
            }
        }
        return sb.ToString().Normalize(NormalizationForm.FormC).Replace(" ", "");
    }

    /// <summary>
    /// Creates a user name by standardizing the given name and appending a four-digit random number.
    /// </summary>
    /// <param name="name">The name to create the user name from.</param>
    /// <returns>The generated user name.</returns>
    public string CreateUserName(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            return string.Empty;
        }

        string standardized = StandardizeCharacters(name);

        int randomNumber = RandomNumberGenerator.GetInt32(0, 10000);

        string randomSuffix = randomNumber.ToString("D4", CultureInfo.InvariantCulture);

        return standardized.ToUpperInvariant() + randomSuffix;
    }

    public bool IsValidEmail(string email)
    {
        if (string.IsNullOrWhiteSpace(email))
        {
            return false;
        }
        try
        {
            var addr = new MailAddress(email);
            return addr.Address == email;
        }
        catch
        {
            return false;
        }
    }
}

