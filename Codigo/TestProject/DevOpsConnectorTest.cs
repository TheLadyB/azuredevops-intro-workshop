using AsignacionTareas;
using Microsoft.Extensions.Configuration;
using Microsoft.TeamFoundation.WorkItemTracking.WebApi.Models;
using System;
using System.Configuration;
using System.IO;
using Xunit;

namespace TestProject
{
    public class DevOpsConnectorTest 
    {
        public static IConfigurationRoot Configuration;
        private DevOpsConnector _connector { get; set; }
        private String _project { get; set; }
        private String _usuario { get; set; }
        public DevOpsConnectorTest() {
            Configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetParent(AppContext.BaseDirectory).FullName)
                .AddJsonFile("appsettings.json", false)
                .Build();

            String Organization = Configuration["Organization"];
            String Pat = Configuration["Pat"];
            _project = Configuration["Project"];
            _usuario = Configuration["Usuario"];

            _connector = new DevOpsConnector(Organization, Pat);
        }

        [Fact]
        public void GetUnnasignedItemsTest()
        {
            WorkItemQueryResult result = _connector.GetUnnasignedItems(_project);
            Assert.NotNull(result);

        }
    }
}
