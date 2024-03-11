using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MaxMind.GeoIP2;
using MaxMind.GeoIP2.Exceptions;
using System;
using System.IO;
using System.Net;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Text.RegularExpressions;

namespace DataPipeline.Helpers.LocationService
{
    public class LocationService : ILocationService
    {

        private bool IsValidIP(string ipAddress)
        {
            // Regular expression pattern for IPv4 address
            string ipv4Pattern = @"^(\d{1,3}\.){3}\d{1,3}$";

            // Regular expression pattern for IPv6 address
            string ipv6Pattern = @"^(([0-9A-Fa-f]{1,4}:){7,7}[0-9A-Fa-f]{1,4}|([0-9A-Fa-f]{1,4}:){1,7}:|([0-9A-Fa-f]{1,4}:){1,6}:[0-9A-Fa-f]{1,4}|([0-9A-Fa-f]{1,4}:){1,5}(:[0-9A-Fa-f]{1,4}){1,2}|([0-9A-Fa-f]{1,4}:){1,4}(:[0-9A-Fa-f]{1,4}){1,3}|([0-9A-Fa-f]{1,4}:){1,3}(:[0-9A-Fa-f]{1,4}){1,4}|([0-9A-Fa-f]{1,4}:){1,2}(:[0-9A-Fa-f]{1,4}){1,5}|[0-9A-Fa-f]{1,4}:((:[0-9A-Fa-f]{1,4}){1,6})|:((:[0-9A-Fa-f]{1,4}){1,7}|:)|fe80:(:[0-9A-Fa-f]{0,4}){0,4}%[0-9a-zA-Z]{1,}|::(ffff(:0{1,4}){0,1}:){0,1}((25[0-5]|(2[0-4]|1{0,1}[0-9]){0,1}[0-9])\.){3,3}(25[0-5]|(2[0-4]|1{0,1}[0-9]){0,1}[0-9])|([0-9A-Fa-f]{1,4}:){1,4}:((25[0-5]|(2[0-4]|1{0,1}[0-9]){0,1}[0-9])\.){3,3}(25[0-5]|(2[0-4]|1{0,1}[0-9]){0,1}[0-9]))$";

            // Check if the input matches either IPv4 or IPv6 pattern
            return Regex.IsMatch(ipAddress, ipv4Pattern) || Regex.IsMatch(ipAddress, ipv6Pattern);
        }


        /// <summary>
        /// this function is used to get country name from ip
        /// </summary>
        /// <param name="ip"></param>
        /// <returns></returns>
        public string getCountryName(string ip)
        {
            if (!IsValidIP(ip) || ip == "::1")
                throw new Exception(Constants.ERROR_INVALID_IP);

            string countryName;
            //Location-->
            var binDirectory = AppDomain.CurrentDomain.BaseDirectory;
            var path = Path.Combine(binDirectory, "Helpers/LocationService", "GeoLite2-Country.mmdb");

            using (var reader = new DatabaseReader(path))
            {
                var response = reader.Country(ip);
                countryName = response.Country.Name.ToString();
            }
            return countryName;
        }
    }
}
