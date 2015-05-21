// <copyright>
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
using System.Web.UI;
using Rock;
using Rock.Constants;
using Rock.Data;
using Rock.Model;
using Rock.Security;
using Rock.Web.Cache;
using Rock.Web.UI.Controls;

namespace RockWeb.Blocks.Registration
{
    /// <summary>
    /// Template block for developers to use to start a new block.
    /// </summary>
    [DisplayName( "Instance Detail" )]
    [Category( "Registration" )]
    [Description( "Template block for editing an event registration instance." )]

    public partial class InstanceDetail : Rock.Web.UI.RockBlock
    {
        #region Fields
        #endregion

        #region Properties
        #endregion

        #region Base Control Methods

        //  overrides of the base RockBlock methods (i.e. OnInit, OnLoad)

        /// <summary>
        /// Raises the <see cref="E:System.Web.UI.Control.Init" /> event.
        /// </summary>
        /// <param name="e">An <see cref="T:System.EventArgs" /> object that contains the event data.</param>
        protected override void OnInit( EventArgs e )
        {
            base.OnInit( e );

            // this event gets fired after block settings are updated. it's nice to repaint the screen if these settings would alter it
            this.BlockUpdated += Block_BlockUpdated;
            this.AddConfigurationUpdateTrigger( upnlContent );

            btnDelete.Attributes["onclick"] = string.Format( "javascript: return Rock.dialogs.confirmDelete(event, '{0}', 'This will also delete all the registration instances of this type!');", RegistrationTemplate.FriendlyTypeName );
            btnSecurity.EntityTypeId = EntityTypeCache.Read( typeof( Rock.Model.RegistrationTemplate ) ).Id;

        }

        /// <summary>
        /// Raises the <see cref="E:System.Web.UI.Control.Load" /> event.
        /// </summary>
        /// <param name="e">The <see cref="T:System.EventArgs" /> object that contains the event data.</param>
        protected override void OnLoad( EventArgs e )
        {
            base.OnLoad( e );

            if ( !Page.IsPostBack )
            {
                ShowDetail();
            }
        }

        #endregion

        #region Events

        /// <summary>
        /// Handles the BlockUpdated event of the control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        protected void Block_BlockUpdated( object sender, EventArgs e )
        {

        }

        protected void btnEdit_Click( object sender, EventArgs e )
        {
            var rockContext = new RockContext();
            var registrationInstance = new RegistrationInstanceService( rockContext ).Get( hfRegistrationInstanceId.Value.AsInteger() );

            ShowEditDetails( registrationInstance, rockContext );
        }

        protected void btnDelete_Click( object sender, EventArgs e )
        {
            var rockContext = new RockContext();

            var service = new RegistrationInstanceService( rockContext );
            var registrationInstance = service.Get( hfRegistrationInstanceId.Value.AsInteger() );

            if ( registrationInstance != null )
            {
                if ( !registrationInstance.IsAuthorized( Authorization.ADMINISTRATE, this.CurrentPerson ) )
                {
                    mdDeleteWarning.Show( "You are not authorized to delete this registration instance.", ModalAlertType.Information );
                    return;
                }

                service.Delete( registrationInstance );

                rockContext.SaveChanges();
            }

            // reload page
            var qryParams = new Dictionary<string, string>();
            NavigateToPage( RockPage.Guid, qryParams );
        }

        protected void btnPreview_Click( object sender, EventArgs e )
        {
        }

        protected void btnSave_Click( object sender, EventArgs e )
        {
            using ( var rockContext = new RockContext() )
            {
                var service = new RegistrationInstanceService( rockContext );

                RegistrationInstance instance = null;

                int? RegistrationInstanceId = hfRegistrationInstanceId.Value.AsIntegerOrNull();
                if ( RegistrationInstanceId.HasValue )
                {
                    instance = service.Get( RegistrationInstanceId.Value );
                }

                if ( instance == null )
                {
                    instance = new RegistrationInstance();
                    instance.RegistrationTemplateId = PageParameter( "RegistrationTemplateId" ).AsInteger();
                    service.Add( instance );
                }

                instance.Name = tbName.Text;
                instance.IsActive = cbIsActive.Checked;
                instance.Details = ceDetails.Text;
                instance.StartDateTime = dtpStart.SelectedDateTime;
                instance.EndDateTime = dtpEnd.SelectedDateTime;
                instance.MaxAttendees = nbMaxAttendees.Text.AsInteger();
                instance.ContactName = tbContactName.Text;
                instance.ContactEmail = ebContactEmail.Text;
                instance.AccountCode = tbAccountCode.Text;
                instance.AdditionalReminderDetails = ceAdditionalReminderDetails.Text;
                instance.AdditionalConfirmationDetails = ceAdditionalConfirmationDetails.Text;

                if ( !Page.IsValid )
                {
                    return;
                }

                rockContext.SaveChanges();

                // Reload instance and show readonly view
                instance = service.Get( instance.Id );
                ShowReadonlyDetails( instance );
            }
        }

        protected void btnCancel_Click( object sender, EventArgs e )
        {
            if ( hfRegistrationInstanceId.Value.Equals( "0" ) )
            {
                var qryParams = new Dictionary<string, string>();

                int? parentTemplateId = PageParameter( "RegistrationTemplateId" ).AsIntegerOrNull();
                if ( parentTemplateId.HasValue )
                {
                    qryParams["RegistrationTemplateId"] = parentTemplateId.ToString();
                }

                // Cancelling on Add.  Return to Grid
                NavigateToParentPage( qryParams );
            }
            else
            {
                // Cancelling on Edit.  Return to Details
                RegistrationInstanceService service = new RegistrationInstanceService( new RockContext() );
                RegistrationInstance item = service.Get( int.Parse( hfRegistrationInstanceId.Value ) );
                ShowReadonlyDetails( item );
            }
        }

        #endregion

        #region Methods

        private void ShowDetail()
        {
            int? RegistrationInstanceId = PageParameter( "RegistrationInstanceId" ).AsIntegerOrNull();
            int? parentTemplateId = PageParameter( "RegistrationTemplateId" ).AsIntegerOrNull();

            if ( !RegistrationInstanceId.HasValue )
            {
                pnlDetails.Visible = false;
                return;
            }

            using ( var rockContext = new RockContext() )
            {
                RegistrationInstance registrationInstance = null;
                if ( RegistrationInstanceId.HasValue )
                {
                    registrationInstance = new RegistrationInstanceService( rockContext ).Get( RegistrationInstanceId.Value );
                }

                if ( registrationInstance == null )
                {
                    registrationInstance = new RegistrationInstance();
                    registrationInstance.Id = 0;
                    registrationInstance.IsActive = true;
                    registrationInstance.RegistrationTemplateId = parentTemplateId ?? 0;
                }

                var template = registrationInstance.RegistrationTemplate;
                if ( template == null && registrationInstance.RegistrationTemplateId > 0 )
                {
                    template = new RegistrationTemplateService( rockContext )
                        .Get( registrationInstance.RegistrationTemplateId );
                }

                hlType.Visible = template != null;
                hlType.Text = template != null ? template.Name : string.Empty;

                pnlDetails.Visible = true;
                hfRegistrationInstanceId.Value = registrationInstance.Id.ToString();

                // render UI based on Authorized 
                bool readOnly = false;

                nbEditModeMessage.Text = string.Empty;

                // User must have 'Edit' rights to block, or 'Administrate' rights to instance
                if ( !UserCanEdit && !registrationInstance.IsAuthorized( Authorization.ADMINISTRATE, CurrentPerson ) )
                {
                    readOnly = true;
                    nbEditModeMessage.Heading = "Information";
                    nbEditModeMessage.Text = EditModeMessage.ReadOnlyEditActionNotAllowed( RegistrationInstance.FriendlyTypeName );
                }

                if ( readOnly )
                {
                    btnEdit.Visible = false;
                    btnSecurity.Visible = false;
                    ShowReadonlyDetails( registrationInstance );
                }
                else
                {
                    btnEdit.Visible = true;

                    btnSecurity.Title = "Secure " + registrationInstance.Name;
                    btnSecurity.EntityId = registrationInstance.Id;

                    if ( registrationInstance.Id > 0 )
                    {
                        ShowReadonlyDetails( registrationInstance );
                    }
                    else
                    {
                        ShowEditDetails( registrationInstance, rockContext );
                    }
                }
            }
        }

        /// <summary>
        /// Shows the edit details.
        /// </summary>
        /// <param name="RegistrationTemplate">The registration template.</param>
        /// <param name="rockContext">The rock context.</param>
        private void ShowEditDetails( RegistrationInstance RegistrationInstance, RockContext rockContext )
        {
            if ( RegistrationInstance.Id == 0 )
            {
                lReadOnlyTitle.Text = ActionTitle.Add( RegistrationInstance.FriendlyTypeName ).FormatAsHtmlTitle();
                hlInactive.Visible = false;
            }

            SetEditMode( true );

            LoadDropDowns( rockContext );

            cbIsActive.Checked = RegistrationInstance.IsActive;
            tbName.Text = RegistrationInstance.Name;
            ceDetails.Text = RegistrationInstance.Details;
            dtpStart.SelectedDateTime = RegistrationInstance.StartDateTime;
            dtpEnd.SelectedDateTime = RegistrationInstance.EndDateTime;
            tbContactName.Text = RegistrationInstance.ContactName;
            ebContactEmail.Text = RegistrationInstance.ContactEmail;
            tbAccountCode.Text = RegistrationInstance.AccountCode;
            nbMaxAttendees.Text = RegistrationInstance.MaxAttendees.ToString();
            ceAdditionalReminderDetails.Text = RegistrationInstance.AdditionalReminderDetails;
            ceAdditionalConfirmationDetails.Text = RegistrationInstance.AdditionalConfirmationDetails;
        }

        /// <summary>
        /// Shows the readonly details.
        /// </summary>
        /// <param name="RegistrationInstance">The registration template.</param>
        private void ShowReadonlyDetails( RegistrationInstance RegistrationInstance )
        {
            SetEditMode( false );

            hfRegistrationInstanceId.SetValue( RegistrationInstance.Id );

            lReadOnlyTitle.Text = RegistrationInstance.Name.FormatAsHtmlTitle();
            hlInactive.Visible = RegistrationInstance.IsActive == false;

            lName.Text = RegistrationInstance.Name;
            lMaxAttendees.Visible = RegistrationInstance.MaxAttendees > 0;
            lMaxAttendees.Text = RegistrationInstance.MaxAttendees.ToString( "N0" );
            lDetails.Text = RegistrationInstance.Details;
        }

        /// <summary>
        /// Sets the edit mode.
        /// </summary>
        /// <param name="editable">if set to <c>true</c> [editable].</param>
        private void SetEditMode( bool editable )
        {
            pnlEditDetails.Visible = editable;
            fieldsetViewDetails.Visible = !editable;
        }

        /// <summary>
        /// Loads the drop downs.
        /// </summary>
        private void LoadDropDowns( RockContext rockContext )
        {

        }

        #endregion
    }
}