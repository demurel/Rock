﻿// <copyright>
// Copyright 2013 by the Spark Development Network
//
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
// http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
// </copyright>
//
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Composition;

using Rock.Attribute;
using Rock.Data;
using Rock.Model;
using Rock.Web.Cache;

namespace Rock.Workflow.Action
{
    /// <summary>
    /// Adds a note to the workflow.
    /// </summary>
    [Description( "Adds a note to the workflow." )]
    [Export( typeof( ActionComponent ) )]
    [ExportMetadata( "ComponentName", "Add Note" )]
    [MemoField( "Note", "The note to add. <span class='tip tip-lava'></span>", true, "" )]
    public class AddNote : ActionComponent
    {
        /// <summary>
        /// Executes the specified workflow.
        /// </summary>
        /// <param name="rockContext">The rock context.</param>
        /// <param name="action">The action.</param>
        /// <param name="entity">The entity.</param>
        /// <param name="errorMessages">The error messages.</param>
        /// <returns></returns>
        public override bool Execute( RockContext rockContext, WorkflowAction action, Object entity, out List<string> errorMessages )
        {
            errorMessages = new List<string>();

            if ( action != null && action.Activity != null &&
                action.Activity.Workflow != null && action.Activity.Workflow.Id > 0 )
            {
                var noteEntityTypeId = EntityTypeCache.Read( typeof( Rock.Model.Workflow ) ).Id;
                var noteType = new NoteTypeService( rockContext ).Get( noteEntityTypeId, "WorkflowNote" );
                var text = GetAttributeValue( action, "Note" ).ResolveMergeFields( GetMergeFields( action ) );

                var note = new Note();
                note.IsSystem = false;
                note.NoteTypeId = noteType.Id;
                note.EntityId = action.Activity.Workflow.Id;
                note.Caption = string.Empty;
                note.Text = text;
                note.IsAlert = false;
                new NoteService( rockContext ).Add( note );

                return true;
            }
            else
            {
                errorMessages.Add( "A Note can only be added to a persisted workflow with a valid ID.");
                return false;
            }
        }
    }
}