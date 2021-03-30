using System.Globalization;
using System.Linq;
using System.Net.Http.Formatting;
using FelixWebsite.Shared.Constants;
using umbraco.BusinessLogic.Actions;
using Umbraco.Web.Models.Trees;
using Umbraco.Web.PublishedContentModels;
using Umbraco.Web.Trees;

namespace FelixWebsite.Core.App_Plugins
{
    public abstract class ApiBaseTreeController: TreeController
    {
        public void CreateStoreNodesInTree(string parentId, TreeNodeCollection nodes, FormDataCollection queryStrings)
        {
            var felixGroup = Umbraco.TypedContentAtRoot().OfType<GroupHome>().FirstOrDefault();
            if (felixGroup == null)
            {
                //    "error tonen";
                return;
            }
            var business = felixGroup.Children.OfType<Businesses>().FirstOrDefault();
            if (business == null) return;
            var allStores = business.Children.OfType<BusinessBrand>();
            foreach (var store in allStores)
            {
                var storeId = store.Id;
                var route = CreateRouteWithoutParameterQuery();
                var parameterQuery = "storeId=" + storeId + "&pageId=";
                switch (parentId)
                {
                    case SocialMediaTreeNodeId.FACEBOOK_ID:
                        var fbPageId = store.FacebookPaginaId;
                        route += "/NavigateToFacebook.html?" + parameterQuery + fbPageId;
                        nodes.Add(CreateTreeNode(store.Id + "", parentId, queryStrings, store.Name, "icon-store color-red", false,
                            route));
                        break;
                    case SocialMediaTreeNodeId.GOOGLE_ID:
                        var googlePageId = store.GooglePaginaId;
                        route += "/NavigateToGoogle.html?" + parameterQuery + googlePageId;
                        nodes.Add(CreateTreeNode(store.Id + "", parentId, queryStrings, store.Name, "icon-store color-red", false,
                            route));
                        break;
                    case SocialMediaTreeNodeId.LINKEDIN_ID:
                        break;
                }
            }
        }

        protected abstract string CreateRouteWithoutParameterQuery();
        protected override MenuItemCollection GetMenuForNode(string id, FormDataCollection queryStrings)
        {
            var menu = new MenuItemCollection();
            menu.Items.Add<RefreshNode, ActionRefresh>(Services.TextService.Localize(ActionRefresh.Instance.Alias, CultureInfo.CurrentCulture));
            return menu;
        }
    }
}
