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
namespace YAF.Modules
{
    using System.Web.UI;

    using YAF.Core.BBCode;
    using YAF.Core.Extensions;
    using YAF.Types.Extensions;
    using YAF.Types.Interfaces;
    using YAF.Web.Controls;

    /// <summary>
    /// The BB Code UserLink Module
    /// </summary>
    public class UserLinkBBCodeModule : BBCodeControl
    {
        /// <summary>
        /// The render.
        /// </summary>
        /// <param name="writer">
        /// The writer.
        /// </param>
        protected override void Render(HtmlTextWriter writer)
        {
            var userName = this.Parameters["inner"];

            if (userName.IsNotSet() || userName.Length > 50)
            {
                return;
            }

            var userId = this.Get<IUserDisplayName>().GetId(userName.Trim());

            if (userId.HasValue)
            {
                var userLink = new UserLink
                                   {
                                       UserID = userId.ToType<int>(),
                                       CssClass = "btn btn-outline-primary",
                                       BlankTarget = true,
                                       ID = $"UserLinkBBCodeFor{userId}"
                                   };

                writer.Write("<!-- BEGIN userlink -->");
                writer.Write(@"<span>");
                userLink.RenderControl(writer);

                writer.Write("</span>");
                writer.Write("<!-- END userlink -->");
            }
            else
            {
                writer.Write(this.HtmlEncode(userName));
            }
        }
    }
}