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
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Runtime.Serialization;

using Newtonsoft.Json;

using Rock.Data;
using Rock.Security;

namespace Rock.Model
{
    /// <summary>
    /// 
    /// </summary>
    [Table( "RegistrationTemplate" )]
    [DataContract]
    public partial class RegistrationTemplate : Model<RegistrationTemplate>
    {

        #region Entity Properties

        [Required]
        [MaxLength( 100 )]
        [DataMember( IsRequired = true )]
        public string Name { get; set; }

        [DataMember]
        public int? CategoryId { get; set; }

        [DataMember]
        public int? GroupTypeId { get; set; }

        [DataMember]
        public int? GroupMemberRoleId { get; set; }

        [DataMember]
        public int? GroupMemberStatusId { get; set; }

        [DataMember]
        public bool NotifyGroupLeaders { get; set; }

        [DataMember]
        [MaxLength( 100 )]
        public string FeeTerm { get; set; }

        [DataMember]
        [MaxLength( 100 )]
        public string RegistrantTerm { get; set; }

        [DataMember]
        [MaxLength( 100 )]
        public string RegistrationTerm { get; set; }

        [DataMember]
        [MaxLength( 100 )]
        public string DiscountCodeTerm { get; set; }

        [DataMember]
        public bool UseDefaultConfirmationEmail { get; set; }

        [DataMember]
        public string ConfirmationEmailTemplate { get; set; }

        [DataMember]
        public string ReminderEmailTemplate { get; set; }

        [DataMember]
        public decimal MinimumInitialPayment { get; set; }

        [DataMember]
        public bool LoginRequired { get; set; }

        [DataMember]
        public bool RegistrantsInSameFamily { get; set; }

        [DataMember]
        public string RequestEntryName { get; set; }

        [DataMember]
        public string SuccessTitle { get; set; }

        [DataMember]
        public string SuccessText { get; set; }

        [DataMember]
        public bool AllowMultipleRegistrants { get; set; }

        [DataMember]
        public int MaxRegistrants { get; set; }

        [DataMember]
        public int? FinancialGatewayId { get; set; }

        [DataMember]
        public bool IsActive { get; set; }

        [DataMember]
        public RegistrationRequestField RequestHomeCampus { get; set; }

        [DataMember]
        public RegistrationRequestField RequestPhone { get; set; }

        [DataMember]
        public RegistrationRequestField RequestHomeAddress { get; set; }

        [DataMember]
        public RegistrationRequestField RequestEmail { get; set; }

        [DataMember]
        public RegistrationRequestField RequestBirthDate { get; set; }

        [DataMember]
        public RegistrationRequestField RequestGender { get; set; }

        [DataMember]
        public RegistrationRequestField RequestMaritalStatus { get; set; }

        #endregion

        #region Virtual Properties

        /// <summary>
        /// Gets or sets the Page entity for the parent page.
        /// </summary>
        /// <value>
        /// The <see cref="Rock.Model.Page"/> entity for the parent Page
        /// </value>
        public virtual Category Category { get; set; }

        public virtual GroupType GroupType { get; set; }

        /// <summary>
        /// Gets or sets the financial gateway.
        /// </summary>
        /// <value>
        /// The financial gateway.
        /// </value>
        public virtual FinancialGateway FinancialGateway { get; set; }

        /// <summary>
        /// Gets or sets the collection of the current page's child pages.
        /// </summary>
        /// <value>
        /// Collection of child pages
        /// </value>
        [DataMember]
        public virtual ICollection<RegistrationInstance> Instances
        {
            get { return _registrationInstances ?? ( _registrationInstances = new Collection<RegistrationInstance>() ); }
            set { _registrationInstances = value; }
        }
        private ICollection<RegistrationInstance> _registrationInstances;

        [DataMember]
        public virtual ICollection<RegistrationTemplateForm> Forms
        {
            get { return _registrationTemplateForms ?? ( _registrationTemplateForms = new Collection<RegistrationTemplateForm>() ); }
            set { _registrationTemplateForms = value; }
        }
        private ICollection<RegistrationTemplateForm> _registrationTemplateForms;

        #endregion

        #region Methods

        /// <summary>
        /// Returns a <see cref="string"/> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="string"/> that represents this instance.
        /// </returns>
        public override string ToString()
        {
            return Name;
        }

        #endregion

    }

    #region Entity Configuration

    /// <summary>
    /// Configuration class.
    /// </summary>
    public partial class RegistrationTemplateConfiguration : EntityTypeConfiguration<RegistrationTemplate>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RegistrationTemplateConfiguration"/> class.
        /// </summary>
        public RegistrationTemplateConfiguration()
        {
            this.HasOptional( t => t.Category ).WithMany().HasForeignKey( t => t.CategoryId ).WillCascadeOnDelete( false );
            this.HasOptional( t => t.GroupType ).WithMany().HasForeignKey( t => t.GroupTypeId ).WillCascadeOnDelete( false );
            this.HasOptional( t => t.FinancialGateway ).WithMany().HasForeignKey( t => t.GroupType ).WillCascadeOnDelete( false );
        }
    }

    #endregion

    #region Enumerations

    /// <summary>
    /// Represents how a <see cref="Rock.Model.Page"/> should be displayed in the page navigation controls.
    /// </summary>
    public enum RegistrationRequestField
    {
        /// <summary>
        /// Do NOT display field to user
        /// </summary>
        No = 0,

        /// <summary>
        /// Display the field to user
        /// </summary>
        Yes = 1,

        /// <summary>
        /// Require user to enter the field value
        /// </summary>
        Require = 2
    }

    #endregion
}
