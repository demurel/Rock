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
using Rock.Data;
using Rock.Model;

namespace Rock.Web.UI.Controls
{
    /// <summary>
    /// 
    /// </summary>
    public class GroupPicker : ItemPicker
    {
        /// <summary>
        /// Raises the <see cref="E:System.Web.UI.Control.Init" /> event.
        /// </summary>
        /// <param name="e">An <see cref="T:System.EventArgs" /> object that contains the event data.</param>
        protected override void OnInit( EventArgs e )
        {
            ItemRestUrlExtraParams = "";
            this.IconCssClass = "fa fa-users";
            base.OnInit( e );
        }

        /// <summary>
        /// Sets the value.
        /// </summary>
        /// <param name="group">The group.</param>
        public void SetValue( Group group )
        {
            if ( group != null )
            {
                ItemId = group.Id.ToString();

                var parentIds = GetGroupAncestorsIdList( group.ParentGroup );

                var parentGroupIds = parentIds.AsDelimited( "," );

                InitialItemParentIds = parentGroupIds.TrimEnd( new[] { ',' } );
                ItemName = group.Name;
            }
            else
            {
                ItemId = Constants.None.IdValue;
                ItemName = Constants.None.TextHtml;
            }
        }

        /// <summary>
        /// Returns a list of the ancestor Groups of the specified Group.
        /// If the ParentGroup property of the Group is not populated, it is assumed to be a top-level node.
        /// </summary>
        /// <param name="group"></param>
        /// <param name="ancestorGroupIds"></param>
        /// <returns></returns>
        private List<int> GetGroupAncestorsIdList( Group group, List<int> ancestorGroupIds = null )
        {
            if ( ancestorGroupIds == null )
            {
                ancestorGroupIds = new List<int>();
            }

            if ( group == null )
            {
                return ancestorGroupIds;
            }

            // If we have encountered this node previously in our tree walk, there is a recursive loop in the tree.
            // Add an invalid Id to indicate that a problem was encountered, and exit.
            if ( ancestorGroupIds.Contains( group.Id ) )
            {
                ancestorGroupIds.Add( -1 );

                return ancestorGroupIds;
            }

            // Create or add this node to the history stack for this tree walk.
            ancestorGroupIds.Add( group.Id );

            ancestorGroupIds = this.GetGroupAncestorsIdList( group.ParentGroup, ancestorGroupIds );

            return ancestorGroupIds;
        }

        /// <summary>
        /// Sets the values.
        /// </summary>
        /// <param name="groups">The groups.</param>
        public void SetValues( IEnumerable<Group> groups )
        {
            var theGroups = groups.ToList();

            if ( theGroups.Any() )
            {
                var ids = new List<string>();
                var names = new List<string>();
                var parentGroupIds = string.Empty;

                foreach ( var group in theGroups )
                {
                    if ( group != null )
                    {
                        ids.Add( group.Id.ToString() );
                        names.Add( group.Name );
                        var parentGroup = group.ParentGroup;

                        while ( parentGroup != null )
                        {
                            parentGroupIds += parentGroup.Id.ToString() + ",";
                            parentGroup = parentGroup.ParentGroup;
                        }
                    }
                }

                InitialItemParentIds = parentGroupIds.TrimEnd( new[] { ',' } );
                ItemIds = ids;
                ItemNames = names;
            }
            else
            {
                ItemId = Constants.None.IdValue;
                ItemName = Constants.None.TextHtml;
            }
        }

        /// <summary>
        /// Sets the value on select.
        /// </summary>
        protected override void SetValueOnSelect()
        {
            var group = new GroupService( new RockContext() ).Get( int.Parse( ItemId ) );
            SetValue( group );
        }

        /// <summary>
        /// Sets the values on select.
        /// </summary>
        protected override void SetValuesOnSelect()
        {
            var groups = new GroupService( new RockContext() ).Queryable().Where( g => ItemIds.Contains( g.Id.ToString() ) );
            this.SetValues( groups );
        }

        /// <summary>
        /// Gets the item rest URL.
        /// </summary>
        /// <value>
        /// The item rest URL.
        /// </value>
        public override string ItemRestUrl
        {
            get { return "~/api/groups/getchildren/"; }
        }

    }
}