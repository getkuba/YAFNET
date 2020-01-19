/* Yet Another Forum.NET
 * Copyright (C) 2003-2005 Bjørnar Henden
 * Copyright (C) 2006-2013 Jaben Cargman
 * Copyright (C) 2014-2020 Ingo Herbote
 * https://www.yetanotherforum.net/
 *
 * Licensed to the Apache Software Foundation (ASF) under one
 * or more contributor license agreements.  See the NOTICE file
 * distributed with this work for additional information
 * regarding copyright ownership.  The ASF licenses this file
 * to you under the Apache License, Version 2.0 (the
 * "License"); you may not use this file except in compliance
 * with the License.  You may obtain a copy of the License at

 * https://www.apache.org/licenses/LICENSE-2.0

 * Unless required by applicable law or agreed to in writing,
 * software distributed under the License is distributed on an
 * "AS IS" BASIS, WITHOUT WARRANTIES OR CONDITIONS OF ANY
 * KIND, either express or implied.  See the License for the
 * specific language governing permissions and limitations
 * under the License.
 */

namespace YAF.Controls
{
    #region Using

    using System;
    using System.Web;
    using System.Web.UI;
    using System.Web.UI.HtmlControls;
    using System.Web.UI.WebControls;

    using YAF.Configuration;
    using YAF.Core.BaseControls;
    using YAF.Core.Extensions;
    using YAF.Dialogs;
    using YAF.Types;
    using YAF.Types.Constants;
    using YAF.Types.Extensions;
    using YAF.Types.Interfaces;
    using YAF.Types.Objects;
    using YAF.Utils;

    #endregion

    /// <summary>
    /// The Header.
    /// </summary>
    public partial class Header : BaseUserControl
    {
        #region Methods

        /// <summary>
        /// The get return url.
        /// </summary>
        /// <returns>
        /// The url.
        /// </returns>
        protected string GetReturnUrl()
        {
            var returnUrl = string.Empty;

            if (this.PageContext.ForumPageType != ForumPages.login)
            {
                returnUrl = HttpContext.Current.Server.UrlEncode(General.GetSafeRawUrl());
            }
            else
            {
                // see if there is already one since we are on the login page
                if (HttpContext.Current.Request.QueryString.GetFirstOrDefault("ReturnUrl").IsSet())
                {
                    returnUrl = HttpContext.Current.Server.UrlEncode(
                        General.GetSafeRawUrl(HttpContext.Current.Request.QueryString.GetFirstOrDefault("ReturnUrl")));
                }
            }

            return returnUrl;
        }

        /// <summary>
        /// Do Logout Dialog
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        protected void LogOutClick([NotNull] object sender, [NotNull] EventArgs e)
        {
            var notification = this.PageContext.CurrentForumPage.Notification.ToType<DialogBox>();

            notification.Show(
                this.GetText("TOOLBAR", "LOGOUT_QUESTION"),
                "Logout?",
                new DialogButton
                    {
                        Text = this.GetText("TOOLBAR", "LOGOUT"),
                        CssClass = "btn btn-primary",
                        ForumPageLink = new ForumLink { ForumPage = ForumPages.logout }
                    },
                new DialogButton { Text = this.GetText("COMMON", "CANCEL"), CssClass = "btn btn-secondary" });
        }

        /// <summary>
        /// The On PreRender event.
        /// </summary>
        /// <param name="e">
        /// the Event Arguments
        /// </param>
        protected override void OnPreRender([NotNull] EventArgs e)
        {
            this.RenderQuickSearch();

            base.OnPreRender(e);
        }

        /// <summary>
        /// Handles the Load event of the Page control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        protected void Page_Load([NotNull] object sender, [NotNull] EventArgs e)
        {
            this.RenderGuestControls();

            this.RenderUserContainer();

            this.RenderMainHeaderMenu();

            this.RenderAdminModMenu();
        }

        /// <summary>
        /// Do Quick Search
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        protected void QuickSearchClick([NotNull] object sender, [NotNull] EventArgs e)
        {
            if (this.searchInput.Text.IsNotSet())
            {
                return;
            }

            BuildLink.Redirect(ForumPages.search, "search={0}", this.Server.UrlEncode(this.searchInput.Text));
        }

        /// <summary>
        /// Render Li and a Item
        /// </summary>
        /// <param name="holder">
        /// The holder.
        /// </param>
        /// <param name="cssClass">
        /// The CSS class.
        /// </param>
        /// <param name="linkText">
        /// The link text.
        /// </param>
        /// <param name="linkToolTip">
        /// The link tool tip.
        /// </param>
        /// <param name="linkUrl">
        /// The link URL.
        /// </param>
        /// <param name="noFollow">
        /// Add no follow to the link
        /// </param>
        /// <param name="showUnread">
        /// The show unread.
        /// </param>
        /// <param name="unread">
        /// The unread.
        /// </param>
        /// <param name="unreadText">
        /// The unread text.
        /// </param>
        /// <param name="isActive">
        /// The is Active.
        /// </param>
        /// <param name="icon">
        /// The icon.
        /// </param>
        private static void RenderMenuItem(
            Control holder,
            string cssClass,
            string linkText,
            string linkToolTip,
            string linkUrl,
            bool noFollow,
            bool showUnread,
            string unread,
            string unreadText,
            bool isActive,
            string icon = "")
        {
            var element = new HtmlGenericControl("li");

            if (cssClass.IsSet())
            {
                element.Attributes.Add("class", "nav-item");
            }

            if (isActive)
            {
                cssClass = $"{cssClass} active";
            }

            if (linkToolTip.IsNotSet())
            {
                linkToolTip = linkText;
            }

            var link = new HyperLink
                           {
                               Target = "_top",
                               ToolTip = linkToolTip,
                               NavigateUrl = linkUrl,
                               Text = icon.IsSet()
                                          ? $"<i class=\"fa fa-{icon} fa-fw\"></i>&nbsp;{linkText}"
                                          : linkText,
                               CssClass = cssClass
                           };

            if (noFollow)
            {
                link.Attributes.Add("rel", "nofollow");
            }

            link.Attributes.Add("data-toggle", "tooltip");

            if (showUnread)
            {
                link.Controls.Add(new LiteralControl(icon.IsSet()
                                                         ? $"<i class=\"fa fa-{icon} fa-fw\"></i>&nbsp;{linkText}&nbsp;"
                                                         : $"{linkText}&nbsp;"));

                var unreadLabel = new Label { CssClass = "badge badge-danger", ToolTip = unreadText, Text = unread };

                unreadLabel.Attributes.Add("data-toggle", "tooltip");

                var unreadLabelText = new Label { CssClass = "sr-only", Text = unreadText };

                link.Controls.Add(unreadLabel);

                link.Controls.Add(unreadLabelText);
            }

            if (cssClass.Equals("nav-link"))
            {
                element.Controls.Add(link);
                holder.Controls.Add(element);
            }
            else
            {
                holder.Controls.Add(link);
            }
        }

        /// <summary>
        ///  Render the Quick Search
        /// </summary>
        private void RenderQuickSearch()
        {
            if (!this.Get<BoardSettings>().ShowQuickSearch
                || !this.Get<IPermissions>().Check(this.Get<BoardSettings>().SearchPermissions))
            {
                return;
            }

            this.quickSearch.Visible = true;

            this.searchInput.Attributes["onkeydown"] =
                $"if(event.which || event.keyCode){{if ((event.which == 13) || (event.keyCode == 13)) {{document.getElementById('{this.doQuickSearch.ClientID}').click();return false;}}}} else {{return true}}; ";
            this.searchInput.Attributes["onfocus"] =
                $"if (this.value == '{this.GetText("TOOLBAR", "SEARCHKEYWORD")}') {{this.value = '';}}";
            this.searchInput.Attributes["onblur"] =
                $"if (this.value == '') {{this.value = '{this.GetText("TOOLBAR", "SEARCHKEYWORD")}';}}";

            this.searchInput.Text = this.GetText("TOOLBAR", "SEARCHKEYWORD");
        }

        /// <summary>
        /// Render the Admin/Moderator Menu Links
        /// </summary>
        private void RenderAdminModMenu()
        {
            // Admin
            if (this.PageContext.IsAdmin)
            {
                this.AdminModHolder.Visible = true;

                this.AdminAdminHolder.Visible = true;
            }

            // Host
            if (this.PageContext.IsHostAdmin)
            {
                this.AdminModHolder.Visible = true;
                this.HostMenuHolder.Visible = true;
            }

            // Moderate
            if (!this.PageContext.IsModeratorInAnyForum)
            {
                return;
            }

            this.AdminModHolder.Visible = true;

            // Admin
            RenderMenuItem(
                this.menuAdminItems,
                "nav-link",
                this.GetText("TOOLBAR", "MODERATE"),
                this.GetText("TOOLBAR", "MODERATE_TITLE"),
                BuildLink.GetLink(ForumPages.moderate_index),
                false,
                this.PageContext.ModeratePosts > 0,
                this.PageContext.ModeratePosts.ToString(),
                this.GetTextFormatted("MODERATE_NEW", this.PageContext.ModeratePosts),
                this.PageContext.ForumPageType == ForumPages.moderate_index);
        }

        /// <summary>
        /// Render the Main Header Menu Links
        /// </summary>
        private void RenderMainHeaderMenu()
        {
            // Active Topics
            if (this.PageContext.IsGuest)
            {
                RenderMenuItem(
                    this.menuListItems,
                    "nav-link",
                    this.GetText("TOOLBAR", "ACTIVETOPICS"),
                    this.GetText("TOOLBAR", "ACTIVETOPICS_TITLE"),
                    BuildLink.GetLink(ForumPages.mytopics),
                    false,
                    false,
                    null,
                    null,
                    this.PageContext.ForumPageType == ForumPages.mytopics,
                    string.Empty);
            }

            // Search
            if (this.Get<IPermissions>().Check(this.Get<BoardSettings>().SearchPermissions))
            {
                RenderMenuItem(
                    this.menuListItems,
                    "nav-link",
                    this.GetText("TOOLBAR", "SEARCH"),
                    this.GetText("TOOLBAR", "SEARCH_TITLE"),
                    BuildLink.GetLink(ForumPages.search),
                    false,
                    false,
                    null,
                    null,
                    this.PageContext.ForumPageType == ForumPages.search,
                    string.Empty);
            }

            // Members
            if (this.Get<IPermissions>().Check(this.Get<BoardSettings>().MembersListViewPermissions))
            {
                RenderMenuItem(
                    this.menuListItems,
                    "nav-link",
                    this.GetText("TOOLBAR", "MEMBERS"),
                    this.GetText("TOOLBAR", "MEMBERS_TITLE"),
                    BuildLink.GetLink(ForumPages.members),
                    false,
                    false,
                    null,
                    null,
                    this.PageContext.ForumPageType == ForumPages.members,
                    string.Empty);
            }

            // Team
            if (this.Get<IPermissions>().Check(this.Get<BoardSettings>().ShowTeamTo))
            {
                RenderMenuItem(
                    this.menuListItems,
                    "nav-link",
                    this.GetText("TOOLBAR", "TEAM"),
                    this.GetText("TOOLBAR", "TEAM_TITLE"),
                    BuildLink.GetLink(ForumPages.team),
                    false,
                    false,
                    null,
                    null, 
                    this.PageContext.ForumPageType == ForumPages.team,
                    string.Empty);
            }

            // Help
            if (this.Get<IPermissions>().Check(this.Get<BoardSettings>().ShowHelpTo))
            {
                RenderMenuItem(
                    this.menuListItems,
                    "nav-link",
                    this.GetText("TOOLBAR", "HELP"),
                    this.GetText("TOOLBAR", "HELP_TITLE"),
                    BuildLink.GetLink(ForumPages.help_index),
                    false,
                    false,
                    null,
                    null,
                    this.PageContext.ForumPageType == ForumPages.help_index,
                    string.Empty);
            }

            if (!this.PageContext.IsGuest || Config.IsAnyPortal)
            {
                return;
            }

            // Login
            if (Config.AllowLoginAndLogoff)
            {
                RenderMenuItem(
                    this.menuListItems,
                    "nav-link  LoginLink",
                    this.GetText("TOOLBAR", "LOGIN"),
                    this.GetText("TOOLBAR", "LOGIN_TITLE"),
                    "javascript:void(0);",
                    true,
                    false,
                    null,
                    null,
                    false,
                    string.Empty);
            }

            // Register
            if (!this.Get<BoardSettings>().DisableRegistrations)
            {
                RenderMenuItem(
                    this.menuListItems,
                    "nav-link",
                    this.GetText("TOOLBAR", "REGISTER"),
                    this.GetText("TOOLBAR", "REGISTER_TITLE"),
                    this.Get<BoardSettings>().ShowRulesForRegistration
                        ? BuildLink.GetLink(ForumPages.rules)
                        : !this.Get<BoardSettings>().UseSSLToRegister
                            ? BuildLink.GetLink(ForumPages.register)
                            : BuildLink.GetLink(ForumPages.register, true).Replace("http:", "https:"),
                    true,
                    false,
                    null,
                    null,
                    this.PageContext.ForumPageType == ForumPages.register,
                    string.Empty);
            }
        }

        /// <summary>
        /// Render The User related Links
        /// </summary>
        private void RenderUserContainer()
        {
            if (this.PageContext.IsGuest)
            {
                return;
            }

            RenderMenuItem(
                this.MyProfile,
                "dropdown-item",
                this.GetText("TOOLBAR", "MYPROFILE"),
                this.GetText("TOOLBAR", "MYPROFILE_TITLE"),
                BuildLink.GetLink(ForumPages.cp_profile),
                false,
                false,
                null,
                null,
                this.PageContext.ForumPageType == ForumPages.cp_profile,
                "address-card");

            RenderMenuItem(
                this.MySettings,
                "dropdown-item",
                this.GetText("TOOLBAR", "MYSETTINGS"),
                this.GetText("TOOLBAR", "MYSETTINGS_TITLE"),
                BuildLink.GetLink(ForumPages.cp_editprofile),
                false,
                false,
                null,
                null,
                this.PageContext.ForumPageType == ForumPages.cp_editprofile,
                "user-cog");

            var unreadActivity =
                this.PageContext.Mention + this.PageContext.Quoted + this.PageContext.ReceivedThanks;

            if (this.Get<BoardSettings>().EnableActivityStream)
            {
                RenderMenuItem(
                    this.MyNotification,
                    "dropdown-item",
                    this.GetText("TOOLBAR", "MYNOTIFY"),
                    this.GetText("TOOLBAR", "MYNOTIFY_TITLE"),
                    BuildLink.GetLink(ForumPages.cp_notification),
                    false,
                    unreadActivity > 0,
                    unreadActivity.ToString(),
                    this.GetTextFormatted("NEWPM", unreadActivity),
                    this.PageContext.ForumPageType == ForumPages.cp_notification,
                    "bell");
            }

            // My Inbox
            if (this.Get<BoardSettings>().AllowPrivateMessages)
            {
                RenderMenuItem(
                    this.MyInboxItem,
                    "dropdown-item",
                    this.GetText("TOOLBAR", "INBOX"),
                    this.GetText("TOOLBAR", "INBOX_TITLE"),
                    BuildLink.GetLink(ForumPages.cp_pm),
                    false,
                    this.PageContext.UnreadPrivate > 0,
                    this.PageContext.UnreadPrivate.ToString(),
                    this.GetTextFormatted("NEWPM", this.PageContext.UnreadPrivate),
                    this.PageContext.ForumPageType == ForumPages.cp_pm,
                    "inbox");
            }

            // My Buddies
            if (this.Get<BoardSettings>().EnableBuddyList && this.PageContext.UserHasBuddies)
            {
                RenderMenuItem(
                    this.MyBuddiesItem,
                    "dropdown-item",
                    this.GetText("TOOLBAR", "BUDDIES"),
                    this.GetText("TOOLBAR", "BUDDIES_TITLE"),
                    BuildLink.GetLink(ForumPages.cp_editbuddies),
                    false,
                    this.PageContext.PendingBuddies > 0,
                    this.PageContext.PendingBuddies.ToString(),
                    this.GetTextFormatted("BUDDYREQUEST", this.PageContext.PendingBuddies),
                    this.PageContext.ForumPageType == ForumPages.cp_editbuddies,
                    "users");
            }

            // My Albums
            if (this.Get<BoardSettings>().EnableAlbum
                && (this.PageContext.UsrAlbums > 0 || this.PageContext.NumAlbums > 0))
            {
                RenderMenuItem(
                    this.MyAlbumsItem,
                    "dropdown-item",
                    this.GetText("TOOLBAR", "MYALBUMS"),
                    this.GetText("TOOLBAR", "MYALBUMS_TITLE"),
                    BuildLink.GetLinkNotEscaped(ForumPages.albums, "u={0}", this.PageContext.PageUserID),
                    false,
                    false,
                    null,
                    null,
                    this.PageContext.ForumPageType == ForumPages.albums,
                    "images");
            }

            // My Topics
            RenderMenuItem(
                this.MyTopicItem,
                "dropdown-item",
                this.GetText("TOOLBAR", "MYTOPICS"),
                this.GetText("TOOLBAR", "MYTOPICS"),
                BuildLink.GetLink(ForumPages.mytopics),
                false,
                false,
                string.Empty,
                string.Empty,
                this.PageContext.ForumPageType == ForumPages.mytopics,
                "comment");

            // Logout
            if (!Config.IsAnyPortal && Config.AllowLoginAndLogoff)
            {
                this.LogutItem.Visible = true;
                this.LogOutButton.Text =
                    $"<i class=\"fa fa-sign-out-alt fa-fw\"></i>&nbsp;{this.GetText("TOOLBAR", "LOGOUT")}";
                this.LogOutButton.ToolTip = this.GetText("TOOLBAR", "LOGOUT");
            }

            // Logged in as : username
            this.LoggedInUserPanel.Visible = true;

            this.UserAvatar.ImageUrl = this.Get<IAvatars>().GetAvatarUrlForCurrentUser();

            var unreadCount = this.PageContext.UnreadPrivate + this.PageContext.PendingBuddies
                                                             + this.PageContext.Mention
                                                             + this.PageContext.Quoted
                                                             + this.PageContext.ReceivedThanks;

            this.UnreadLabel.Text = unreadCount.ToString();

            this.UnreadPlaceHolder.Visible = unreadCount > 0;
        }

        /// <summary>
        /// Render The GuestBar
        /// </summary>
        private void RenderGuestControls()
        {
            if (!this.PageContext.IsGuest)
            {
                return;
            }

            this.GuestUserMessage.Visible = true;

            this.GuestMessage.Text = this.GetText("TOOLBAR", "WELCOME_GUEST_FULL");

            var endPoint = new Label { Text = "." };

            var isLoginAllowed = false;
            var isRegisterAllowed = false;

            if (Config.IsAnyPortal)
            {
                this.GuestMessage.Text = this.GetText("TOOLBAR", "WELCOME_GUEST");
            }
            else
            {
                if (Config.AllowLoginAndLogoff)
                {
                    // show login
                    var loginLink = new HyperLink
                                        {
                                            Text = this.GetText("TOOLBAR", "LOGIN"),
                                            ToolTip = this.GetText("TOOLBAR", "LOGIN"),
                                            NavigateUrl = "javascript:void(0);",
                                            CssClass = "alert-link LoginLink"
                    };

                    this.GuestUserMessage.Controls.Add(loginLink);

                    isLoginAllowed = true;
                }

                if (!this.Get<BoardSettings>().DisableRegistrations)
                {
                    if (isLoginAllowed)
                    {
                        this.GuestUserMessage.Controls.Add(
                            new Label { Text = $"&nbsp;{this.GetText("COMMON", "OR")}&nbsp;" });
                    }

                    // show register link
                    var registerLink = new HyperLink
                                           {
                                               Text = this.GetText("TOOLBAR", "REGISTER"),
                                               NavigateUrl =
                                                   this.Get<BoardSettings>().ShowRulesForRegistration
                                                       ? BuildLink.GetLink(ForumPages.rules)
                                                       : !this.Get<BoardSettings>().UseSSLToRegister
                                                           ? BuildLink.GetLink(ForumPages.register)
                                                           : BuildLink.GetLink(
                                                               ForumPages.register,
                                                               true).Replace("http:", "https:"),
                                               CssClass = "alert-link"
                                           };

                    this.GuestUserMessage.Controls.Add(registerLink);

                    this.GuestUserMessage.Controls.Add(endPoint);

                    isRegisterAllowed = true;
                }
                else
                {
                    this.GuestUserMessage.Controls.Add(endPoint);

                    this.GuestUserMessage.Controls.Add(
                        new Label { Text = this.GetText("TOOLBAR", "DISABLED_REGISTER") });
                }

                // If both disallowed
                if (isLoginAllowed || isRegisterAllowed)
                {
                    return;
                }

                this.GuestUserMessage.Controls.Clear();
                this.GuestUserMessage.Controls.Add(new Label { Text = this.GetText("TOOLBAR", "WELCOME_GUEST_NO") });
            }
        }

#endregion
    }
}