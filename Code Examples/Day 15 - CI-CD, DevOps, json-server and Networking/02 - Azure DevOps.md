# 02 - Azure DevOps
 
**Azure DevOps** is Microsoft's suite of collaboration and delivery tools. The pieces you'll actually touch:
 
| Service | Purpose |
| --- | --- |
| **Azure Repos** | Git repositories (branches, PRs, branch policies). |
| **Azure Pipelines** | The CI/CD engine — builds, tests, deployments defined in YAML (or the classic visual editor). |
| **Azure Boards** | Work items, backlogs, sprints (Kanban/Scrum). |
| **Azure Artifacts** | Hosted package feeds (NuGet, npm, etc.). |
| **Azure Test Plans** | Manual and exploratory test management. |
 
In the flow above, **Azure Repos** hosts the branch and PR, and **Azure Pipelines** *is* the "CI/CD Pipeline" box running `build → test → deploy`.
 
> [!NOTE] Azure DevOps vs. GitHub Actions
> Both do CI/CD. Azure DevOps is the older, enterprise-oriented suite (boards + repos + pipelines in one); GitHub Actions is CI/CD built into GitHub. Microsoft owns both; many teams now use GitHub for source + Actions for pipelines, but Azure DevOps remains common in enterprise .NET shops.
