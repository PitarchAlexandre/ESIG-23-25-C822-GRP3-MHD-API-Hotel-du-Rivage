using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;

namespace AP_Groupe3_Hotel.Utilities
{
    /// <summary>
    /// Codé par : Alexandre
    /// Modifié par : Alexandre
    /// </summary>
    internal class StatutToStringConverter : IValueConverter
    {
        /// <summary>
        /// Permet de convertir le 1 en Oui et le 0 en Non.
        /// </summary>
        /// <param name="value"></param>
        /// <param name="targetType"></param>
        /// <param name="parameter"></param>
        /// <param name="culture"></param>
        /// <returns></returns>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            // Convertir la valeur du statut de string à string
            if (value != null && value.ToString() == "1")
            {
                return "Oui";
            }
            else
            {
                return "Non";
            }
        }
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
    /// <summary>
    /// Permet de convertir le 1 en true et le 0 en false.
    /// </summary>
    public class StatutToBooleanConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            // Convertir la valeur du statut de string à string
            if (value != null && value.ToString() == "1")
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Permet de convertir le booléen en string.
        /// </summary>
        /// <param name="value"></param>
        /// <param name="targetType"></param>
        /// <param name="parameter"></param>
        /// <param name="culture"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"></exception>
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is bool isChecked)
            {
                // Convertir le booléen en string
                if (isChecked)
                {
                    return "1";
                }
                else
                {
                    return "0";
                }
            }
            // Affiche ce message si la valeur n'est pas prise en compte (booléenne)
            else
            {
                throw new ArgumentException("La valeur doit être de type booléen.");
            }
        }
    }
    public class CodeChaConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is int codeCha)
            {
                // Vérifier si codeCha < 10
                if (codeCha < 10)
                {
                    return $"0{codeCha}";
                }
                else
                {
                    return codeCha.ToString();
                }
            }
            // Valeur par défaut si ce n'est pas un int
            return value?.ToString();
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class DateTimeToDateOnlyConverter : IValueConverter
    {

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is DateOnly dateOnly)
            {
                //Retourne un DateTime à partir de la valeur DateOnly
                return new DateTime(dateOnly.Year, dateOnly.Month, dateOnly.Day);
            }
            return DependencyProperty.UnsetValue;

        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is DateTime dateTime)
            {
                //retourne un DateOnly à partir de la valeur DateTime
                return new DateOnly(dateTime.Year, dateTime.Month, dateTime.Day);
            }
            return DependencyProperty.UnsetValue;
        }
    }
}
