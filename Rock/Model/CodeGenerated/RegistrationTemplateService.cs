//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by the Rock.CodeGeneration project
//     Changes to this file will be lost when the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------
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
using System.Linq;

using Rock.Data;

namespace Rock.Model
{
    /// <summary>
    /// RegistrationTemplate Service class
    /// </summary>
    public partial class RegistrationTemplateService : Service<RegistrationTemplate>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RegistrationTemplateService"/> class
        /// </summary>
        /// <param name="context">The context.</param>
        public RegistrationTemplateService(RockContext context) : base(context)
        {
        }

        /// <summary>
        /// Determines whether this instance can delete the specified item.
        /// </summary>
        /// <param name="item">The item.</param>
        /// <param name="errorMessage">The error message.</param>
        /// <returns>
        ///   <c>true</c> if this instance can delete the specified item; otherwise, <c>false</c>.
        /// </returns>
        public bool CanDelete( RegistrationTemplate item, out string errorMessage )
        {
            errorMessage = string.Empty;
            return true;
        }
    }

    /// <summary>
    /// Generated Extension Methods
    /// </summary>
    public static partial class RegistrationTemplateExtensionMethods
    {
        /// <summary>
        /// Clones this RegistrationTemplate object to a new RegistrationTemplate object
        /// </summary>
        /// <param name="source">The source.</param>
        /// <param name="deepCopy">if set to <c>true</c> a deep copy is made. If false, only the basic entity properties are copied.</param>
        /// <returns></returns>
        public static RegistrationTemplate Clone( this RegistrationTemplate source, bool deepCopy )
        {
            if (deepCopy)
            {
                return source.Clone() as RegistrationTemplate;
            }
            else
            {
                var target = new RegistrationTemplate();
                target.CopyPropertiesFrom( source );
                return target;
            }
        }

        /// <summary>
        /// Copies the properties from another RegistrationTemplate object to this RegistrationTemplate object
        /// </summary>
        /// <param name="target">The target.</param>
        /// <param name="source">The source.</param>
        public static void CopyPropertiesFrom( this RegistrationTemplate target, RegistrationTemplate source )
        {
            target.Id = source.Id;
            target.AllowMultipleRegistrants = source.AllowMultipleRegistrants;
            target.CategoryId = source.CategoryId;
            target.ConfirmationEmailTemplate = source.ConfirmationEmailTemplate;
            target.Cost = source.Cost;
            target.DiscountCodeTerm = source.DiscountCodeTerm;
            target.FeeTerm = source.FeeTerm;
            target.FinancialGatewayId = source.FinancialGatewayId;
            target.GroupMemberRoleId = source.GroupMemberRoleId;
            target.GroupMemberStatus = source.GroupMemberStatus;
            target.GroupTypeId = source.GroupTypeId;
            target.IsActive = source.IsActive;
            target.LoginRequired = source.LoginRequired;
            target.MaxRegistrants = source.MaxRegistrants;
            target.MinimumInitialPayment = source.MinimumInitialPayment;
            target.Name = source.Name;
            target.NotifyGroupLeaders = source.NotifyGroupLeaders;
            target.RegistrantsSameFamily = source.RegistrantsSameFamily;
            target.RegistrantTerm = source.RegistrantTerm;
            target.RegistrationTerm = source.RegistrationTerm;
            target.ReminderEmailTemplate = source.ReminderEmailTemplate;
            target.RequestEntryName = source.RequestEntryName;
            target.SuccessText = source.SuccessText;
            target.SuccessTitle = source.SuccessTitle;
            target.UseDefaultConfirmationEmail = source.UseDefaultConfirmationEmail;
            target.CreatedDateTime = source.CreatedDateTime;
            target.ModifiedDateTime = source.ModifiedDateTime;
            target.CreatedByPersonAliasId = source.CreatedByPersonAliasId;
            target.ModifiedByPersonAliasId = source.ModifiedByPersonAliasId;
            target.Guid = source.Guid;
            target.ForeignId = source.ForeignId;

        }
    }
}
