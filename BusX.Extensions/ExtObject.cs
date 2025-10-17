using System.Globalization;
using System.Text;
namespace BusX.Extensions
{
    public static class ExtObject
    {
        public static int ToInt32(this object obj) => obj == DBNull.Value ? 0 : Convert.ToInt32(obj);
        public static int? ToNullableInt32(this object obj) => obj == null || obj == DBNull.Value || string.IsNullOrEmpty(obj.ToString()) ? new int?() : Convert.ToInt32(obj);
        public static DateTime ToDDMMYYYYDateTime(this object obj) => obj == DBNull.Value ? new DateTime() : DateTime.ParseExact(Convert.ToString(obj), "dd.MM.yyyy", CultureInfo.InvariantCulture);
        public static string CamelCaseToUnderscore(this string input)
        {
            if (string.IsNullOrEmpty(input)) return input;
            var result = new StringBuilder();
            result.Append(char.ToLower(input[0], CultureInfo.InvariantCulture));
            for (int i = 1; i < input.Length; i++)
            {
                if (char.IsUpper(input[i])) result.Append('_').Append(char.ToLower(input[i], CultureInfo.InvariantCulture));
                else result.Append(input[i]);
            }
            return result.ToString().Replace("_i_d", "_id");
        }
    }
}