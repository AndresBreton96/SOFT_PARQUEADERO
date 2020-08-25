using System;
using System.Xml;

namespace Transversales.Utilitarios.Tools
{
    public static class ResourcesReader
    {
        private static XmlDocument _xmlDocument = null;

        private static void ReadXml(string propertyType)
        {
            if (_xmlDocument == null)
            {
                _xmlDocument = new XmlDocument();
                _xmlDocument.Load($"{AppDomain.CurrentDomain.BaseDirectory}\\Resources\\{propertyType}.xml");
            }

        }

        public static string GetProperty(string propertyType, string property)
        {
            ReadXml(propertyType);
            XmlNode node = _xmlDocument.DocumentElement.SelectSingleNode($"/{propertyType}/{property}");
            if (node == null)
                return null;
            return node.InnerText;
        }

        public static string GetPropertyWithLanguage(string propertyType, string property)
        {
            ReadXml(propertyType);
            var culture = System.Globalization.CultureInfo.CurrentCulture.Name;
            XmlNode node = _xmlDocument.DocumentElement.SelectSingleNode($"/{propertyType}/{property}/{culture}");
            if (node == null)
                return null;
            return node.InnerText;
        }
    }
}
