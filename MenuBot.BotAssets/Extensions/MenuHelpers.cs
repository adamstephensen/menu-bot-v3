using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MenuBot.BotAssets.Extensions
{
    public static class MenuHelpers
    {
        public static string[] getMenuOptions(string menu)
        {
            switch (menu)
            {
                case "Home":
                    return new[]
                    {
                        "Why Azure",
                        "Solutions",
                        "Products",
                        "Documentation",
                        "Pricing",
                        "Support"
                    };

                case "Why Azure":
                    return new[]
                    {
                        "What is Azure",
                        "Azure for Windows Server",
                        "Azure for open source",
                        "Azure Global Infrastructure",
                        "Getting Started",
                        "ICT Risk Management",
                        "Information Services Branch",
                        "Planning",
                        "Software Standards"
                    };
                case "Products":
                    return new[] {
                        "Featured",
                        "AI + Machine Learning",
                        "Analytics",
                        "Compute",
                        "Containers",
                        "Databases",
                        "DevOps",
                        "Networking",
                        "Web"
                    };
                case "Support":
                    return new[] {
                        "Azure Support",
                        "Compare Support Plans",
                        "Support community",
                        "Knowledge Centre",
                        "Azure Status Dashboard"
                    };
                case "Featured":
                    return new[] {
                       "Virtual Machines",
                        "Azure SQL Database",
                        "Azure Cosmos DB"
                    };
                case "AI + Machine Learning":
                    return new[] {
                        "Cognitive Services",
                        "Azure Databricks",
                        "Azure Bot Service",
                        "Machine Learning"
                    };
                
                default:
                    return null;
            };

        }
    }

}
