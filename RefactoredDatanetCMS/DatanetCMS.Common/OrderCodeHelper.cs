namespace DatanetCMS.Common
{
    public static class OrderCodeHelper
    {
        /// <summary>
        /// generate order code by rule: prefix + 5 numbers
        /// </summary>
        /// <param name="prefix"></param>
        /// <param name="number"></param>
        /// <returns></returns>
        public static string GenerateOrderCode(string prefix, int number)
        {
            string numbers = number.ToString("D5");
            string code = $"{prefix}{numbers}";
            return code;
        }
    }
}
