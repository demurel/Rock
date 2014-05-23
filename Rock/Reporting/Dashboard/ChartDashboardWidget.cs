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
using System.Text;
using Rock.Attribute;
using Rock.Model;
using Rock.Utility;
using Rock.Web.Cache;
using Rock.Web.UI;
using Rock.Web.UI.Controls;

namespace Rock.Reporting.Dashboard
{
    /// <summary>
    /// 
    /// </summary>
    [DefinedValueField( Rock.SystemGuid.DefinedType.CHART_STYLES, "Chart Style", Order = 3 )]
    [CustomCheckboxListField( "Metric Value Types", "Select which metric value types to display in the chart", "Goal,Measure", false, "Measure", Order = 4 )]
    [MetricEntityField( "Metric", "Select the metric and the filter", Order = 5 )]
    [SlidingDateRangeField( "Date Range", Key = "SlidingDateRange", DefaultValue = "1||4", Order = 6 )]
    [LinkedPage( "Detail Page", "Select the page to navigate to when the chart is clicked", Order = 7 )]
    public abstract class ChartDashboardWidget : DashboardWidget
    {
        /// <summary>
        /// Raises the <see cref="E:System.Web.UI.Control.Init" /> event.
        /// </summary>
        /// <param name="e">An <see cref="T:System.EventArgs" /> object that contains the event data.</param>
        protected override void OnInit( EventArgs e )
        {
            base.OnInit( e );

            this.BlockUpdated += Block_BlockUpdated;
        }

        /// <summary>
        /// Raises the <see cref="E:System.Web.UI.Control.Load" /> event.
        /// </summary>
        /// <param name="e">The <see cref="T:System.EventArgs" /> object that contains the event data.</param>
        protected override void OnLoad( EventArgs e )
        {
            base.OnLoad( e );

            LoadChart();
        }

        /// <summary>
        /// Handles the BlockUpdated event of the Block control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        protected void Block_BlockUpdated( object sender, EventArgs e )
        {
            LoadChart();
        }

        /// <summary>
        /// Loads the chart.
        /// </summary>
        public abstract void LoadChart();

        /// <summary>
        /// Gets the metric identifier.
        /// </summary>
        /// <value>
        /// The metric identifier.
        /// </value>
        public int? MetricId
        {
            get
            {
                var valueParts = GetAttributeValue( "Metric" ).Split( '|' );
                if ( valueParts.Length > 1 )
                {
                    Guid metricGuid = valueParts[0].AsGuid();
                    var metric = new Rock.Model.MetricService( new Rock.Data.RockContext() ).Get( metricGuid );
                    if ( metric != null )
                    {
                        return metric.Id;
                    }
                }

                return null;
            }
        }

        /// <summary>
        /// Gets a value indicating whether to get the Entity from the Page context vs. the configured EntityId
        /// </summary>
        /// <value>
        /// <c>true</c> if [get entity from context]; otherwise, <c>false</c>.
        /// </value>
        private bool GetEntityFromContext
        {
            get
            {
                var valueParts = GetAttributeValue( "Metric" ).Split( '|' );
                if ( valueParts.Length > 2 )
                {
                    return valueParts[2].AsBoolean();
                }

                return false;
            }
        }

        /// <summary>
        /// Gets the entity identifier either from a specific selection, PageContext, or null, depending on the Metric/Entity selection
        /// </summary>
        /// <value>
        /// The entity identifier.
        /// </value>
        public int? EntityId
        {
            get
            {
                int? result = null;
                if ( GetEntityFromContext )
                {
                    EntityTypeCache entityTypeCache = null;
                    if ( this.MetricId.HasValue )
                    {
                        var metric = new Rock.Model.MetricService( new Rock.Data.RockContext() ).Get( this.MetricId ?? 0 );
                        if ( metric != null )
                        {
                            entityTypeCache = EntityTypeCache.Read( metric.EntityTypeId ?? 0 );
                        }
                    }

                    if ( this.ContextEntity( entityTypeCache.Name ) != null )
                    {
                        result = this.ContextEntity( entityTypeCache.Name ).Id;
                    }

                    // if Getting the EntityFromContext, and we didn't get it from ContextEntity, get it from the Page Param
                    if ( !result.HasValue )
                    {
                        // figure out what the param name should be ("CampusId, GroupId, etc") depending on metric's entityType
                        var entityParamName = "EntityId";
                        if ( entityTypeCache != null )
                        {
                            entityParamName = entityTypeCache.Name + "Id";
                        }

                        result = this.PageParameter( entityParamName ).AsIntegerOrNull();
                    }
                }
                else
                {
                    var valueParts = GetAttributeValue( "Metric" ).Split( '|' );
                    if ( valueParts.Length > 1 )
                    {
                        result = valueParts[1].AsIntegerOrNull();
                    }
                }

                return result;
            }
        }

        /// <summary>
        /// Gets the detail page.
        /// </summary>
        /// <value>
        /// The detail page.
        /// </value>
        public Guid? DetailPageGuid
        {
            get
            {
                return ( GetAttributeValue( "DetailPage" ) ?? string.Empty ).AsGuidOrNull();
            }
        }

        /// <summary>
        /// Gets a value indicating whether to combine values with different EntityId values into one series vs. showing each in it's own series
        /// </summary>
        /// <value>
        ///   <c>true</c> if [combine values]; otherwise, <c>false</c>.
        /// </value>
        public bool CombineValues
        {
            get
            {
                var valueParts = GetAttributeValue( "Metric" ).Split( '|' );
                if ( valueParts.Length > 3 )
                {
                    return valueParts[3].AsBoolean();
                }

                return false;
            }
        }

        /// <summary>
        /// Gets the type of the metric value.
        /// </summary>
        /// <value>
        /// The type of the metric value.
        /// </value>
        public MetricValueType? MetricValueType
        {
            get
            {
                string[] metricValueTypes = this.GetAttributeValue( "MetricValueTypes" ).SplitDelimitedValues();
                var selected = metricValueTypes.Select( a => a.ConvertToEnum<MetricValueType>() ).ToArray();
                if ( selected.Length == 1 )
                {
                    // if they picked one, return that one
                    return selected[0];
                }
                else
                {
                    // if they picked both or neither, return null, which indicates to show both
                    return null;
                }
            }
        }

        /// <summary>
        /// Gets the chart style.
        /// </summary>
        /// <value>
        /// The chart style.
        /// </value>
        public ChartStyle ChartStyle
        {
            get
            {
                Guid? chartStyleDefinedValueGuid = this.GetAttributeValue( "ChartStyle" ).AsGuidOrNull();
                if ( chartStyleDefinedValueGuid.HasValue )
                {
                    var rockContext = new Rock.Data.RockContext();
                    var definedValue = new DefinedValueService( rockContext ).Get( chartStyleDefinedValueGuid.Value );
                    if ( definedValue != null )
                    {
                        try
                        {
                            definedValue.LoadAttributes( rockContext );
                            return ChartStyle.CreateFromJson( definedValue.Name, definedValue.GetAttributeValue( "ChartStyle" ) );
                        }
                        catch ( Exception ex )
                        {
                            WidgetErrorMessage = "Error loading Chart Style: " + definedValue.Name;
                            WidgetErrorDetails = ex.Message;
                        }
                    }
                }

                return new ChartStyle();
            }
        }

        /// <summary>
        /// Gets the date range.
        /// </summary>
        /// <value>
        /// The date range.
        /// </value>
        public DateRange DateRange
        {
            get
            {
                return SlidingDateRangePicker.CalculateDateRangeFromDelimitedValues( GetAttributeValue( "SlidingDateRange" ) ?? "1||4" );
            }
        }
    }
}