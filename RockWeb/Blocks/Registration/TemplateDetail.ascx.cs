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

using Newtonsoft.Json;

using Rock;
using Rock.Attribute;
using Rock.Constants;
using Rock.Data;
using Rock.Model;
using Rock.Security;
using Rock.Web.Cache;
using Rock.Web.UI;
using Rock.Web.UI.Controls;
using Attribute = Rock.Model.Attribute;

namespace RockWeb.Blocks.Registration
{
    [DisplayName( "Template Detail" )]
    [Category( "Registration" )]
    [Description( "Displays the details of the given registration template." )]

    public partial class TemplateDetail : RockBlock
    {
        #region Properties

        private List<RegistrationTemplateDiscount> DiscountState { get; set; }
        private List<RegistrationTemplateFee> FeeState { get; set; }

        #endregion

        #region Base Control Methods

        /// <summary>
        /// Restores the view-state information from a previous user control request that was saved by the <see cref="M:System.Web.UI.UserControl.SaveViewState" /> method.
        /// </summary>
        /// <param name="savedState">An <see cref="T:System.Object" /> that represents the user control state to be restored.</param>
        protected override void LoadViewState( object savedState )
        {
            base.LoadViewState( savedState );

            string json = ViewState["DiscountState"] as string;
            if ( string.IsNullOrWhiteSpace( json ) )
            {
                DiscountState = new List<RegistrationTemplateDiscount>();
            }
            else
            {
                DiscountState = JsonConvert.DeserializeObject<List<RegistrationTemplateDiscount>>( json );
            }

            json = ViewState["FeeState"] as string;
            if ( string.IsNullOrWhiteSpace( json ) )
            {
                FeeState = new List<RegistrationTemplateFee>();
            }
            else
            {
                FeeState = JsonConvert.DeserializeObject<List<RegistrationTemplateFee>>( json );
            }
        }

        /// <summary>
        /// Raises the <see cref="E:System.Web.UI.Control.Init" /> event.
        /// </summary>
        /// <param name="e">An <see cref="T:System.EventArgs" /> object that contains the event data.</param>
        protected override void OnInit( EventArgs e )
        {
            base.OnInit( e );

            // assign discounts grid actions
            gDiscounts.AddCssClass( "discount-grid" );
            gDiscounts.DataKeyNames = new string[] { "Guid" };
            gDiscounts.Actions.ShowAdd = true;
            gDiscounts.Actions.AddClick += gDiscounts_AddClick; ;
            gDiscounts.GridRebind += gDiscounts_GridRebind;
            gDiscounts.GridReorder += gDiscounts_GridReorder;

            // assign fees grid actions
            gFees.AddCssClass( "fee-grid" );
            gFees.DataKeyNames = new string[] { "Guid" };
            gFees.Actions.ShowAdd = true;
            gFees.Actions.AddClick += gFees_AddClick;
            gFees.GridRebind += gFees_GridRebind;
            gFees.GridReorder += gFees_GridReorder;
            
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
            else
            {
                ShowDialog();
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
            var jsonSetting = new JsonSerializerSettings 
            { 
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                ContractResolver = new Rock.Utility.IgnoreUrlEncodedKeyContractResolver()
            };

            ViewState["DiscountState"] = JsonConvert.SerializeObject( DiscountState, Formatting.None, jsonSetting );
            ViewState["FeeState"] = JsonConvert.SerializeObject( FeeState, Formatting.None, jsonSetting );

            return base.SaveViewState();
        }

        #endregion

        #region Events

        /// <summary>
        /// Handles the Click event of the btnEdit control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs" /> instance containing the event data.</param>
        protected void btnEdit_Click( object sender, EventArgs e )
        {
            var rockContext = new RockContext();
            var RegistrationTemplate = new RegistrationTemplateService( rockContext ).Get( hfRegistrationTemplateId.Value.AsInteger() );

            LoadStateDetails( RegistrationTemplate, rockContext );
            ShowEditDetails( RegistrationTemplate, rockContext );
        }

        /// <summary>
        /// Handles the Click event of the btnDelete control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs" /> instance containing the event data.</param>
        protected void btnDelete_Click( object sender, EventArgs e )
        {
            var rockContext = new RockContext();

            var service = new RegistrationTemplateService( rockContext );
            var RegistrationTemplate = service.Get( int.Parse( hfRegistrationTemplateId.Value ) );

            if ( RegistrationTemplate != null )
            {
                if ( !RegistrationTemplate.IsAuthorized( Authorization.ADMINISTRATE, this.CurrentPerson ) )
                {
                    mdDeleteWarning.Show( "You are not authorized to delete this registration template.", ModalAlertType.Information );
                    return;
                }

                service.Delete( RegistrationTemplate );

                rockContext.SaveChanges();
            }

            // reload page
            var qryParams = new Dictionary<string, string>();
            NavigateToPage( RockPage.Guid, qryParams );
        }

        /// <summary>
        /// Handles the Click event of the btnCopy control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        protected void btnCopy_Click( object sender, EventArgs e )
        {
            var rockContext = new RockContext();
            var RegistrationTemplate = new RegistrationTemplateService( rockContext ).Get( hfRegistrationTemplateId.Value.AsInteger() );

            if ( RegistrationTemplate != null )
            {
                // Load the state objects for the source registration template
                LoadStateDetails( RegistrationTemplate, rockContext );

                // clone the registration template
                var newRegistrationTemplate = RegistrationTemplate.Clone( false );
                newRegistrationTemplate.CreatedByPersonAlias = null;
                newRegistrationTemplate.CreatedByPersonAliasId = null;
                newRegistrationTemplate.CreatedDateTime = RockDateTime.Now;
                newRegistrationTemplate.ModifiedByPersonAlias = null;
                newRegistrationTemplate.ModifiedByPersonAliasId = null;
                newRegistrationTemplate.ModifiedDateTime = RockDateTime.Now;
                newRegistrationTemplate.Id = 0;
                newRegistrationTemplate.Guid = Guid.NewGuid();
                newRegistrationTemplate.Name = RegistrationTemplate.Name + " - Copy";

                // Create temporary state objects for the new registration template
                var newDiscountState = new List<RegistrationTemplateDiscount>();
                var newFeeState = new List<RegistrationTemplateFee>();

                foreach ( var discount in DiscountState )
                {
                    var newDiscount = discount.Clone( false );
                    newDiscount.RegistrationTemplateId = 0;
                    newDiscount.Id = 0;
                    newDiscount.Guid = Guid.NewGuid();
                    newDiscountState.Add( newDiscount );
                }

                foreach ( var fee in FeeState )
                {
                    var newFee = fee.Clone( false );
                    newFee.RegistrationTemplateId = 0;
                    newFee.Id = 0;
                    newFee.Guid = Guid.NewGuid();
                    newFeeState.Add( newFee );
                }

                RegistrationTemplate = newRegistrationTemplate;
                DiscountState = newDiscountState;
                FeeState = newFeeState;

                hfRegistrationTemplateId.Value = RegistrationTemplate.Id.ToString();
                ShowEditDetails( RegistrationTemplate, rockContext );
            }
        }

        /// <summary>
        /// Handles the Click event of the btnSave control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs" /> instance containing the event data.</param>
        protected void btnSave_Click( object sender, EventArgs e )
        {
            var rockContext = new RockContext();
            var service = new RegistrationTemplateService( rockContext );

            RegistrationTemplate RegistrationTemplate = null;

            int? RegistrationTemplateId = hfRegistrationTemplateId.Value.AsIntegerOrNull();
            if ( RegistrationTemplateId.HasValue )
            {
                RegistrationTemplate = service.Get( RegistrationTemplateId.Value );
            }

            if ( RegistrationTemplate == null )
            {
                RegistrationTemplate = new RegistrationTemplate();
            }

            RegistrationTemplate.IsActive = cbIsActive.Checked;
            RegistrationTemplate.Name = tbName.Text;
            RegistrationTemplate.CategoryId = cpCategory.SelectedValueAsInt();
            RegistrationTemplate.GroupTypeId = gtpGroupType.SelectedGroupTypeId;
            RegistrationTemplate.GroupMemberRoleId = rpGroupTypeRole.GroupRoleId;
            RegistrationTemplate.GroupMemberStatus = ddlGroupMemberStatus.SelectedValueAsEnum<GroupMemberStatus>();
            RegistrationTemplate.NotifyGroupLeaders = cbNotifyLeaders.Checked;
            RegistrationTemplate.LoginRequired = cbLoginRequired.Checked;
            RegistrationTemplate.AllowMultipleRegistrants = cbMultipleRegistrants.Checked;
            RegistrationTemplate.MaxRegistrants = nbMaxRegistrants.Text.AsInteger();
            RegistrationTemplate.RegistrantsSameFamily = rblRegistrantsInSameFamily.SelectedValueAsEnum<RegistrantsSameFamily>();
            RegistrationTemplate.FinancialGatewayId = fgpFinancialGateway.SelectedValueAsInt();
            RegistrationTemplate.MinimumInitialPayment = cbMinimumInitialPayment.Text.AsDecimal();

            RegistrationTemplate.RequestHomeCampus = rblRequestHomeCampus.SelectedValueAsEnum<RegistrationRequestField>();
            RegistrationTemplate.RequestPhone = rblRequestPhone.SelectedValueAsEnum<RegistrationRequestField>();
            RegistrationTemplate.RequestEmail = rblRequestEmail.SelectedValueAsEnum<RegistrationRequestField>();
            RegistrationTemplate.RequestBirthDate = rblRequstBirthdate.SelectedValueAsEnum<RegistrationRequestField>();
            RegistrationTemplate.RequestGender = rblRequestGender.SelectedValueAsEnum<RegistrationRequestField>();
            RegistrationTemplate.RequestMaritalStatus = rblRequestMaritalStatus.SelectedValueAsEnum<RegistrationRequestField>();

            RegistrationTemplate.ReminderEmailTemplate = ceReminderEmailTemplate.Text;
            RegistrationTemplate.UseDefaultConfirmationEmail = cbUserDefaultConfirmation.Checked;
            RegistrationTemplate.ConfirmationEmailTemplate = ceConfirmationEmailTemplate.Text;

            RegistrationTemplate.RegistrationTerm = tbRegistrationTerm.Text;
            RegistrationTemplate.RegistrantTerm = tbRegistrantTerm.Text;
            RegistrationTemplate.FeeTerm = tbFeeTerm.Text;
            RegistrationTemplate.DiscountCodeTerm = tbDiscountCodeTerm.Text;
            RegistrationTemplate.SuccessTitle = tbSuccessTitle.Text;
            RegistrationTemplate.SuccessText = tbSuccessText.Text;

            if ( !Page.IsValid || !RegistrationTemplate.IsValid )
            {
                return;
            }

            rockContext.WrapTransaction( () =>
            {
                // Save the entity field changes to registration template
                if ( RegistrationTemplate.Id.Equals( 0 ) )
                {
                    service.Add( RegistrationTemplate );
                }
                rockContext.SaveChanges();

                // delete discounts that aren't assigned in the UI anymore
                var registrationTemplateDiscountService = new RegistrationTemplateDiscountService( rockContext );
                var uiGuids = DiscountState.Select( u => u.Guid ).ToList();
                {
                    foreach ( var discount in registrationTemplateDiscountService
                        .Queryable()
                        .Where( d =>
                            d.RegistrationTemplateId == RegistrationTemplate.Id &&
                            !uiGuids.Contains( d.Guid ) ) )
                    {
                        registrationTemplateDiscountService.Delete( discount );
                    }
                }

                // delete fees that aren't assigned in the UI anymore
                var registrationTemplateFeeService = new RegistrationTemplateFeeService( rockContext );
                uiGuids = FeeState.Select( u => u.Guid ).ToList();
                foreach ( var fee in  registrationTemplateFeeService
                    .Queryable()
                    .Where( d => 
                        d.RegistrationTemplateId == RegistrationTemplate.Id &&
                        !uiGuids.Contains( d.Guid ) ) )
                {
                    registrationTemplateFeeService.Delete( fee );
                }

                // add/updated discounts
                foreach ( var discountUI in DiscountState )
                {
                    var discount = RegistrationTemplate.Discounts.FirstOrDefault( a => a.Guid.Equals( discountUI.Guid ) );
                    if ( discount == null )
                    {
                        discount = new RegistrationTemplateDiscount();
                        discount.Guid = discountUI.Guid;
                        RegistrationTemplate.Discounts.Add( discount );
                    }
                    discount.Code = discountUI.Code;
                    discount.DiscountPercentage = discountUI.DiscountPercentage;
                    discount.DiscountAmount = discountUI.DiscountAmount;
                    discount.Order = discountUI.Order;
                }

                // add/updated fees
                foreach ( var feeUI in FeeState )
                {
                    var fee = RegistrationTemplate.Fees.FirstOrDefault( a => a.Guid.Equals( feeUI.Guid ) );
                    if ( fee == null )
                    {
                        fee = new RegistrationTemplateFee();
                        fee.Guid = feeUI.Guid;
                        RegistrationTemplate.Fees.Add( fee );
                    }
                    fee.Name = feeUI.Name;
                    fee.FeeType = feeUI.FeeType;
                    fee.CostValue = feeUI.CostValue;
                    fee.DiscountApplies = feeUI.DiscountApplies;
                    fee.AllowMultiple = feeUI.AllowMultiple;
                    fee.Order = feeUI.Order;
                }

                rockContext.SaveChanges();

            } );

            var qryParams = new Dictionary<string, string>();
            qryParams["RegistrationTemplateId"] = RegistrationTemplate.Id.ToString();
            NavigateToPage( RockPage.Guid, qryParams );
        }

        /// <summary>
        /// Handles the Click event of the btnCancel control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs" /> instance containing the event data.</param>
        protected void btnCancel_Click( object sender, EventArgs e )
        {
            if ( hfRegistrationTemplateId.Value.Equals( "0" ) )
            {
                int? parentCategoryId = PageParameter( "ParentCategoryId" ).AsIntegerOrNull();
                if ( parentCategoryId.HasValue )
                {
                    // Cancelling on Add, and we know the parentCategoryId, so we are probably in treeview mode, so navigate to the current page
                    var qryParams = new Dictionary<string, string>();
                    qryParams["CategoryId"] = parentCategoryId.ToString();
                    NavigateToPage( RockPage.Guid, qryParams );
                }
                else
                {
                    // Cancelling on Add.  Return to Grid
                    NavigateToParentPage();
                }
            }
            else
            {
                // Cancelling on Edit.  Return to Details
                RegistrationTemplateService service = new RegistrationTemplateService( new RockContext() );
                RegistrationTemplate item = service.Get( int.Parse( hfRegistrationTemplateId.Value ) );
                ShowReadonlyDetails( item );
            }
        }

        protected void gtpGroupType_SelectedIndexChanged( object sender, EventArgs e )
        {
            rpGroupTypeRole.GroupTypeId = gtpGroupType.SelectedGroupTypeId ?? 0;
        }

        protected void cbMultipleRegistrants_CheckedChanged( object sender, EventArgs e )
        {
            nbMaxRegistrants.Visible = cbMultipleRegistrants.Checked;
        }

        protected void cbUserDefaultConfirmation_CheckedChanged( object sender, EventArgs e )
        {
            ceConfirmationEmailTemplate.Visible = !cbUserDefaultConfirmation.Checked;
        }

        protected void rblDiscountType_SelectedIndexChanged( object sender, EventArgs e )
        {
            if ( rblDiscountType.SelectedValue == "Amount" )
            {
                nbDiscountPercentage.Visible = false;
                cbDiscountAmount.Visible = true;
            }
            else
            {
                nbDiscountPercentage.Visible = true;
                cbDiscountAmount.Visible = false;
            }
        }

        protected void rblFeeType_SelectedIndexChanged( object sender, EventArgs e )
        {
            var feeType = rblFeeType.SelectedValueAsEnum<RegistrationFeeType>();
            cCost.Visible = feeType == RegistrationFeeType.Single;
            kvlMultipleFees.Visible = feeType == RegistrationFeeType.Multiple;
        }

        #endregion

        #region Methods

        #region Show Details

        /// <summary>
        /// Shows the detail.
        /// </summary>
        private void ShowDetail()
        {
            int? RegistrationTemplateId = PageParameter( "RegistrationTemplateId" ).AsIntegerOrNull();
            int? parentCategoryId = PageParameter( "ParentCategoryId" ).AsIntegerOrNull();

            if ( !RegistrationTemplateId.HasValue )
            {
                pnlDetails.Visible = false;
                return;
            }

            var rockContext = new RockContext();

            RegistrationTemplate RegistrationTemplate = null;
            if ( RegistrationTemplateId.HasValue )
            {
                RegistrationTemplate = new RegistrationTemplateService( rockContext ).Get( RegistrationTemplateId.Value );
            }

            if ( RegistrationTemplate == null )
            {
                RegistrationTemplate = new RegistrationTemplate();
                RegistrationTemplate.Id = 0;
                RegistrationTemplate.IsActive = true;
                RegistrationTemplate.CategoryId = parentCategoryId;
                RegistrationTemplate.UseDefaultConfirmationEmail = true;
            }

            pnlDetails.Visible = true;
            hfRegistrationTemplateId.Value = RegistrationTemplate.Id.ToString();

            // render UI based on Authorized and IsSystem
            bool readOnly = false;

            nbEditModeMessage.Text = string.Empty;

            // User must have 'Edit' rights to block, or 'Administrate' rights to template
            if ( !UserCanEdit && !RegistrationTemplate.IsAuthorized( Authorization.ADMINISTRATE, CurrentPerson ) )
            {
                readOnly = true;
                nbEditModeMessage.Heading = "Information";
                nbEditModeMessage.Text = EditModeMessage.ReadOnlyEditActionNotAllowed( RegistrationTemplate.FriendlyTypeName );
            }

            if ( readOnly )
            {
                btnEdit.Visible = false;
                btnSecurity.Visible = false;
                ShowReadonlyDetails( RegistrationTemplate );
            }
            else
            {
                btnEdit.Visible = true;

                btnSecurity.Title = "Secure " + RegistrationTemplate.Name;
                btnSecurity.EntityId = RegistrationTemplate.Id;

                if ( RegistrationTemplate.Id > 0 )
                {
                    ShowReadonlyDetails( RegistrationTemplate );
                }
                else
                {
                    LoadStateDetails(RegistrationTemplate, rockContext);
                    ShowEditDetails( RegistrationTemplate, rockContext );
                }
            }
        }

        private void LoadStateDetails( RegistrationTemplate RegistrationTemplate, RockContext rockContext )
        {
            if ( RegistrationTemplate != null )
            {
                DiscountState = RegistrationTemplate.Discounts.OrderBy( a => a.Order ).ToList();
                FeeState = RegistrationTemplate.Fees.OrderBy( a => a.Order ).ToList();

            }
            else
            {
                DiscountState = new List<RegistrationTemplateDiscount>();
                FeeState = new List<RegistrationTemplateFee>();
            }
        }

        /// <summary>
        /// Shows the edit details.
        /// </summary>
        /// <param name="RegistrationTemplate">The registration template.</param>
        /// <param name="rockContext">The rock context.</param>
        private void ShowEditDetails( RegistrationTemplate RegistrationTemplate, RockContext rockContext )
        {
            if ( RegistrationTemplate.Id == 0 )
            {
                lReadOnlyTitle.Text = ActionTitle.Add( RegistrationTemplate.FriendlyTypeName ).FormatAsHtmlTitle();
                hlInactive.Visible = false;
                hlType.Visible = false;
            }
            else
            {
                pwDetails.Expanded = false;
            }

            SetEditMode( true );

            LoadDropDowns( rockContext );

            cbIsActive.Checked = RegistrationTemplate.IsActive;
            tbName.Text = RegistrationTemplate.Name;
            cpCategory.SetValue( RegistrationTemplate.CategoryId );

            gtpGroupType.SelectedGroupTypeId = RegistrationTemplate.GroupTypeId;
            rpGroupTypeRole.GroupTypeId = RegistrationTemplate.GroupTypeId ?? 0;
            rpGroupTypeRole.GroupRoleId = RegistrationTemplate.GroupMemberRoleId;
            ddlGroupMemberStatus.SetValue( RegistrationTemplate.GroupMemberStatus.ConvertToInt() );
            cbNotifyLeaders.Checked = RegistrationTemplate.NotifyGroupLeaders;
            cbLoginRequired.Checked = RegistrationTemplate.LoginRequired;
            cbMultipleRegistrants.Checked = RegistrationTemplate.AllowMultipleRegistrants;
            nbMaxRegistrants.Visible = RegistrationTemplate.AllowMultipleRegistrants;
            nbMaxRegistrants.Text = RegistrationTemplate.MaxRegistrants.ToString();
            rblRegistrantsInSameFamily.SetValue( RegistrationTemplate.RegistrantsSameFamily.ConvertToInt() );
            fgpFinancialGateway.SetValue( RegistrationTemplate.FinancialGatewayId );
            cbMinimumInitialPayment.Text = RegistrationTemplate.MinimumInitialPayment.ToString();

            rblRequestHomeCampus.SetValue( RegistrationTemplate.RequestHomeCampus.ConvertToInt() );
            rblRequestPhone.SetValue( RegistrationTemplate.RequestPhone.ConvertToInt() );
            rblRequestEmail.SetValue( RegistrationTemplate.RequestEmail.ConvertToInt() );
            rblRequstBirthdate.SetValue( RegistrationTemplate.RequestBirthDate.ConvertToInt() );
            rblRequestGender.SetValue( RegistrationTemplate.RequestGender.ConvertToInt() );
            rblRequestMaritalStatus.SetValue( RegistrationTemplate.RequestMaritalStatus.ConvertToInt() );

            ceReminderEmailTemplate.Text = RegistrationTemplate.ReminderEmailTemplate;
            cbUserDefaultConfirmation.Checked = RegistrationTemplate.UseDefaultConfirmationEmail;
            ceConfirmationEmailTemplate.Visible = !cbUserDefaultConfirmation.Checked;
            ceConfirmationEmailTemplate.Text = RegistrationTemplate.ConfirmationEmailTemplate;

            tbRegistrationTerm.Text = RegistrationTemplate.RegistrationTerm;
            tbRegistrantTerm.Text = RegistrationTemplate.RegistrantTerm;
            tbFeeTerm.Text = RegistrationTemplate.FeeTerm;
            tbDiscountCodeTerm.Text = RegistrationTemplate.DiscountCodeTerm;

            tbSuccessTitle.Text = RegistrationTemplate.SuccessTitle;
            tbSuccessText.Text = RegistrationTemplate.SuccessText;

            BindDiscountsGrid();
            BindFeesGrid();
        }

        /// <summary>
        /// Shows the readonly details.
        /// </summary>
        /// <param name="RegistrationTemplate">The registration template.</param>
        private void ShowReadonlyDetails( RegistrationTemplate RegistrationTemplate )
        {
            SetEditMode( false );

            hfRegistrationTemplateId.SetValue( RegistrationTemplate.Id );
            DiscountState = null;
            FeeState = null;

            lReadOnlyTitle.Text = RegistrationTemplate.Name.FormatAsHtmlTitle();
            hlInactive.Visible = RegistrationTemplate.IsActive == false;
            hlType.Visible = RegistrationTemplate.Category != null;
            hlType.Text = RegistrationTemplate.Category != null ?
                RegistrationTemplate.Category.Name : string.Empty;
            lGroupType.Text = RegistrationTemplate.GroupType != null ?
                RegistrationTemplate.GroupType.Name : string.Empty;

            lGateway.Text = RegistrationTemplate.FinancialGateway != null ?
                RegistrationTemplate.FinancialGateway.Name : string.Empty;

            rFees.DataSource = RegistrationTemplate.Fees.OrderBy( f => f.Order ).ToList();
            rFees.DataBind();
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
            gtpGroupType.GroupTypes = new GroupTypeService( rockContext )
                .Queryable().AsNoTracking()
                .Where( t => t.ShowInNavigation )
                .OrderBy( t => t.Order )
                .ThenBy( t => t.Name )
                .ToList();
                
            ddlGroupMemberStatus.BindToEnum<GroupMemberStatus>();
            rblRegistrantsInSameFamily.BindToEnum<RegistrantsSameFamily>();
            rblRequestHomeCampus.BindToEnum<RegistrationRequestField>();
            rblRequestPhone.BindToEnum<RegistrationRequestField>();
            rblRequestEmail.BindToEnum<RegistrationRequestField>();
            rblRequstBirthdate.BindToEnum<RegistrationRequestField>();
            rblRequestGender.BindToEnum<RegistrationRequestField>();
            rblRequestMaritalStatus.BindToEnum<RegistrationRequestField>();
            rblFeeType.BindToEnum<RegistrationFeeType>();
        }

        #endregion

        #region Discount Grid

        protected void gDiscounts_GridReorder( object sender, GridReorderEventArgs e )
        {
            var movedItem = DiscountState.Where( a => a.Order == e.OldIndex ).FirstOrDefault();
            if ( movedItem != null )
            {
                if ( e.NewIndex < e.OldIndex )
                {
                    // Moved up
                    foreach ( var otherItem in DiscountState.Where( a => a.Order < e.OldIndex && a.Order >= e.NewIndex ) )
                    {
                        otherItem.Order = otherItem.Order + 1;
                    }
                }
                else
                {
                    // Moved Down
                    foreach ( var otherItem in DiscountState.Where( a => a.Order > e.OldIndex && a.Order <= e.NewIndex ) )
                    {
                        otherItem.Order = otherItem.Order - 1;
                    }
                }

                movedItem.Order = e.NewIndex;
            }

            int order = 0;
            DiscountState.OrderBy( d => d.Order ).ToList().ForEach( d => d.Order = order++ );

            BindDiscountsGrid();
        }

        protected void gDiscounts_GridRebind( object sender, EventArgs e )
        {
            BindDiscountsGrid();
        }

        protected void gDiscounts_AddClick( object sender, EventArgs e )
        {
            ShowDiscountEdit( Guid.NewGuid() );
        }

        protected void gDiscounts_Edit( object sender, RowEventArgs e )
        {
            ShowDiscountEdit( e.RowKeyValue.ToString().AsGuid() );
        }

        protected void dlgDiscount_SaveClick( object sender, EventArgs e )
        {
            RegistrationTemplateDiscount discount = null;
            var discountGuid = hfDiscountGuid.Value.AsGuidOrNull();
            if ( discountGuid.HasValue )
            {
                discount = DiscountState.Where( f => f.Guid.Equals( discountGuid.Value ) ).FirstOrDefault();
            }

            if ( discount == null )
            {
                discount = new RegistrationTemplateDiscount();
                discount.Guid = Guid.NewGuid();
                discount.Order = DiscountState.Any() ? DiscountState.Max( d => d.Order ) + 1 : 0;
                DiscountState.Add( discount );
            }

            discount.Code = tbDiscountCode.Text;
            if ( rblDiscountType.SelectedValue == "Amount" )
            {
                discount.DiscountPercentage = 0;
                discount.DiscountAmount = cbDiscountAmount.Text.AsDecimal();
            }
            else
            {
                discount.DiscountPercentage = nbDiscountPercentage.Text.AsDouble();
                discount.DiscountAmount = 0;
            }

            HideDialog();

            hfDiscountGuid.Value = string.Empty;

            BindDiscountsGrid();
        }


        protected void gDiscounts_Delete( object sender, RowEventArgs e )
        {
            var discountGuid = e.RowKeyValue.ToString().AsGuid();
            var discount = DiscountState.FirstOrDefault( f => f.Guid.Equals( e.RowKeyValue.ToString().AsGuid() ) );
            if ( discount != null )
            {
                DiscountState.Remove( discount );

                int order = 0;
                DiscountState.OrderBy( f => f.Order ).ToList().ForEach( f => f.Order = order++ );

                BindDiscountsGrid();
            }
        }

        private void BindDiscountsGrid()
        {
            gDiscounts.DataSource = DiscountState.OrderBy( d => d.Order )
                .Select( d => new {
                    d.Guid,
                    d.Id,
                    d.Code,
                    Discount = d.DiscountAmount > 0 ?
                        d.DiscountAmount.ToString("C2") :
                        d.DiscountPercentage.ToString("N0") + " %"
                }).ToList();
            gDiscounts.DataBind();
        }

        private void ShowDiscountEdit( Guid discountGuid )
        {
            var discount = DiscountState.FirstOrDefault( d => d.Guid.Equals( discountGuid ));
            if ( discount == null )
            {
                discount = new RegistrationTemplateDiscount();
            }

            hfDiscountGuid.Value = discount.Guid.ToString();
            tbDiscountCode.Text = discount.Code;
            nbDiscountPercentage.Text = discount.DiscountPercentage.ToString();
            cbDiscountAmount.Text = discount.DiscountAmount.ToString();

            if ( discount.DiscountAmount > 0 )
            {
                rblDiscountType.SetValue( "Amount" );
                nbDiscountPercentage.Visible = false;
                cbDiscountAmount.Visible = true;
            }
            else
            {
                rblDiscountType.SetValue( "Percentage" );
                nbDiscountPercentage.Visible = true;
                cbDiscountAmount.Visible = false;
            }

            ShowDialog( "Discounts" );
        }

        #endregion

        #region Fee Grid

        protected void gFees_GridReorder( object sender, GridReorderEventArgs e )
        {
            var movedItem = FeeState.Where( a => a.Order == e.OldIndex ).FirstOrDefault();
            if ( movedItem != null )
            {
                if ( e.NewIndex < e.OldIndex )
                {
                    // Moved up
                    foreach ( var otherItem in FeeState.Where( a => a.Order < e.OldIndex && a.Order >= e.NewIndex ) )
                    {
                        otherItem.Order = otherItem.Order + 1;
                    }
                }
                else
                {
                    // Moved Down
                    foreach ( var otherItem in FeeState.Where( a => a.Order > e.OldIndex && a.Order <= e.NewIndex ) )
                    {
                        otherItem.Order = otherItem.Order - 1;
                    }
                }

                movedItem.Order = e.NewIndex;
            }

            int order = 0;
            FeeState.OrderBy( f => f.Order ).ToList().ForEach( f => f.Order = order++ );

            BindFeesGrid();
        }

        protected void gFees_GridRebind( object sender, EventArgs e )
        {
            BindFeesGrid();
        }

        protected void gFees_AddClick( object sender, EventArgs e )
        {
            ShowFeeEdit( Guid.NewGuid() );
        }        

        protected void gFees_Edit( object sender, RowEventArgs e )
        {
            ShowFeeEdit( e.RowKeyValue.ToString().AsGuid() );
        }

        protected void dlgFee_SaveClick( object sender, EventArgs e )
        {
            RegistrationTemplateFee fee = null;
            var feeGuid = hfFeeGuid.Value.AsGuidOrNull();
            if ( feeGuid.HasValue )
            {
                fee = FeeState.Where( f => f.Guid.Equals( feeGuid.Value ) ).FirstOrDefault();
            }

            if ( fee == null )
            {
                fee = new RegistrationTemplateFee();
                fee.Guid = Guid.NewGuid();
                fee.Order = FeeState.Any() ? FeeState.Max( d => d.Order ) + 1 : 0;
                FeeState.Add( fee );
            }

            fee.Name = tbFeeName.Text;
            fee.FeeType = rblFeeType.SelectedValueAsEnum<RegistrationFeeType>();
            if ( fee.FeeType == RegistrationFeeType.Single )
            {
                fee.CostValue = cCost.Text;
            }
            else
            {
                fee.CostValue = kvlMultipleFees.Value;
            }
            fee.AllowMultiple = cbAllowMultiple.Checked;
            fee.DiscountApplies = cbDiscountApplies.Checked;

            HideDialog();

            hfFeeGuid.Value = string.Empty;

            BindFeesGrid();
        }
            
        protected void gFees_Delete( object sender, RowEventArgs e )
        {
            var feeGuid = e.RowKeyValue.ToString().AsGuid();
            var fee = FeeState.FirstOrDefault( f => f.Guid.Equals( e.RowKeyValue.ToString().AsGuid()));
            if ( fee != null )
            {
                FeeState.Remove( fee );

                int order = 0;
                FeeState.OrderBy( f => f.Order ).ToList().ForEach( f => f.Order = order++ );

                BindFeesGrid();
            }
        }

        private void BindFeesGrid()
        {
            gFees.DataSource = FeeState.OrderBy( f => f.Order )
                .Select( f => new
                {
                    f.Id,
                    f.Guid,
                    f.Name,
                    f.FeeType,
                    Cost = FormatFeeCost( f.CostValue ),
                    f.AllowMultiple,
                    f.DiscountApplies
                } )
                .ToList();
            gFees.DataBind();
        }

        private void ShowFeeEdit( Guid feeGuid )
        {
            var fee = FeeState.FirstOrDefault( d => d.Guid.Equals( feeGuid ));
            if ( fee == null )
            {
                fee = new RegistrationTemplateFee();
            }

            hfFeeGuid.Value = fee.Guid.ToString();
            tbFeeName.Text = fee.Name;

            rblFeeType.SetValue( fee.FeeType.ConvertToInt() );

            cCost.Visible = fee.FeeType == RegistrationFeeType.Single;
            cCost.Text = fee.CostValue;

            kvlMultipleFees.Visible = fee.FeeType == RegistrationFeeType.Multiple;
            kvlMultipleFees.Value = fee.CostValue;

            cbAllowMultiple.Checked = fee.AllowMultiple;
            cbDiscountApplies.Checked = fee.DiscountApplies;

            ShowDialog( "Fees" );
        }

        protected string FormatFeeCost( string value )
        {
            var values = new List<string>();

            string[] nameValues = value.Split( new char[] { '|' }, StringSplitOptions.RemoveEmptyEntries );
            foreach ( string nameValue in nameValues )
            {
                string[] nameAndValue = nameValue.Split( new char[] { '^' }, StringSplitOptions.RemoveEmptyEntries );
                if ( nameAndValue.Length == 2 )
                {
                    values.Add( string.Format( "{0}-{1:C2}", nameAndValue[0], nameAndValue[1].AsDecimal() ) );
                }
                else
                {
                    values.Add( string.Format( "{0:C2}", nameValue.AsDecimal() ) );
                }
            }

            return values.AsDelimited( ", " );
        }
        #endregion

        #region Dialog

        /// <summary>
        /// Shows the dialog.
        /// </summary>
        /// <param name="dialog">The dialog.</param>
        /// <param name="setValues">if set to <c>true</c> [set values].</param>
        private void ShowDialog( string dialog, bool setValues = false )
        {
            hfActiveDialog.Value = dialog.ToUpper().Trim();
            ShowDialog( setValues );
        }

        /// <summary>
        /// Shows the dialog.
        /// </summary>
        /// <param name="setValues">if set to <c>true</c> [set values].</param>
        private void ShowDialog( bool setValues = false )
        {
            switch ( hfActiveDialog.Value )
            {
                case "DISCOUNTS":
                    dlgDiscount.Show();
                    break;
                case "FEES":
                    dlgFee.Show();
                    break;
            }
        }

        /// <summary>
        /// Hides the dialog.
        /// </summary>
        private void HideDialog()
        {
            switch ( hfActiveDialog.Value )
            {
                case "DISCOUNTS":
                    dlgDiscount.Hide();
                    break;
                case "FEES":
                    dlgFee.Hide();
                    break;
            }

            hfActiveDialog.Value = string.Empty;
        }

        #endregion

        #endregion
    }
}