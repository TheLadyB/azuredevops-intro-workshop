using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.TeamFoundation.SourceControl.WebApi;
using Microsoft.TeamFoundation.WorkItemTracking.WebApi;
using Microsoft.TeamFoundation.WorkItemTracking.WebApi.Models;
using Microsoft.VisualStudio.Services.Common;
using Microsoft.VisualStudio.Services.WebApi;
using Microsoft.VisualStudio.Services.WebApi.Patch;
using Microsoft.VisualStudio.Services.WebApi.Patch.Json;
using NLog.Fluent;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace AsignacionTareas
{
    class Program
    {
        public static IConfigurationRoot Configuration;
        static void Main(string[] args)
        {
            Configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetParent(AppContext.BaseDirectory).FullName)
                .AddJsonFile("appsettings.json", false)
                .Build();

            String Organization = Configuration["Organization"];
            String Pat = Configuration["Pat"];
            String Project = Configuration["Project"];
            String usuario = Configuration["Usuario"];

            DevOpsConnector connector = new DevOpsConnector(Organization, Pat);
            WorkItemQueryResult result = connector.GetUnnasignedItems(Project);

            if (result.WorkItems.Any())
            {
                int skip = 0;
                const int batchSize = 100;
                IEnumerable<WorkItemReference> workItemRefs;
                do
                {
                    workItemRefs = result.WorkItems.Skip(skip).Take(batchSize);
                    if (workItemRefs.Any())
                    {
                        // get details for each work item in the batch
                        List<WorkItem> workItems = connector.GetWorkItems(workItemRefs);
                        foreach (WorkItem workItem in workItems)
                        {
                            WorkItem update_result = connector.UpdateAssignedUser(workItem, usuario);
                        }
                    }
                    skip += batchSize;
                }
                while (workItemRefs.Count() == batchSize);
            }


        }
    }
}
