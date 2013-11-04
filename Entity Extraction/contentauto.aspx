<%@ Page Title="" Language="C#" MasterPageFile="~/Templates/Layout.master" AutoEventWireup="true" CodeFile="contentauto.aspx.cs" Inherits="Content" %>

<%@ Register Assembly="Ektron.Cms.Controls" Namespace="Ektron.Cms.Controls" TagPrefix="CMS" %>
<%@ Register Src="~/Workarea/PageBuilder/PageControls/PageHost.ascx" TagPrefix="PB" TagName="PageHost" %>
<%@ Register Src="~/Workarea/PageBuilder/PageControls/DropZone.ascx" TagPrefix="PB" TagName="DropZone" %>
<%@ Register Assembly="Ektron.Cms.Widget" Namespace="Ektron.Cms.PageBuilder" TagPrefix="PB" %>
<%@ Register Src="../userControls/Carousel.ascx" TagName="Carousel" TagPrefix="CMS" %>
<%@ Register Src="../userControls/DestinationsMenu.ascx" TagName="DestinationsMenu" TagPrefix="CMS" %>

<asp:Content ID="contentHead" ContentPlaceHolderID="head" runat="Server">
    <title>Trek-On Travel</title>
</asp:Content>

<asp:Content ID="contentCarousel" ContentPlaceHolderID="carouselContent" runat="Server">
    <!-- PageBuilder ribbon -->
    <pb:pagehost id="PageHost1" runat="server" />

    <div id="carouselCategories">
        <div id="myCarousel" class="carousel slide hidden-phone">
            <div class="carousel-inner">
                <asp:Literal runat="server" ID="bannerImage" />
            </div>
        </div>
    </div>

</asp:Content>

<asp:Content ID="contentBody" ContentPlaceHolderID="mainContent" runat="Server">
    <div class="row-fluid">

        <div class="span3 attractions">
            <asp:Literal ID="moreone" runat="server" Visible="false"></asp:Literal>
            <h4 id="H1" runat="server" visible="false"><%= Resources.StaticData.noAttractions %></h4>
            <asp:ListView ID="moreoneList" runat="server">

                <LayoutTemplate>
                    <asp:PlaceHolder ID="itemPlaceholder" runat="server" />
                </LayoutTemplate>

                <ItemTemplate>

                    <div class="media cid<%# Eval("Id") %>>">

                        <div class="media-body">
                            <h4 class="media-heading"><a href="<%# Eval("QuickLink") %>"><%# Eval("Title") %></a></h4>
                            <p><%# Eval("Description") %>&nbsp;&nbsp;&nbsp;<b><a href="<%# Eval("QuickLink") %>"><%= Resources.StaticData.ctaTextViewMore %> &raquo;</a></b></p>
                        </div>
                    </div>

                </ItemTemplate>

            </asp:ListView>

            <asp:Literal ID="moretwo" runat="server" Visible="false"></asp:Literal>
            <h4 id="H2" runat="server" visible="false"><%= Resources.StaticData.noAttractions %></h4>
            <asp:ListView ID="moretwoList" runat="server">

                <LayoutTemplate>
                    <asp:PlaceHolder ID="itemPlaceholder" runat="server" />
                </LayoutTemplate>

                <ItemTemplate>

                    <div class="media cid<%# Eval("Id") %>>">

                        <div class="media-body">
                            <h4 class="media-heading"><a href="<%# Eval("QuickLink") %>"><%# Eval("Title") %></a></h4>
                            <p><%# Eval("Description") %>&nbsp;&nbsp;&nbsp;<b><a href="<%# Eval("QuickLink") %>"><%= Resources.StaticData.ctaTextViewMore %> &raquo;</a></b></p>
                        </div>
                    </div>

                </ItemTemplate>

            </asp:ListView>

        </div>
        <div class="span9 row-fluid">
            <div class="span8" style="position: relative">
                <h3 class="hidden-desktop hidden-tablet">
                    <asp:Literal ID="phoneHeading" runat="server" /></h3>
                <div id="bodyContent">
                    <CMS:ContentBlock runat="server" DynamicParameter="id" SuppressWrapperTags="True" />
                </div>
                <asp:Literal runat="server" ID="merchandiseHeader"></asp:Literal>
                <div class="row-fluid">
                    
                    <asp:ListView ID="merchandiseList" runat="server">

                        <LayoutTemplate>
                            <asp:PlaceHolder ID="itemPlaceholder" runat="server" />
                        </LayoutTemplate>

                        <ItemTemplate>
                            <div class="span6">
                                <div class="attractions">
                                    <div class="media" style="margin-bottom: 0px;">
                                        <div class="media-image">
                                            <img alt="" src="<%# Eval("SmartForm.Image") %>" />
                                            <span class="mask"></span>
                                        </div>
                                        <div class="media-body attr">
                                            <h4 class="media-heading"><a href="<%# Eval("Content.QuickLink") %>"><%# Eval("SmartForm.Short_title") %></a></h4>
                                            <p><%# Eval("SmartForm.Summary") %>&nbsp;&nbsp;&nbsp;<b><a href="<%# Eval("Content.QuickLink") %>"><%= Resources.StaticData.ctaTextViewMore %>&raquo;</a></b></p>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </ItemTemplate>
                    </asp:ListView>
                </div>
                <div class="row-fluid">
                    <div class="span6">
                        <h4 style="color: #fff; background-color: #000; width: 100%; padding: 3px;">People Mentioned Here</h4>
                        <asp:ListView ID="peopleList" runat="server">
                            <LayoutTemplate>
                                <asp:PlaceHolder ID="itemPlaceholder" runat="server" />
                            </LayoutTemplate>
                            <ItemTemplate>
                                <div>
                                    <h5><%# Eval("name") %></h5>
                                    <div style="<%# Eval("homepageShow") %>"><span class="<%# Eval("homepageClass") %>"></span><a href="<%# Eval("homepageLink") %>"><%# Eval("homepageName") %></a></div>
                                    <%--<div style="<%# Eval("websiteShow") %>"><span class="<%# Eval("websiteClass") %>"></span><a href="<%# Eval("websitelink") %>"><%# Eval("websitename") %></a></div>--%>
                                    <div><span class="icon-search"></span>Search for more about&nbsp;<a href="<%# Eval("searchlink") %>"><%# Eval("name") %></a></div>
                                </div>
                            </ItemTemplate>
                        </asp:ListView>
                    </div>
                    <div class="span6">
                        <h4 style="color: #fff; background-color: #000; width: 100%; padding: 3px;">Organizations Mentioned Here</h4>
                        <asp:ListView ID="orgList" runat="server">

                            <LayoutTemplate>

                                <asp:PlaceHolder ID="itemPlaceholder" runat="server" />
                            </LayoutTemplate>

                            <ItemTemplate>
                                <div>
                                    <h5><%# Eval("name") %></h5>
                                    <div style="<%# Eval("homepageShow") %>"><span class="<%# Eval("homepageClass") %>"></span><a href="<%# Eval("homepageLink") %>"><%# Eval("homepageName") %></a></div>
                                    <%--<div style="<%# Eval("websiteShow") %>"><span class="<%# Eval("websiteClass") %>"></span><a href="<%# Eval("websitelink") %>"><%# Eval("websitename") %></a></div>--%>
                                    <div><span class="icon-search"></span>Search for more about&nbsp;<a href="<%# Eval("searchlink") %>"><%# Eval("name") %></a></div>
                                </div>
                            </ItemTemplate>
                        </asp:ListView>
                    </div>
                </div>

            </div>
            <div class="span4 attractions">
               <asp:Literal ID="morethree" runat="server" Visible="false"></asp:Literal>
                <h4 id="noAttractions" runat="server" visible="false"><%= Resources.StaticData.noAttractions %></h4>
                <asp:ListView ID="morethreeList" runat="server">
                    <LayoutTemplate>
                        <asp:PlaceHolder ID="itemPlaceholder" runat="server" />
                    </LayoutTemplate>
                    <ItemTemplate>
                        <div class="media cid<%# Eval("Id") %>>">
                            <div class="media-body">
                                <h4 class="media-heading"><a href="<%# Eval("QuickLink") %>"><%# Eval("Title") %></a></h4>
                                <p><%# Eval("Description") %>&nbsp;&nbsp;&nbsp;<b><a href="<%# Eval("QuickLink") %>"><%= Resources.StaticData.ctaTextViewMore %> &raquo;</a></b></p>
                            </div>
                        </div>
                    </ItemTemplate>
                </asp:ListView>
                <asp:Literal ID="morefour" runat="server" Visible="false"></asp:Literal>
                <h4 id="H3" runat="server" visible="false"><%= Resources.StaticData.noAttractions %></h4>
                <asp:ListView ID="morefourList" runat="server">
                    <LayoutTemplate>
                        <asp:PlaceHolder ID="itemPlaceholder" runat="server" />
                    </LayoutTemplate>
                    <ItemTemplate>
                        <div class="media cid<%# Eval("Id") %>>">
                            <div class="media-body">
                                <h4 class="media-heading"><a href="<%# Eval("QuickLink") %>"><%# Eval("Title") %></a></h4>
                                <p><%# Eval("Description") %>&nbsp;&nbsp;&nbsp;<b><a href="<%# Eval("QuickLink") %>"><%= Resources.StaticData.ctaTextViewMore %> &raquo;</a></b></p>
                            </div>
                        </div>
                    </ItemTemplate>
                </asp:ListView>
            </div>
        </div>
        <!--/span6-->
    </div>
    <!--/container-fluid-->
</asp:Content>
