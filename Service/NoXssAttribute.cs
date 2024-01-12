using System.ComponentModel.DataAnnotations;

namespace Service;

public class NoXssAttribute : ValidationAttribute
{
    public override bool IsValid(object value)
    {
        if (value == null)
        {
            return true; // Not checking for null - assuming you have other attributes for this purpose
        }

        var stringValue = value.ToString();
        // Optionally use an anti-XSS library like Microsoft's AntiXSS library
        // stringValue = Microsoft.Security.Application.Encoder.HtmlEncode(stringValue);

        // Checks for characters typically used in XSS attacks
        if (stringValue.IndexOf('<') >= 0
            || stringValue.IndexOf('>') >= 0
            || stringValue.IndexOf('&') >= 0
            || stringValue.IndexOf('"') >= 0
            || stringValue.IndexOf('\'') >= 0)
        {
            return false;
        }

        return true;
    }

    public override string FormatErrorMessage(string name)
    {
        return "The input contains invalid characters.";
    }
}