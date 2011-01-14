#region Copyright
// Copyright Syncfusion Inc. 2001 - 2010. All rights reserved.
// Use of this code is subject to the terms of our license.
// A copy of the current license can be obtained at any time by e-mailing
// licensing@syncfusion.com. Any infringement will be prosecuted under
// applicable laws. 
#endregion

using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.Xml.Linq;
using Manoli.Utils.CSharpFormat;
using YTech.Learn.SharpMVC.Web.Controllers.BrowserClasses;
using Syncfusion.Mvc.Shared;
using Syncfusion.Mvc.Tools;


public abstract class MyBaseController : Controller
{
    string description;

    ArrayList FileContentList = new ArrayList();

    ArrayList FileNametoDisplay = new ArrayList();

    ArrayList FileFormatreq = new ArrayList();

    string jQueruUIDiv = "{0}<img class=\"jQueryUiIcon\" src=\"{1}\" title=\"Powered by jQuery UI\" />";
    ///// <summary>
    ///// Initializes the controller.
    ///// </summary>
    ///// <param name="requestContext">The request context.</param>
    protected override void Initialize(RequestContext requestContext)
    {
        base.Initialize(requestContext);
        this.MainAccordion();
        this.MainTabs();
        this.GetHeading();
        this.GetProductList();
    }
    protected bool IsAjaxCall()
    {
        return ControllerContext.HttpContext.Request.Headers["X-Requested-With"] == "XMLHttpRequest";
    }

    //public MvcSampleBrowser.Models.Northwind SqlCE
    //{
    //    get
    //    {
    //        AppDomain.CurrentDomain.SetData("SQLServerCompactEditionUnderWebHosting", true);
    //        string connectionString = @"Data Source=|DataDirectory|\Northwind.sdf";
    //        return new MvcSampleBrowser.Models.Northwind(connectionString);
    //    }
    //}
    //public MvcSampleBrowser.Models.EFModels.PUBSEntities EntitySQL
    //{
    //    get
    //    {
    //        return new MvcSampleBrowser.Models.EFModels.PUBSEntities();
    //    }
    //}
    //public MvcSampleBrowser.Models.PUBSEntities1 OutlookSQL
    //{
    //    get
    //    {
    //        return new MvcSampleBrowser.Models.PUBSEntities1();
    //    }
    //}
    ///// <summary>
    ///// Accordions this instance.
    ///// </summary>
    public void MainAccordion()
    {
        string DynamicUrl = System.Web.HttpContext.Current.Request.RawUrl.ToString();
        //    string order = "-1";
        string[] subURL = { };
        string[] selectedItem = { };

        // Checks for query string format
        if (DynamicUrl.Contains('?'))
        {
            subURL = DynamicUrl.Split('?');
            selectedItem = subURL[1].Split('=');
            DynamicUrl = subURL[0];
        }


        AccordionModel accordionModel = new AccordionModel();
        DirectoryInfo dir = new DirectoryInfo(HttpContext.Request.PhysicalApplicationPath + "Views");
        FileInfo[] files = dir.GetFiles("Category.xml", SearchOption.AllDirectories);
        XDocument doc = new XDocument();
        XElement root = new XElement("root");

        // Reads the file
        foreach (FileInfo fileInfo in files)
        {
            root.Add(XDocument.Load(fileInfo.FullName).FirstNode);
        }

        // Gets the categories
        var catgories = from category in root.Elements("Category")
                        orderby category.Attribute("Order").Value
                        select new
                        {
                            categName = category.Attribute("Name").Value,
                            categOrder = category.Attribute("Order").Value,
                            subItem = category.Elements("Samples"),
                            jQueryUI = category.Attribute("jQueryUI")
                        };

        foreach (var Categ in catgories)
        {

            string name = Categ.categName;


            var subItems = from category in Categ.subItem
                           orderby category.Attribute("order").Value
                           select new
                           {
                               subItemName = category.Attribute("displayname").Value,
                               subItemUrl = Url.Content(String.Format("~/{0}", category.Attribute("url").Value)),
                               child = category.FirstNode,
                               filesList = category.Elements("FilesList").Elements("File")
                               
                           };

            //Gets the sub categories
            foreach (var subCateg in subItems)
            {
                TagBuilder div = new TagBuilder("div");
                HtmlA anchor = new HtmlA("", subCateg.subItemName, subCateg.subItemUrl);

                if (DynamicUrl.ToLower() == subCateg.subItemUrl.ToLower())
                {
                    var OtherFilesList = from category in subCateg.filesList
                                         select new
                                         {
                                             subItemName = category.Attribute("displayname").Value,
                                             subItemUrl = category.Attribute("url").Value
                                         };
                    foreach (var file in OtherFilesList)
                    {
                        string FileContent = string.Empty;
                        ReadFileStreams MyStream = new ReadFileStreams();
                        List<string> Filestream = new List<string>();
                        Filestream = MyStream.GetFileData(HttpContext.Request.PhysicalApplicationPath + file.subItemUrl);
                        foreach (string m in (IEnumerable)Filestream)
                        {
                            if (m != null)
                            {
                                FileContent += m.ToString();
                            }
                        }
                        FileContentList.Add(FileContent);
                        FileNametoDisplay.Add(file.subItemName);
                        int ExtensionStart = (file.subItemUrl).LastIndexOf(".");
                        string Extension = file.subItemUrl.Substring(ExtensionStart + 1,file.subItemUrl.Length - ExtensionStart - 1);
                        CSharpFormat FileCSFormat = new CSharpFormat();
                        HtmlFormat FileHtmlFormat = new HtmlFormat();
                        string FileFormatneed;
                        if (Extension == "cs")
                        {
                            FileFormatneed = FileCSFormat.FormatCode(FileContent);
                        }
                        else
                        {
                            FileFormatneed = FileHtmlFormat.FormatCode(FileContent);
                        }
                        FileFormatreq.Add(FileFormatneed);
                    }
                    description = ((System.Xml.Linq.XText)(subCateg.child)).Value;
                    ViewData["CurrentParent"] = Categ.categName;
                    ViewData["CurrentItem"] = subCateg.subItemName;
                }
                div.InnerHtml = anchor.ToString();

            }
        }

        accordionModel.Collapsible = true;
        accordionModel.Navigation = false;

        accordionModel.CustomCss = "MyAccordionStyle";


        accordionModel.AutoHeight = false;
        ViewData["Mainaccordion"] = accordionModel;
    }

    ///// <summary>
    ///// Tabses this instance.
    ///// </summary>
    public void MainTabs()
    {
        string viewContent = string.Empty;
        string controllerContent = string.Empty;
        List<string> Filestream = PlaceTabViewContent("View", HttpContext.Request.PhysicalApplicationPath + "Views");

        foreach (string m in (IEnumerable)Filestream)
        {
            if (m != null)
            {

                viewContent += m.ToString();

            }
        }

        controllerContent = PlaceTabContent("Controller", HttpContext.Request.PhysicalApplicationPath + "Controllers");


        // View
        HtmlFormat format = new HtmlFormat();
        string newFormat = format.FormatCode(viewContent);


        // controller
        CSharpFormat csFormat = new CSharpFormat();
        string csForm = csFormat.FormatCode(controllerContent);

        // Generating Source/Html tabs using JQuery Tabs
        TagBuilder tabDiv = new TagBuilder("div");
        tabDiv.GenerateId("main_Tab");
        tabDiv.MergeAttribute("style", "visibility:hidden");

        TagBuilder tabUL = new TagBuilder("ul");

        TagBuilder descriptionTab = new TagBuilder("li");
        TagBuilder descriptionRef = new TagBuilder("a");
        descriptionRef.MergeAttribute("href", "#tabs-1");
        descriptionRef.InnerHtml = "Description";
        descriptionTab.InnerHtml = descriptionRef.ToString();

        TagBuilder viewTab = new TagBuilder("li");
        TagBuilder viewRef = new TagBuilder("a");
        viewRef.MergeAttribute("href", "#tabs-2");
        viewRef.InnerHtml = "View";
        viewTab.InnerHtml = viewRef.ToString();

        string OtherFilesTab = string.Empty;
        for (int LoopCount = 0; LoopCount < FileContentList.Count; LoopCount++)
        {
            string FileFormat = string.Empty;
            TagBuilder PartialviewTab = new TagBuilder("li");
            TagBuilder PartialviewRef = new TagBuilder("a");
            if (FileContentList[LoopCount].ToString() != string.Empty)
            {                
                PartialviewRef.MergeAttribute("href", "#tabs-" + (4+LoopCount));
                PartialviewRef.InnerHtml = FileNametoDisplay[LoopCount].ToString();
                PartialviewTab.InnerHtml = PartialviewRef.ToString();
            }
            OtherFilesTab += PartialviewTab.ToString();
        }



        TagBuilder ControllerTab = new TagBuilder("li");
        TagBuilder ControllerRef = new TagBuilder("a");
        ControllerRef.MergeAttribute("href", "#tabs-3");
        ControllerRef.InnerHtml = "Controller";
        ControllerTab.InnerHtml = ControllerRef.ToString();

        tabUL.InnerHtml = descriptionTab.ToString() + viewTab.ToString();

        tabUL.InnerHtml += ControllerTab.ToString();
        if (OtherFilesTab != string.Empty)
        {
            tabUL.InnerHtml += OtherFilesTab;
        }
        tabDiv.InnerHtml += tabUL.ToString();

        TagBuilder tab1Div = new TagBuilder("div");
        tab1Div.AddCssClass("maintab");
        tab1Div.GenerateId("tabs-1");


        TagBuilder descriptionDiv = new TagBuilder("p");
        descriptionDiv.InnerHtml = description;
        tab1Div.InnerHtml = descriptionDiv.ToString();

        TagBuilder tab2Div = new TagBuilder("div");
        tab2Div.GenerateId("tabs-2");
        tab2Div.AddCssClass("maintab");


        TagBuilder viewDiv = new TagBuilder("p");
        viewDiv.InnerHtml = newFormat.ToString();
        tab2Div.InnerHtml = viewDiv.ToString();

        string OtherFilesDiv = string.Empty;
        if (OtherFilesTab != string.Empty)
        {
            for (int LoopCount = 0; LoopCount < FileContentList.Count; LoopCount++)
            {
                TagBuilder tabPartialViewDiv = new TagBuilder("div");
                TagBuilder PartialViewDiv = new TagBuilder("p");
                tabPartialViewDiv.GenerateId("tabs-" + (4 + LoopCount));
                tabPartialViewDiv.AddCssClass("maintab");

                PartialViewDiv.InnerHtml = FileFormatreq[LoopCount].ToString();
                tabPartialViewDiv.InnerHtml = PartialViewDiv.ToString();
                OtherFilesDiv += tabPartialViewDiv.ToString();
            }
        }

        TagBuilder tab3Div = new TagBuilder("div");
        tab3Div.GenerateId("tabs-3");
        tab3Div.AddCssClass("maintab");


        TagBuilder controllerDiv = new TagBuilder("p");
        controllerDiv.InnerHtml = csForm.ToString();
        tab3Div.InnerHtml = controllerDiv.ToString();

        tabDiv.InnerHtml += tab1Div.ToString();
        tabDiv.InnerHtml += tab2Div.ToString();
        tabDiv.InnerHtml += tab3Div.ToString();
        if (OtherFilesDiv != string.Empty)
        {
            tabDiv.InnerHtml += OtherFilesDiv;
        }
        ViewData["TabContent"] = tabDiv;


        TabModel tab = new TabModel();


        tab.CustomCss = "Mytabstyle";
        ViewData["tabModel"] = tab;
    }

    public static List<string> PlaceTabViewContent(string LoadType, string Path)
    {
        ReadFileStreams MyStream = new ReadFileStreams();
        HttpContext context = System.Web.HttpContext.Current;
        String DynamicUrl = string.Empty;
        if (context.Request.PhysicalPath.Length > context.Request.PhysicalApplicationPath.Length)
            DynamicUrl = context.Request.PhysicalPath.Remove(0, context.Request.PhysicalApplicationPath.Length).Replace("\\", "/");
        List<string> Filestream = new List<string>();

        string[] StreamSplit = DynamicUrl.Split('/');

        if (StreamSplit.Length >= 2 && LoadType == "View")
        {  //LoadView.

            Filestream = MyStream.GetFileData(Path + @"\" + StreamSplit[0] + @"\" + StreamSplit[1] + ".aspx");
        }
        if (StreamSplit.Length >= 2 && LoadType == "PartialView")
        {  //LoadView.
            Filestream = MyStream.GetFileData(Path + @"\" + StreamSplit[0] + @"\" + StreamSplit[1] + "PartialView.ascx");
        }

        return Filestream;
    }


    public static string PlaceTabContent(string LoadType, string Path)
    {

        ReadFileStreams MyStream = new ReadFileStreams();
        HttpContext context = System.Web.HttpContext.Current;
        String DynamicUrl = string.Empty;
        if (context.Request.PhysicalPath.Length > context.Request.PhysicalApplicationPath.Length)
            DynamicUrl = context.Request.PhysicalPath.Remove(0, context.Request.PhysicalApplicationPath.Length).Replace("\\", "/");
        List<string> Filestream = new List<string>();
        string CatchStreamStr = "";


        string[] StreamSplit = DynamicUrl.Split('/');

        if (StreamSplit.Length >= 2)
        {

            if (LoadType == "Controller")
            {
                if (StreamSplit[1] == "edit" || StreamSplit[1] == "delete")
                    StreamSplit[1] = "CRUD";
                if (StreamSplit[1] == "orders")
                    StreamSplit[1] = "ConditionalFormatting";
                //LoadController.
                Filestream = MyStream.GetFileData(Path + @"\" + StreamSplit[0] + @"\" + StreamSplit[1] + "Controller.cs");
                CatchStreamStr = "";
                foreach (string StreamStr in Filestream)
                {
                    CatchStreamStr = CatchStreamStr + StreamStr;
                }
            }
        }

        return CatchStreamStr;
    }

    /// <summary>
    /// Gets teh control heading
    /// </summary>
    public void GetHeading()
    {
        DirectoryInfo dir = new DirectoryInfo(HttpContext.Request.PhysicalApplicationPath);
        FileInfo[] files = dir.GetFiles("ControlHeading.xml", SearchOption.AllDirectories);
        XDocument doc = new XDocument();
        XElement root = new XElement("root");

        // Reads the file
        foreach (FileInfo fileInfo in files)
        {
            root.Add(XDocument.Load(fileInfo.FullName).FirstNode);
        }

        // Gets the Control heading
        var catgories = from category in root.Elements("Heading").Elements("Control")

                        select new
                        {
                            subItem = category.Attribute("name").Value

                        };


        foreach (var Categ in catgories)
        {
            ViewData["Heading"] = Categ.subItem;
        }

        // Gets the URL for the top icons

        var items = from category in root.Elements("Heading").Elements("Item")

                    select new
                    {
                        Name = category.Attribute("Name").Value,
                        URL = category.Attribute("url").Value

                    };


        foreach (var Categ in items)
        {

            switch (Categ.Name)
            {
                case "Product":
                    ViewData["ProductURL"] = Categ.URL;
                    break;
                case "Support":
                    ViewData["ProductSettings"] = Categ.URL;
                    break;
                case "Help":
                    ViewData["ProductHelp"] = Categ.URL;
                    break;
                case "Sales":
                    ViewData["ProductCart"] = Categ.URL;
                    break;
                case "Home":
                    ViewData["Home"] = Url.Content(Categ.URL);
                    break;
                case "Trial":
                    ViewData["Trial"] = Url.Content(Categ.URL);
                    break;

            }

        }
    }


    /// <summary>
    /// Gets the Other Product List
    /// </summary>
    public void GetProductList()
    {
        //To get into Parent Directory "MVC" from CurrentApplicationPath
        DirectoryInfo currentPath = new DirectoryInfo(HttpContext.Request.PhysicalApplicationPath).Parent.Parent;
        DirectoryInfo MVCPath = currentPath.Parent;

        //Getting the currentProduct name and XML file path
        string currentProduct = currentPath.ToString().TrimEnd(".MVC".ToCharArray());
        string filePath = MVCPath.FullName + @"\ProductList.xml";

        //Generating ProductList.xml file.

        //Creating XDocument
        XDocument xmlDoc = new XDocument();
        XElement productList = new XElement("ProductList");

        DirectoryInfo[] dir = MVCPath.GetDirectories();
        int numberOfProduct = dir.Length;
        XElement[] prod = new XElement[numberOfProduct];

        //Loop to get all the sub-directories inside MVC and write into ProductList.xml
        for (int temp_i = 0; temp_i < numberOfProduct; temp_i++)
        {
            prod[temp_i] = new XElement("Product");
           switch (dir[temp_i].ToString())
            {
                /*  Other Product List */
                case "Grid.MVC":
                    prod[temp_i].SetAttributeValue("Name", "Grid");
                    prod[temp_i].SetAttributeValue("Index", 1);
                    break;
                case "Tools.MVC":
                    prod[temp_i].SetAttributeValue("Name", "Tools");
                    prod[temp_i].SetAttributeValue("Index", 2);
                    break;
                case "Chart.MVC":
                    prod[temp_i].SetAttributeValue("Name", "Chart");
                    prod[temp_i].SetAttributeValue("Index", 3);
                    break;
                case "Schedule.MVC":
                    prod[temp_i].SetAttributeValue("Name", "Schedule");
                    prod[temp_i].SetAttributeValue("Index", 4);
                    break;
                case "Gauge.MVC":
                    prod[temp_i].SetAttributeValue("Name", "Gauge");
                    prod[temp_i].SetAttributeValue("Index", 5);
                    break;
                default:
                    continue;
            }
            productList.Add(prod[temp_i]);
        }

        xmlDoc.Add(productList);
        xmlDoc.Save(filePath);


        /* Reading ProductList.xml*/
        XDocument readXML = new XDocument();
        XElement root = new XElement("root");

        root.Add(XDocument.Load(filePath).FirstNode);

        var item = from product in root.Elements("ProductList").Elements("Product")
                   where (product.Attribute("Name").Value.ToLower() != currentProduct.ToLower())
                   orderby product.Attribute("Index").Value
                   select new
                   {
                       Name = product.Attribute("Name").Value
                   };

        //Creating List for ProductList
        List<string> prodList = new List<string>();
        foreach (var pro in item)
        {
            prodList.Add(pro.Name);
        }

        ViewData["ProductList"] = prodList;

        xmlDoc = null;
        readXML = null;
    }

}

