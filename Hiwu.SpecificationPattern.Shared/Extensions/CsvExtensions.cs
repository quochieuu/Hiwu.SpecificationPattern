using System.Globalization;
using System.Text;

namespace Hiwu.SpecificationPattern.Shared.Extensions
{
    public class CsvExtensions
    {
        /// <summary>
        /// Method to parse CSV from file path
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="filePath"></param>
        /// <param name="delimiter"></param>
        /// <returns></returns>
        public static List<T> ParseCsvToClass<T>(string filePath, char delimiter = ',') where T : new()
        {
            // Read all lines from the file
            var lines = File.ReadAllLines(filePath);
            // Parse the lines into a list of objects of type T
            return ParseCsvLinesToClass<T>(lines, delimiter);
        }

        /// <summary>
        /// Method to parse CSV from file content (string)
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="csvContent"></param>
        /// <param name="delimiter"></param>
        /// <returns></returns>
        public static List<T> ParseCsvContentToClass<T>(string csvContent, char delimiter = ',') where T : new()
        {
            // Split the content into lines based on newline characters
            var lines = csvContent.Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);
            // Parse the lines into a list of objects of type T
            return ParseCsvLinesToClass<T>(lines, delimiter);
        }

        /// <summary>
        /// Helper method to parse CSV lines into a list of objects of type T
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="lines"></param>
        /// <param name="delimiter"></param>
        /// <returns></returns>
        private static List<T> ParseCsvLinesToClass<T>(string[] lines, char delimiter) where T : new()
        {
            var result = new List<T>();
            // Split the first line (header) to get property names
            var header = lines[0].Split(delimiter);

            // Get properties of the type T
            var properties = typeof(T).GetProperties();
            for (int i = 1; i < lines.Length; i++)
            {
                // Split each line into values based on the delimiter
                var values = lines[i].Split(delimiter);
                var obj = new T();
                for (int j = 0; j < header.Length; j++)
                {
                    // Find the property in the class that matches the header name
                    var property = properties.FirstOrDefault(p => p.Name.Equals(header[j], StringComparison.OrdinalIgnoreCase));
                    if (property != null && values[j] != "")
                    {
                        // Convert the value to the appropriate type and set it to the property
                        var convertedValue = Convert.ChangeType(values[j], property.PropertyType, CultureInfo.InvariantCulture);
                        property.SetValue(obj, convertedValue);
                    }
                }
                // Add the parsed object to the result list
                result.Add(obj);
            }
            return result;
        }

        /// <summary>
        /// Method to export a list of objects to CSV file
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="data"></param>
        /// <param name="filePath"></param>
        /// <param name="delimiter"></param>
        public static void ExportToCsv<T>(List<T> data, string filePath, char delimiter = ',')
        {
            var sb = new StringBuilder();
            var properties = typeof(T).GetProperties();

            // Write the header
            sb.AppendLine(string.Join(delimiter, properties.Select(p => p.Name)));

            // Write the data rows
            foreach (var item in data)
            {
                var values = properties.Select(p => p.GetValue(item, null)?.ToString() ?? string.Empty);
                sb.AppendLine(string.Join(delimiter, values));
            }

            // Save to file
            File.WriteAllText(filePath, sb.ToString());
        }
    }
}
