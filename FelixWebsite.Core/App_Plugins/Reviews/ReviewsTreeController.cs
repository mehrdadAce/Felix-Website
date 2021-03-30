using System.Globalization;
using System.Linq;
using System.Net.Http.Formatting;
using FelixWebsite.Shared.Constants;
using umbraco.BusinessLogic.Actions;
using Umbraco.Web.Models.Trees;
using Umbraco.Web.Mvc;
using Umbraco.Web.PublishedContentModels;
using Umbraco.Web.Trees;

namespace FelixWebsite.Core.App_Plugins.Reviews
{
    [PluginController(Constants.REVIEW_SECTION_NAME)]
    [Tree(Constants.REVIEW_SECTION_ALIAS, Constants.REVIEW_TREE_ALIAS, Constants.REVIEW_TREE_NAME)]
    public class ReviewsTreeController : ApiBaseTreeController
    {
        protected override TreeNodeCollection GetTreeNodes(string id, FormDataCollection queryStrings)
        {
            TreeNodeCollection nodes = new TreeNodeCollection();

            if (id == "-1")
            {
                nodes.Add(CreateTreeNode(SocialMediaTreeNodeId.FACEBOOK_ID, id, queryStrings, "Facebook reviews", "~/img/sections/FBIcon.svg", true));
                nodes.Add(CreateTreeNode(SocialMediaTreeNodeId.GOOGLE_ID, id, queryStrings, "Google reviews", "~/img/sections/business.svg", true));
            }
            else
            {
                CreateStoreNodesInTree(id, nodes, queryStrings);
            }

            return nodes;
        }

        protected override string CreateRouteWithoutParameterQuery()
        {
            return "/" + Constants.REVIEW_SECTION_ALIAS + "/" + Constants.REVIEW_TREE_ALIAS;
        }
    }
}

