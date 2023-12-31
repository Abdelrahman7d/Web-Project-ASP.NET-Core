
namespace Entity.Core
{
    public static class Helper
    {
        public static List<string>? ToStringArray(string? value, char splitter = ',')
        {
            if (string.IsNullOrEmpty(value))
            {
                return null;
            }

            return value.Trim(splitter).Split(splitter).ToList();
        }
    }
}
