using Microsoft.TeamFoundation.WorkItemTracking.WebApi;
using Microsoft.TeamFoundation.WorkItemTracking.WebApi.Models;
using Microsoft.VisualStudio.Services.Common;
using Microsoft.VisualStudio.Services.WebApi;
using Microsoft.VisualStudio.Services.WebApi.Patch;
using Microsoft.VisualStudio.Services.WebApi.Patch.Json;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AsignacionTareas
{
    public class DevOpsConnector
    {
        private String _organization_uri { get; set; }
        private VssBasicCredential _credentials { get; set; }

        public DevOpsConnector(String Organization, String Pat) {
            _organization_uri = $"https://dev.azure.com/{Organization}";
            _credentials = new VssBasicCredential(string.Empty, Pat);
        }


        public WorkItemQueryResult GetUnnasignedItems(String Project)
        {
            var connection = new VssConnection(new Uri(_organization_uri), _credentials);

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
                return result;

            }
            else
            {
                throw new Exception($"My Queries folder not found in Project {Project}");
            }
        }

        public WorkItem UpdateAssignedUser(WorkItem Item, String User_mail) {
            var connection = new VssConnection(new Uri(_organization_uri), _credentials);
            WorkItemTrackingHttpClient witClient = connection.GetClient<WorkItemTrackingHttpClient>();

            JsonPatchDocument patchDocument = new JsonPatchDocument();
            patchDocument.Add(
                new JsonPatchOperation()
                {
                    Operation = Operation.Add,
                    Path = "/fields/System.AssignedTo",
                    Value = User_mail
                }
            );
            WorkItem update_result = witClient.UpdateWorkItemAsync(patchDocument, (int)Item.Id, false).Result;
            return update_result;
        }

        public List<WorkItem> GetWorkItems(IEnumerable<WorkItemReference> workItemRefs) {
            var connection = new VssConnection(new Uri(_organization_uri), _credentials);
            WorkItemTrackingHttpClient witClient = connection.GetClient<WorkItemTrackingHttpClient>();
            return witClient.GetWorkItemsAsync(workItemRefs.Select(wir => wir.Id)).Result;
        }
    }
}
