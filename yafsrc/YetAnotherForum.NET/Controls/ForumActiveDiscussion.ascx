﻿<%@ Control Language="C#" AutoEventWireup="true" EnableViewState="false" Inherits="YAF.Controls.ForumActiveDiscussion"
    CodeBehind="ForumActiveDiscussion.ascx.cs" %>

<div class="col-md-6">
    <asp:PlaceHolder runat="server" ID="ActiveDiscussionPlaceHolder">
                        <div class="card mb-3">
                            <div class="card-header">
                                <span class="fa-stack">
                                    <i class="fas fa-comments fa-2x fa-fw text-secondary mr-1"></i>
                                </span>
                                <YAF:LocalizedLabel runat="server" ID="ActiveDiscussionHeader"
                                                    LocalizedTag="ACTIVE_DISCUSSIONS" />
                            </div>
                            <asp:Repeater runat="server" ID="LatestPosts" 
                                              OnItemDataBound="LatestPosts_ItemDataBound">
                                    <HeaderTemplate>
                                        <ul class="list-group list-group-flush">
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <li class="list-group-item pt-2 pb-0 list-group-item-action">
                                            <h6>
                                                <asp:PlaceHolder runat="server" ID="PostIcon"></asp:PlaceHolder>
                                                <asp:HyperLink ID="TextMessageLink" runat="server" 
                                                               CssClass="font-weight-bold" />
                                                <YAF:ThemeButton runat="server" ID="Info"
                                                                 Icon="info-circle"
                                                                 IconColor="text-info"
                                                                 IconCssClass="fas fa-lg"
                                                                 Type="Link"
                                                                 DataToggle="popover"
                                                                 Size="Small"
                                                                 CssClass="topic-link-popover">
                                                </YAF:ThemeButton>
                                                <div class="btn-group" role="group">
                                                    <YAF:ThemeButton runat="server" ID="GoToLastUnread" 
                                                                     Size="Small"
                                                                     Icon="book-reader"
                                                                     Type="OutlineSecondary"
                                                                     DataToggle="tooltip"
                                                                     TitleLocalizedTag="GO_LASTUNREAD_POST">
                                                    </YAF:ThemeButton>
                                                    <YAF:ThemeButton runat="server" ID="GoToLastPost" 
                                                                     Size="Small"
                                                                     Icon="share-square"
                                                                     Type="OutlineSecondary"
                                                                     DataToggle="tooltip"
                                                                     TitleLocalizedTag="GO_LAST_POST">
                                                    </YAF:ThemeButton>
                                                </div>
                                            </h6>
                                        </li>
                                    </ItemTemplate>
                                    <FooterTemplate>
                                        </ul>
                                    </FooterTemplate>
                                </asp:Repeater>

                            <asp:Panel runat="server" ID="Footer" CssClass="card-footer" >
                                <div class="btn-group float-right" role="group" aria-label="Tools">
                                    <YAF:RssFeedLink ID="RssFeed" runat="server" FeedType="LatestPosts" />
                                </div>
                            </asp:Panel>
                        </div>
            </asp:PlaceHolder>
</div>