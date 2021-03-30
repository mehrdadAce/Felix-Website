using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Formatting;
using FelixWebsite.Bdo.Models.Acquisition;
using FelixWebsite.Bll.Services.Interfaces;
using FelixWebsite.Core.App_Start;
using Ninject;
using Umbraco.Core.Logging;
using Umbraco.Web.Models.Trees;
using Umbraco.Web.Mvc;
using Umbraco.Web.Trees;

namespace FelixWebsite.Core.App_Plugins.DamageTool
{
    [PluginController(Constants.DAMAGE_TOOL_NAME)]
    [Tree(Constants.DAMAGE_TOOL_ALIAS, Constants.DAMAGE_TOOL_TREE_ALIAS, Constants.DAMAGE_TOOL_TREE_NAME)]
    public class DamageToolTreeController : TreeController
    {
        private IAcquisitionInformationService acquisitionInformationService;
        private List<UserInformation> sortedUserInformation;

        protected override TreeNodeCollection GetTreeNodes(string id, FormDataCollection queryStrings)
        {
            TreeNodeCollection nodes = new TreeNodeCollection();
            acquisitionInformationService = NinjectWebCommon.bootstrapper.Kernel.Get<IAcquisitionInformationService>();

            if (sortedUserInformation == null)
            {
                var list = acquisitionInformationService.GetAllCompletedDamages();
                sortedUserInformation = list?.OrderBy(element => element.CreatedDate)
                                            ?.ToList() ?? new List<UserInformation>();
            }

            var splitId = id.Split('-');
            if (id == "-1")
            {
                LogHelper.Info(GetType(), "Create the year nodes in the acquisition tool tree");
                CreateYearNodes(sortedUserInformation, id, queryStrings, nodes);
            }
            else if (splitId[0].Equals("year") && splitId.Length == 2)
            {
                LogHelper.Info(GetType(), "Create the month nodes in the acquisition tool tree");
                var year = Convert.ToInt16(id.Split('-')[1]);
                var sortedMonthList = sortedUserInformation.Where(ac => ac.CreatedDate.Year == year).ToList();
                CreateMonthNodes(sortedMonthList, id, queryStrings, nodes);
            }
            else
            {
                LogHelper.Info(GetType(), "Create the person nodes in the acquisition tool tree");
                var year = Convert.ToInt16(splitId[1]);
                var month = Convert.ToInt16(splitId[3]);
                var sortedAcquisitionList = sortedUserInformation.Where(ac => ac.CreatedDate.Month == month && ac.CreatedDate.Year == year).ToList();
                CreateAcquisitionNodes(sortedAcquisitionList, id, queryStrings, nodes);
            }
            return nodes;
        }

        protected override MenuItemCollection GetMenuForNode(string id, FormDataCollection queryStrings)
        {
            throw new System.NotImplementedException();
        }

        public void CreateYearNodes(List<UserInformation> sortedAcquisitionList, string parentId, FormDataCollection queryStrings, TreeNodeCollection nodes)
        {
            try
            {
                var currentYear = 0;
                foreach (var acquisition in sortedAcquisitionList)
                {
                    if (currentYear == acquisition.CreatedDate.Year) continue;
                    var year = acquisition.CreatedDate.Year;
                    var yearId = "year-" + year;
                    var node = CreateTreeNode(yearId, parentId, queryStrings, year + string.Empty, "icon-calendar-alt",
                        true,
                        "/" + Constants.DAMAGE_TOOL_ALIAS + "/" + Constants.DAMAGE_TOOL_TREE_ALIAS +
                        "/OvernameVoorbeeld.html?acquisitionId=" + acquisition.Id);
                    nodes.Add(node);
                    currentYear = year;
                }
            }
            catch (Exception e)
            {
                LogHelper.Error(GetType(), "Something went wrong while creating the year nodes in the acquisition tool tree. The thrown error is: ", e);
            }
        }

        public void CreateMonthNodes(List<UserInformation> sortedAcquisitionList, string parentId, FormDataCollection queryStrings, TreeNodeCollection nodes)
        {
            try
            {
                var currentMonth = 0;
                foreach (var acquisition in sortedAcquisitionList)
                {
                    if (currentMonth == acquisition.CreatedDate.Month) continue;
                    var month = acquisition.CreatedDate.Month;
                    var monthId = parentId + "-month-" + month;
                    var node = CreateTreeNode(monthId, parentId, queryStrings, acquisition.CreatedDate.ToString("MMMM"),
                        "icon-calendar-alt", true,
                        "/" + Constants.DAMAGE_TOOL_ALIAS + "/" + Constants.DAMAGE_TOOL_TREE_ALIAS +
                        "/OvernameVoorbeeld.html?acquisitionId=" + acquisition.Id);
                    nodes.Add(node);
                    currentMonth = month;
                }
            }
            catch (Exception e)
            {
                LogHelper.Error(GetType(), "Something went wrong while creating the month nodes in the acquisition tool tree. The thrown error is: ", e);
            }
        }

        public void CreateAcquisitionNodes(List<UserInformation> sortedAcquisitionList, string parentId, FormDataCollection queryStrings, TreeNodeCollection nodes)
        {
            try
            {
                var sortedByNameAcqList = sortedAcquisitionList.OrderBy(ac => ac.Name);
                foreach (var acquisition in sortedByNameAcqList)
                {
                    var fullname = acquisition.LicensePlate + " " + acquisition.Name;
                    var node = CreateTreeNode(fullname, parentId, queryStrings, fullname, "icon-adressbook", false,
                        "/" + Constants.DAMAGE_TOOL_ALIAS + "/" + Constants.DAMAGE_TOOL_TREE_ALIAS +
                        "/OvernameVoorbeeld.html?acquisitionId=" + acquisition.Id);
                    nodes.Add(node);
                }
            }
            catch (Exception e)
            {
                LogHelper.Error(GetType(), "Something went wrong while creating the person nodes in the acquisition tool tree. The thrown error is: ", e);
            }
        }
    }
}