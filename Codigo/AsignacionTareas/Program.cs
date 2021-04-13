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
            String collectionUri = $"https://dev.azure.com/{Organization}";
            String repoName = Project;
            String usuario = Configuration["Usuario"];


            var creds = new VssBasicCredential(string.Empty, Pat);

            // Connect to Azure DevOps Services
            var connection = new VssConnection(new Uri(collectionUri), creds);

            WorkItemTrackingHttpClient witClient = connection.GetClient<WorkItemTrackingHttpClient>();

            // Get 2 levels of query hierarchy items
            List<QueryHierarchyItem> queryHierarchyItems = witClient.GetQueriesAsync(Project, depth: 2).Result;

            // Search for 'My Queries' folder
            QueryHierarchyItem myQueriesFolder = queryHierarchyItems.FirstOrDefault(qhi => qhi.Name.Equals("My Queries"));
            if (myQueriesFolder != null)
            {
                string queryName = "Bugs sin asignar";

                // See if our query already exists under 'My Queries' folder.
                QueryHierarchyItem NoAssignedBugsQuery = null;
                if (myQueriesFolder.Children != null)
                {
                    NoAssignedBugsQuery = myQueriesFolder.Children.FirstOrDefault(qhi => qhi.Name.Equals(queryName));
                }
                if (NoAssignedBugsQuery == null)
                {
                    // if the 'REST Sample' query does not exist, create it.
                    NoAssignedBugsQuery = new QueryHierarchyItem()
                    {
                        Name = queryName,
                        Wiql = "SELECT [System.Id],[System.WorkItemType],[System.Title],[System.AssignedTo],[System.State],[System.Tags] FROM WorkItems WHERE [System.TeamProject] = @project AND [System.WorkItemType] = 'Bug' AND [System.AssignedTo] = ''",
                        IsFolder = false
                    };
                    NoAssignedBugsQuery = witClient.CreateQueryAsync(NoAssignedBugsQuery, Project, myQueriesFolder.Name).Result;
                }

                // run the 'REST Sample' query
                WorkItemQueryResult result = witClient.QueryByIdAsync(NoAssignedBugsQuery.Id).Result;

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
                            List<WorkItem> workItems = witClient.GetWorkItemsAsync(workItemRefs.Select(wir => wir.Id)).Result;
                            foreach (WorkItem workItem in workItems)
                            {
                                JsonPatchDocument patchDocument = new JsonPatchDocument();
                                patchDocument.Add(
                                    new JsonPatchOperation()
                                    {
                                        Operation = Operation.Add,
                                        Path = "/fields/System.AssignedTo",
                                        Value = usuario
                                    }
                                );
                                WorkItem update_result = witClient.UpdateWorkItemAsync(patchDocument, (int)workItem.Id, false).Result;
                            }
                        }
                        skip += batchSize;
                    }
                    while (workItemRefs.Count() == batchSize);
                }
            }


        }
    }
}
