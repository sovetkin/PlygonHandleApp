using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.RegularExpressions;

namespace PlygonHandle.Validation
{
    public static class StringValidationRules
    {
        public static ValidationResult Validate(string input, double width, double height)
        {
            if (String.IsNullOrEmpty(input))
                return ValidationResult.Success;

            if (!ValidateInputString(input))
                return new("Введенная строка не может быть преобразована в координаты");

            if (!ValidateCoordinates(input, (width, height)))
                return new("Координаты выходят за границы холста");

            return ValidationResult.Success;
        }

        private static bool ValidateCoordinates(string pattern, (double w, double h) values)
        {
            double[] list = pattern.Split(", ")
                                   .Select(x => Double.Parse(x))
                                   .ToArray();

            foreach (double item in GetOddValues(list))
            {
                if (item > values.h || item < 0)
                    return false;
            }

            foreach (double item in GetEvenValues(list))
            {
                if (item > values.w || item < 0)
                    return false;
            }

            return true;
        }

        private static IEnumerable<double> GetOddValues(double[] list)
        {
            return list.Where((x, i) => i % 2d != 0)
                       .ToArray();
        }

        private static IEnumerable<double> GetEvenValues(double[] list)
        {
            return list.Where((x, i) => i % 2d == 0)
                       .ToArray();
        }

        private static bool ValidateInputString(string input) =>
            Regex.IsMatch(input, "(?:(?:(?:\\d+),? (?:\\d+)),? ){2,}(?:(?:\\d+),? (?:\\d+))");
    }
}
