using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using Microsoft.WindowsAzure;
using Microsoft.WindowsAzure.Management.WebSites;
using Microsoft.WindowsAzure.Management.WebSites.Models;

namespace AzureAPITestConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            ListSites().Wait();
        }

        async static Task ListSites()
        {
            // Retrieve settings from app settings
            string pfxPath = ConfigurationManager.AppSettings["pfxPath"];
            string pfxPassword = ConfigurationManager.AppSettings["pfxPassword"];
            string subscription = ConfigurationManager.AppSettings["subscription"];

            var cert = new X509Certificate2(pfxPath, pfxPassword);
            var creds = new CertificateCloudCredentials(subscription, cert);
            var client = new WebSiteManagementClient(creds);

            // Enumerate all the webspaces and sites they contain
            foreach (var webSpace in await client.WebSpaces.ListAsync())
            {
                Console.WriteLine(webSpace.Name);
                foreach (var site in await client.WebSpaces.ListWebSitesAsync(webSpace.Name, new WebSiteListParameters()))
                {
                    Console.WriteLine("    " + site.Name);
                }
            }
        }
    }
}
