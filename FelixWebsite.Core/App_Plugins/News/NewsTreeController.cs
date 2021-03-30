using System.Linq;
using System.Net.Http.Formatting;
using FelixWebsite.Shared.Constants;
using Umbraco.Web.Models.Trees;
using Umbraco.Web.Mvc;
using Umbraco.Web.PublishedContentModels;
using Umbraco.Web.Trees;

namespace FelixWebsite.Core.App_Plugins.News
{
    [PluginController(Constants.SECTION_NAME)]
    [Tree(Constants.SECTION_ALIAS, Constants.TREE_ALIAS, Constants.TREE_NAME)]
    public class NewsTreeController: ApiBaseTreeController
    {
        protected override TreeNodeCollection GetTreeNodes(string id, FormDataCollection queryStrings)
        {
            TreeNodeCollection nodes = new TreeNodeCollection();

            if (id == "-1")
            {
                nodes.Add(CreateTreeNode(SocialMediaTreeNodeId.FACEBOOK_ID, id, queryStrings, "Facebook posts",
                    "~/img/sections/FBIcon.svg", true));
                //nodes.Add(CreateTreeNode(SocialMediaTreeNodeId.LINKEDIN_ID, id, queryStrings, "LinkedIn posts",
                //    "~/img/sections/linkedin.svg", true));
            }
            else
            {
                CreateStoreNodesInTree(id, nodes, queryStrings);
            }

            return nodes;
        }
        
        protected override string CreateRouteWithoutParameterQuery()
        {
            return "/" + Constants.SECTION_ALIAS + "/" + Constants.TREE_ALIAS;
        }
    }
}
