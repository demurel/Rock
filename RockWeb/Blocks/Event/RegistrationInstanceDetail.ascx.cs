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
using System.Data.Entity;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using Newtonsoft.Json;
using Rock;
using Rock.Attribute;
using Rock.Constants;
using Rock.Data;
using Rock.Model;
using Rock.Security;
using Rock.Web.Cache;
using Rock.Web.UI.Controls;

namespace RockWeb.Blocks.Event
{
    /// <summary>
    /// Template block for developers to use to start a new block.
    /// </summary>
    [DisplayName( "Registration Instance Detail" )]
    [Category( "Event" )]
    [Description( "Template block for editing an event registration instance." )]

    [AccountField( "Default Account", "The default account to use for new registration instances", false, "2A6F9E5F-6859-44F1-AB0E-CE9CF6B08EE5", "", 0 )]
    public partial class RegistrationInstanceDetail : Rock.Web.UI.RockBlock
    {
        #region Fields

        private List<FinancialTransactionDetail> RegistrationPayments;

        #endregion

        #region Properties

        public List<RegistrantFormField> RegistrantFields { get; set; }

        /// <summary>
        /// Gets or sets the active tab.
        /// </summary>
        /// <value>
        /// The active tab.
        /// </value>
        protected string ActiveTab { get; set; }

        #endregion

        #region Base Control Methods

        /// <summary>
        /// Restores the view-state information from a previous user control request that was saved by the <see cref="M:System.Web.UI.UserControl.SaveViewState" /> method.
        /// </summary>
        /// <param name="savedState">An <see cref="T:System.Object" /> that represents the user control state to be restored.</param>
        protected override void LoadViewState( object savedState )
        {
            base.LoadViewState( savedState );

            ActiveTab = ( ViewState["ActiveTab"] as string ) ?? "";
            RegistrantFields = ViewState["RegistrantFields"] as List<RegistrantFormField>;

            AddDynamicRegistrantControls();
        }

        /// <summary>
        /// Raises the <see cref="E:System.Web.UI.Control.Init" /> event.
        /// </summary>
        /// <param name="e">An <see cref="T:System.EventArgs" /> object that contains the event data.</param>
        protected override void OnInit( EventArgs e )
        {
            base.OnInit( e );

            fRegistrations.ApplyFilterClick += fRegistrations_ApplyFilterClick;
            gRegistrations.DataKeyNames = new string[] { "Id" };
            gRegistrations.Actions.ShowAdd = false;
            gRegistrations.RowDataBound += gRegistrations_RowDataBound;
            gRegistrations.GridRebind += gRegistrations_GridRebind;

            fRegistrants.ApplyFilterClick += fRegistrants_ApplyFilterClick;
            gRegistrants.DataKeyNames = new string[] { "Id" };
            gRegistrants.Actions.ShowAdd = false;
            gRegistrants.RowDataBound += gRegistrants_RowDataBound;
            gRegistrants.GridRebind += gRegistrants_GridRebind;

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

        /// <summary>
        /// Saves any user control view-state changes that have occurred since the last page postback.
        /// </summary>
        /// <returns>
        /// Returns the user control's current view state. If there is no view state associated with the control, it returns null.
        /// </returns>
        protected override object SaveViewState()
        {
            ViewState["RegistrantFields"] = RegistrantFields;
            ViewState["ActiveTab"] = ActiveTab;

            return base.SaveViewState();
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
                instance.AccountId = apAccount.SelectedValue.AsInteger();
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

        protected void lbTab_Click( object sender, EventArgs e )
        {
            LinkButton lb = sender as LinkButton;
            if ( lb != null )
            {
                ActiveTab = lb.ID;
                ShowTab();
            }
        }


        #region Registration Grid Events

        protected void fRegistrations_ApplyFilterClick( object sender, EventArgs e )
        {
            fRegistrations.SaveUserPreference( "Date Range", drpRegistrationDateRange.DelimitedValues );
            fRegistrations.SaveUserPreference( "Payment Status", ddlRegistrationPaymentStatus.SelectedValue );
            fRegistrations.SaveUserPreference( "Processor First Name", tbRegistrationProcessorFirstName.Text );
            fRegistrations.SaveUserPreference( "Processor Last Name", tbRegistrationProcessorLastName.Text );
            fRegistrations.SaveUserPreference( "Registrant First Name", tbRegistrationRegistrantFirstName.Text );
            fRegistrations.SaveUserPreference( "Registrant Last Name", tbRegistrationRegistrantLastName.Text );

            BindRegistrationsGrid();
        }

        protected void fRegistrations_DisplayFilterValue( object sender, GridFilter.DisplayFilterValueArgs e )
        {
            switch ( e.Key )
            {
                case "Date Range":
                    {
                        e.Value = DateRangePicker.FormatDelimitedValues( e.Value );
                        break;
                    }
                case "Payment Status":
                case "Processor First Name":
                case "Processor Last Name":
                case "Registrant First Name":
                case "Registrant Last Name":
                    {
                        break;
                    }
                default:
                    {
                        e.Value = string.Empty;
                        break;
                    }
            }
        }

        protected void gRegistrations_GridRebind( object sender, EventArgs e )
        {
            BindRegistrationsGrid();
        }

        void gRegistrations_RowDataBound( object sender, GridViewRowEventArgs e )
        {
            var registration = e.Row.DataItem as Registration;
            if ( registration != null )
            {

                // Set the processor value
                var lProcessor = e.Row.FindControl( "lProcessor" ) as Literal;
                if ( lProcessor != null )
                {
                    if ( registration.PersonAlias != null && registration.PersonAlias.Person != null )
                    {
                        lProcessor.Text = registration.PersonAlias.Person.FullNameReversed;
                    }
                    else
                    {
                        lProcessor.Text = string.Format( "{0}, {1}", registration.LastName, registration.FirstName );
                    }
                }

                string registrantNames = string.Empty;
                decimal totalCost = 0;
                if ( registration.Registrants != null && registration.Registrants.Any() )
                {
                    var registrants = registration.Registrants
                        .Where( r =>
                            r.PersonAlias != null &&
                            r.PersonAlias.Person != null )
                        .OrderBy( r => r.PersonAlias.Person.NickName )
                        .ThenBy( r => r.PersonAlias.Person.LastName )
                        .ToList();

                    registrantNames = registrants
                        .Select( r => r.PersonAlias.Person.NickName + " " + r.PersonAlias.Person.LastName )
                        .ToList()
                        .AsDelimited( "<br/>" );

                    totalCost = 
                        registrants.Sum( r => r.Cost ) +
                        registrants.SelectMany( r => r.Fees ).Sum( f => f.Cost );
                }

                // Set the Registrants
                var lRegistrants = e.Row.FindControl( "lRegistrants" ) as Literal;
                if ( lRegistrants != null )
                {
                    lRegistrants.Text = registrantNames;
                }

                // Set the Cost
                var lCost = e.Row.FindControl( "lCost" ) as Literal;
                if ( lCost != null )
                {
                    lCost.Text = totalCost.ToString( "C2" );
                }

                // Set the payment
                var lblPaid = e.Row.FindControl( "lblPaid" ) as Label;
                if ( lblPaid != null && RegistrationPayments != null )
                {
                    decimal totalPaid = RegistrationPayments
                        .Where( d => d.EntityId == registration.Id )
                        .Sum( d => d.Amount );
                    lblPaid.Text = totalPaid.ToString( "C2" );

                    if ( totalPaid < totalCost )
                    {
                        lblPaid.AddCssClass( "label" );
                        lblPaid.AddCssClass( "label-danger" );
                    }
                    else if ( totalPaid > totalCost )
                    {
                        lblPaid.AddCssClass( "label" );
                        lblPaid.AddCssClass( "label-success" );
                    }
                }
            }
        }

        protected void gRegistrations_Delete( object sender, RowEventArgs e )
        {

        }

        protected void gRegistrations_RowSelected( object sender, RowEventArgs e )
        {

        }

        #endregion

        #region Registrant Grid Events

        protected void fRegistrants_ApplyFilterClick( object sender, EventArgs e )
        {
            fRegistrants.SaveUserPreference( "Date Range", drpRegistrantDateRange.DelimitedValues );
            fRegistrants.SaveUserPreference( "First Name", tbRegistrantFirstName.Text );
            fRegistrants.SaveUserPreference( "Last Name", tbRegistrantLastName.Text );

            if ( RegistrantFields != null )
            {
                foreach ( var field in RegistrantFields )
                {
                    if ( field.Attribute != null )
                    {
                        var attribute = field.Attribute;
                        var filterControl = phRegistrantFormFieldFilters.FindControl( "filter_" + attribute.Id.ToString() );
                        if ( filterControl != null )
                        {
                            try
                            {
                                var values = attribute.FieldType.Field.GetFilterValues( filterControl, field.Attribute.QualifierValues );
                                fRegistrants.SaveUserPreference( attribute.Key, attribute.Name, attribute.FieldType.Field.GetFilterValues( filterControl, attribute.QualifierValues ).ToJson() );
                            }
                            catch { }
                        }
                    }
                }
            }

            BindRegistrantsGrid();
        }

        protected void fRegistrants_DisplayFilterValue( object sender, GridFilter.DisplayFilterValueArgs e )
        {
            if ( RegistrantFields != null )
            {
                var attribute = RegistrantFields
                    .Where( a =>
                        a.Attribute != null &&
                        a.Attribute.Key == e.Key )
                    .Select( a => a.Attribute )
                    .FirstOrDefault();

                if ( attribute != null )
                {
                    try
                    {
                        var values = JsonConvert.DeserializeObject<List<string>>( e.Value );
                        e.Value = attribute.FieldType.Field.FormatFilterValues( attribute.QualifierValues, values );
                        return;
                    }
                    catch { }
                }
            }

            switch ( e.Key )
            {
                case "Date Range":
                    {
                        e.Value = DateRangePicker.FormatDelimitedValues( e.Value );
                        break;
                    }
                case "First Name":
                case "Last Name":
                    {
                        break;
                    }
                default:
                    {
                        e.Value = string.Empty;
                        break;
                    }
            }
        }

        protected void gRegistrants_GridRebind( object sender, EventArgs e )
        {
            BindRegistrantsGrid();
        }

        void gRegistrants_RowDataBound( object sender, GridViewRowEventArgs e )
        {
            var registrant = e.Row.DataItem as RegistrationRegistrant;
            if ( registrant != null )
            {

                // Set the registrant name value
                var lRegistrant = e.Row.FindControl( "lRegistrant" ) as Literal;
                if ( lRegistrant != null )
                {
                    if ( registrant.PersonAlias != null && registrant.PersonAlias.Person != null )
                    {
                        lRegistrant.Text = registrant.PersonAlias.Person.FullNameReversed;
                    }
                    else
                    {
                        lRegistrant.Text = string.Empty;
                    }
                }

                // TODO Set the Group Name

                // Set the Fees
                var lFees = e.Row.FindControl( "lFees" ) as Literal;
                if ( lFees != null )
                {
                    if ( registrant.Fees != null && registrant.Fees.Any() )
                    {
                        var feeDesc = new List<string>();
                        foreach( var fee in registrant.Fees )
                        {
                             feeDesc.Add( string.Format( "{0}{1} ({2})",
                                 fee.Quantity > 1 ? fee.Quantity.ToString("N0") + " " : "",
                                 fee.Quantity > 1 ? fee.RegistrationTemplateFee.Name.Pluralize() : fee.RegistrationTemplateFee.Name,
                                 fee.Cost.ToString("C2")));
                        }
                        lFees.Text = feeDesc.AsDelimited( "<br/>" );
                    }
                }

            }
        }

        protected void gRegistrants_Delete( object sender, RowEventArgs e )
        {

        }

        protected void gRegistrants_RowSelected( object sender, RowEventArgs e )
        {

        }

        #endregion

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
                    registrationInstance = new RegistrationInstanceService( rockContext )
                        .Queryable( "RegistrationTemplate,Account,RegistrationTemplate.Forms.Fields" )
                        .AsNoTracking()
                        .FirstOrDefault( i => i.Id == RegistrationInstanceId.Value );
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
            apAccount.SetValue( RegistrationInstance.AccountId );
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

            lAccount.Visible = RegistrationInstance.Account != null;
            lAccount.Text = RegistrationInstance.Account != null ? RegistrationInstance.Account.Name : "";

            lMaxAttendees.Visible = RegistrationInstance.MaxAttendees > 0;
            lMaxAttendees.Text = RegistrationInstance.MaxAttendees.ToString( "N0" );

            lDetails.Visible = !string.IsNullOrWhiteSpace( RegistrationInstance.Details );
            lDetails.Text = RegistrationInstance.Details;

            LoadRegistrantFormFields( RegistrationInstance );
            BindRegistrationsFilter();
            BindRegistrantsFilter();

            ShowTab();
        }

        /// <summary>
        /// Sets the edit mode.
        /// </summary>
        /// <param name="editable">if set to <c>true</c> [editable].</param>
        private void SetEditMode( bool editable )
        {
            pnlEditDetails.Visible = editable;
            fieldsetViewDetails.Visible = !editable;
            pnlTabs.Visible = !editable;
        }

        private void ShowTab()
        {
            liRegistrations.RemoveCssClass( "active" );
            pnlRegistrations.Visible = false;

            liRegistrants.RemoveCssClass( "active" );
            pnlRegistrants.Visible = false;

            liLinkage.RemoveCssClass( "active" );
            pnlLinkages.Visible = false;

            switch ( ActiveTab ?? string.Empty )
            {
                case "lbRegistrants":
                    {
                        liRegistrants.AddCssClass( "active" );
                        pnlRegistrants.Visible = true;
                        BindRegistrantsGrid();
                        break;
                    }

                case "lbLinkage":
                    {
                        liLinkage.AddCssClass( "active" );
                        pnlLinkages.Visible = true;
                        BindLinkages();
                        break;
                    }

                default:
                    {
                        liRegistrations.AddCssClass( "active" );
                        pnlRegistrations.Visible = true;
                        BindRegistrationsGrid();
                        break;
                    }
            }
        }

        private void BindRegistrationsFilter()
        {
            drpRegistrationDateRange.DelimitedValues = fRegistrations.GetUserPreference( "Date Range" );
            ddlRegistrationPaymentStatus.SetValue( fRegistrations.GetUserPreference( "Payment Status" ) );
            tbRegistrationProcessorFirstName.Text = fRegistrations.GetUserPreference( "Processor First Name" );
            tbRegistrationProcessorLastName.Text = fRegistrations.GetUserPreference( "Processor Last Name" );
            tbRegistrationRegistrantFirstName.Text = fRegistrations.GetUserPreference( "Registrant First Name" );
            tbRegistrationRegistrantLastName.Text = fRegistrations.GetUserPreference( "Registrant Last Name" );
        }

        private void BindRegistrationsGrid()
        {
            int? instanceId = hfRegistrationInstanceId.Value.AsIntegerOrNull();
            if ( instanceId.HasValue )
            {
                using ( var rockContext = new RockContext() )
                {
                    var registrationEntityType = EntityTypeCache.Read( typeof( Rock.Model.Registration ) );

                    var qry = new RegistrationService( rockContext )
                        .Queryable( "PersonAlias.Person,Registrants.PersonAlias.Person,Registrants.Fees" )
                        .AsNoTracking()
                        .Where( r => r.RegistrationInstanceId == instanceId.Value );

                    if ( drpRegistrationDateRange.LowerValue.HasValue )
                    {
                        qry = qry.Where( r =>
                            r.CreatedDateTime.HasValue && 
                            r.CreatedDateTime.Value >= drpRegistrationDateRange.LowerValue.Value );
                    }
                    if ( drpRegistrationDateRange.UpperValue.HasValue )
                    {
                        qry = qry.Where( r =>
                            r.CreatedDateTime.HasValue && 
                            r.CreatedDateTime.Value <= drpRegistrationDateRange.UpperValue.Value );
                    }

                    if ( !string.IsNullOrWhiteSpace( tbRegistrationProcessorFirstName.Text ) )
                    {
                        string pfname = tbRegistrationProcessorFirstName.Text;
                        qry = qry.Where( r =>
                            r.FirstName.StartsWith( pfname ) ||
                            r.PersonAlias.Person.NickName.StartsWith( pfname ) ||
                            r.PersonAlias.Person.FirstName.StartsWith( pfname ) );
                    }

                    if ( !string.IsNullOrWhiteSpace( tbRegistrationProcessorLastName.Text ) )
                    {
                        string plname = tbRegistrationProcessorLastName.Text;
                        qry = qry.Where( r =>
                            r.LastName.StartsWith( plname ) ||
                            r.PersonAlias.Person.LastName.StartsWith( plname ) );
                    }

                    if ( !string.IsNullOrWhiteSpace( tbRegistrationRegistrantFirstName.Text ) )
                    {
                        string rfname = tbRegistrationRegistrantFirstName.Text;
                        qry = qry.Where( r =>
                            r.Registrants.Any( p => 
                                p.PersonAlias.Person.NickName.StartsWith( rfname ) ||
                                p.PersonAlias.Person.FirstName.StartsWith( rfname ) ) );
                    }

                    if ( !string.IsNullOrWhiteSpace( tbRegistrationRegistrantLastName.Text ) )
                    {
                        string rlname = tbRegistrationRegistrantLastName.Text;
                        qry = qry.Where( r =>
                            r.Registrants.Any( p =>
                                p.PersonAlias.Person.LastName.StartsWith( rlname ) ) );
                    }

                    // If filtering on payment status, need to do some sub-querying...
                    if ( ddlRegistrationPaymentStatus.SelectedValue != "" && registrationEntityType != null )
                    {
                        // Get all the registrant costs
                        var rCosts = new Dictionary<int, decimal>();
                        qry
                            .Select( r => new
                                {
                                    RegistrationId = r.Id,
                                    Costs = r.Registrants.Sum( p => p.Cost ),
                                    Fees = r.Registrants.SelectMany( p => p.Fees ).Sum( f => f.Cost )
                                } ).ToList()
                            .ForEach( c => 
                                rCosts.AddOrReplace( c.RegistrationId, c.Costs + c.Fees ) );

                        var rPayments = new Dictionary<int, decimal>();
                        new FinancialTransactionDetailService( rockContext )
                            .Queryable().AsNoTracking()
                            .Where( d =>
                                d.EntityTypeId.HasValue &&
                                d.EntityId.HasValue &&
                                d.EntityTypeId.Value == registrationEntityType.Id &&
                                rCosts.Keys.Contains( d.EntityId.Value ) )
                            .Select( d => new
                            {
                                RegistrationId = d.EntityId.Value,
                                Payment = d.Amount
                            } )
                            .ToList()
                            .GroupBy( d => d.RegistrationId )
                            .Select( d => new
                            {
                                RegistrationId = d.Key,
                                Payments = d.Sum( p => p.Payment )
                            } )
                            .ToList()
                            .ForEach( p =>
                                rPayments.AddOrReplace( p.RegistrationId, p.Payments ) );

                        var rPmtSummary = rCosts
                            .Join( rPayments, c => c.Key, p => p.Key, ( c, p ) => new
                            {
                                RegistrationId = c.Key,
                                Costs = c.Value,
                                Payments = p.Value
                            } )
                            .ToList();


                        var ids = new List<int>();

                        if ( ddlRegistrationPaymentStatus.SelectedValue == "Paid in Full" )
                        {
                            ids = rPmtSummary
                                .Where( r => r.Costs <= r.Payments)
                                .Select( r => r.RegistrationId )
                                .ToList();
                        }
                        else
                        {
                            ids = rPmtSummary
                                .Where( r => r.Costs > r.Payments )
                                .Select( r => r.RegistrationId )
                                .ToList();
                        }

                        qry = qry.Where( r => ids.Contains( r.Id ) );
                    }

                    IOrderedQueryable<Registration> orderedQry = null;
                    SortProperty sortProperty = gRegistrations.SortProperty;
                    if ( sortProperty != null )
                    {
                        orderedQry = qry.Sort( sortProperty );
                    }
                    else
                    {
                        orderedQry = qry.OrderByDescending( r => r.CreatedDateTime );
                    }

                    gRegistrations.SetLinqDataSource( orderedQry );

                    // Get all the payments for any registrations being displayed on the current page.
                    // This is used in the RowDataBound event but queried now so that each row does
                    // not have to query for the data.
                    var currentPageRegistrations = gRegistrations.DataSource as List<Registration>;
                    if ( currentPageRegistrations != null && registrationEntityType != null )
                    {
                        var registrationIds = currentPageRegistrations
                            .Select( r => r.Id )
                            .ToList();

                        RegistrationPayments = new FinancialTransactionDetailService( rockContext )
                            .Queryable().AsNoTracking()
                            .Where( d =>
                                d.EntityTypeId.HasValue &&
                                d.EntityId.HasValue &&
                                d.EntityTypeId.Value == registrationEntityType.Id &&
                                registrationIds.Contains( d.EntityId.Value ) )
                            .ToList();
                    }

                    gRegistrations.DataBind();
                }
            }
        }

        private void BindRegistrantsFilter()
        {
            drpRegistrantDateRange.DelimitedValues = fRegistrants.GetUserPreference( "Date Range" );
            tbRegistrantFirstName.Text = fRegistrants.GetUserPreference( "First Name" );
            tbRegistrantLastName.Text = fRegistrants.GetUserPreference( "Last Name" );

            AddDynamicRegistrantControls();
        }

        private void BindRegistrantsGrid()
        {
            int? instanceId = hfRegistrationInstanceId.Value.AsIntegerOrNull();
            if ( instanceId.HasValue )
            {

                // Get all the registrant attributes selected to be on grid
                var registrantAttributes = RegistrantFields
                    .Where( f =>
                        f.Attribute != null &&
                        f.FieldSource == RegistrationFieldSource.RegistrationAttribute )
                    .Select( f => f.Attribute )
                    .ToList();
                var registrantAttributeIds = registrantAttributes.Select( a => a.Id ).Distinct().ToList();

                // Get all the person attributes selected to be on grid
                var personAttributes = RegistrantFields
                    .Where( f =>
                        f.Attribute != null &&
                        f.FieldSource == RegistrationFieldSource.PersonAttribute )
                    .Select( f => f.Attribute )
                    .ToList();
                var personAttributesIds = personAttributes.Select( a => a.Id ).Distinct().ToList();

                // Get all the group member attributes selected to be on grid
                var groupMemberAttributes = RegistrantFields
                    .Where( f =>
                        f.Attribute != null &&
                        f.FieldSource == RegistrationFieldSource.GroupMemberAttribute )
                    .Select( f => f.Attribute )
                    .ToList();
                var groupMemberAttributesIds = groupMemberAttributes.Select( a => a.Id ).Distinct().ToList();

                using ( var rockContext = new RockContext() )
                {
                    // Start query for registrants
                    var qry = new RegistrationRegistrantService( rockContext )
                        .Queryable( "PersonAlias.Person,Fees.RegistrationTemplateFee" ).AsNoTracking()
                        .Where( r => 
                            r.Registration.RegistrationInstanceId == instanceId.Value &&
                            r.PersonAlias != null &&
                            r.PersonAlias.Person != null );

                    // Filter by daterange
                    if ( drpRegistrantDateRange.LowerValue.HasValue )
                    {
                        qry = qry.Where( r =>
                            r.CreatedDateTime.HasValue &&
                            r.CreatedDateTime.Value >= drpRegistrantDateRange.LowerValue.Value );
                    }
                    if ( drpRegistrantDateRange.UpperValue.HasValue )
                    {
                        qry = qry.Where( r =>
                            r.CreatedDateTime.HasValue &&
                            r.CreatedDateTime.Value <= drpRegistrantDateRange.UpperValue.Value );
                    }

                    // Filter by first name
                    if ( !string.IsNullOrWhiteSpace( tbRegistrantFirstName.Text ) )
                    {
                        string rfname = tbRegistrantFirstName.Text;
                        qry = qry.Where( r =>
                            r.PersonAlias.Person.NickName.StartsWith( rfname ) ||
                            r.PersonAlias.Person.FirstName.StartsWith( rfname ) );
                    }

                    // Filter by last name
                    if ( !string.IsNullOrWhiteSpace( tbRegistrantLastName.Text ) )
                    {
                        string rlname = tbRegistrantLastName.Text;
                        qry = qry.Where( r =>
                            r.PersonAlias.Person.LastName.StartsWith( rlname ) );
                    }

                    // Filter query by any configured registrant attribute filters
                    if ( registrantAttributes != null && registrantAttributes.Any() )
                    {
                        var attributeValueService = new AttributeValueService( rockContext );
                        var parameterExpression = attributeValueService.ParameterExpression;
                        foreach ( var attribute in registrantAttributes )
                        {
                            var filterControl = phRegistrantFormFieldFilters.FindControl( "filter_" + attribute.Id.ToString() );
                            if ( filterControl != null )
                            {
                                var filterValues = attribute.FieldType.Field.GetFilterValues( filterControl, attribute.QualifierValues );
                                var expression = attribute.FieldType.Field.AttributeFilterExpression( attribute.QualifierValues, filterValues, parameterExpression );
                                if ( expression != null )
                                {
                                    var attributeValues = attributeValueService
                                        .Queryable()
                                        .Where( v => v.Attribute.Id == attribute.Id );
                                    attributeValues = attributeValues.Where( parameterExpression, expression, null );
                                    qry = qry
                                        .Where( r => attributeValues.Select( v => v.EntityId ).Contains( r.Id ) );
                                }
                            }
                        }
                    }

                    // Filter query by any configured person attribute filters
                    if ( personAttributes != null && personAttributes.Any() )
                    {
                        var attributeValueService = new AttributeValueService( rockContext );
                        var parameterExpression = attributeValueService.ParameterExpression;
                        foreach ( var attribute in personAttributes )
                        {
                            var filterControl = phRegistrantFormFieldFilters.FindControl( "filter_" + attribute.Id.ToString() );
                            if ( filterControl != null )
                            {
                                var filterValues = attribute.FieldType.Field.GetFilterValues( filterControl, attribute.QualifierValues );
                                var expression = attribute.FieldType.Field.AttributeFilterExpression( attribute.QualifierValues, filterValues, parameterExpression );
                                if ( expression != null )
                                {
                                    var attributeValues = attributeValueService
                                        .Queryable()
                                        .Where( v => v.Attribute.Id == attribute.Id );
                                    attributeValues = attributeValues.Where( parameterExpression, expression, null );
                                    qry = qry
                                        .Where( r => attributeValues.Select( v => v.EntityId ).Contains( r.PersonAlias.PersonId ) );
                                }
                            }
                        }
                    }


                    // Filter query by any configured person attribute filters
                    if ( groupMemberAttributes != null && groupMemberAttributes.Any() )
                    {
                        var attributeValueService = new AttributeValueService( rockContext );
                        var parameterExpression = attributeValueService.ParameterExpression;
                        foreach ( var attribute in groupMemberAttributes )
                        {
                            var filterControl = phRegistrantFormFieldFilters.FindControl( "filter_" + attribute.Id.ToString() );
                            if ( filterControl != null )
                            {
                                var filterValues = attribute.FieldType.Field.GetFilterValues( filterControl, attribute.QualifierValues );
                                var expression = attribute.FieldType.Field.AttributeFilterExpression( attribute.QualifierValues, filterValues, parameterExpression );
                                if ( expression != null )
                                {
                                    var attributeValues = attributeValueService
                                        .Queryable()
                                        .Where( v => v.Attribute.Id == attribute.Id );
                                    attributeValues = attributeValues.Where( parameterExpression, expression, null );
                                    qry = qry
                                        .Where( r => r.GroupMemberId.HasValue && 
                                        attributeValues.Select( v => v.EntityId ).Contains( r.GroupMemberId.Value ) );
                                }
                            }
                        }
                    }

                    // Sort the query
                    IOrderedQueryable<RegistrationRegistrant> orderedQry = null;
                    SortProperty sortProperty = gRegistrants.SortProperty;
                    if ( sortProperty != null )
                    {
                        orderedQry = qry.Sort( sortProperty );
                    }
                    else
                    {
                        orderedQry = qry
                            .OrderBy( r => r.PersonAlias.Person.LastName )
                            .ThenBy( r => r.PersonAlias.Person.NickName );
                    }

                    // Set the grids LinqDataSource which will run query and set results for current page
                    gRegistrants.SetLinqDataSource<RegistrationRegistrant>( orderedQry );

                    // If there are any attributes that were selected to be displayed, we're going 
                    // to try and read all attribute values in one query and then put them into a 
                    // custom grid ObjectList property so that the AttributeField columns don't need 
                    // to do the LoadAttributes and querying of values for each row/column
                    if ( personAttributesIds.Any() || groupMemberAttributesIds.Any() || registrantAttributeIds.Any() )
                    {
                        // Get the query results for the current page
                        var currentPageRegistrants = gRegistrants.DataSource as List<RegistrationRegistrant>;
                        if ( currentPageRegistrants != null )
                        {
                            // Get all the registrant ids in current page of query results
                            var registrantIds = currentPageRegistrants
                                .Select( r => r.Id )
                                .Distinct()
                                .ToList();

                            // Get all the person ids in current page of query results
                            var personIds = currentPageRegistrants
                                .Select( r => r.PersonAlias.PersonId )
                                .Distinct()
                                .ToList();

                            // Get all the group member ids in current page of query results
                            var groupMemberIds = currentPageRegistrants
                                .Where( r => r.GroupMemberId.HasValue )
                                .Select( r => r.GroupMemberId.Value )
                                .Distinct()
                                .ToList();

                            // Query the attribute values for all rows and attributes
                            var attributeValues = new AttributeValueService( rockContext )
                                .Queryable( "Attribute" ).AsNoTracking()
                                .Where( v =>
                                    v.EntityId.HasValue &&
                                    (
                                        (
                                            personAttributesIds.Contains( v.AttributeId ) &&
                                            personIds.Contains( v.EntityId.Value )
                                        ) ||
                                        (
                                            groupMemberAttributesIds.Contains( v.AttributeId ) &&
                                            groupMemberIds.Contains( v.EntityId.Value )
                                        ) ||
                                        (
                                            registrantAttributeIds.Contains( v.AttributeId ) &&
                                            registrantIds.Contains( v.EntityId.Value )
                                        )
                                    )
                                )
                                .ToList();

                            // Get the attributes to add to each row's object
                            var attributes = new Dictionary<string, AttributeCache>();
                            RegistrantFields
                                    .Where( f => f.Attribute != null )
                                    .Select( f => f.Attribute )
                                    .ToList()
                                .ForEach( a => attributes
                                    .Add( a.Id.ToString() + a.Key, a ));

                            // Initialize the grid's object list 
                            gRegistrants.ObjectList = new Dictionary<string,object>();

                            // Loop through each of the current page's registrants and build an attribute object for
                            // storing attributes and the values for the registrant
                            foreach( var registrant in currentPageRegistrants )
                            {
                                // Create a row attribute object 
                                var gridObject = new GridRowAttributesObject();

                                // Add the attributes to the attribute object
                                gridObject.Attributes = attributes;

                                // Add any person attribute values to object
                                attributeValues
                                    .Where( v => 
                                        personAttributesIds.Contains( v.AttributeId ) &&
                                        v.EntityId.Value == registrant.PersonAlias.PersonId )
                                    .ToList()
                                    .ForEach( v => gridObject.AttributeValues
                                        .Add( v.AttributeId.ToString() + v.Attribute.Key, v ));

                                // Add any group member attribute values to object
                                if ( registrant.GroupMemberId.HasValue )
                                {
                                    attributeValues
                                        .Where( v =>
                                            groupMemberAttributesIds.Contains( v.AttributeId ) &&
                                            v.EntityId.Value == registrant.GroupMemberId.Value )
                                        .ToList()
                                        .ForEach( v => gridObject.AttributeValues
                                            .Add( v.AttributeId.ToString() + v.Attribute.Key, v ) );
                                }

                                // Add any registrant attribute values to object
                                attributeValues
                                    .Where( v =>
                                        registrantAttributeIds.Contains( v.AttributeId ) &&
                                        v.EntityId.Value == registrant.PersonAlias.PersonId )
                                    .ToList()
                                    .ForEach( v => gridObject.AttributeValues
                                        .Add( v.AttributeId.ToString() + v.Attribute.Key, v ) );

                                // Add row attribute object to grid's object list
                                gRegistrants.ObjectList.Add( registrant.Id.ToString(), gridObject);
                            }
                        }
                    }

                    gRegistrants.DataBind();
                }
            }
        }


        private void LoadRegistrantFormFields( RegistrationInstance registrationInstance )
        {
            RegistrantFields = new List<RegistrantFormField>();

            if ( registrationInstance != null )
            {
                foreach( var form in registrationInstance.RegistrationTemplate.Forms )
                {
                    foreach ( var formField in form.Fields )
                    {
                        if ( formField.FieldSource == RegistrationFieldSource.PersonField )
                        {
                            if ( formField.PersonFieldType != RegistrationPersonFieldType.FirstName &&
                                formField.PersonFieldType != RegistrationPersonFieldType.LastName )
                            {
                                RegistrantFields.Add(
                                    new RegistrantFormField
                                    {
                                        FieldSource = formField.FieldSource,
                                        PersonFieldType = formField.PersonFieldType
                                    } );
                            }
                        }
                        else
                        {
                            RegistrantFields.Add(
                                new RegistrantFormField
                                {
                                    FieldSource = formField.FieldSource,
                                    Attribute = AttributeCache.Read( formField.AttributeId.Value )
                                } );
                        }
                    }
                }
            }
        }

        private void AddDynamicRegistrantControls()
        {
            phRegistrantFormFieldFilters.Controls.Clear();

            foreach( var column in gRegistrants.Columns.OfType<AttributeField>().ToList() )
            {
                gRegistrants.Columns.Remove( column );
            }

            if ( RegistrantFields != null && RegistrantFields.Any() )
            {
                foreach( var field in RegistrantFields )
                {
                    if ( field.Attribute != null )
                    {
                        var attribute = field.Attribute;
                        var control = attribute.FieldType.Field.FilterControl( attribute.QualifierValues, "filter_" + attribute.Id.ToString(), false );
                        if ( control != null )
                        {
                            if ( control is IRockControl )
                            {
                                var rockControl = (IRockControl)control;
                                rockControl.Label = attribute.Name;
                                rockControl.Help = attribute.Description;
                                phRegistrantFormFieldFilters.Controls.Add( control );
                            }
                            else
                            {
                                var wrapper = new RockControlWrapper();
                                wrapper.ID = control.ID + "_wrapper";
                                wrapper.Label = attribute.Name;
                                wrapper.Controls.Add( control );
                                phRegistrantFormFieldFilters.Controls.Add( wrapper );
                            }

                            string savedValue = fRegistrants.GetUserPreference( attribute.Key );
                            if ( !string.IsNullOrWhiteSpace( savedValue ) )
                            {
                                try
                                {
                                    var values = JsonConvert.DeserializeObject<List<string>>( savedValue );
                                    attribute.FieldType.Field.SetFilterValues( control, attribute.QualifierValues, values );
                                }
                                catch { }
                            }
                        }

                        string dataFieldExpression = attribute.Id.ToString() + attribute.Key;
                        bool columnExists = gRegistrants.Columns.OfType<AttributeField>().FirstOrDefault( a => a.DataField.Equals( dataFieldExpression ) ) != null;
                        if ( !columnExists )
                        {
                            AttributeField boundField = new AttributeField();
                            boundField.DataField = dataFieldExpression;
                            boundField.HeaderText = attribute.Name;
                            boundField.SortExpression = string.Empty;
                            boundField.UseObjectListFirst = true;

                            var attributeCache = Rock.Web.Cache.AttributeCache.Read( attribute.Id );
                            if ( attributeCache != null )
                            {
                                boundField.ItemStyle.HorizontalAlign = attributeCache.FieldType.Field.AlignValue;
                            }

                            gRegistrants.Columns.Add( boundField );
                        }

                    }
                }
            }

            // Add delete column
            var deleteField = new DeleteField();
            gRegistrants.Columns.Add( deleteField );
            deleteField.Click += gRegistrants_Delete;
        }

        private void BindLinkages()
        {

        }

        /// <summary>
        /// Loads the drop downs.
        /// </summary>
        private void LoadDropDowns( RockContext rockContext )
        {

        }

        #endregion

        /// <summary>
        /// Helper class for tracking registration form fields
        /// </summary>
        [Serializable]
        public class RegistrantFormField
        {
            /// <summary>
            /// Gets or sets the field source.
            /// </summary>
            /// <value>
            /// The field source.
            /// </value>
            public RegistrationFieldSource FieldSource { get; set; }

            /// <summary>
            /// Gets or sets the type of the person field.
            /// </summary>
            /// <value>
            /// The type of the person field.
            /// </value>
            public RegistrationPersonFieldType? PersonFieldType { get; set; }

            /// <summary>
            /// Gets or sets the attribute.
            /// </summary>
            /// <value>
            /// The attribute.
            /// </value>
            public AttributeCache Attribute { get; set; }
        }

        /// <summary>
        /// Helper class used by registrant grid to pre-load all the attributes/values so that
        /// the attribute field columns don't need to load or query for any values on each row bind
        /// </summary>
        public class GridRowAttributesObject : IHasAttributes
        {
            int Id { get; set; }

            int IHasAttributes.Id
            {
                get { throw new NotImplementedException(); }
            }

            /// <summary>
            /// List of attributes associated with the object.  This property will not include the attribute values.
            /// The <see cref="AttributeValues" /> property should be used to get attribute values.  Dictionary key
            /// is the attribute key, and value is the cached attribute
            /// </summary>
            /// <value>
            /// The attributes.
            /// </value>
            public Dictionary<string, AttributeCache> Attributes { get; set; }

            /// <summary>
            /// Dictionary of all attributes and their value.  Key is the attribute key, and value is the associated attribute value
            /// </summary>
            /// <value>
            /// The attribute values.
            /// </value>
            public Dictionary<string, AttributeValue> AttributeValues { get; set; }

            /// <summary>
            /// Gets the attribute value defaults.  This property can be used by a subclass to override the parent class's default
            /// value for an attribute
            /// </summary>
            /// <value>
            /// The attribute value defaults.
            /// </value>
            public Dictionary<string, string> AttributeValueDefaults
            {
                get { return null; }
            }

            /// <summary>
            /// Gets the value of an attribute key.
            /// </summary>
            /// <param name="key">The key.</param>
            /// <returns></returns>
            public string GetAttributeValue( string key )
            {
                if ( this.AttributeValues != null &&
                    this.AttributeValues.ContainsKey( key ) )
                {
                    return this.AttributeValues[key].Value;
                }

                if ( this.Attributes != null &&
                    this.Attributes.ContainsKey( key ) )
                {
                    return this.Attributes[key].DefaultValue;
                }

                return null;
            }

            /// <summary>
            /// Gets the value of an attribute key - splitting that delimited value into a list of strings.
            /// </summary>
            /// <param name="key">The key.</param>
            /// <returns>
            /// A list of string values or an empty list if none exist.
            /// </returns>
            public List<string> GetAttributeValues( string key )
            {
                string value = GetAttributeValue( key );
                if ( !string.IsNullOrWhiteSpace( value ) )
                {
                    return value.SplitDelimitedValues().ToList();
                }

                return new List<string>();
            }

            /// <summary>
            /// Sets the value of an attribute key in memory.  Note, this will not persist value to database
            /// </summary>
            /// <param name="key">The key.</param>
            /// <param name="value">The value.</param>
            public void SetAttributeValue( string key, string value )
            {
                if ( this.AttributeValues != null &&
                    this.AttributeValues.ContainsKey( key ) )
                {
                    this.AttributeValues[key].Value = value;
                }
            }

            /// <summary>
            /// Initializes a new instance of the <see cref="GridRowAttributesObject"/> class.
            /// </summary>
            public GridRowAttributesObject()
            {
                Attributes = new Dictionary<string, AttributeCache>();
                AttributeValues = new Dictionary<string, AttributeValue>();
            }
        }
    }
}