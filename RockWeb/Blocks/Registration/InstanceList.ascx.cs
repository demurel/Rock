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
using System.Web.UI.WebControls;
using Newtonsoft.Json;
using Rock;
using Rock.Attribute;
using Rock.Data;
using Rock.Model;
using Rock.Security;
using Rock.Web.Cache;
using Rock.Web.UI;
using Rock.Web.UI.Controls;

namespace RockWeb.Blocks.Registration
{
    [DisplayName( "Instance List" )]
    [Category( "Registration" )]
    [Description( "Lists all the instances of the given registration template." )]

    [LinkedPage( "Detail Page" )]
    public partial class InstanceList : RockBlock, ISecondaryBlock
    {
        #region Private Variables

        private DefinedValueCache _inactiveStatus = null;
        private RegistrationTemplate _template = null;
        private bool _canView = false;

        #endregion

        #region Control Methods

        /// <summary>
        /// Raises the <see cref="E:System.Web.UI.Control.Init" /> event.
        /// </summary>
        /// <param name="e">An <see cref="T:System.EventArgs" /> object that contains the event data.</param>
        protected override void OnInit( EventArgs e )
        {
            base.OnInit( e );

            int templateId = PageParameter( "TemplateId" ).AsInteger();
            if ( templateId != 0 )
            {
                _template = new RegistrationTemplateService( new RockContext() ).Queryable( "GroupType.Roles" )
                    .Where( g => g.Id == templateId )
                    .FirstOrDefault();

                if ( _template != null && _template.IsAuthorized( Authorization.VIEW, CurrentPerson ) )
                {
                    _canView = true;

                    rFilter.ApplyFilterClick += rFilter_ApplyFilterClick;
                    gInstances.DataKeyNames = new string[] { "Id" };
                    gInstances.RowDataBound += gInstances_RowDataBound;
                    gInstances.Actions.AddClick += gInstances_AddClick;
                    gInstances.GridRebind += gInstances_GridRebind;
                    gInstances.ExportFilename = _template.Name;

                    // make sure they have Auth to edit the block OR edit to the Group
                    bool canEditBlock = IsUserAuthorized( Authorization.EDIT ) || _template.IsAuthorized( Authorization.EDIT, this.CurrentPerson );
                    gInstances.Actions.ShowAdd = canEditBlock;
                    gInstances.IsDeleteEnabled = canEditBlock;
                }
            }
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
                pnlContent.Visible = _canView;
                if ( _canView )
                {
                    SetFilter();
                    BindInstancesGrid();
                }
            }
        }

        #endregion

        #region Instances Grid

        /// <summary>
        /// Handles the RowDataBound event of the gInstances control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Web.UI.WebControls.GridViewRowEventArgs"/> instance containing the event data.</param>
        void gInstances_RowDataBound( object sender, System.Web.UI.WebControls.GridViewRowEventArgs e )
        {
            if ( e.Row.RowType == DataControlRowType.DataRow )
            {
                var groupMember = e.Row.DataItem as Instance;
                if ( groupMember != null && groupMember.Person != null )
                {
                    if ( _inactiveStatus != null &&
                        groupMember.Person.RecordStatusValueId.HasValue &&
                        groupMember.Person.RecordStatusValueId == _inactiveStatus.Id )
                    {
                        e.Row.AddCssClass( "inactive" );
                    }

                    if ( groupMember.Person.IsDeceased ?? false )
                    {
                        e.Row.AddCssClass( "deceased" );
                    }
                }
            }
        }

        /// <summary>
        /// Handles the ApplyFilterClick event of the rFilter control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs" /> instance containing the event data.</param>
        protected void rFilter_ApplyFilterClick( object sender, EventArgs e )
        {
            rFilter.SaveUserPreference( MakeKeyUniqueToGroup( "First Name" ), "First Name", tbFirstName.Text );
            rFilter.SaveUserPreference( MakeKeyUniqueToGroup( "Last Name" ), "Last Name", tbLastName.Text );
            rFilter.SaveUserPreference( MakeKeyUniqueToGroup( "Role" ), "Role", cblRole.SelectedValues.AsDelimited( ";" ) );
            rFilter.SaveUserPreference( MakeKeyUniqueToGroup( "Status" ), "Status", cblStatus.SelectedValues.AsDelimited( ";" ) );

            if ( AvailableAttributes != null )
            {
                foreach( var attribute in AvailableAttributes )
                {
                    var filterControl = phAttributeFilters.FindControl( "filter_" + attribute.Id.ToString() );
                    if ( filterControl != null )
                    {
                        try
                        {
                            var values = attribute.FieldType.Field.GetFilterValues( filterControl, attribute.QualifierValues );
                            rFilter.SaveUserPreference( MakeKeyUniqueToGroup( attribute.Key ), attribute.Name, attribute.FieldType.Field.GetFilterValues( filterControl, attribute.QualifierValues ).ToJson() );
                        }
                        catch { }
                    }
                }
            }

            BindInstancesGrid();
        }

        /// <summary>
        /// Rs the filter_ display filter value.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The e.</param>
        protected void rFilter_DisplayFilterValue( object sender, GridFilter.DisplayFilterValueArgs e )
        {

            if ( AvailableAttributes != null )
            {
                var attribute = AvailableAttributes.FirstOrDefault( a => MakeKeyUniqueToGroup( a.Key ) == e.Key );
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

            if ( e.Key == MakeKeyUniqueToGroup( "First Name" ) )
            {
                return;
            }
            else if ( e.Key == MakeKeyUniqueToGroup( "Last Name" ) )
            {
                return;
            }
            else if ( e.Key == MakeKeyUniqueToGroup( "Role" ) )
            {
                e.Value = ResolveValues( e.Value, cblRole );
            }
            else if ( e.Key == MakeKeyUniqueToGroup( "Status" ) )
            {
                e.Value = ResolveValues( e.Value, cblStatus );
            }
            else
            {
                e.Value = string.Empty;
            }

        }

        /// <summary>
        /// Handles the Click event of the DeleteInstance control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="Rock.Web.UI.Controls.RowEventArgs" /> instance containing the event data.</param>
        protected void DeleteInstance_Click( object sender, Rock.Web.UI.Controls.RowEventArgs e )
        {
            RockContext rockContext = new RockContext();
            InstanceService groupMemberService = new InstanceService( rockContext );
            Instance groupMember = groupMemberService.Get( e.RowKeyId );
            if ( groupMember != null )
            {
                string errorMessage;
                if ( !groupMemberService.CanDelete( groupMember, out errorMessage ) )
                {
                    mdGridWarning.Show( errorMessage, ModalAlertType.Information );
                    return;
                }

                int groupId = groupMember.GroupId;

                groupMemberService.Delete( groupMember );
                rockContext.SaveChanges();

                Group group = new GroupService( rockContext ).Get( groupId );
                if ( group.IsSecurityRole || group.GroupType.Guid.Equals( Rock.SystemGuid.GroupType.GROUPTYPE_SECURITY_ROLE.AsGuid() ) )
                {
                    // person removed from SecurityRole, Flush
                    Rock.Security.Role.Flush( group.Id );
                    Rock.Security.Authorization.Flush();
                }
            }

            BindInstancesGrid();
        }

        /// <summary>
        /// Handles the AddClick event of the gInstances control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs" /> instance containing the event data.</param>
        /// <exception cref="System.NotImplementedException"></exception>
        protected void gInstances_AddClick( object sender, EventArgs e )
        {
            NavigateToLinkedPage( "DetailPage", "InstanceId", 0, "GroupId", _template.Id );
        }

        /// <summary>
        /// Handles the Edit event of the gInstances control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RowEventArgs" /> instance containing the event data.</param>
        protected void gInstances_Edit( object sender, RowEventArgs e )
        {
            NavigateToLinkedPage( "DetailPage", "InstanceId", e.RowKeyId );
        }

        /// <summary>
        /// Handles the GridRebind event of the gInstances control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs" /> instance containing the event data.</param>
        /// <exception cref="System.NotImplementedException"></exception>
        protected void gInstances_GridRebind( object sender, EventArgs e )
        {
            BindInstancesGrid();
        }

        #endregion

        #region Internal Methods

        /// <summary>
        /// Binds the filter.
        /// </summary>
        private void SetFilter()
        {
            if ( _template != null )
            {
                cblRole.DataSource = _template.GroupType.Roles.OrderBy( a => a.Order ).ToList();
                cblRole.DataBind();
            }

            cblStatus.BindToEnum<InstanceStatus>();

            BindAttributes();
            AddDynamicControls();

            tbFirstName.Text = rFilter.GetUserPreference( MakeKeyUniqueToGroup( "First Name" ) );
            tbLastName.Text = rFilter.GetUserPreference( MakeKeyUniqueToGroup( "Last Name" ) );

            string roleValue = rFilter.GetUserPreference( MakeKeyUniqueToGroup( "Role" ) );
            if ( !string.IsNullOrWhiteSpace( roleValue ) )
            {
                cblRole.SetValues( roleValue.Split( ';' ).ToList() );
            }

            string statusValue = rFilter.GetUserPreference( MakeKeyUniqueToGroup( "Status" ) );
            if ( !string.IsNullOrWhiteSpace( statusValue ) )
            {
                cblStatus.SetValues( statusValue.Split( ';' ).ToList() );
            }

        }

        private void BindAttributes()
        {
            // Parse the attribute filters 
            AvailableAttributes = new List<AttributeCache>();
            if ( _template != null )
            {
                int entityTypeId = new Instance().TypeId;
                string groupQualifier = _template.Id.ToString();
                string groupTypeQualifier = _template.GroupTypeId.ToString();
                foreach ( var attributeModel in new AttributeService( new RockContext() ).Queryable()
                    .Where( a =>
                        a.EntityTypeId == entityTypeId &&
                        a.IsGridColumn &&
                        ( ( a.EntityTypeQualifierColumn.Equals( "GroupId", StringComparison.OrdinalIgnoreCase ) && a.EntityTypeQualifierValue.Equals( groupQualifier ) ) ||
                         ( a.EntityTypeQualifierColumn.Equals( "GroupTypeId", StringComparison.OrdinalIgnoreCase ) && a.EntityTypeQualifierValue.Equals( groupTypeQualifier ) ) ) )
                    .OrderByDescending( a => a.EntityTypeQualifierColumn )
                    .ThenBy( a => a.Order )
                    .ThenBy( a => a.Name ) )
                {
                    AvailableAttributes.Add( AttributeCache.Read( attributeModel ) );
                }
            }
        }

        /// <summary>
        /// Adds the attribute columns.
        /// </summary>
        private void AddDynamicControls()
        {
            // Clear the filter controls
            phAttributeFilters.Controls.Clear();

            // Remove attribute columns
            foreach ( var column in gInstances.Columns.OfType<AttributeField>().ToList() )
            {
                gInstances.Columns.Remove( column );
            }

            if ( AvailableAttributes != null )
            {
                foreach( var attribute in AvailableAttributes )
                {
                    var control = attribute.FieldType.Field.FilterControl( attribute.QualifierValues, "filter_" + attribute.Id.ToString(), false );
                    if ( control != null )
                    {
                        if ( control is IRockControl )
                        {
                            var rockControl = (IRockControl)control;
                            rockControl.Label = attribute.Name;
                            rockControl.Help = attribute.Description;
                            phAttributeFilters.Controls.Add( control );
                        }
                        else
                        {
                            var wrapper = new RockControlWrapper();
                            wrapper.ID = control.ID + "_wrapper";
                            wrapper.Label = attribute.Name;
                            wrapper.Controls.Add( control );
                            phAttributeFilters.Controls.Add( wrapper );
                        }

                        string savedValue = rFilter.GetUserPreference( MakeKeyUniqueToGroup( attribute.Key ) );
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

                    string dataFieldExpression = attribute.Key;
                    bool columnExists = gInstances.Columns.OfType<AttributeField>().FirstOrDefault( a => a.DataField.Equals( dataFieldExpression ) ) != null;
                    if ( !columnExists )
                    {
                        AttributeField boundField = new AttributeField();
                        boundField.DataField = dataFieldExpression;
                        boundField.HeaderText = attribute.Name;
                        boundField.SortExpression = string.Empty;

                        var attributeCache = Rock.Web.Cache.AttributeCache.Read( attribute.Id );
                        if ( attributeCache != null )
                        {
                            boundField.ItemStyle.HorizontalAlign = attributeCache.FieldType.Field.AlignValue;
                        }

                        gInstances.Columns.Add( boundField );
                    }
                }
            }

            // Add Link to Profile Page Column
            if ( !string.IsNullOrEmpty( GetAttributeValue( "PersonProfilePage" ) ) )
            {
                AddPersonProfileLinkColumn();
            }

            // Add delete column
            var deleteField = new DeleteField();
            gInstances.Columns.Add( deleteField );
            deleteField.Click += DeleteInstance_Click;
        }

        /// <summary>
        /// Adds the column with a link to profile page.
        /// </summary>
        private void AddPersonProfileLinkColumn()
        {
            HyperLinkField hlPersonProfileLink = new HyperLinkField();
            hlPersonProfileLink.ItemStyle.HorizontalAlign = HorizontalAlign.Center;
            hlPersonProfileLink.HeaderStyle.CssClass = "grid-columncommand";
            hlPersonProfileLink.ItemStyle.CssClass = "grid-columncommand";
            hlPersonProfileLink.DataNavigateUrlFields = new String[1] { "PersonId" };
            hlPersonProfileLink.DataNavigateUrlFormatString = LinkedPageUrl( "PersonProfilePage", new Dictionary<string, string> { { "PersonId", "###" } } ).Replace( "###", "{0}" );
            hlPersonProfileLink.DataTextFormatString = "<div class='btn btn-default'><i class='fa fa-user'></i></div>";
            hlPersonProfileLink.DataTextField = "PersonId";
            gInstances.Columns.Add( hlPersonProfileLink );
        }

        /// <summary>
        /// Binds the group members grid.
        /// </summary>
        protected void BindInstancesGrid()
        {
            if ( _template != null )
            {
                pnlInstances.Visible = true;

                lHeading.Text = string.Format( "{0} {1}", _template.GroupType.GroupTerm, _template.GroupType.InstanceTerm.Pluralize() );

                if ( _template.GroupType.Roles.Any() )
                {
                    nbRoleWarning.Visible = false;
                    rFilter.Visible = true;
                    gInstances.Visible = true;

                    var rockContext = new RockContext();

                    InstanceService groupMemberService = new InstanceService( rockContext );
                    var qry = groupMemberService.Queryable( "Person,GroupRole", true ).AsNoTracking()
                        .Where( m => m.GroupId == _template.Id );

                    // Filter by First Name
                    string firstName = tbFirstName.Text;
                    if ( !string.IsNullOrWhiteSpace( firstName ) )
                    {
                        qry = qry.Where( m => m.Person.FirstName.StartsWith( firstName ) );
                    }

                    // Filter by Last Name
                    string lastName = tbLastName.Text;
                    if ( !string.IsNullOrWhiteSpace( lastName ) )
                    {
                        qry = qry.Where( m => m.Person.LastName.StartsWith( lastName ) );
                    }

                    // Filter by role
                    var validGroupTypeRoles = _template.GroupType.Roles.Select( r => r.Id ).ToList();
                    var roles = new List<int>();
                    foreach ( string role in cblRole.SelectedValues )
                    {
                        if ( !string.IsNullOrWhiteSpace( role ) )
                        {
                            int roleId = int.MinValue;
                            if ( int.TryParse( role, out roleId ) && validGroupTypeRoles.Contains( roleId ) )
                            {
                                roles.Add( roleId );
                            }
                        }
                    }
                    if ( roles.Any() )
                    {
                        qry = qry.Where( m => roles.Contains( m.GroupRoleId ) );
                    }

                    // Filter by Status
                    var statuses = new List<InstanceStatus>();
                    foreach ( string status in cblStatus.SelectedValues )
                    {
                        if ( !string.IsNullOrWhiteSpace( status ) )
                        {
                            statuses.Add( status.ConvertToEnum<InstanceStatus>() );
                        }
                    }
                    if ( statuses.Any() )
                    {
                        qry = qry.Where( m => statuses.Contains( m.InstanceStatus ) );
                    }

                    // Filter query by any configured attribute filters
                    if ( AvailableAttributes != null && AvailableAttributes.Any() )
                    {
                        var attributeValueService = new AttributeValueService( rockContext );
                        var parameterExpression = attributeValueService.ParameterExpression;

                        foreach ( var attribute in AvailableAttributes )
                        {
                            var filterControl = phAttributeFilters.FindControl( "filter_" + attribute.Id.ToString() );
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

                                    qry = qry.Where( w => attributeValues.Select( v => v.EntityId ).Contains( w.Id ) );
                                }
                            }
                        }
                    }


                    _inactiveStatus = DefinedValueCache.Read( Rock.SystemGuid.DefinedValue.PERSON_RECORD_STATUS_INACTIVE );

                    SortProperty sortProperty = gInstances.SortProperty;

                    List<Instance> instances = null;

                    if ( sortProperty != null )
                    {
                        instances = qry.Sort( sortProperty ).ToList();
                    }
                    else
                    {
                        instances = qry.OrderBy( a => a.GroupRole.Order ).ThenBy( a => a.Person.LastName ).ThenBy( a => a.Person.FirstName ).ToList();
                    }

                    // Since we're not binding to actual group member list, but are using AttributeField columns,
                    // we need to save the workflows into the grid's object list
                    gInstances.ObjectList = new Dictionary<string, object>();
                    instances.ForEach( m => gInstances.ObjectList.Add( m.Id.ToString(), m ) );
                    gInstances.EntityTypeId = EntityTypeCache.Read( Rock.SystemGuid.EntityType.GROUP_MEMBER.AsGuid() ).Id;

                    gInstances.DataSource = instances.Select( m => new
                    {
                        m.Id,
                        m.Guid,
                        m.PersonId,
                        Name = m.Person.NickName + " " + m.Person.LastName,
                        GroupRole = m.GroupRole.Name,
                        m.InstanceStatus
                    } ).ToList();

                    gInstances.DataBind();
                }
                else
                {
                    nbRoleWarning.Text = string.Format(
                        "{0} cannot be added to this {1} because the '{2}' group type does not have any roles defined.",
                        _template.GroupType.InstanceTerm.Pluralize(),
                        _template.GroupType.GroupTerm,
                        _template.GroupType.Name );

                    nbRoleWarning.Visible = true;
                    rFilter.Visible = false;
                    gInstances.Visible = false;
                }
            }
            else
            {
                pnlInstances.Visible = false;
            }
        }

        /// <summary>
        /// Resolves the values.
        /// </summary>
        /// <param name="values">The values.</param>
        /// <param name="listControl">The list control.</param>
        /// <returns></returns>
        private string ResolveValues( string values, System.Web.UI.WebControls.CheckBoxList checkBoxList )
        {
            var resolvedValues = new List<string>();

            foreach ( string value in values.Split( ';' ) )
            {
                var item = checkBoxList.Items.FindByValue( value );
                if ( item != null )
                {
                    resolvedValues.Add( item.Text );
                }
            }

            return resolvedValues.AsDelimited( ", " );
        }

        /// <summary>
        /// Makes the key unique to group.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns></returns>
        private string MakeKeyUniqueToGroup( string key )
        {
            if ( _template != null )
            {
                return string.Format( "{0}-{1}", _template.Id, key );
            }
            return key;
        }

        #endregion

        #region ISecondaryBlock

        /// <summary>
        /// Sets the visible.
        /// </summary>
        /// <param name="visible">if set to <c>true</c> [visible].</param>
        public void SetVisible( bool visible )
        {
            pnlContent.Visible = visible;
        }

        #endregion
    }
}