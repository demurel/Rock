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
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using Rock.Model;

namespace Rock.Web.UI.Controls
{
    /// <summary>
    /// Report Filter control
    /// </summary>
    [ToolboxData( "<{0}:WorkflowActivityTypeEditor runat=server></{0}:WorkflowActivityTypeEditor>" )]
    public class WorkflowActivityEditor : CompositeControl
    {
        private HiddenField _hfActivityTypeGuid;
        private Label _lblActivityTypeName;
        private Label _lblActivityTypeDescription;
        private Label _lblInactive;
        private LinkButton _lbDeleteActivityType;

        private RockCheckBox _cbActivityTypeIsActive;
        private DataTextBox _tbActivityTypeName;
        private DataTextBox _tbActivityTypeDescription;
        private RockCheckBox _cbActivityTypeIsActivatedWithWorkflow;

        private LinkButton _lbAddActionType;

        /// <summary>
        /// Gets or sets a value indicating whether to force content visible.
        /// </summary>
        /// <value>
        ///   <c>true</c> if [force content visible]; otherwise, <c>false</c>.
        /// </value>
        public bool ForceContentVisible { private get; set; }

        /// <summary>
        /// Gets or sets the activity type unique identifier.
        /// </summary>
        /// <value>
        /// The activity type unique identifier.
        /// </value>
        public Guid ActivityTypeGuid
        {
            get 
            {
                EnsureChildControls();
                return _hfActivityTypeGuid.Value.AsGuid();
            }
        }

        /// <summary>
        /// Gets the name.
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        public string Name
        {
            get
            {
                EnsureChildControls();
                return _tbActivityTypeName.Text;
            }

        }
        
        /// <summary>
        /// Raises the <see cref="E:System.Web.UI.Control.Init" /> event.
        /// </summary>
        /// <param name="e">An <see cref="T:System.EventArgs" /> object that contains the event data.</param>
        protected override void OnInit( EventArgs e )
        {
            base.OnInit( e );

            string script = @"
// activity animation
$('.workflow-activity > header').click(function () {
    $(this).siblings('.panel-body').slideToggle();

    $('i.workflow-activity-state', this).toggleClass('fa-chevron-down');
    $('i.workflow-activity-state', this).toggleClass('fa-chevron-up');
});

// fix so that the Remove button will fire its event, but not the parent event 
$('.workflow-activity a.js-activity-delete').click(function (event) {
    event.stopImmediatePropagation();
});

// fix so that the Reorder button will fire its event, but not the parent event 
$('.workflow-activity a.workflow-activity-reorder').click(function (event) {
    event.stopImmediatePropagation();
});

";

            ScriptManager.RegisterStartupScript( this.Page, this.Page.GetType(), "WorkflowActivityTypeEditorScript", script, true );
        }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is delete enabled.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance is delete enabled; otherwise, <c>false</c>.
        /// </value>
        public bool IsDeleteEnabled
        {
            get
            {
                bool? b = ViewState["IsDeleteEnabled"] as bool?;
                return ( b == null ) ? true : b.Value;
            }

            set
            {
                ViewState["IsDeleteEnabled"] = value;
            }
        }

        /// <summary>
        /// Gets or sets the type of the workflow activity.
        /// </summary>
        /// <value>
        /// The type of the workflow activity.
        /// </value>
        public WorkflowActivityType GetWorkflowActivityType()
        {
            EnsureChildControls();
            WorkflowActivityType result = new WorkflowActivityType();
            result.Guid = new Guid( _hfActivityTypeGuid.Value );
            result.Name = _tbActivityTypeName.Text;
            result.Description = _tbActivityTypeDescription.Text;
            result.IsActive = _cbActivityTypeIsActive.Checked;
            result.IsActivatedWithWorkflow = _cbActivityTypeIsActivatedWithWorkflow.Checked;
            result.ActionTypes = new List<WorkflowActionType>();
            int order = 0;
            foreach ( WorkflowActionEditor workflowActionTypeEditor in this.Controls.OfType<WorkflowActionEditor>() )
            {
                WorkflowActionType workflowActionType = workflowActionTypeEditor.WorkflowActionType;
                workflowActionType.Order = order++;
                result.ActionTypes.Add( workflowActionType );
            }

            return result;
        }

        /// <summary>
        /// Sets the type of the workflow activity.
        /// </summary>
        /// <param name="value">The value.</param>
        public void SetWorkflowActivityType( WorkflowActivityType value )
        {
            EnsureChildControls();
            _hfActivityTypeGuid.Value = value.Guid.ToString();
            _tbActivityTypeName.Text = value.Name;
            _tbActivityTypeDescription.Text = value.Description;
            _cbActivityTypeIsActive.Checked = value.IsActive ?? false;
            _cbActivityTypeIsActivatedWithWorkflow.Checked = value.IsActivatedWithWorkflow;
        }

        /// <summary>
        /// Called by the ASP.NET page framework to notify server controls that use composition-based implementation to create any child controls they contain in preparation for posting back or rendering.
        /// </summary>
        protected override void CreateChildControls()
        {
            Controls.Clear();

            _hfActivityTypeGuid = new HiddenField();
            _hfActivityTypeGuid.ID = this.ID + "_hfActivityTypeGuid";

            _lblActivityTypeName = new Label();
            _lblActivityTypeName.ClientIDMode = ClientIDMode.Static;
            _lblActivityTypeName.ID = this.ID + "_lblActivityTypeName";
            _lblActivityTypeDescription = new Label();
            _lblActivityTypeDescription.ClientIDMode = ClientIDMode.Static;
            _lblActivityTypeDescription.ID = this.ID + "_lblActivityTypeDescription";

            _lblInactive = new Label();
            _lblInactive.ClientIDMode = ClientIDMode.Static;
            _lblInactive.ID = this.ID + "_lblInactive";
            _lblInactive.CssClass = "label label-important pull-right";
            _lblInactive.Text = "Inactive";

            _lbDeleteActivityType = new LinkButton();
            _lbDeleteActivityType.CausesValidation = false;
            _lbDeleteActivityType.ID = this.ID + "_lbDeleteActivityType";
            _lbDeleteActivityType.CssClass = "btn btn-xs btn-danger js-activity-delete";
            _lbDeleteActivityType.Click += lbDeleteActivityType_Click;
            _lbDeleteActivityType.Controls.Add( new LiteralControl { Text = "<i class='fa fa-times'></i>" } );

            _cbActivityTypeIsActive = new RockCheckBox { Text = "Active" };
            _cbActivityTypeIsActive.ID = this.ID + "_cbActivityTypeIsActive";
            string checkboxScriptFormat = @"
javascript: 
    if ($(this).is(':checked')) {{ 
        $('#{0}').hide(); 
        $('#{1}').removeClass('workflow-activity-inactive'); 
    }} 
    else {{ 
        $('#{0}').show(); 
        $('#{1}').addClass('workflow-activity-inactive'); 
    }}
";

            _cbActivityTypeIsActive.InputAttributes.Add( "onclick", string.Format( checkboxScriptFormat, _lblInactive.ID, this.ID + "_section" ) );

            _tbActivityTypeName = new DataTextBox();
            _tbActivityTypeName.ID = this.ID + "_tbActivityTypeName";
            _tbActivityTypeName.Label = "Name";

            // set label when they exit the edit field
            _tbActivityTypeName.Attributes["onblur"] = string.Format( "javascript: $('#{0}').text($(this).val());", _lblActivityTypeName.ID );
            _tbActivityTypeName.SourceTypeName = "Rock.Model.WorkflowActivityType, Rock";
            _tbActivityTypeName.PropertyName = "Name";

            _tbActivityTypeDescription = new DataTextBox();
            _tbActivityTypeDescription.ID = this.ID + "_tbActivityTypeDescription";
            _tbActivityTypeDescription.Label = "Description";
            _tbActivityTypeDescription.TextMode = TextBoxMode.MultiLine;
            _tbActivityTypeDescription.Rows = 2;

            // set label when they exit the edit field
            _tbActivityTypeDescription.Attributes["onblur"] = string.Format( "javascript: $('#{0}').text($(this).val());", _lblActivityTypeDescription.ID );
            _tbActivityTypeDescription.SourceTypeName = "Rock.Model.WorkflowActivityType, Rock";
            _tbActivityTypeDescription.PropertyName = "Description";

            _cbActivityTypeIsActivatedWithWorkflow = new RockCheckBox { Text = "Activated with Workflow" };
            _cbActivityTypeIsActivatedWithWorkflow.ID = this.ID + "_cbActivityTypeIsActivatedWithWorkflow";

            _lbAddActionType = new LinkButton();
            _lbAddActionType.ID = this.ID + "_lbAddAction";
            _lbAddActionType.CssClass = "btn btn-xs btn-action";
            _lbAddActionType.Click += lbAddActionType_Click;
            _lbAddActionType.CausesValidation = false;
            _lbAddActionType.Controls.Add( new LiteralControl { Text = "<i class='fa fa-plus'></i> Add Action" } );

            Controls.Add( _hfActivityTypeGuid );
            Controls.Add( _lblActivityTypeName );
            Controls.Add( _lblActivityTypeDescription );
            Controls.Add( _lblInactive );
            Controls.Add( _tbActivityTypeName );
            Controls.Add( _tbActivityTypeDescription );
            Controls.Add( _cbActivityTypeIsActive );
            Controls.Add( _cbActivityTypeIsActivatedWithWorkflow );
            Controls.Add( _lbDeleteActivityType );
            Controls.Add( _lbAddActionType );
        }

        /// <summary>
        /// Writes the <see cref="T:System.Web.UI.WebControls.CompositeControl" /> content to the specified <see cref="T:System.Web.UI.HtmlTextWriter" /> object, for display on the client.
        /// </summary>
        /// <param name="writer">An <see cref="T:System.Web.UI.HtmlTextWriter" /> that represents the output stream to render HTML content on the client.</param>
        public override void RenderControl( HtmlTextWriter writer )
        {
            if ( _cbActivityTypeIsActive.Checked )
            {
                writer.AddAttribute(HtmlTextWriterAttribute.Class, "panel panel-widget workflow-activity");
            }
            else
            {
                writer.AddAttribute( HtmlTextWriterAttribute.Class, "panel workflow-activity workflow-activity-inactive" );
            }

            writer.AddAttribute( "data-key", _hfActivityTypeGuid.Value );
            writer.AddAttribute( HtmlTextWriterAttribute.Id, this.ID + "_section" );
            writer.RenderBeginTag( "section" );

            writer.AddAttribute( HtmlTextWriterAttribute.Class, "panel-heading clearfix clickable" );
            writer.RenderBeginTag( "header" );

            writer.AddAttribute( HtmlTextWriterAttribute.Class, "filter-toogle pull-left" );
            writer.RenderBeginTag( HtmlTextWriterTag.Div );

            writer.AddAttribute("class", "panel-title");
            writer.RenderBeginTag( HtmlTextWriterTag.H3 );
            _lblActivityTypeName.Text = _tbActivityTypeName.Text;
            _lblActivityTypeName.RenderControl( writer );

            // H3 tag
            writer.RenderEndTag();
            _lblActivityTypeDescription.Text = _tbActivityTypeDescription.Text;
            _lblActivityTypeDescription.RenderControl( writer );

            // Name/Description div
            writer.RenderEndTag();

            writer.AddAttribute( HtmlTextWriterAttribute.Class, "pull-right" );
            writer.RenderBeginTag( HtmlTextWriterTag.Div );

            writer.WriteLine( "<a class='btn btn-xs btn-link workflow-activity-reorder'><i class='fa fa-bars'></i></a>" );
            writer.WriteLine( "<a class='btn btn-xs btn-link'><i class='workflow-activity-state fa fa-chevron-down'></i></a>" );

            if ( IsDeleteEnabled )
            {
                _lbDeleteActivityType.Visible = true;
                _lbDeleteActivityType.RenderControl( writer );
            }
            else
            {
                _lbDeleteActivityType.Visible = false;
            }

            // Add/ChevronUpDown/Delete div
            writer.RenderEndTag();

            _lblInactive.Style[HtmlTextWriterStyle.Display] = _cbActivityTypeIsActive.Checked ? "none" : string.Empty;
            _lblInactive.RenderControl( writer );

            // header div
            writer.RenderEndTag();

            bool forceContentVisible = !GetWorkflowActivityType().IsValid || ForceContentVisible;

            if ( !forceContentVisible )
            {
                foreach ( WorkflowActionEditor workflowActionTypeEditor in this.Controls.OfType<WorkflowActionEditor>().OrderBy( a => a.WorkflowActionType.Order ) )
                {
                    if ( !workflowActionTypeEditor.WorkflowActionType.IsValid || workflowActionTypeEditor.ForceContentVisible )
                    {
                        forceContentVisible = true;
                        break;
                    }
                }
            }

            writer.AddAttribute( HtmlTextWriterAttribute.Class, "panel-body" );

            if ( !forceContentVisible )
            {
                // hide details if the activity and actions are valid
                writer.AddStyleAttribute( "display", "none" );
            }

            writer.RenderBeginTag( HtmlTextWriterTag.Div );

            // activity edit fields
            writer.AddAttribute( HtmlTextWriterAttribute.Class, "row" );
            writer.RenderBeginTag( HtmlTextWriterTag.Div );

            writer.AddAttribute( HtmlTextWriterAttribute.Class, "col-md-6" );
            writer.RenderBeginTag( HtmlTextWriterTag.Div );
            _tbActivityTypeName.RenderControl( writer );
            writer.RenderEndTag();

            writer.AddAttribute( HtmlTextWriterAttribute.Class, "col-md-2" );
            writer.RenderBeginTag( HtmlTextWriterTag.Div );
            _cbActivityTypeIsActive.RenderControl( writer );
            writer.RenderEndTag();

            writer.AddAttribute( HtmlTextWriterAttribute.Class, "col-md-4" );
            writer.RenderBeginTag( HtmlTextWriterTag.Div );
            _cbActivityTypeIsActivatedWithWorkflow.RenderControl( writer );
            writer.RenderEndTag();

            writer.RenderEndTag();

            _tbActivityTypeDescription.RenderControl( writer );

            // actions
            writer.RenderBeginTag( "fieldset" );

            writer.RenderBeginTag( "legend" );
            writer.WriteLine( "Actions" );
            writer.AddAttribute( HtmlTextWriterAttribute.Class, "pull-right" );
            writer.RenderBeginTag( HtmlTextWriterTag.Span );
            _lbAddActionType.RenderControl( writer );
            writer.RenderEndTag();
            writer.RenderEndTag();

            writer.AddAttribute( HtmlTextWriterAttribute.Class, "workflow-action-list" );
            writer.RenderBeginTag( HtmlTextWriterTag.Div );
            foreach ( WorkflowActionEditor workflowActionTypeEditor in this.Controls.OfType<WorkflowActionEditor>().OrderBy( a => a.WorkflowActionType.Order ) )
            {
                workflowActionTypeEditor.RenderControl( writer );
            }

            // workflow-action-list div
            writer.RenderEndTag();

            // actions fieldset
            writer.RenderEndTag();

            // widget-content div
            writer.RenderEndTag();

            // section tag
            writer.RenderEndTag();
        }

        /// <summary>
        /// Handles the Click event of the lbDeleteActivityType control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs" /> instance containing the event data.</param>
        protected void lbDeleteActivityType_Click( object sender, EventArgs e )
        {
            if ( DeleteActivityTypeClick != null )
            {
                DeleteActivityTypeClick( this, e );
            }
        }

        /// <summary>
        /// Handles the Click event of the lbAddActionType control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs" /> instance containing the event data.</param>
        protected void lbAddActionType_Click( object sender, EventArgs e )
        {
            if ( AddActionTypeClick != null )
            {
                AddActionTypeClick( this, e );
            }
        }

        /// <summary>
        /// Occurs when [delete activity type click].
        /// </summary>
        public event EventHandler DeleteActivityTypeClick;

        /// <summary>
        /// Occurs when [add action type click].
        /// </summary>
        public event EventHandler AddActionTypeClick;
    }
}