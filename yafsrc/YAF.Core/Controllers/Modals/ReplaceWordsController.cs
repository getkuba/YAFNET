﻿/* Yet Another Forum.NET
 * Copyright (C) 2003-2005 Bjørnar Henden
 * Copyright (C) 2006-2013 Jaben Cargman
 * Copyright (C) 2014-2023 Ingo Herbote
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

namespace YAF.Core.Controllers.Modals;

using System;
using System.Data;

using Microsoft.Extensions.Logging;

using YAF.Core.BasePages;
using YAF.Core.Filters;
using YAF.Core.Model;
using YAF.Types.Modals;
using YAF.Types.Models;
using YAF.Types.Objects;

/// <summary>
/// Replace Words Controller
/// Implements the <see cref="ForumBaseController" />
/// </summary>
/// <seealso cref="ForumBaseController" />
[Produces("application/json")]
[Route("api/[controller]")]
[ApiController]
[AdminAuthorization]
public class ReplaceWordsController : ForumBaseController
{
    /// <summary>
    /// Import
    /// </summary>
    /// <returns>IActionResult.</returns>
    [ValidateAntiForgeryToken]
    [HttpPost("Import")]
    public IActionResult Import()
    {
        var import = this.Request.Form.Files[0]; 
        
        if (!import.ContentType.StartsWith("text"))
        {
            return this.Ok(
                new MessageModalNotification(
                    this.GetTextFormatted("MSG_IMPORTED_FAILEDX", import.ContentType),
                    MessageTypes.danger));
        }

        try
        {
            // import replace words...
            var replaceWords = new DataSet();
            replaceWords.ReadXml(import.OpenReadStream());

            if (replaceWords.Tables["YafReplaceWords"]?.Columns["badword"] != null
                && replaceWords.Tables["YafReplaceWords"].Columns["goodword"] != null)
            {
                var importedCount = 0;

                var replaceWordsList = this.GetRepository<Replace_Words>().GetByBoardId();

                // import any extensions that don't exist...
                replaceWords.Tables["YafReplaceWords"].Rows.Cast<DataRow>().ForEach(
                    row =>
                    {
                        if (replaceWordsList.Any(
                                w => w.BadWord == row["badword"].ToString()
                                     && w.GoodWord == row["goodword"].ToString()))
                        {
                            return;
                        }

                        // add this...
                        this.GetRepository<Replace_Words>().Save(
                            null,
                            row["badword"].ToString(),
                            row["goodword"].ToString());
                        importedCount++;
                    });

                return this.Ok(
                    new MessageModalNotification(
                    importedCount > 0
                        ? string.Format(this.GetText("ADMIN_REPLACEWORDS_IMPORT", "MSG_IMPORTED"), importedCount)
                        : this.GetText("ADMIN_REPLACEWORDS_IMPORT", "MSG_NOTHING"),
                    MessageTypes.success));
            }

            return this.Ok(
                new MessageModalNotification(
               this.GetText("ADMIN_REPLACEWORDS_IMPORT", "MSG_IMPORTED_FAILED"),
                MessageTypes.warning));
        }
        catch (Exception x)
        {
            this.Get<ILogger<ReplaceWordsController>>().Error(
                x,
                string.Format(this.GetText("ADMIN_SPAMWORDS_IMPORT", "IMPORT_FAILED"), x.Message));

            return this.Ok(
                new MessageModalNotification(
               string.Format(this.GetText("ADMIN_REPLACEWORDS_IMPORT", "MSG_IMPORTED_FAILEDX"), x.Message),
                MessageTypes.danger));
        }
    }

    /// <summary>
    /// Add or Edit
    /// </summary>
    /// <param name="model">The model.</param>
    /// <returns>IActionResult.</returns>
    [ValidateAntiForgeryToken]
    [HttpPost("Edit")]
    public IActionResult Edit([FromBody] ReplaceWordsEditModal model)
    {
        if (model.Id is 0)
        {
            model.Id = null;
        }
        
        if (!ValidationHelper.IsValidRegex(model.BadWord.Trim()))
        {
            return this.Ok(
                new MessageModalNotification(
                 this.GetText("ADMIN_REPLACEWORDS_EDIT", "MSG_REGEX_BAD"),
                MessageTypes.warning));        }

        this.GetRepository<Replace_Words>()
            .Save(
                model.Id,
                model.BadWord,
                model.GoodWord);

        return this.Ok();
    }
}