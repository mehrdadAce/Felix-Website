using System;
using System.Collections.Generic;
using System.Net.Http.Formatting;
using FelixWebsite.Core.App_Plugins.AcquisitionTool;
using FelixWebsite.Core.Models.AcquisitionTool;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Umbraco.Web;
using Umbraco.Web.Models.Trees;
using Umbraco.Web.Trees;
using TreeNodeCollection = Umbraco.Web.Models.Trees.TreeNodeCollection;

namespace FelixWebsite.Tests.Sections.AcquisitionTool
{
    [TestClass]
    public class AcquisitionToolTreeControllerTests
    {

        //[TestMethod]
        //public void CreateTree_WhenCalledWithEmptyList_ShouldNotHitUmbraco()
        //{
        //    // Arrange
        //    var acquisitionList = new List<AcquisitionInformation>();
        //    var treeControllerMock = new Mock<TreeController>();
        //    var treeNodes = new TreeNodeCollection();
        //    var formData = new FormDataCollection(new KeyValuePair<string, string>[1]); 
        //    var context = new Mock<UmbracoContext>();
        //    var controller = new AcquisitionToolTreeController(context.Object);
            
        //    // Act
        //    var nodes = AcquisitionHelper.CreateTree(acquisitionList, "", formData, treeNodes, treeControllerMock.Object.CreateTreeNode);
        //    // Assert
            
            
        //}

        //[TestMethod]
        //public void CreateTree_WhenCalledWithListWithOneAcquisition_ShouldMakeYearMonthAndAcquisitionNode()
        //{
        //    // Arrange

        //    // Act

        //    // Assert
        //}

        //[TestMethod]
        //public void CreateTree_WhenCalledWithListWithTwoDifferentMonths_ShouldMakeTwoDifferentMonthNodes()
        //{

        //}

        //[TestMethod]
        //public void CreateTree_WhenCalledWithTwoDifferentYears_ShouldMakeTwoDifferentYearNodes()
        //{

        //}
    }
}
